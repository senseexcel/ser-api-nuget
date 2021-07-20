namespace Ser.Api.Model
{
    #region Usings
    using System;
    using System.ComponentModel;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    #endregion

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
        /// Do not cancel if an image is not rendered properly (optional).
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(false)]
        public bool IgnoreRendererRuntimeErrors { get; set; }

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
}