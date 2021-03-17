using System;
using System.Collections.Generic;
using System.Linq;
using Intercom.Data;
using Intercom.Exceptions;
using Intercom.Factories;
using Newtonsoft.Json;
using RestSharp;

namespace Intercom.Core
{
    public class Client
    {
        protected const string INTERCOM_API_BASE_URL = "https://api.intercom.io/";
        protected const string CONTENT_TYPE_HEADER = "Content-Type";
        protected const string CONTENT_TYPE_VALUE = "application/json";
        protected const string ACCEPT_HEADER = "Accept";
        protected const string ACCEPT_VALUE = "application/json";
        protected const string ACCEPT_CHARSET_HEADER = "Accept-Charset";
        protected const string ACCEPT_CHARSET_VALUE = "UTF-8";
        protected const string USER_AGENT_HEADER = "User-Agent";
        protected const string USER_AGENT_VALUE = "intercom-dotnet/2.0.0";

        protected readonly string RESRC;
        protected readonly RestClientFactory _restClientFactory;

        public Client(string resource, RestClientFactory restClientFactory)
        {
            RESRC = resource ?? throw new ArgumentNullException(nameof(resource));;
            _restClientFactory = restClientFactory ?? throw new ArgumentNullException(nameof(restClientFactory));
        }

        public Client(string intercomApiUrl, string resource, Authentication authentication)
        {
            if (authentication == null)
            {
                throw new ArgumentNullException(nameof(authentication));
            }

            if (string.IsNullOrEmpty(intercomApiUrl))
            {
                throw new ArgumentNullException(nameof(intercomApiUrl));
            }

            RESRC = resource ?? throw new ArgumentNullException(nameof(resource));;
            _restClientFactory = new RestClientFactory(authentication, intercomApiUrl);
        }

        protected virtual ClientResponse<T> Get<T>(
            Dictionary<string, string> headers = null,
            Dictionary<string, string> parameters = null,
            string resource = null)
            where T : class
        {
            ClientResponse<T> clientResponse = null;

            IRestClient client = null;
            try
            {
                client = BuildClient();
                IRestRequest request = BuildRequest(httpMethod: Method.GET, headers: headers, parameters: parameters, resource: resource);
                IRestResponse response = client.Execute(request);
                clientResponse = HandleResponse<T>(response);
            }
            catch(ApiException ex)
            {
                throw ex;
            }
            catch (JsonConverterException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new IntercomException("An exception occurred " + $"while calling the endpoint. Method: GET, Url: {client.BaseUrl}, Resource: {RESRC}, Sub-Resource: {resource}", ex); 
            }

            return clientResponse;
        }

        protected virtual ClientResponse<T> Post<T>(string body, 
            Dictionary<string, string> headers = null, 
            Dictionary<string, string> parameters = null,
            string resource = null)
            where T : class
        {
            if (string.IsNullOrEmpty(body))
            {
                throw new ArgumentNullException(nameof(body));
            }

            ClientResponse<T> clientResponse = null;

            IRestClient client = null;
            try
            {
                client = BuildClient();
                IRestRequest request = BuildRequest(httpMethod: Method.POST, headers: headers, parameters: parameters, body: body, resource: resource);
                IRestResponse response = client.Execute(request);
                clientResponse = HandleResponse <T>(response);
            }
            catch(ApiException ex)
            {
                throw ex;
            }
            catch (JsonConverterException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new IntercomException("An exception occurred " + $"while calling the endpoint. Method: POST, Url: {client.BaseUrl}, Resource: {RESRC}, Sub-Resource: {resource}, Body: {body}", ex); 
            }

            return clientResponse;
        }

        protected virtual ClientResponse<T> Post<T>(T body, 
            Dictionary<string, string> headers = null, 
            Dictionary<string, string> parameters = null,
            string resource = null)
            where T : class
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            ClientResponse<T> clientResponse = null;

            IRestClient client = null;
            try
            {
                string requestBody = Serialize<T>(body);
                client = BuildClient();
                IRestRequest request = BuildRequest(httpMethod: Method.POST, headers: headers, parameters: parameters, body: requestBody, resource: resource);
                IRestResponse response = client.Execute(request);
                clientResponse = HandleResponse <T>(response);
            }
            catch(ApiException ex)
            {
                throw ex;
            }
            catch (JsonConverterException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new IntercomException($"An exception occurred while calling the endpoint. Method: POST, Url: {client.BaseUrl}, Resource: {RESRC}, Sub-Resource: {resource}, Body-Type: {typeof(T)}", ex); 
            }

            return clientResponse;
        }

        protected virtual ClientResponse<T> Put<T>(string body, 
            Dictionary<string, string> headers = null, 
            Dictionary<string, string> parameters = null,
            string resource = null)
            where T : class
        {
            if (string.IsNullOrEmpty(body))
            {
                throw new ArgumentNullException(nameof(body));
            }

            ClientResponse<T> clientResponse = null;

            IRestClient client = null;
            try
            {
                client = BuildClient();
                IRestRequest request = BuildRequest(httpMethod: Method.PUT, headers: headers, parameters: parameters, body: body, resource: resource);
                IRestResponse response = client.Execute(request);
                clientResponse = HandleResponse <T>(response);
            }
            catch(ApiException ex)
            {
                throw ex;
            }
            catch (JsonConverterException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new IntercomException($"An exception occurred while calling the endpoint. Method: POST, Url: {client.BaseUrl}, Resource: {RESRC}, Sub-Resource: {resource}, Body: {body}", ex); 
            }

            return clientResponse;
        }

