namespace XUnitJsonTest
{
    #region Usings
    using Hjson;
    using Newtonsoft.Json;
    using SerApi;
    using System;
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

        [Fact(DisplayName = "ReadArrays")]
        public void ReadArrays()
        {
            var json = GetHJsonPath("arrays.hjson");
            var result = JsonConvert.DeserializeObject<Test1>(json);
            Assert.True(result.SingleValues.Length == 1);
            Assert.True(result.MultipleValues.Length == 2);
        }

        [Fact(DisplayName = "SerializeTaskConfig")]
        public void SerializeTaskConfig()
        {
            var serConfig = new SerConfig();
            var result = JsonConvert.SerializeObject(serConfig);
            
        }
    }
}