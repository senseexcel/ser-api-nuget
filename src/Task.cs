#region License
/*
Copyright (c) 2018 Konrad Mattheis und Martin Berthold
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
#endregion

namespace SerApi
{
    #region Usings
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hjson;
    using System.IO;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json.Converters;
    #endregion

    public class SerTask
    {
        #region Properties
        [JsonProperty(nameof(General))]
        public SerGeneral General { get; set; } = new SerGeneral();

        [JsonProperty(nameof(Template))]
        public SerTemplate Template { get; set; } = new SerTemplate();

        [JsonProperty(nameof(Evaluate))]
        public JObject Evaluate { get; set; }

        [JsonProperty(nameof(Connection))]
        public SerConnection Connection { get; set; } = new SerConnection();
        #endregion 
    }

    public class SerGeneral
    {
        #region Properties
        [JsonProperty(nameof(Timeout))]
        public int Timeout { get; set; } = 300;

        [JsonProperty(nameof(ErrorRepeatCount))]
        public int ErrorRepeatCount { get; set; } = 3;

        [JsonProperty(nameof(UseSandbox))]
        public bool UseSandbox { get; set; } = true;

        [JsonProperty(nameof(UseUserSelections))]
        public int UseUserSelections { get; set; } = 0;

        [JsonProperty(nameof(TaskCount))]
        public int TaskCount { get; set; } = -1;
        #endregion
    }

    public class SerTemplate
    {
        #region Properties
        [JsonProperty(nameof(Input))]
        public string Input { get; set; }

        [JsonProperty(nameof(Output))]
        public string Output { get; set; }

        [JsonProperty(nameof(OutputFormat))]
        public string OutputFormat { get; set; }

        [JsonProperty(nameof(OutputPassword))]
        public string OutputPassword { get; set; }

        [JsonProperty(nameof(KeepFormula))]
        public bool KeepFormula { get; set; } = false;

        [JsonProperty(nameof(ScriptKeys))]
        public List<string> ScriptKeys { get; set; } = new List<string>();

        [JsonProperty(nameof(ScriptArgs))]
        public List<string> ScriptArgs { get; set; } = new List<string>();

        [JsonProperty(nameof(Selection))]
        public SerSenseSelection Selection { get; set; }
        #endregion
    }

    public class SerCredentials
    {
        #region Properties
        [JsonProperty(nameof(Type)), JsonConverter(typeof(StringEnumConverter))]
        public QlikCredentialType Type { get; set; }

        [JsonProperty(nameof(Key))]
        public string Key { get; set; }

        [JsonProperty(nameof(Value))]
        public string Value { get; set; }

        //entweder CERT als PEM String oder Pfad zum File (Beides)
        [JsonProperty(nameof(Cert))]
        public string Cert { get; set; }

        [JsonProperty(nameof(PrivateKey))]
        public string PrivateKey { get; set; }
        #endregion
    }

    public class SerConnection
    {
        #region Properties
        [JsonProperty(nameof(ConnectUri))]
        public string ConnectUri { get; set; }

        [JsonProperty(nameof(VirtualProxyPath))]
        public string VirtualProxyPath { get; set; }

        [JsonProperty(nameof(App))]
        public string App { get; set; }

        [JsonProperty(nameof(Credentials))]
        public SerCredentials Credentials { get; set; }
        #endregion

        public override string ToString()
        {
            return $"{ConnectUri}-{App}";
        }
    }

    public class SerSenseSelection
    {
        #region Properties
        [JsonProperty(nameof(Fields))]
        public List<SerSenseField> Fields { get; set; } = new List<SerSenseField>();

        //Unbekannt prüfen
        [JsonProperty(nameof(Generated))]
        public bool Generated { get; set; } = false;
        #endregion
    }

    public class SerSenseField
    {
        #region Enums
        public enum FieldType
        {
            Static,
            Dynamic
        }
        #endregion

        #region Properties
        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        [JsonProperty(nameof(Values))]
        public List<string> Values { get; set; } = new List<string>();

        [JsonProperty(nameof(AllValues))]
        public bool AllValues { get; set; }

        [JsonProperty(nameof(Type))]
        public FieldType Type { get; set; }
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}