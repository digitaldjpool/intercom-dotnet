using System;
using Intercom.Data;
using Intercom.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Intercom.Converters.ClassConverters
{
    public class AppCountJsonConverter: JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(AppCount);
        }

        public override object ReadJson(JsonReader reader, 
                                        Type objectType, 
                                        object existingValue,
                                        JsonSerializer serializer)
        {
            JObject j = null;

            try
            {
                j = JObject.Load(reader);
                int companyCount = j["company"].Value<int>("count");
                int segmentCount = j["segment"].Value<int>("count");
                int userCount = j["user"].Value<int>("count");
                int tagCount = j["tag"].Value<int>("count");
                int contactCount = j["lead"].Value<int>("count");

                return new AppCount()
                { 
                    company = companyCount, 
                    segment = segmentCount, 
                    tag = tagCount, 
                    user = userCount ,
                    lead = contactCount
                };
            }
            catch (Exception ex)
            {
                throw new JsonConverterException("Error while serializing AppCount endpoint json result.", ex)
                { 
                    Json = j == null ? string.Empty : j.ToString(),
                    SerializationType = objectType.FullName
                };
            }
        }

        public override void WriteJson(JsonWriter writer, 
                                       object value,
                                       JsonSerializer serializer)
        {
            string s = JsonConvert.SerializeObject(value,
                           Formatting.None,
                           new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            writer.WriteRawValue(s);
        }

        public override bool CanRead
        {
            get { return true; }
        }
    }
}