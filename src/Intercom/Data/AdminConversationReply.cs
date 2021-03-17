using System;
using Intercom.Core;
using Intercom.Data;


using Intercom.Clients;

using Intercom.Exceptions;

using System.Collections.Generic;

namespace Intercom.Data
{
    public class AdminConversationReply : Reply
    {
        public override string type
        {
            get
            {
                return Reply.ReplySenderType.ADMIN;
            }
        }

        public string admin_id { set; get; }

        public string assignee_id { set; get; }

        public AdminConversationReply(string conversationId,
                                      string adminId, 
                                      string messageType = Reply.ReplyMessageType.COMMENT,
                                      string body = "",
                                      List<string> attachementUrls = null)
            : base(conversationId, messageType, body, attachementUrls)
        {

            if (string.IsNullOrEmpty(conversationId))
                throw new ArgumentNullException(nameof(conversationId));

            if ((messageType == Reply.ReplyMessageType.COMMENT ||
                messageType == Reply.ReplyMessageType.NOTE) &&
                string.IsNullOrEmpty(body))
            {
                throw new ArgumentException("'body' argument shouldnt be empty when the message type is 'comment' or 'note'.");
            }

            this.admin_id = adminId;
        }
    }
}