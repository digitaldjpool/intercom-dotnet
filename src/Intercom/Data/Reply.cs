using System;
using Intercom.Core;
using Intercom.Data;


using Intercom.Clients;

using Intercom.Exceptions;

using System.Collections.Generic;

namespace Intercom.Data
{
    public class Reply : Model
    {
        public static class ReplyMessageType
        {
            public const string ASSIGNMENT = "assignment";
            public const string COMMENT = "comment";
            public const string CLOSE = "close";
            public const string OPEN = "open";
            public const string NOTE = "note";
        }

        public static class ReplySenderType
        {
            public const string USER = "user";
            public const string ADMIN = "admin";
        }

        public virtual string conversation_id { set; get; }
        public virtual string message_type { set; get; }
        public virtual string body { set; get; }
        public virtual List<string> attachment_urls { get; set; }

        public Reply(
            string conversation_id,
            string messageType = Reply.ReplyMessageType.COMMENT,
            string body = "",
            List<string> attachementUrls = null)
        {
            if (attachementUrls != null && attachementUrls.Count > 5)
            {
                throw new ArgumentException("'attachment_urls' need to be equal or less than 5 urls.");
            }

            this.body = body;
            this.conversation_id = conversation_id;
            this.message_type = messageType;
            this.attachment_urls = attachementUrls;
        }
    }
}

