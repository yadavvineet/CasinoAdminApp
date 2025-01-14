﻿using System;
using System.Runtime.Serialization;

namespace Casino.AdminPortal.Shared
{
    /// <summary>
    /// Represents data access exception,
    /// Author: Nagarro
    /// </summary>
    [Serializable]
    public class DacException : ExceptionBase
    {
        private const string MessageHeader = "DAC Exception: ";

        /// <summary>
        /// Initializes a new instance of the <see cref="DacException"/> class.
        /// </summary>
        public DacException()
            : base(MessageHeader, null)
        {
            //this.DefaultExceptionPolicyType = ExceptionPolicyType.Basic;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DacException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        public DacException(int errorCode)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DacException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DacException(string message)
            : base(MessageHeader + message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DacException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DacException(string message, Exception innerException)
            : base(MessageHeader + message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DacException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected DacException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
