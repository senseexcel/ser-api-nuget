namespace Ser.Api.Model
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    #endregion

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
}