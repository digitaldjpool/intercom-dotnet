using System;
using System.Collections.Generic;
using System.Linq;
using Intercom.Core;
using Intercom.Data;
using Intercom.Factories;

namespace Intercom.Clients
{
    public class EventsClient : Client
    {
        private const string EVENTS_RESOURCE = "events";

        public EventsClient (RestClientFactory restClientFactory)
            : base (EVENTS_RESOURCE, restClientFactory)
        {
        }

        public Event Create (Event @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException (nameof(@event));
            }

            if (string.IsNullOrEmpty(@event.event_name))
            {
                throw new ArgumentException ("'event_name' argument is null or empty.");
            }

            if (!@event.created_at.HasValue)
            {
                throw new ArgumentException ("'created_at' argument must have value.");
            }

            ClientResponse<Event> result = Post<Event>(@event);
            return result.Result;
        }

        public Events List (User user)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {Constants.TYPE, "user"}
            };

            if (!string.IsNullOrEmpty (user.id)) 
            {
                parameters.Add (Constants.INTERCOM_USER_ID, user.id);
            } 
            else if (!string.IsNullOrEmpty (user.user_id)) 
            {
                parameters.Add (Constants.USER_ID, user.user_id);
            } 
            else if (!string.IsNullOrEmpty (user.email)) 
            {
                parameters.Add (Constants.EMAIL, user.email);
            } 
            else 
            {
                throw new ArgumentException ($"you should provide at least value for one of these parameters {Constants.INTERCOM_USER_ID}, or {Constants.USER_ID}, or {Constants.EMAIL} .");
            }

            ClientResponse<Events> result = Get<Events>(parameters: parameters);
            return result.Result;
        }

        public Events List (Dictionary<string, string> parameters)
        {
            if (parameters == null) 
            {
                throw new ArgumentNullException (nameof(parameters));
            }

            if (!parameters.Any ()) 
            {
                throw new ArgumentException ("'parameters' argument should include at least one parameter key, value.");
            }

            if (!parameters.ContainsKey (Constants.INTERCOM_USER_ID) &&
                !parameters.ContainsKey (Constants.USER_ID) &&
                !parameters.ContainsKey (Constants.EMAIL)) {
                throw new ArgumentException ($"'parameters' argument should include at least {Constants.INTERCOM_USER_ID}, or {Constants.USER_ID}, or {Constants.EMAIL} parameter.");
            }

            if (!parameters.Contains (new KeyValuePair<string, string> (Constants.TYPE, "user"))) 
            {
                parameters.Add (Constants.TYPE, "user");
            }

            ClientResponse<Events> result = null;
            result = Get<Events> (parameters: parameters);
            return result.Result;
        }
    }
}