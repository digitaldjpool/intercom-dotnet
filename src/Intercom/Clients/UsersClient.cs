using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Intercom.Core;
using Intercom.Data;
using Intercom.Factories;
using Newtonsoft.Json;

namespace Intercom.Clients
{
    public class UsersClient : Client
    {
        // TODO: Implement paging
        private static class UserSortBy
        {
            public const string created_at = "created_at";
            public const string updated_at = "updated_at";
            public const string signed_up_at = "signed_up_at";
        }

        private const string USERS_RESOURCE = "users";
        private const string PERMANENT_DELETE_RESOURCE = "user_delete_requests";

        public UsersClient(RestClientFactory restClientFactory)
            : base(USERS_RESOURCE, restClientFactory)
        {
        }

        public User Create(User user)
        {
            Guard.AgainstNull(nameof(user), user);

            if (string.IsNullOrEmpty(user.user_id) && string.IsNullOrEmpty(user.email))
            {
                throw new ArgumentException("you need to provide either 'user.user_id', 'user.email' to create a user.");
            }

            ClientResponse<User> result = Post<User>(Transform(user));
            
            return result.Result;
        }

        public User Update(User user)
        {
            Guard.AgainstNull(nameof(user), user);

            if (string.IsNullOrEmpty(user.id) && string.IsNullOrEmpty(user.user_id) && string.IsNullOrEmpty(user.email))
            {
                throw new ArgumentException("you need to provide either 'user.id', 'user.user_id', 'user.email' to update a user.");
            }

            ClientResponse<User> result = Post<User>(Transform(user));

            return result.Result;
        }

        private User CreateOrUpdate(User user)
        {
            if (user.custom_attributes != null && user.custom_attributes.Any())
            {
                if (user.custom_attributes.Count > 100)
                {
                    throw new ArgumentException("Maximum of 100 fields.");
                }

                foreach (var attr in user.custom_attributes)
                {
                    if (attr.Key.Contains(".") || attr.Key.Contains("$"))
                    {
                        throw new ArgumentException($"Field names must not contain Periods (.) or Dollar ($) characters. key: {attr.Key}");
                    }

                    if (attr.Key.Length > 190)
                    {
                        throw new ArgumentException($"Field names must be no longer than 190 characters. key: {attr.Key}");
                    }

                    if (attr.Value == null)
                    {
                        throw new ArgumentException($"'value' is null. key: {attr.Key}");
                    }
                }
            }

            ClientResponse<User> result = Post<User>(Transform(user));
            
            return result.Result;
        }

        public User View(Dictionary<string, string> parameters)
        {
            Guard.AgainstNull(nameof(parameters), parameters);
            
            if (!parameters.Any())
            {
                throw new ArgumentException("'parameters' argument should include user_id parameter.");
            }

            ClientResponse<User> result = Get<User>(parameters: parameters);
            
            return result.Result;
        }

        public User View(string id)
        {
            Guard.AgainstNullAndEmpty(nameof(id), id);
                
            ClientResponse<User> result = Get<User>(resource: USERS_RESOURCE + Path.DirectorySeparatorChar + id);
            
            return result.Result;
        }

        public User View(User user)
        {
            Guard.AgainstNull(nameof(user), user);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            ClientResponse<User> result;

            if (!string.IsNullOrEmpty(user.id))
            {
                result = Get<User>(resource: USERS_RESOURCE + Path.DirectorySeparatorChar + user.id);
            }
            else if (!string.IsNullOrEmpty(user.user_id))
            {
                parameters.Add(Constants.USER_ID, user.user_id);
                result = Get<User>(parameters: parameters);
            }
            else if (!string.IsNullOrEmpty(user.email))
            {
                parameters.Add(Constants.EMAIL, user.email);
                result = Get<User>(parameters: parameters);
            }
            else
            {
                throw new ArgumentException("you need to provide either 'user.id', 'user.user_id', 'user.email' to view a user.");
            }

            return result.Result;
        }

        public Users List()
        {
            ClientResponse<Users> result = Get<Users>();
            
            return result.Result;
        }

        public Users List(Dictionary<string, string> parameters)
        {
            ClientResponse<Users> result = Get<Users>(parameters: parameters);
            
            return result.Result;
        }

