using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Domain.Exceptions
{
    public class ModelException : Exception
    {
        public string ArgumentName { get; set; }
        public int StatusCode { get; set; }

        public override string ToString()
        {
            return $"{Message} (Parameter: {ArgumentName})";
        }

        public ModelException(string message, string argument) : base(message)
        {
            ArgumentName = argument;
        }
    }
    public class BadArgumentException : ModelException
    {
        public BadArgumentException(string message, string argument) : base(message,argument)
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }

    public class NotFoundException : ModelException
    {
        public NotFoundException(string message, string argument) : base(message, argument)
        {
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
