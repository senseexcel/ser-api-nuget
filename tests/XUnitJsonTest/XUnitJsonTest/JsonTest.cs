using Hjson;
using Newtonsoft.Json;
using SerApi;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace XUnitJsonTest
{
    public class JsonTest
    {
        [Fact]
        public void TestDefaultValues()
        {
            try
            {
                var converter = new SettingsConverter();
                var jsonSettings = new JsonSerializerSettings();
                //jsonSettings.Converters.Add(converter);

                var sampleJson = HjsonValue.Load(@"C:\Users\MBerthold\Documents\Entwicklung\Projects\SenseExcelReporting\ser-api-nuget\docs\examples\valueSingle.hjson");
                var json = sampleJson.ToString(Stringify.Formatted);
                var result = JsonConvert.DeserializeObject<Test>(json, jsonSettings);
            }
            catch (Exception ex)
            {
                var dd = ex.ToString();
            }
        }
    }

    public class Test
    {
        public bool Active { get; set; }
        [JsonConverter(typeof(SettingsConverter))]
        public List<Test2> Values { get; set; }
    }

    public class Test2
    {
        public string Url { get; set; }
        public string Server { get; set; }
    }

    public class SettingsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(string)) || (objectType == typeof(List<string>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                var result = new List<string>();
                reader.Read();
                while (reader.TokenType != JsonToken.EndArray)
                {
                    result.Add(reader.Value as string);
                    reader.Read();
                }
                return result;
            }
            else if(reader.TokenType == JsonToken.StartObject)
            {
                return new List<Test2> { reader.Value as Test2 };
            }
            else
            {
                return new List<string> { reader.Value as string };
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //ToDo here we can decide to write the json as 
            //if only has one attribute output as string if it has more output as list
        }
    }
}
