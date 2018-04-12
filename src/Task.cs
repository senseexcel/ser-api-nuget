﻿#region License
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
    using NLog;
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
        public SerGeneral General { get; set; } = new SerGeneral();

        [JsonProperty(Required = Required.Always)]
        public SerTemplate Template { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public JObject Distribute { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SerConnection Connection { get; set; }
        #endregion 
    }

    public class SerGeneral
    {
        #region Properties
        [JsonProperty]
        [DefaultValue(300)]
        public int Timeout { get; set; } = 300;

        [JsonProperty]
        [DefaultValue(2)]
        public int ErrorRepeatCount { get; set; } = 2;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(true)]
        public bool UseSandbox { get; set; } = true;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(1)]
        public int TaskCount { get; set; } = 1;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore), 
         JsonConverter(typeof(StringEnumConverter))]
        [DefaultValue(SelectionMode.Normal)]
        public SelectionMode UseUserSelections { get; set; } = SelectionMode.Normal;
        #endregion
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SerTemplate
    {
        #region Properties
        [JsonProperty(Required = Required.Always)]
        public string Input { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Output { get; set; }

        [JsonProperty]
        public string OutputFormat { get; set; }

        [JsonProperty]
        public string OutputPassword { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(false)]
        public bool KeepFormula { get; set; } = false;

        [JsonProperty]
        public List<string> ScriptKeys { get; set; }

        [JsonProperty]
        public List<string> ScriptArgs { get; set; }

        [JsonProperty, JsonConverter(typeof(SingleValueArrayConverter))]
        public List<SerSenseSelection> Selections { get; set; }

        [JsonIgnore]
        public bool Generated { get; set; } = false;
        #endregion

        #region Public Methods
        public List<SerSenseSelection> GetSelectionObjects(SelectionType type)
        {
            try
            {
                return Selections?.Where(f => f.Type == type).ToList() ?? new List<SerSenseSelection>();
            }
            catch
            {
                return new List<SerSenseSelection>();
            }
        }
        #endregion
    }

    [JsonObject(ItemNullValueHandling=NullValueHandling.Ignore)]
    public class SerCredentials
    {
        #region Properties
        [JsonProperty, JsonConverter(typeof(StringEnumConverter))]
        public QlikCredentialType Type { get; set; }

        [JsonProperty]
        public string Key { get; set; }

        [JsonProperty]
        public string Value { get; set; }

        [JsonProperty]
        public string Cert { get; set; }

        [JsonProperty]
        public string PrivateKey { get; set; }
        #endregion
    }

    public class SerConnection
    {
        #region Logger
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Variables && Properties
        private Uri privateURI;

        [JsonProperty(Required = Required.Always)]
        public Uri ServerUri
        {
            get
            {
                try
                {
                    var newUri = new UriBuilder(privateURI);
                    switch (newUri.Scheme.ToLowerInvariant())
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
                catch (Exception ex)
                {
                    logger.Error(ex, $"The server uri {privateURI} is invalid.");
                    return null;
                }
            }
            set
            {
                if (value != privateURI)
                    privateURI = value;
            }
        }

        [JsonProperty]
        public string App { get; set; }

        [JsonProperty]
        [DefaultValue(true)]
        public bool SslVerify { get; set; } = true;

        [JsonProperty]
        public List<SerThumbprint> SslValidThumbprints { get; set; }

        [JsonProperty]
        [DefaultValue(false)]
        public bool SharedSession { get; set; } = false;

        [JsonProperty]
        public SerCredentials Credentials { get; set; }

        [JsonProperty, JsonConverter(typeof(SingleValueArrayConverter))]
        public List<string> Lefs { get; set; }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return $"{ServerUri}-{App}";
        }
        #endregion
    }

    public class SerThumbprint
    {
        #region Properties
        [JsonProperty(Required = Required.Always)]
        public string Url { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Thumbprint { get; set; }
        #endregion
    }

    public class SerSenseSelection
    {
        #region Properties
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string ObjectType { get; set; }

        [JsonProperty, JsonConverter(typeof(SingleValueArrayConverter))]
        public List<string> Values { get; set; }

        [JsonProperty, JsonConverter(typeof(StringEnumConverter))]
        public SelectionType Type { get; set; }
        #endregion
    }
}