using System;
using Intercom.Core;
using Intercom.Data;
using Intercom.Clients;
using Intercom.Exceptions;


namespace Intercom.Data
{
    public class Message : Model
	{
		public static class MessageType
		{
			public const string IN_APP = "inapp";
			public const string EMAIL = "email";
		}

		public static class MessageTemplate
		{
			public const string PLAIN = "plain";
			public const string PERSONAL = "personal";
		}

		public static class MessageFromOrToType
		{
			public const string USER = "user";
			public const string ADMIN = "admin";
			public const string CONTACT = "contact";
		}

		public virtual string body { get; set; }

        public Message ()
		{
		}
	}
}