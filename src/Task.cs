#region License
/*
Copyright (c) 2018 Konrad Mattheis und Martin Berthold
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion

namespace Ser.Api
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
    using Newtonsoft.Json.Serialization;
    #endregion

    #region Enumerations
    /// <summary>
    /// The type of the selection
    /// </summary>
#if NET45
    [Reinforced.Typings.Attributes.TsEnum()]
#endif
    public enum SelectionType
    {
        /// <summary>
        /// Use a fixed filter name or bookmark.
        /// </summary>
        Static,

        /// <summary>
        /// Loop over a field or dimension.
        /// </summary>
        Dynamic
    }
    #endregion

    /// <summary>
    /// The full task object and configuration.
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
#if NET45
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public class SerTask
    {
        #region Properties
        /// <summary>
        /// The reports to be generating.
        /// </summary>
        [JsonProperty]
        public List<SerReport> Reports { get; set; } = new List<SerReport>();
        #endregion
    }

    /// <summary>
    /// The report of the ser task.
    /// </summary>
    /// <seealso cref="SerTask"/>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
#if NET45
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public class SerReport
    {
        #region Properties   
        /// <summary>
        /// The general setting of a report.
        /// </summary>
        public SerGeneral General { get; set; } = new SerGeneral();

        /// <summary>
        /// The template of a report.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public SerTemplate Template { get; set; }

        /// <summary>
        /// This property includes the distribute options.
        /// It is a json structure.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
#if NET45
        [Reinforced.Typings.Attributes.TsIgnore]
#endif
        public JObject Distribute { get; set; }

        /// <summary>
        /// The available connections to Qlik.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore),
         JsonConverter(typeof(SingleValueArrayConverter))]
        public List<SerConnection> Connections { get; set; }
        #endregion
    }

    /// <summary>
    /// The general settings for the report.
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
#if NET45
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public class SerGeneral
    {
        #region Properties
        /// <summary>
        /// The time after clean up the temp folder.
        /// </summary>
        [JsonProperty]
        [DefaultValue(10)]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public int CleanupTimeOut { get; set; } = 10;

        /// <summary>
        /// The time until the report is aborted.
        /// </summary>
        [JsonProperty]
        [DefaultValue(900)]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public int Timeout { get; set; } = 900;

        /// <summary>
        /// The repeat until the report is canceled as error.
        /// </summary>
        [JsonProperty]
        [DefaultValue(2)]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public int ErrorRepeatCount { get; set; } = 2;

        /// <summary>
        /// Use the sand box for script execute.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(true)]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public bool UseSandbox { get; set; } = true;

        /// <summary>
        /// The count of cpus or cpu cores for the ser engine.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(-1)]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public int CPULimitInCore { get; set; } = -1;

        /// <summary>
        /// The soft memory limit for the ser engine.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(-1.0)]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public double MemoryLimitInGB { get; set; } = -1.0;
        #endregion
    }

    /// <summary>
    /// The template for the report.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore,
                NamingStrategyType = typeof(CamelCaseNamingStrategy))]
#if NET45
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public class SerTemplate
    {
        #region Properties
        /// <summary>
        /// The Input path of the template.
        /// You can use the protokoll 'content' or 'lib'
        /// Samples: 
        /// Use 'content:///[TEMPLATE]' for InApp template
        /// Use 'content://[CONTENTLIBARY]/[TEMPLATE]' for a content libary.
        /// Use 'lib://[LIB-FOLDER]/[TEMPLATE]' for a special folder.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Input { get; set; }

        /// <summary>
        /// The output name of the report.
        /// Note:
        /// You can use the output name with or without file extention.
        /// If you use the output name without a file extension, you must also use the property 'Outputformat'.
        /// If you do not specify a file extension, PDF will be used automatically.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Output { get; set; }

        /// <summary>
        /// The file extention for the output name.
        /// It is optional.
        /// </summary>
        /// <seealso cref="SerTemplate.Output"/>
        [JsonProperty]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public string OutputFormat { get; set; }

        /// <summary>
        /// The password what should be set for the reports. 
        /// Note:
        /// It only works for PDF and XLSX files.
        /// If you not want the password remove it in the json.
        /// Please do not leave blank, because the human json convention can not handle it.
        /// </summary>
        [JsonProperty]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public string OutputPassword { get; set; }

        /// <summary>
        /// Flexible output format options.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
#if NET45
        [Reinforced.Typings.Attributes.TsIgnore]
#endif
        public JObject OutputFormatOptions { get; set; }

        /// <summary>
        /// Save the formula as json in the template
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(false)]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public bool KeepFormula { get; set; } = false;

        /// <summary>
        /// The signature for the script executing.
        /// </summary>
        [JsonProperty]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public List<string> ScriptKeys { get; set; }

        /// <summary>
        /// The arguments for the executing .NET scripts
        /// </summary>
        [JsonProperty]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public List<string> ScriptArgs { get; set; }

        /// <summary>
        /// The qlik filters used for the report.
        /// </summary>
        [JsonProperty, JsonConverter(typeof(SingleValueArrayConverter))]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public List<SerSenseSelection> Selections { get; set; }

        /// <summary>
        /// Clear or clear not the Qlik selections in a session.
        /// This mode is importent for the OnDemand Extention.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(true)]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public bool SelectionsClearAll { get; set; } = true;
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the selections of a special type.
        /// </summary>
        /// <param name="type">The type of a selection.</param>
        /// <returns>A list of selections</returns>
#if NET45
        [Reinforced.Typings.Attributes.TsIgnore]
#endif
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

    /// <summary>
    /// The credentials for the report.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore,
                NamingStrategyType = typeof(CamelCaseNamingStrategy))]
#if NET45
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public class SerCredentials
    {
        #region Properties
        /// <summary>
        /// The credential type for the connection.
        /// </summary>
        [JsonProperty, JsonConverter(typeof(StringEnumConverter))]
        public QlikCredentialType Type { get; set; }

        /// <summary>
        /// The name of the cookie.
        /// </summary>
        [JsonProperty]
        public string Key { get; set; }

        /// <summary>
        /// The Value of the cookie.
        /// </summary>
        [JsonProperty]
        public string Value { get; set; }

        /// <summary>
        /// The path to the JWT certificate.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public string Cert { get; set; }

        /// <summary>
        /// The path to the private key.
        /// It's generate the JWT token.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public string PrivateKey { get; set; }
        #endregion
    }

    /// <summary>
    /// The connection class of the report.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore,
                NamingStrategyType = typeof(CamelCaseNamingStrategy))]
#if NET45
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public class SerConnection
    {
        #region Logger
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Variables && Properties
        private Uri privateURI;

        /// <summary>
        /// The Connection uri to Qlik.
        /// Note:
        /// Desktop: "Qlik Sense Desktop"
        /// Server: "https://NAME_OF_SERVER/VIRTUALPROXY"
        /// </summary>
        [JsonProperty(Required = Required.Always)]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable= true, Type="string")]        
#endif
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
                {
                    privateURI = value;
                    var uri = privateURI.OriginalString.TrimEnd('/');
                    privateURI = new Uri(uri);
                }
            }
        }

        /// <summary>
        /// Qlik app name or id
        /// </summary>
        [JsonProperty]
        public string App { get; set; }

        /// <summary>
        /// The reload timeout for InAppMode.
        /// Wait for data reload of the app.
        /// </summary>
        [JsonProperty]
        public int ReloadTimeout { get; set; }

        /// <summary>
        /// Use the ssl validation
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(true)]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public bool SslVerify { get; set; } = true;

        /// <summary>
        /// The thumb print from qlik client ssl certificate.
        /// </summary>
        [JsonProperty]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public List<SerThumbprint> SslValidThumbprints { get; set; }

        /// <summary>
        /// Share the qlik session or create a new session with a identity
        /// </summary>
        [JsonProperty, JsonConverter(typeof(SingleValueArrayConverter))]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public List<string> Identities { get; set; }

        /// <summary>
        /// The credentials for the qlik communication.
        /// </summary>
        [JsonProperty]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public SerCredentials Credentials { get; set; }

        /// <summary>
        /// The lisence keys
        /// </summary>
        [JsonProperty, JsonConverter(typeof(SingleValueArrayConverter))]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public List<string> Lefs { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get the server uri.
        /// </summary>
        /// <returns></returns>
#if NET45
        [Reinforced.Typings.Attributes.TsIgnore]
#endif
        public override string ToString()
        {
            return $"{ServerUri}";
        }
        #endregion
    }

    /// <summary>
    /// The thumb print for the report.
    /// Note:
    /// Copy the thumb print from thw qlik client certificate.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore,
                NamingStrategyType = typeof(CamelCaseNamingStrategy))]
#if NET45
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public class SerThumbprint
    {
        #region Properties
        /// <summary>
        /// The server url to the qlilk certificate
        /// </summary>
        [JsonProperty]
        public string Url { get; set; }

        /// <summary>
        /// The thumb print from the qlik clint certificate
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Thumbprint { get; set; }
        #endregion
    }

    /// <summary>
    /// The selection of the report.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore,
                NamingStrategyType = typeof(CamelCaseNamingStrategy))]
#if NET45
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public class SerSenseSelection
    {
        #region Properties
        /// <summary>
        /// The Name of the filter.
        /// </summary>
        [JsonProperty]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public string Name { get; set; }

        /// <summary>
        /// Type of the filter type.
        /// By Default it will used 'Field'.
        /// Other Values are 'bookmark' and 'hiddenbookmark'
        /// </summary>
        [JsonProperty]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public string ObjectType { get; set; }

        /// <summary>
        /// The values that should be used.
        /// </summary>
        [JsonProperty, JsonConverter(typeof(SingleValueArrayConverter))]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public List<string> Values { get; set; }

        /// <summary>
        /// Type of the selection.
        /// </summary>
        [JsonProperty, JsonConverter(typeof(StringEnumConverter))]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public SelectionType Type { get; set; }

        /// <summary>
        /// Export a Root node for the sheet names.
        /// </summary>
        [JsonProperty]
        [DefaultValue(true)]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public bool ExportRootNode { get; private set; } = true;

        /// <summary>
        /// Give the sheet a seperate sheet name form a formaula
        /// You can also use @@sheetname@@ as placeholter for the orignal sheet name.
        /// </summary>
        [JsonProperty]
#if NET45
        [Reinforced.Typings.Attributes.TsProperty(ForceNullable=true)]
#endif
        public string SheetName { get; private set; }
        #endregion
    }
}