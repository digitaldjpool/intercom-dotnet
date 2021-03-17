using System;
using Intercom.Core;
using Intercom.Data;


using Intercom.Clients;

using Intercom.Exceptions;


namespace Intercom.Data
{
    public class UserConversationMessage : Message
	{

		public class From : Model
		{
			public string email { set; get; }
			public string user_id { set; get; }

            public From(string type = Message.MessageFromOrToType.USER, string id = null, string email = null, string user_id = null, User user = null)
            {
                //Validate type of message
                if (string.IsNullOrEmpty(type))
                    throw new ArgumentNullException(nameof(type));

                //Validate required fields related to User
                if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(email) && string.IsNullOrEmpty(user_id))
                    throw new ArgumentException("you need to provide either 'id', 'user_id', 'email' to view a user.");

                //Validate User required fields
                if (user != null && string.IsNullOrEmpty(user.id) && string.IsNullOrEmpty(user.email) && string.IsNullOrEmpty(user.user_id))
                    throw new ArgumentException("you need to provide either 'id', 'user_id', 'email' to view a user.");

                //Validate required types
                if (type != Message.MessageFromOrToType.USER && type != Message.MessageFromOrToType.CONTACT)
                    throw new ArgumentException("'type' value must be either 'contact' or 'user'.");

                if (user != null)
                {
                    this.id = user.id;
                    this.email = user.email;
                    this.user_id = user.user_id;
                }
                else
                {
                    this.id = id;
                    this.email = email;
                    this.user_id = user_id;
                }

                this.type = type;
            }
        }

		public From from { set; get; }

        public UserConversationMessage (From from, string body)
		{
			this.from = from;
			this.body = body;
		}
	}
}