using Newtonsoft.Json;
using Ser.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitJsonTest
{
    public class Test1
    {
        [JsonConverter(typeof(SingleValueArrayConverter))]
        public Test2[] SingleValues { get; set; }

        [JsonConverter(typeof(SingleValueArrayConverter))]
        public Test2[] MultipleValues { get; set; }
    }

    public class Test2
    {
        public string Url { get; set; }
        public string Server { get; set; }
        public Test3 Info { get; set; }
    }

    public class Test3
    {
        public int Count { get; set; }
        public bool HasCount { get; set; }
    }
}
