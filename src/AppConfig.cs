namespace Ser.Api
{
    #region Usings
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>
    /// The configuration of SER for working with JSON
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class SerConfig
    {
        /// <summary>
        /// All tasks to be performed
        /// </summary>
        #region Properties
        [JsonProperty]
        public List<SerTask> Tasks { get; set; } = new List<SerTask>();
        #endregion
    }
}