        // TODO: Implement paging (by Pages argument)
        private Users Next(Pages pages)
        {
            throw new NotImplementedException();
        }

        // TODO: Implement paging
        private Users Next(int page = 1, int perPage = 50, OrderBy orderBy = OrderBy.Dsc, string sortBy = UserSortBy.created_at)
        {
            throw new NotImplementedException();
        }

        public Users Scroll(string scrollParam = null)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            
            if (!string.IsNullOrWhiteSpace(scrollParam))
            {
                parameters.Add("scroll_param", scrollParam);
            }

            ClientResponse<Users> result = Get<Users>(parameters: parameters, resource: USERS_RESOURCE + Path.DirectorySeparatorChar + "scroll");
            return result.Result;
        }

        public User Archive(User user)
        {
            Guard.AgainstNull(nameof(user), user);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            ClientResponse<User> result = null;

            if (!string.IsNullOrEmpty(user.id))
            {
                result = Delete<User>(resource: USERS_RESOURCE + Path.DirectorySeparatorChar + user.id);
            }
            else if (!string.IsNullOrEmpty(user.user_id))
            {
                parameters.Add(Constants.USER_ID, user.user_id);
                result = Delete<User>(parameters: parameters);
            }
            else if (!string.IsNullOrEmpty(user.email))
            {
                parameters.Add(Constants.EMAIL, user.email);
                result = Delete<User>(parameters: parameters);
            }
            else
            {
                throw new ArgumentException("you need to provide either 'user.id', 'user.user_id', 'user.email' to view a user.");
            }

            return result.Result;
        }

        [Obsolete("Replaced by Archive(User user). Renamed for consistency with API language.")]
        public User Delete(User user)
        {
            return Archive(user);
        }

        public User Archive(string id)
        {
            Guard.AgainstNullAndEmpty(nameof(id), id);

            ClientResponse<User> result = Delete<User>(resource: USERS_RESOURCE + Path.DirectorySeparatorChar + id);
            
            return result.Result;
        }

        [Obsolete("Replaced by Archive(String id). Renamed for consistency with API language.")]
        public User Delete(string id)
        {
            return Archive(id);
        }

        public User UpdateLastSeenAt(string id, long timestamp)
        {
            Guard.AgainstNullAndEmpty(nameof(id), id);

            if (timestamp <= 0)
            {
                throw new ArgumentException("'timestamp' argument should be bigger than zero.");
            }

            var body = JsonConvert.SerializeObject(new { id = id, last_request_at = timestamp });
            ClientResponse<User> result = Post<User>(body);
            
            return result.Result;
        }

        public User UpdateLastSeenAt(User user, long timestamp)
        {
            Guard.AgainstNull(nameof(user), user);
            
            Guard.AgainstNegativeAndZero(nameof(timestamp), timestamp);

            string body;

            if (!string.IsNullOrEmpty(user.id))
            {
                body = JsonConvert.SerializeObject(new { id = user.id, last_request_at = timestamp });
            }
            else if (!string.IsNullOrEmpty(user.user_id))
            {
                body = JsonConvert.SerializeObject(new { user_id = user.user_id, last_request_at = timestamp });
            }
            else if (!string.IsNullOrEmpty(user.email))
            {
                body = JsonConvert.SerializeObject(new { email = user.email, last_request_at = timestamp });
            }
            else
            {
                throw new ArgumentException("you need to provide either 'user.id', 'user.user_id', 'user.email' to update a user's last seen at.");
            }

            ClientResponse<User> result = Post<User>(body);
            
            return result.Result;
        }

        public User UpdateLastSeenAt(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var body = JsonConvert.SerializeObject(new { id = id, update_last_request_at = true });
            
            ClientResponse<User> result = Post<User>(body);
            return result.Result;
        }

