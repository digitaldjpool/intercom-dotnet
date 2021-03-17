using System;

namespace Intercom.Core
{
    public class Authentication
    {
        public string AppId { get;} 
        public string AppKey{ get;}
        public string PersonalAccessToken { get; }

        public Authentication (string personalAccessToken)
        {
            if (string.IsNullOrEmpty(personalAccessToken))
            {
                throw new ArgumentException ("'PersonalAccessToken' argument is not found.");
            }

            PersonalAccessToken = personalAccessToken;
        }

        public Authentication (string appId, string appKey)
        {
            if (string.IsNullOrEmpty(appId))
            {
                throw new ArgumentException ("'appId' argument is not found.");
            }

            if (string.IsNullOrEmpty(appKey))
            {
                throw new ArgumentException ("'appKey' argument is not found.");
            }

            AppId = appId;
            AppKey = appKey;
        }
    }
}