using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Intercom.Core;
using Intercom.Data;
using Intercom.Factories;

namespace Intercom.Clients
{
    public class SegmentsClient: Client
    {
        private const string SEGMENTS_RESOURCE = "segments";

        public SegmentsClient(RestClientFactory restClientFactory)
            : base(SEGMENTS_RESOURCE, restClientFactory)
        {
        }

        [Obsolete("This constructor is deprecated as of 3.0.0 and will soon be removed, please use SegmentsClient(RestClientFactory restClientFactory)")]
        public SegmentsClient(Authentication authentication)
            : base(INTERCOM_API_BASE_URL, SEGMENTS_RESOURCE, authentication)
        {
        }

        [Obsolete("This constructor is deprecated as of 3.0.0 and will soon be removed, please use SegmentsClient(RestClientFactory restClientFactory)")]
        public SegmentsClient(string intercomApiUrl, Authentication authentication)
            : base(string.IsNullOrEmpty(intercomApiUrl) ? INTERCOM_API_BASE_URL : intercomApiUrl, SEGMENTS_RESOURCE, authentication)
        {
        }

        public Segments List(bool company = false)
        {
            ClientResponse<Segments> result = null;

            if (company)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add(Constants.TYPE, Constants.COMPANY);
                result = Get<Segments>(parameters: parameters);
            }
            else
            {
                result = Get<Segments>();
            }

            return result.Result;
        }

        public Segments List(Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (!parameters.Any())
            {
                throw new ArgumentException ("'parameters' argument is empty.");
            }

            ClientResponse<Segments> result = null;
            result = Get<Segments>(parameters: parameters);
            return result.Result;
        }

        public Segment View(string id, bool? includeCount = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (includeCount != null && includeCount.HasValue)
            {
                parameters.Add("include_count", includeCount.ToString());
            };

            ClientResponse<Segment> result = null;
            result = Get<Segment>(parameters: parameters, resource: SEGMENTS_RESOURCE + Path.DirectorySeparatorChar + id);
            return result.Result;
        }

        public Segment View(Segment segment, bool? includeCount = null)
        {
            if (segment == null)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            if (string.IsNullOrEmpty(segment.id))
            {
                throw new ArgumentException("you must provide value for 'segment.id'.");
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (includeCount != null && includeCount.HasValue)
            {
                parameters.Add("include_count", includeCount.ToString());
            };

            ClientResponse<Segment> result = null;
            result = Get<Segment>(parameters: parameters, resource: SEGMENTS_RESOURCE + Path.DirectorySeparatorChar + segment.id);
            return result.Result;  
        }
    }
}