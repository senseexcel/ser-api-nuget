namespace XUnitJsonTest
{
    #region Usings
    using Hjson;
    using Newtonsoft.Json;
    using Ser.Api;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Xunit;
    #endregion

    public class JsonTest
    {
        private string GetHJsonPath(string filename)
        {
            var fullPath = Path.GetFullPath($"..\\..\\..\\..\\..\\..\\docs\\examples\\{filename}");
            var sampleJson = HjsonValue.Load(fullPath);
            return sampleJson.ToString(Stringify.Formatted);
        }

        [Fact(DisplayName = "ReadTaskConfigWithSingleSelection")]
        public void ReadTaskConfigWithSingleSelection()
        {
            var json = GetHJsonPath("selectionsSingle.hjson");
            var result = JsonConvert.DeserializeObject<SerTask>(json);
            Assert.True(result.Template.Selections.Count == 1);
        }

        [Fact(DisplayName = "ReadTaskConfigWithSelectionList")]
        public void ReadTaskConfigWithSelectionList()
        {
            var json = GetHJsonPath("selectionsList.hjson");
            var result = JsonConvert.DeserializeObject<SerTask>(json);
            Assert.True(result.Template.Selections.Count == 2);
        }

        [Fact(DisplayName = "DeserializeArrays")]
        public void DeserializeArrays()
        {
            var json = GetHJsonPath("arrays.hjson");
            var result = JsonConvert.DeserializeObject<Test1>(json);
            Assert.True(result.SingleValues.Length == 1);
            Assert.True(result.MultipleValues.Length == 2);
        }

        [Fact(DisplayName = "SerializeTaskConfig")]
        public void SerializeTaskConfig()
        {
            var serTask = new SerTask()
            {
                Template = new SerTemplate()
                {
                    Input = "Demo.pdf",
                    Output = "Demo",
                    Selections = new List<SerSenseSelection>()
                         {
                              new SerSenseSelection()
                              {
                                  Name = "Test",
                                  ObjectType = "field",
                                  Type =  SelectionType.Static,
                                  Values = new List<string>() {"Demo"}
                              }
                         }
                }
            };

            var json = JsonConvert.SerializeObject(serTask, Formatting.Indented);
            var result = JsonConvert.DeserializeObject<SerTask>(json);
            Assert.True(result.Template.Selections.Count == 1);
        }

        [Fact(DisplayName = "SerializeArrays")]
        public void SerializeArrays()
        {
            var value1 = new Test2()
            {
                Server = "http://test1",
                Url = "myserver1",
                Info = new Test3()
                {
                    Count = 22,
                    HasCount = true,
                }
            };

            var value2 = new Test2()
            {
                Server = "http://test2",
                Url = "myserver2",
                Info = new Test3()
                {
                    Count = 33,
                    HasCount = true,
                }
            };

            var testClass = new Test1
            {
                SingleValues = new Test2[] { value1 },
                MultipleValues = new Test2[2] { value1 , value2 }
            };            

            var json = JsonConvert.SerializeObject(testClass, Formatting.Indented);
            var result = JsonConvert.DeserializeObject<Test1>(json);
            Assert.True(result.SingleValues.Length == 1);
            Assert.True(result.MultipleValues.Length == 2);
        }
    }
}