﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBridge.EF.Internals
{
    /// <summary>
    /// A value that identifies and is used to locate a particular element within a <see cref="Domain.Record"/>.
    /// </summary>
    internal class FieldIndex
    {
        [Obsolete("Runtime only", true)]
        public FieldIndex() { }
        public FieldIndex(Guid recordId, string name, object value)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            RecordId = recordId;
            Name = name;

            UpdateValue(value);
        }

        /// <summary>
        /// Sets the appropriate values of Text, Moment, Number, and Float fields.
        /// Exceptions if <paramref name="value"/>'s type is invalid.
        /// </summary>
        /// <param name="value"></param>
        public void UpdateValue(object value)
        {
            Guid = null;
            Text = null;
            Moment = null;
            Number = null;
            Float = null;

            if (value is Guid?)
            {
                Guid = (Guid?)value;
            }
            else if (value is string)
            {
                Text = (string)value;
            }
            else if (value is DateTimeOffset?)
            {
                Moment = (DateTimeOffset?)value;
            }
            else if (value is int? || value is decimal?)
            {
                Number = (decimal?)value;
            }
            else if (value is float?)
            {
                Float = (float?)value;
            }
            else
            {
                throw new ArgumentException("value",
                    string.Format("The argument of type '{0}' is not supported and must be of type String, DateTimeOffset, Int32, Decimal, or Float.", value.GetType()));
            }
        }

        [Key, Column(Order = 1)]
        [Required]
        public Guid RecordId { get; protected set; }
        public Record Record { get; protected set; }

        [Key, Column(Order = 2)]
        [Required, StringLength(442)]
        public string Name { get; protected set; }

        [Index]
        public Guid? Guid { get; protected set; }

        /// <summary>
        /// Text is constrained to the first 450 characters due to SQL Server's index.
        /// </summary>
        [Index, StringLength(450)]
        public string Text
        {
            get { return _Text; }
            protected set { _Text = value != null && value.Length > 450 ? value.Substring(0, 450) : value; }
        }
        private string _Text;

        [Index]
        public DateTimeOffset? Moment { get; protected set; }

        [Index]
        public decimal? Number { get; protected set; }

        [Index]
        public float? Float { get; protected set; }
    }
}