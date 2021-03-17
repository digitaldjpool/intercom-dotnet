using System;

namespace Intercom.Exceptions
{
    public class JsonConverterException : IntercomException
    {
        public string Json { set; get; }
        public string SerializationType { set; get; }

        public JsonConverterException(string message) 
            :base(message)
        {
        }

        public JsonConverterException (string message, Exception innerException) 
            :base(message, innerException)
        {
        }
    }
}