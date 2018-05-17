namespace Ser.Api
{
    #region Usings
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class SingleValueArrayConverter : JsonConverter
    {
        public enum ObjectMode
        {
            Standard,
            Array,
            List
        }

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
            else if(reader.TokenType == JsonToken.Null)
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

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IEnumerable enumerable)
            {
                object singleItem = null;
                int itemCount = 0;
                foreach(var item in enumerable)
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
    }
}
