namespace Ser.Api.Model
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    #endregion

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
}