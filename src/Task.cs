namespace Ser.Api
{
    #region Usings
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hjson;
    using Newtonsoft.Json.Converters;
    using System.ComponentModel;
    using NLog;
    using Newtonsoft.Json.Serialization;
    #endregion

    #region Enumerations
    /// <summary>
    /// The type of the selection
    /// </summary>
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
    public class SerTask
    {
        #region Properties
        /// <summary>
        /// The GUID of the Task
        /// </summary>
        [JsonProperty]
        public Guid Id { get; set; }

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
        public SerTemplate Template { get; set; }

        /// <summary>
        /// This property includes the distribute options.
        /// It is a json structure.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
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
    public class SerGeneral
    {
        #region Properties
        /// <summary>
        /// Task aktivieren oder deaktivierung für Alerting (optional).
        /// </summary>
        [JsonProperty]
        public string Active { get; set; }

        /// <summary>
        /// The time after clean up the temp folder (optional).
        /// </summary>
        [JsonProperty]
        [DefaultValue(10)]
        public int CleanupTimeOut { get; set; } = 10;

        /// <summary>
        /// Auto fit the colunms and rows from tables and pivottables (optional).
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(false)]
        public bool AutoFitTables { get; set; }

        /// <summary>
        /// Disable dataSource linking by pivot tables (optional).
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(false)]
        public bool DisableAutoLink { get; set; }

        /// <summary>
        /// Disable new logic for advanced table columns or generated excel formulas from qlik (optional).
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(false)]
        public bool DisableAdvancedTableMode { get; set; }

        /// <summary>
        /// Disable new logic for advanced table columns or generated excel formulas from qlik (optional).
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(false)]
        public bool DisableGenerateImage { get; set; }

        /// <summary>
        /// Use the internal perfomance analyzer (optional).
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(false)]
        public bool UsePerfomanceAnalyzer { get; set; }

        /// <summary>
        /// Writes the resolved SenseEv formulas into the custom xml structure. (optional).
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(false)]
        public bool WriteFormulaInCustomXml { get; set; }
        
        /// <summary>
        /// The time until the report is aborted (optional).
        /// </summary>
        [JsonProperty]
        [DefaultValue(900)]
        public int Timeout { get; set; } = 900;

        /// <summary>
        /// The repeat until the report is canceled as error (optional).
        /// </summary>
        [JsonProperty]
        [DefaultValue(2)]
        public int ErrorRepeatCount { get; set; } = 2;

        /// <summary>
        /// The count of cpus or cpu cores for the ser engine (optional).
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(-1)]
        public int CPULimitInCore { get; set; } = -1;

        /// <summary>
        /// The soft memory limit for the ser engine (optional).
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(-1.0)]
        public double MemoryLimitInGB { get; set; } = -1.0;
        #endregion
    }

    /// <summary>
    /// The template for the report.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore,
                NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class SerTemplate
    {
        #region Properties
        /// <summary>
        /// The Input path of the template (optional).
        /// You can use the protokoll 'content' or 'lib'
        /// Samples:
        /// Use 'content:///[TEMPLATE]' for InApp template
        /// Use 'content://[CONTENTLIBARY]/[TEMPLATE]' for a content libary.
        /// Use 'lib://[LIB-FOLDER]/[TEMPLATE]' for a special folder.
        /// </summary>
        [JsonProperty]
        public string Input { get; set; }

        /// <summary>
        /// The output name of the report (optional).
        /// Note:
        /// You can use the output name with or without file extention.
        /// If you use the output name without a file extension, you must also use the property 'Outputformat'.
        /// If you do not specify a file extension, PDF will be used automatically.
        /// </summary>
        [JsonProperty]
        public string Output { get; set; }

        /// <summary>
        /// The file extention for the output name (optional).
        /// It is optional.
        /// </summary>
        /// <seealso cref="SerTemplate.Output"/>
        [JsonProperty]
        public string OutputFormat { get; set; }

        /// <summary>
        /// The password what should be set for the reports (optional).
        /// Note:
        /// It only works for PDF and XLSX files.
        /// If you not want the password remove it in the json.
        /// Please do not leave blank, because the human json convention can not handle it.
        /// </summary>
        [JsonProperty]
        public string OutputPassword { get; set; }

        /// <summary>
        /// If you wont to use a BASE64 encoded password.
        /// </summary>
        [JsonProperty]
        public bool UseBase64Password { get; set; }

        /// <summary>
        /// Flexible output format options (optional).
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public JObject OutputFormatOptions { get; set; }

        /// <summary>
        /// The signature for the script executing (optional)..
        /// </summary>
        [JsonProperty]
        public List<string> ScriptKeys { get; set; }

        /// <summary>
        /// The arguments for the executing .NET scripts (optional).
        /// </summary>
        [JsonProperty]
        public List<string> ScriptArgs { get; set; }

        /// <summary>
        /// The qlik filters used for the report (optional).
        /// </summary>
        [JsonProperty, JsonConverter(typeof(SingleValueArrayConverter))]
        public List<SerSenseSelection> Selections { get; set; }

        /// <summary>
        /// Clear or clear not the Qlik selections in a session (optional).
        /// This mode is importent for the OnDemand Extention.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(true)]
        public bool SelectionsClearAll { get; set; } = true;
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the selections of a special type.
        /// </summary>
        /// <param name="type">The type of a selection.</param>
        /// <returns>A list of selections</returns>
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
        public string Cert { get; set; }

        /// <summary>
        /// The path to the private key.
        /// It's generate the JWT token.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string PrivateKey { get; set; }
        #endregion
    }

    /// <summary>
    /// The connection class of the report.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore,
                NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class SerConnection
    {
        #region Logger
        private readonly static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Variables && Properties
        private Uri privateURI;

        /// <summary>
        /// The Connection uri to Qlik for TypeScript (optional).
        /// Note:
        /// Desktop: "Qlik Sense Desktop"
        /// Server: "https://NAME_OF_SERVER/VIRTUALPROXY"
        /// </summary>
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
        /// Wait for data reload of the app (optional).
        /// Wait for other apps or processes
        /// </summary>
        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RetryTimeout { get; set; } = 0;

        /// <summary>
        /// Use the ssl validation (optional).
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(true)]
        public bool SslVerify { get; set; } = true;

        /// <summary>
        /// The thumb print from qlik client ssl certificate (optional).
        /// </summary>
        [JsonProperty]
        public List<SerThumbprint> SslValidThumbprints { get; set; }

        /// <summary>
        /// Share the qlik session or create a new session with a identity (optional).
        /// </summary>
        [JsonProperty, JsonConverter(typeof(SingleValueArrayConverter))]
        public List<string> Identities { get; set; }

        /// <summary>
        /// The credentials for the qlik communication for TypeScript (optional).
        /// </summary>
        [JsonProperty]
        public SerCredentials Credentials { get; set; }

        /// <summary>
        /// The lisence keys (optional).
        /// </summary>
        [JsonProperty, JsonConverter(typeof(SingleValueArrayConverter))]
        public List<string> Lefs { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get the server uri.
        /// </summary>
        /// <returns></returns>
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
    public class SerSenseSelection
    {
        #region Properties
        /// <summary>
        /// The Name of the filter (optional).
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// The Name of the selection state default is $ (optional).
        /// </summary>
        [JsonProperty]
        public string StateName { get; set; } = "$";

        /// <summary>
        /// Type of the filter type.
        /// By Default it will used 'Field'.
        /// Other Values are 'bookmark' and 'hiddenbookmark'
        /// </summary>
        [JsonProperty]
        public string ObjectType { get; set; }

        /// <summary>
        /// The values that should be used (optional).
        /// </summary>
        [JsonProperty, JsonConverter(typeof(SingleValueArrayConverter))]
        public List<string> Values { get; set; }

        /// <summary>
        /// Sort dynamic values (optional).
        /// </summary>
        [JsonProperty]
        public string Sorted { get; set; }

        /// <summary>
        /// Type of the selection.
        /// </summary>
        [JsonProperty, JsonConverter(typeof(StringEnumConverter))]
        public SelectionType Type { get; set; }

        /// <summary>
        /// Export a Root node for the sheet names (optional).
        /// </summary>
        [JsonProperty]
        [DefaultValue(true)]
        public bool ExportRootNode { get; private set; } = true;

        /// <summary>
        /// Give the sheet a seperate sheet name form a formaula (optional).
        /// You can also use @@sheetname@@ as placeholter for the orignal sheet name.
        /// </summary>
        [JsonProperty]
        public string SheetName { get; private set; }
        #endregion
    }
}