using Intercom.Core;
using Intercom.Data;
using System;


using Intercom.Clients;

using Intercom.Exceptions;

using System.Collections.Generic;
using System.Linq;

namespace Intercom.Data
{
    public class Metadata
    {
        public class RichLink: ICloneable
        {
            public string url { set; get; }

            public string value { set; get; }

            public RichLink(string url, string value)
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentNullException(nameof(url));
                }

                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.url = url;
                this.value = value;
            }

            public object Clone()
            {
                return new RichLink(url, value);
            }
        }

        public class MonetaryAmount :ICloneable
        {
            public int amount { set; get; }

            public string currency { set; get; }

            public MonetaryAmount(int amount, string currency)
            {
                if (string.IsNullOrEmpty(currency))
                {
                    throw new ArgumentNullException(nameof(currency));
                }

                this.amount = amount;
                this.currency = currency;
            }

            public object Clone()
            {
                return new MonetaryAmount(amount, currency);
            }
        }

        private Dictionary<string, object> data = new Dictionary<string, object>();

        // TODO: Implement indexer
        public object this [string key]
        {
            get
            {
                return null;
            }
            set { }
        }

        public Metadata()
        {
        }

        public void Add(string key, object value)
        {
			
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            data.Add(key, value);
        }

        public void Add(Dictionary<string, object> metadata)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            if (!metadata.Any())
            {
                throw new ArgumentException("'metadata' argument is empty.");
            }

            foreach (var m in metadata)
            {
                data.Add(m.Key, m.Value);
            }
        }

        public void Add(string key, Metadata.RichLink richLink)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (richLink == null)
            {
                throw new ArgumentException("'richLink' argument is null.");
            }

            data.Add(key, richLink);
        }

        public void Add(string key, Metadata.MonetaryAmount monetaryAmount)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (monetaryAmount == null)
            {
                throw new ArgumentException("'monetaryAmount' argument is null.");
            }

            data.Add(key, monetaryAmount);
        }

        public Dictionary<string, object> GetMetadata()
        {
            Dictionary<string, object> result = new Dictionary<string, object>(data.Count, data.Comparer);

            foreach (KeyValuePair<string, object> entry in data)
                result.Add(entry.Key, entry.Value is ICloneable ? ((ICloneable)entry.Value).Clone() : entry.Value);

            return result;
        }

        public object GetMetadata(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            object result = null;

            if (data.ContainsKey(key))
            {
                result = data[key];
            }
			
            return result;
        }

        public MonetaryAmount GetMonetaryAmount(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            MonetaryAmount result = null;

            if (data.ContainsKey(key) && data[key] is MonetaryAmount)
            {
                result = ((ICloneable)data[key]).Clone() as MonetaryAmount;
            }

            return result;
        }

        public List<MonetaryAmount> GetMonetaryAmounts()
        {
            List<MonetaryAmount> result = new List<MonetaryAmount>();

            foreach (var d in data)
            {
                if (d.Value is MonetaryAmount)
                    result.Add(((ICloneable)d.Value).Clone() as MonetaryAmount);
            }

            return result;
        }

        public RichLink GetRichLink(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            RichLink result = null;

            if (data.ContainsKey(key) && data[key] is RichLink)
            {
                result = ((ICloneable)data[key]).Clone() as RichLink;
            }

            return result;
        }

        public List<RichLink> GetRichLinks()
        {
            List<RichLink> result = new List<RichLink>();

            foreach (var d in data)
            {
                if (d.Value is RichLink)
                    result.Add(((ICloneable)d.Value).Clone() as RichLink);
            }

            return result;		
        }
    }
}