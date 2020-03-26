namespace Ser.Api
{
    #region Usings
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    #endregion

    /// <summary>
    /// This converter converts a single object into an array item.
    /// </summary>
    public class SingleValueArrayConverter : JsonConverter
    {
        #region Enumerations
        /// <summary>
        /// The mode of the json object.
        /// </summary>
        public enum ObjectMode
        {
            /// <summary>
            /// No list and no array.
            /// </summary>
            Standard,

            /// <summary>
            /// It is a array.
            /// </summary>
            Array,

            /// <summary>
            /// It is a list.
            /// </summary>
            List
        }
        #endregion

        #region Private Methods
        private object ReadJsonInternal(Type innerType, Type objectType, JsonReader reader,
                                        JsonSerializer serializer, object retVal, ObjectMode mode)
        {
            if (reader.TokenType == JsonToken.StartObject ||
                reader.TokenType == JsonToken.String ||
                reader.TokenType == JsonToken.Boolean ||
                reader.TokenType == JsonToken.Date ||
                reader.TokenType == JsonToken.Bytes ||
                reader.TokenType == JsonToken.Float ||
                reader.TokenType == JsonToken.Integer)
            {
                var instance = serializer.Deserialize(reader, innerType);
                if (mode == ObjectMode.Array)
                {
                    var myArrayObject = Activator.CreateInstance(objectType, 1);
                    var array = myArrayObject as object[];
                    array[0] = instance;
                    retVal = array;
                }
                else if (mode == ObjectMode.List)
                {
                    var myListObject = Activator.CreateInstance(objectType);
                    var list = myListObject as IList;
                    list.Add(instance);
                    retVal = list;
                }
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                retVal = serializer.Deserialize(reader, objectType);
            }
            else if (reader.TokenType == JsonToken.Null)
            {
                retVal = null;
            }

            return retVal;
        }

        private bool HasGenericInterface(Type type, Type interf, Type typeparameter)
        {
            foreach (Type i in type.GetInterfaces())
                if (i.IsGenericType && i.GetGenericTypeDefinition() == interf)
                    if (i.GetGenericArguments().SingleOrDefault() == typeparameter)
                        return true;

            return false;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified object type, otherwise false.</returns>
        /// <see href="https://www.newtonsoft.com/json/help/html/M_Newtonsoft_Json_JsonConverter_CanConvert.htm"/>
        public override bool CanConvert(Type objectType)
        {
            var innerType = objectType.GetElementType();
            if (innerType == null)
                return false;
            if (HasGenericInterface(objectType, typeof(ICollection<>), innerType))
                return true;
            else if (objectType.IsArray)
                return true;
            return false;
        }

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
            var retVal = new object();
            var innerType = objectType.GetElementType();
            if (innerType == null)
                innerType = objectType.GetGenericArguments().SingleOrDefault();
            if (innerType == null)
                return retVal;

            var mode = ObjectMode.Standard;
            if (objectType.IsArray)
                mode = ObjectMode.Array;
            else if (HasGenericInterface(objectType, typeof(ICollection<>), innerType))
                mode = ObjectMode.List;

            return ReadJsonInternal(innerType, objectType, reader, serializer, retVal, mode);
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
            if (value is IEnumerable enumerable)
            {
                object singleItem = null;
                int itemCount = 0;
                foreach (var item in enumerable)
                {
                    itemCount++;
                    if (itemCount > 1)
                        break;
                    singleItem = item;
                }
                if (itemCount == 1)
                    value = singleItem;
            }

            serializer.Serialize(writer, value);
        }
        #endregion
    }
}