        public User UpdateLastSeenAt(User user)
        {
            Guard.AgainstNull(nameof(user), user);

            string body;

            if (!string.IsNullOrEmpty(user.id))
            {
                body = JsonConvert.SerializeObject(new { id = user.id, update_last_request_at = true });
            }
            else if (!string.IsNullOrEmpty(user.user_id))
            {
                body = JsonConvert.SerializeObject(new { user_id = user.user_id, update_last_request_at = true });
            }
            else if (!string.IsNullOrEmpty(user.email))
            {
                body = JsonConvert.SerializeObject(new { email = user.email, update_last_request_at = true });
            }
            else
            {
                throw new ArgumentException("you need to provide either 'user.id', 'user.user_id', 'user.email' to update a user's last seen at.");
            }

            ClientResponse<User> result = Post<User>(body);
            
            return result.Result;
        }

        public User IncrementUserSession(string id)
        {
            Guard.AgainstNullAndEmpty(nameof(id), id);

            var body = JsonConvert.SerializeObject(new { id = id, new_session = true });
            
            ClientResponse<User> result = Post<User>(body);
            return result.Result;
        }

        public User IncrementUserSession(string id, List<string> companyIds)
        {
            Guard.AgainstNullAndEmpty(nameof(id), id);
            Guard.AgainstNull(nameof(companyIds), companyIds);
            Guard.AgainstEmpty(nameof(companyIds), companyIds);

            var body = JsonConvert.SerializeObject(new { id = id, new_session = true, companies = companyIds.Select(c => new { id = c }) });
            ClientResponse<User> result = Post<User>(body);
            
            return result.Result;
        }

        public User RemoveCompanyFromUser(string id, List<string> companyIds)
        {
            Guard.AgainstNullAndEmpty(nameof(id), id);
            Guard.AgainstNull(nameof(companyIds), companyIds);
            Guard.AgainstEmpty(nameof(companyIds), companyIds);

            var body = JsonConvert.SerializeObject(new { id = id, companies = companyIds.Select(c => new { id = c, remove = true }) });
            ClientResponse<User> result = Post<User>(body);
            
            return result.Result;
        }

        public User RemoveCompanyFromUser(User user, List<string> companyIds)
        {
            Guard.AgainstNullAndEmpty("user.id", user.id);
            Guard.AgainstNull(nameof(companyIds), companyIds);
            Guard.AgainstEmpty(nameof(companyIds), companyIds);

            var body = JsonConvert.SerializeObject(new { id = user.id, companies = companyIds.Select(c => new { id = c, remove = true }) });
            ClientResponse<User> result = Post<User>(body);
            
            return result.Result;
        }

        public User PermanentlyDeleteUser(string id)
        {
            Guard.AgainstNullAndEmpty(nameof(id), id);

            var body = JsonConvert.SerializeObject(new { intercom_user_id = id });
            ClientResponse<User> result = Post<User>(resource: PERMANENT_DELETE_RESOURCE, body: body);
            
            return result.Result;
        }

        private string Transform(User user)
        {
            object companies;

            if (user.companies != null && user.companies.Any())
            {
                companies = user.companies.Select(c => new
                {
                    remote_created_at = c.remote_created_at,
                    company_id = c.company_id,
                    name = c.name,
                    monthly_spend = c.monthly_spend,
                    custom_attributes = c.custom_attributes,
                    plan = c.plan != null ? c.plan.name : null,
                    website = c.website,
                    size = c.size,
                    industry = c.industry,
                    remove = c.remove
                }).ToList();
            }
            else
            {
                companies = null;
            }

            var body = new
            {
                id = user.id,
                user_id = user.user_id,
                email = user.email,
                phone = user.phone,
                name = user.name,
                companies = companies,
                avatar = user.avatar,
                signed_up_at = user.signed_up_at,
                last_seen_ip = user.last_seen_ip,
                custom_attributes = user.custom_attributes,
                new_session = user.new_session,
                last_seen_user_agent = user.user_agent_data,
                last_request_at = user.last_request_at,
                unsubscribed_from_emails = user.unsubscribed_from_emails,
                referrer = user.referrer,
                utm_campaign = user.utm_campaign,
                utm_content = user.utm_content,
                utm_medium = user.utm_medium,
                utm_source = user.utm_source,
                utm_term = user.utm_term
            };

            return JsonConvert.SerializeObject(body,
                           Formatting.None,
                           new JsonSerializerSettings
                           {
                               NullValueHandling = NullValueHandling.Ignore
                           });
        }
    }
}
