using System;
using Intercom.Core;
using Intercom.Data;
using Intercom.Clients;
using Intercom.Exceptions;
using RestSharp;

namespace Intercom.Exceptions
{
    public class IntercomException : Exception
    {
        public IntercomException ()
        {
        }

        public IntercomException (string message) 
            :base(message)
        {
        }

        public IntercomException (string message, Exception innerException) 
            :base(message, innerException)
        {
        }
    }
}