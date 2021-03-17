using System;
using Intercom.Core;
using Intercom.Data;
using Intercom.Clients;
using Intercom.Exceptions;


namespace Intercom.Data
{
    public class AdminConversationMessage : Message
	{

        public class From { 
			public string id { set; get; }
			public string type { private set; get; }

			public From(string id)
			{
				if(string.IsNullOrEmpty(id))
					throw new ArgumentNullException (nameof(id));
				
				this.id = id;
                this.type = Message.MessageFromOrToType.ADMIN;
			}
		}

		public class To { 
			public string id { set; get; }
			public string email { set; get; }
			public string user_id { set; get; }
			public string type { set; get; }

            public To(string type = Message.MessageFromOrToType.USER,  string id = null, string email = null, string user_id = null)
			{
				if(string.IsNullOrEmpty(type))
					throw new ArgumentNullException (nameof(type));

				if(string.IsNullOrEmpty(id) && string.IsNullOrEmpty(email) && string.IsNullOrEmpty(user_id))
					throw new ArgumentException ("you need to provide either 'id', 'user_id', 'email' to view a user.");

                if(type != Message.MessageFromOrToType.USER && type != Message.MessageFromOrToType.CONTACT)
					throw new ArgumentException ("'type' vale must be either 'contact' or 'user'.");

				this.id = id;
				this.email = email;
				this.user_id = user_id;
				this.type = type;
			}
		}

		public string message_type { get; set; }
		public string subject { get; set; }
		public string template { get; set; }
		public From from { get; set; }
		public To to { get; set; }

        public AdminConversationMessage (
            AdminConversationMessage.From from, 
            AdminConversationMessage.To to,
            string message_type = Message.MessageType.EMAIL,
            string template = Message.MessageTemplate.PLAIN,
			string subject = "", 
			string body = "")
		{
			this.to = to;
			this.from = from;
			this.message_type = message_type;
			this.template = template;
			this.subject = subject;
			this.body = body;
		}
	}
}
