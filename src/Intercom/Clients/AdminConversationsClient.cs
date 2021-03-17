using System;
using System.Collections.Generic;
using System.IO;
using Intercom.Core;
using Intercom.Data;
using Intercom.Factories;

namespace Intercom.Clients
{
    public class AdminConversationsClient: Client
    {
        private const string CONVERSATIONS_RESOURCE = "conversations";
        private const string MESSAGES_RESOURCE = "messages";
        private const string REPLY_RESOURCE = "reply";

        public AdminConversationsClient(RestClientFactory restClientFactory)
            : base(CONVERSATIONS_RESOURCE, restClientFactory)
        {
        }

        [Obsolete("This constructor is deprecated as of 3.0.0 and will soon be removed, please use AdminConversationsClient(RestClientFactory restClientFactory)")]
        public AdminConversationsClient(Authentication authentication)
            : base(INTERCOM_API_BASE_URL, CONVERSATIONS_RESOURCE, authentication)
        {
        }

        [Obsolete("This constructor is deprecated as of 3.0.0 and will soon be removed, please use AdminConversationsClient(RestClientFactory restClientFactory)")]
        public AdminConversationsClient(string intercomApiUrl, Authentication authentication)
            : base(string.IsNullOrEmpty(intercomApiUrl) ? INTERCOM_API_BASE_URL : intercomApiUrl, CONVERSATIONS_RESOURCE, authentication)
        {
        }

        public Conversation Reply(AdminConversationReply reply)
        {
            if (reply == null)
            {
                throw new ArgumentNullException(nameof(reply));
            }

            ClientResponse<Conversation> result = null;
            string body = Serialize<AdminConversationReply>(reply);
            result = Post<Conversation>(body, resource: CONVERSATIONS_RESOURCE + Path.DirectorySeparatorChar + reply.conversation_id + Path.DirectorySeparatorChar + REPLY_RESOURCE);
            return result.Result;
        }

        public AdminConversationMessage Create(AdminConversationMessage adminMessage)
        {
            if (adminMessage == null)
            {
                throw new ArgumentNullException(nameof(adminMessage));
            }

            ClientResponse<AdminConversationMessage> result = null;
            result = Post<AdminConversationMessage>(adminMessage, resource: MESSAGES_RESOURCE);
            return result.Result;
        }

        public Conversations List(Admin admin, bool? open = null, bool? displayAsPlainText = null)
        {
            if (admin == null)
            {
                throw new ArgumentNullException(nameof(admin));
            }

            if (string.IsNullOrEmpty(admin.id))
            {
                throw new ArgumentException("'admin.id' argument is null or empty.");
            }

            ClientResponse<Conversations> result = null;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add(Constants.TYPE, Constants.ADMIN);

            if (open != null && open.HasValue)
            {
                parameters.Add(Constants.OPEN, open.Value.ToString());
            }

            if (displayAsPlainText != null && displayAsPlainText.HasValue)
            {
                parameters.Add(Constants.DISPLAY_AS, Constants.PLAIN_TEXT);
            }

            parameters.Add(Constants.ADMIN_ID, admin.id);

            result = Get<Conversations>(parameters: parameters);
            return result.Result;
        }

        public Conversation ReplyLastConversation(AdminLastConversationReply lastConversationReply)
        {
            if (lastConversationReply.intercom_user_id == null)
            {
                throw new ArgumentNullException(nameof(lastConversationReply.intercom_user_id));
            }

            ClientResponse<Conversation> result = null;
            string body = Serialize<AdminLastConversationReply>(lastConversationReply);
            result = Post<Conversation>(body, resource: CONVERSATIONS_RESOURCE + Path.DirectorySeparatorChar + "last" + Path.DirectorySeparatorChar + REPLY_RESOURCE);
            return result.Result;
        }
    }
}