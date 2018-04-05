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

    #region Enumerations
    public enum SelectionMode
    {
        Normal = 0,
        OnDemandOff = 1,
        OnDemandOn = 2
    }

    public enum SelectionType
    {
        Static,
        Dynamic
    }
    #endregion

    public class SerTask
    {
        #region Properties
        [JsonProperty(nameof(General))]
        public SerGeneral General { get; set; } = new SerGeneral();

        [JsonProperty(nameof(Template))]
        public SerTemplate Template { get; set; } = new SerTemplate();

        [JsonProperty(nameof(Distribute))]
        public JObject Distribute { get; set; }

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
        public SelectionMode UseUserSelections { get; set; } = SelectionMode.Normal;

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

        [JsonProperty(nameof(Selections))]
        public List<SerSenseSelection> Selections { get; private set; }

        [JsonIgnore]
        public bool Generated { get; set; } = false;
        #endregion

        #region Public Methods
        public List<SerSenseSelection> GetDynamicFields()
        {
            return Selections?.Where(f => f.Type == SelectionType.Dynamic).ToList();
        }

        public List<SerSenseSelection> GetStaticFields()
        {
            return Selections?.Where(f => f.Type == SelectionType.Static).ToList();
        }
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

        [JsonProperty(nameof(SslVerify))]
        public bool SslVerify { get; set; } = true;

        [JsonProperty(nameof(SharedSession))]
        public bool SharedSession { get; set; }

        [JsonProperty(nameof(SslValidThumbprints))]
        public List<SerThumbprint> SslValidThumbprints { get; set; }

        [JsonProperty(nameof(Credentials))]
        public SerCredentials Credentials { get; set; }

        [JsonProperty(nameof(Lefs))]
        public List<string> Lefs { get; set; }

        [JsonProperty(nameof(Lef))]
        public string Lef { get; set; }

        #endregion

        public override string ToString()
        {
            return $"{ConnectUri}-{App}";
        }
    }

    public class SerThumbprint
    {
        [JsonProperty(nameof(Url))]
        public string Url { get; set; }

        [JsonProperty(nameof(Thumbprint))]
        public string Thumbprint { get; set; }
    }

    public class SerSenseSelection
    {
        #region Properties
        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        [JsonProperty(nameof(ObjectType))]
        public string ObjectType { get; set; }

        [JsonProperty(nameof(Value))]
        public string Value { get; private set; }

        [JsonProperty(nameof(Values))]
        public List<string> Values { get; private set; }

        [JsonProperty(nameof(Type))]
        public SelectionType Type { get; private set; }
        #endregion
    }
}