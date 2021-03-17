using System;
using System.Collections.Generic;
using Intercom.Data;
using Intercom.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Intercom.Converters.ClassConverters
{
    public class ConversationAdminCountJsonConverter: JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ConversationAdminCount);
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
                JArray result = j["conversation"]["admin"] as JArray;
                List<ConversationAdminCount.AdminCount> admins = new List<ConversationAdminCount.AdminCount>();

                foreach (var r in result)
                {
                    int open = 0;
                    int closed = 0;
                    int.TryParse(r["open"].Value<string>(), out open);
                    int.TryParse(r["closed"].Value<string>(), out closed);

                    admins.Add(new ConversationAdminCount.AdminCount()
                        { 
                            id = r["id"].Value<string>(),
                            name = r["name"].Value<string>(),
                            open = open,
                            closed = closed
                        });
                }

                return new ConversationAdminCount() { admins = admins };
            }
            catch (Exception ex)
            {
                throw new JsonConverterException("Error while serializing ConversationAdminCount endpoint json result.", ex)
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