namespace Ser.Api
{
    #region Usings
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    #region Enumerations
#if NET45
    [Reinforced.Typings.Attributes.TsEnum]
#endif
    public enum ScriptResult
    {
        UNKOWN,
        SUCCESS,
        SCRIPT_ERROR,
        PERMISSION_ERROR,
        ERROR,
        WARNING
    }

#if NET45
    [Reinforced.Typings.Attributes.TsEnum]
#endif
    public enum TaskStatusInfo
    {
        ABORT,
        SUCCESS,
        WARNING,
        ERROR,
        RETRYERROR
    }
    #endregion

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
#if NET45
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public class JobResult
    {
        #region Variables & Properties
        [JsonProperty]
        public Guid TaskId { get; private set; }

        [JsonProperty]
        public DateTime StartTime { get; private set; }

        [JsonProperty]
        public TimeSpan RunTime { get; private set; }

        [JsonProperty]
        public TaskStatusInfo Status { get; private set; }

        [JsonProperty]
        public string AsposeVersion { get; private set; }

        [JsonProperty]
        public string EngineVersion { get; private set; }

        [JsonProperty]
        public int Count { get; private set; }

        [JsonProperty]
        public IList<Report> Reports { get; private set; }
        #endregion
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
#if NET45
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public class Report
    {
        #region Properties and Variables
        [JsonProperty]
        public string Name { get; private set; }

        [JsonProperty]
        public List<string> Paths { get; set; }

        [JsonProperty]
        public List<ScriptResult> PreScriptResult { get; set; }

        [JsonProperty]
        public List<ScriptResult> AfterScriptResult { get; set; }

        [JsonProperty]
#if NET45
        [Reinforced.Typings.Attributes.TsIgnore]
#endif
        public JObject Distribute { get; set; }

        [JsonProperty]
        public SerConnection Connection { get; set; }
        #endregion
    }
}