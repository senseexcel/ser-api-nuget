namespace Ser.Api.Model
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    using Ser.Api.JsonConverters;
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
        /// Disable the static filter validation (optional).
        /// </summary>
        [JsonProperty]
        public bool DisableFilterValidation { get; set; }

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