using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Stubble.Core;
using Stubble.Core.Builders;
using Stubble.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FRW.TR.Commun.Utils
{

    public class ConvertisseurFRW : CustomCreationConverter<IDictionary<object, object>>
    {
        public override IDictionary<object, object> Create(Type objectType)
        {
            return new Dictionary<object, object>();
        }

        public override bool CanConvert(Type objectType)
        {
            // in addition to handling IDictionary<string, object>
            // we want to handle the deserialization of dict value
            // which is of type object
            return objectType == typeof(object) || base.CanConvert(objectType);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject
                || reader.TokenType == JsonToken.Null)
                return base.ReadJson(reader, objectType, existingValue, serializer);

            // if the next token is not an object
            // then fall back on standard deserializer (strings, numbers etc.)

            /*if (reader.TokenType == JsonToken.String && reader.Value.Equals("true"))
            {
                return serializer.Deserialize<bool>(reader);
            }*/

            if (reader.TokenType == JsonToken.StartArray)
            {
                return serializer.Deserialize<object[]>(reader);
            }

            return serializer.Deserialize(reader);
        }
    }
}