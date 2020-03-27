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
        public string Method { get; set; }

        public override string ToString()
        {
            return $"{Message} At {Method} (Parameter: {ArgumentName})";
        }

        public ModelException(string message, string argument, string method) : base(message)
        {
            ArgumentName = argument;
            Method = method;
        }
    }
    public class BadArgumentException : ModelException
    {
        public BadArgumentException(string message, string argument, string method) : base(message,argument, method)
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }

    public class NotFoundException : ModelException
    {
        public NotFoundException(string message, string argument, string method) : base(message, argument, method)
        {
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
