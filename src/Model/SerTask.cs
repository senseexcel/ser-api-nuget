namespace Ser.Api.Model
{
    #region Usings
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using Hjson;
    using Newtonsoft.Json.Serialization;
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
}