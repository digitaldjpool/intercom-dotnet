using System;
using System.Collections.Generic;
using Intercom.Data;
using Intercom.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Intercom.Converters.AttributeConverters
{
    public class MetadataJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(User);
        }

        public override object ReadJson(JsonReader reader, 
                                        Type objectType, 
                                        object existingValue, 
                                        JsonSerializer serializer)
        {
            JObject jobject = null;

            try
            {
                jobject = JObject.Load(reader);
                Metadata result = new Metadata();

                foreach (var j in jobject)
                {
                    if (j.Value is JObject)
                    {
					
                        JObject complex = j.Value as JObject;

                        if (complex["url"] != null && complex["value"] != null)
                        {
                            result.Add(j.Key, new Metadata.RichLink(complex["url"].Value<string>(), complex["value"].Value<string>()));
                        }
                        else if (complex["amount"] != null && complex["currency"] != null)
                        {
                            int amount = 0;
                            int.TryParse(complex["amount"].ToString(), out amount);

                            result.Add(j.Key, new Metadata.MonetaryAmount(amount, complex["currency"].ToString()));
                        }
                    }
                    else
                    {
                        result.Add(j.Key, j.Value.Value<string>());
                    }
                }

                return result;

            }
            catch (Exception ex)
            {
                throw new JsonConverterException("Error while serializing AppCount endpoint json result.", ex)
                { 
                    Json = jobject == null ? string.Empty : jobject.ToString(),
                    SerializationType = objectType.FullName
                };
            }
        }

        public override void WriteJson(JsonWriter writer, 
                                       object value,
                                       JsonSerializer serializer)
        {
            Metadata metadata = value as Metadata;
            Dictionary<string, object> metadataDictionary = metadata.GetMetadata();

            string s = JsonConvert.SerializeObject(metadataDictionary,
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