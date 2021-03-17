using System;
using System.Collections.Generic;
using System.IO;
using Intercom.Core;
using Intercom.Data;
using Intercom.Factories;

namespace Intercom.Clients
{
    public class ConversationsClient : Client
    {
        public static class MessageType
        {
            public const string ASSIGNMENT = "assignment";
            public const string COMMENT = "comment";
            public const string CLOSE = "close";
            public const string OPEN = "open";
            public const string NOTE = "note";
        }

        private const string CONVERSATIONS_RESOURCE = "conversations";

        public ConversationsClient( RestClientFactory restClientFactory)
            : base(CONVERSATIONS_RESOURCE, restClientFactory)
        {
        }

        [Obsolete("This constructor is deprecated as of 3.0.0 and will soon be removed, please use ConversationsClient(RestClientFactory restClientFactory)")]
        public ConversationsClient(Authentication authentication)
            : base(INTERCOM_API_BASE_URL, CONVERSATIONS_RESOURCE, authentication)
        {
        }

        [Obsolete("This constructor is deprecated as of 3.0.0 and will soon be removed, please use ConversationsClient(RestClientFactory restClientFactory)")]
        public ConversationsClient(string intercomApiUrl, Authentication authentication)
            : base(string.IsNullOrEmpty(intercomApiUrl) ? INTERCOM_API_BASE_URL : intercomApiUrl, CONVERSATIONS_RESOURCE, authentication)
        {
        }

        public Conversation View(string id, bool? displayAsPlainText = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (displayAsPlainText != null && displayAsPlainText.HasValue)
            {
                parameters.Add(Constants.DISPLAY_AS, Constants.PLAIN_TEXT);
            }

            ClientResponse<Conversation> result = null;
            result = Get<Conversation>(resource: CONVERSATIONS_RESOURCE + Path.DirectorySeparatorChar + id);
            return result.Result;
        }

        public Conversations ListAll ()
        {
            ClientResponse<Conversations> result = null;
            result = Get<Conversations>(resource: CONVERSATIONS_RESOURCE, parameters: null);
            return result.Result;
        }

        public Conversations ListAll(Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            ClientResponse<Conversations> result = null;
            result = Get<Conversations>(resource: CONVERSATIONS_RESOURCE, parameters: parameters);
            return result.Result;
        }
    }
}
