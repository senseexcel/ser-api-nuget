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
    using System.ComponentModel;
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

        [JsonProperty(nameof(Distribute), DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(null)]
        public JObject Distribute { get; set; }

        [JsonProperty(nameof(Connection))]
        public SerConnection Connection { get; set; }
        #endregion 
    }

    public class SerGeneral
    {
        #region Properties
        [JsonProperty(nameof(Timeout))]
        [DefaultValue(300)]
        public int Timeout { get; set; } = 300;

        [JsonProperty(nameof(ErrorRepeatCount))]
        [DefaultValue(2)]
        public int ErrorRepeatCount { get; set; } = 2;

        [JsonProperty(nameof(UseSandbox), DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(true)]
        public bool UseSandbox { get; set; } = true;

        [JsonProperty(nameof(TaskCount), DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(1)]
        public int TaskCount { get; set; } = 1;

        [JsonProperty(nameof(UseUserSelections), DefaultValueHandling = DefaultValueHandling.Ignore), 
         JsonConverter(typeof(StringEnumConverter))]
        [DefaultValue(SelectionMode.Normal)]
        public SelectionMode UseUserSelections { get; set; } = SelectionMode.Normal;
        #endregion
    }

    public class SerTemplate
    {
        #region Properties
        [JsonProperty(nameof(Input))]
        public string Input { get; set; }

        [JsonProperty(nameof(Output))]
        public string Output { get; set; }

        [JsonProperty(nameof(OutputFormat), DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(null)]
        public string OutputFormat { get; set; }

        [JsonProperty(nameof(OutputPassword), DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(null)]
        public string OutputPassword { get; set; }

        [JsonProperty(nameof(KeepFormula), DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(false)]
        public bool KeepFormula { get; set; } = false;

        [JsonProperty(nameof(ScriptKeys), DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(null)]
        public List<string> ScriptKeys { get; set; }

        [JsonProperty(nameof(ScriptArgs), DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(null)]
        public List<string> ScriptArgs { get; set; }

        [JsonProperty(nameof(Selections), DefaultValueHandling = DefaultValueHandling.Ignore),
         JsonConverter(typeof(SettingsConverter))]
        [DefaultValue(null)]
        public List<SerSenseSelection> Selections { get; set; }

        [JsonIgnore]
        public bool Generated { get; set; } = false;
        #endregion

        #region Public Methods
        public List<SerSenseSelection> GetSelectionObjects(SelectionType type)
        {
            return Selections?.Where(f => f.Type == type).ToList();
        }
        #endregion
    }

    public class SerCredentials
    {
        #region Properties
        [JsonProperty(nameof(Type)), JsonConverter(typeof(StringEnumConverter))]
        public QlikCredentialType Type { get; set; }

        [JsonProperty(nameof(Key))]
        [DefaultValue(null)]
        public string Key { get; set; }

        [JsonProperty(nameof(Value))]
        [DefaultValue(null)]
        public string Value { get; set; }

        //entweder CERT als PEM String oder Pfad zum File (Beides)
        [JsonProperty(nameof(Cert))]
        [DefaultValue(null)]
        public string Cert { get; set; }

        [JsonProperty(nameof(PrivateKey))]
        [DefaultValue(null)]
        public string PrivateKey { get; set; }
        #endregion
    }

    public class SerConnection
    {
        #region Properties
        [JsonProperty(nameof(ConnectUri))]
        public string ConnectUri { get; set; }

        //Weg?? alles über connuri
        [JsonProperty(nameof(VirtualProxyPath))]
        public string VirtualProxyPath { get; set; }

        [JsonIgnore]
        public Uri ServerUri
        {
            get
            {
                var newUri = new UriBuilder(ConnectUri);
                if (!String.IsNullOrEmpty(VirtualProxyPath))
                    newUri.Path = VirtualProxyPath;

                switch(newUri.Scheme.ToLowerInvariant())
                {
                    case "ws":
                        newUri.Scheme = "http";
                        break;
                    case "wss":
                        newUri.Scheme = "https";
                        break;
                }

                return newUri.Uri;
            }
        }

        [JsonProperty(nameof(App))]
        public string App { get; set; }

        [JsonProperty(nameof(SslVerify))]
        [DefaultValue(true)]
        public bool SslVerify { get; set; } = true;

        [JsonProperty(nameof(SslValidThumbprints))]
        [DefaultValue(null)]
        public List<SerThumbprint> SslValidThumbprints { get; set; }

        [JsonProperty(nameof(SharedSession))]
        [DefaultValue(false)]
        public bool SharedSession { get; set; } = false;

        [JsonProperty(nameof(Credentials))]
        [DefaultValue(null)]
        public SerCredentials Credentials { get; set; }

        [JsonProperty(nameof(Lefs)), JsonConverter(typeof(SettingsConverter))]
        [DefaultValue(null)]
        public List<string> Lefs { get; set; }
        #endregion

        public override string ToString()
        {
            return $"{ConnectUri}-{App}";
        }
    }

    public class SerThumbprint
    {
        [JsonProperty(nameof(Url))]
        [DefaultValue(null)]
        public string Url { get; set; }

        [JsonProperty(nameof(Thumbprint))]
        [DefaultValue(null)]
        public string Thumbprint { get; set; }
    }

    public class SerSenseSelection
    {
        #region Properties
        [JsonProperty(nameof(Name))]
        [DefaultValue(null)]
        public string Name { get; set; }

        [JsonProperty(nameof(ObjectType))]
        public string ObjectType { get; set; }

        [JsonProperty(nameof(Values)), JsonConverter(typeof(SettingsConverter))]
        [DefaultValue(null)]
        public List<string> Values { get; set; }

        [JsonProperty(nameof(Type)), JsonConverter(typeof(StringEnumConverter))]
        public SelectionType Type { get; set; }
        #endregion
    }
}