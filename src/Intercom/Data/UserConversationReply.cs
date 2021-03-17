using System;
using Intercom.Core;
using Intercom.Data;
using Intercom.Clients;
using Intercom.Exceptions;
using System.Collections.Generic;

namespace Intercom.Data
{
    public class UserConversationReply : Reply
    {
        public override string type
        {
            get
            {
                return Reply.ReplySenderType.USER;
            }
        }

        public override string message_type
        { 
            get
            { 
                return Reply.ReplyMessageType.COMMENT; 
            }
        }

        public string email { set; get; }

        public string user_id { set; get; }

        public string intercom_user_id { set; get; }

        public UserConversationReply(
            string conversationId,
            string body,
            string intercomUserId = null, 
            string email = null, 
            string userId = null,
            List<string> attachementUrls = null)
            : base(conversationId, Reply.ReplyMessageType.COMMENT, body, attachementUrls)
        {

            if (string.IsNullOrEmpty(conversationId))
                throw new ArgumentNullException(nameof(conversationId));
            
            if (string.IsNullOrEmpty(intercomUserId) && string.IsNullOrEmpty(email) && string.IsNullOrEmpty(userId))
                throw new ArgumentException("you need to provide either 'intercomUserId', 'userId', 'email' to view a user.");

            this.email = email;
            this.user_id = userId;
            this.intercom_user_id = intercomUserId;
        }
    }
}
