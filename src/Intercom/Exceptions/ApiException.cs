using System;
using Intercom.Data;

namespace Intercom.Exceptions
{
    public class ApiException : IntercomException
    {
        public int StatusCode { get; }
        public string StatusDescription { get; }
        public string ApiResponseBody { get; }
        public Errors ApiErrors { get; }

        public ApiException (string message, Exception innerException) 
            :base(message, innerException)
        {
        }

        public ApiException (int statusCode, string statusDescription, Errors apiErrors, string apiResponseBody)
        {
            StatusCode = statusCode;
            StatusDescription = statusDescription;
            ApiErrors = apiErrors;
            ApiResponseBody = apiResponseBody;
        }

        public ApiException (string message, Exception innerException, int statusCode, string statusDescription, Errors apiErrors, string apiResponseBody)
            :base(message, innerException)
        {
            StatusCode = statusCode;
            StatusDescription = statusDescription;
            ApiErrors = apiErrors;
            ApiResponseBody = apiResponseBody;
        }

    }
}