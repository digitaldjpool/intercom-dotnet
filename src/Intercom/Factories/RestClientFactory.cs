using RestSharp;
using RestSharp.Authenticators;
using Intercom.Core;

namespace Intercom.Factories
{
    public class RestClientFactory
    {

        private const string INTERCOM_API_BASE_URL = "https://api.intercom.io/";
        private readonly Authentication _authentication;
        private readonly string _url;
        private IRestClient _restClient;

        private readonly object padlock = new object();

        public RestClientFactory(Authentication authentication)
        {
            _authentication = authentication;
            _url = INTERCOM_API_BASE_URL;
        }

        public RestClientFactory(Authentication authentication, string url)
        {
            _authentication = authentication;
            _url = url;
        }

        public virtual IRestClient RestClient
        {
            get
            {
                lock(padlock)
                {
                    if (_restClient != null)
                    {
                        return _restClient;
                    }
                    
                    ConstructClient();
                    return _restClient; 
                }
            }
        }

        private void ConstructClient()
        {
            var client = new RestClient(_url)
            {
                Authenticator = !string.IsNullOrEmpty(_authentication.AppId) && !string.IsNullOrEmpty(_authentication.AppKey)
                    ? new HttpBasicAuthenticator(_authentication.AppId, _authentication.AppKey)
                    : new HttpBasicAuthenticator(_authentication.PersonalAccessToken, string.Empty)
            };

            _restClient = client;
        }
    }
}