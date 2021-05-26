namespace Ser.Api.JsonConverters
{
    #region Usings
    using System;
    using System.Text;
    using Newtonsoft.Json;
    #endregion

    /// <summary>
    /// Json Encoding Converter
    /// </summary>
    public class JsonEncodingConverter : JsonConverter
    {
        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The JsonReader to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns></returns>
        /// <see href="https://www.newtonsoft.com/json/help/html/M_Newtonsoft_Json_JsonConverter_ReadJson.htm"/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (existingValue == null)
                existingValue = reader.Value;
            return Encoding.GetEncoding(existingValue.ToString());
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The JsonWriter to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <see href="https://www.newtonsoft.com/json/help/html/M_Newtonsoft_Json_JsonConverter_WriteJson.htm"/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, (value as Encoding).BodyName);
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified object type, otherwise false.</returns>
        /// <see href="https://www.newtonsoft.com/json/help/html/M_Newtonsoft_Json_JsonConverter_CanConvert.htm"/>
        public override bool CanConvert(Type objectType)
        {
            return (typeof(Encoding).IsAssignableFrom(objectType));
        }
    }
}