        protected virtual ClientResponse<T> Put<T>(T body, 
            Dictionary<string, string> headers = null, 
            Dictionary<string, string> parameters = null,
            string resource = null)
            where T : class
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            ClientResponse<T> clientResponse = null;

            IRestClient client = null;
            try
            {
                string requestBody = Serialize<T>(body);
                client = BuildClient();
                IRestRequest request = BuildRequest(httpMethod: Method.PUT, headers: headers, parameters: parameters, body: requestBody, resource: resource);
                IRestResponse response = client.Execute(request);
                clientResponse = HandleResponse <T>(response);
            }
            catch(ApiException ex)
            {
                throw ex;
            }
            catch (JsonConverterException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new IntercomException($"An exception occurred while calling the endpoint. Method: POST, Url: {client.BaseUrl}, Resource: {RESRC}, Sub-Resource: {resource}", ex); 
            }

            return clientResponse;
        }

        protected virtual ClientResponse<T>  Delete<T>(
            Dictionary<string, string> headers = null, 
            Dictionary<string, string> parameters = null,
            string resource = null)
            where T : class
        {
            ClientResponse<T> clientResponse = null;

            IRestClient client = null;
            try
            {
                client = BuildClient();
                IRestRequest request = BuildRequest(httpMethod: Method.DELETE, headers: headers, parameters: parameters, resource: resource);
                IRestResponse response = client.Execute(request);
                clientResponse = HandleResponse<T>(response);
            }
            catch(ApiException ex)
            {
                throw ex;
            }
            catch (JsonConverterException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new IntercomException($"An exception occurred while calling the endpoint. Method: POST, Url: {client.BaseUrl}, Resource: {RESRC}, Sub-Resource: {resource}", ex); 
            }
        
            return clientResponse;
        }

        protected virtual IRestRequest BuildRequest(Method httpMethod = Method.GET,
            Dictionary<string, string> headers = null, 
            Dictionary<string, string> parameters = null, 
            string body = null,
            string resource = null)
        {
            string final = string.IsNullOrEmpty(resource) ? RESRC : resource;

            IRestRequest request = new RestRequest(final, httpMethod);
            request.AddHeader(CONTENT_TYPE_HEADER, CONTENT_TYPE_VALUE);
            request.AddHeader(ACCEPT_CHARSET_HEADER, ACCEPT_CHARSET_VALUE);
            request.AddHeader(ACCEPT_HEADER, ACCEPT_VALUE);
            request.AddHeader(USER_AGENT_HEADER, USER_AGENT_VALUE);

            if (headers != null && headers.Any())
                AddHeaders(request, headers);

            if (parameters != null && parameters.Any())
                AddParameters(request, parameters);

            if (!string.IsNullOrEmpty(body))
                AddBody(request, body);

            return request;
        }

        protected virtual IRestClient BuildClient()
        {
            return _restClientFactory.RestClient;
        }

        protected virtual void AddHeaders(IRestRequest request, 
            Dictionary<string, string> headers)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (headers == null)
                throw new ArgumentNullException(nameof(headers));

            foreach (var header in headers)
                request.AddParameter(header.Key, header.Value, ParameterType.HttpHeader);
        }

        protected virtual void AddParameters(IRestRequest request, 
            Dictionary<string, string> parameters)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            foreach (var parameter in parameters)
                request.AddParameter(parameter.Key, parameter.Value, ParameterType.QueryString);
        }

        protected virtual void AddBody(IRestRequest request, string body)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!string.IsNullOrEmpty(body))
                request.AddParameter("application/json", body, ParameterType.RequestBody);
        }

        protected virtual ClientResponse<T> HandleResponse<T>(IRestResponse response)
            where T: class
        {
            ClientResponse<T> clientResponse = null;
            int statusCode = (int)response.StatusCode;

            if (statusCode >= 200 && statusCode < 300)
                clientResponse = HandleNormalResponse <T>(response) as ClientResponse<T>;
            else
                clientResponse = HandleErrorResponse <T>(response) as ClientResponse<T>;

            AssertIfAnyErrors(clientResponse);

            return clientResponse;
        }

        protected T Deserialize<T>(string data)
            where T : class
        {
            return JsonConvert.DeserializeObject(data, typeof(T)) as T;
        }

        protected string Serialize<T>(T data)
            where T : class
        {
            return JsonConvert.SerializeObject(data,
                typeof(T),
                Formatting.None, 
                new JsonSerializerSettings
                { 
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        protected ClientResponse<T> HandleErrorResponse<T>(IRestResponse response)
            where T : class
        {
            if (string.IsNullOrEmpty(response.Content))
            {
                return new ClientResponse<T>(response: response);
            }
            else
            {
                Errors errors = Deserialize<Errors>(response.Content);
                return new ClientResponse<T>(response: response, errors: errors);
            }
        }

        protected ClientResponse<T> HandleNormalResponse<T>(IRestResponse response)
            where T : class
        {
            return new ClientResponse<T>(response: response, result: Deserialize<T>(response.Content));
        }

        protected void AssertIfAnyErrors<T>(ClientResponse<T> response)
            where T : class
        {
            if (response.Errors != null && response.Errors.errors != null && response.Errors.errors.Any())
            {
                throw new ApiException((int)response.Response.StatusCode, 
                    response.Response.StatusDescription,
                    response.Errors,
                    response.Response.Content);
            }
        }
    }
}