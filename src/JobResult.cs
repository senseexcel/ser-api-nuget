namespace Ser.Api
{
    #region Usings
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    #region Enumerations
    public enum ScriptResult
    {
        UNKOWN,
        SUCCESS,
        SCRIPT_ERROR,
        PERMISSION_ERROR,
        ERROR,
        WARNING
    }

    public enum TaskStatusInfo
    {
        ABORT,
        SUCCESS,
        WARNING,
        ERROR,
        RETRYERROR
    }
    #endregion

    public class JobResult
    {
        #region Variables & Properties
        [JsonProperty(nameof(TaskId))]
        public Guid TaskId { get; private set; }

        [JsonProperty(nameof(StartTime))]
        public DateTime StartTime { get; private set; }

        [JsonProperty(nameof(RunTime))]
        public TimeSpan RunTime { get; private set; }

        [JsonProperty(nameof(Status))]
        public TaskStatusInfo Status { get; private set; }

        [JsonProperty(nameof(AsposeVersion))]
        public string AsposeVersion { get; private set; }

        [JsonProperty(nameof(Count))]
        public int Count { get; private set; }

        [JsonProperty(nameof(Reports))]
        public IList<Report> Reports { get; private set; }
        #endregion
    }

    public class Report
    {
        #region Properties and Variables
        [JsonProperty(nameof(Name))]
        public string Name { get; private set; }

        [JsonProperty(nameof(Paths))]
        public List<string> Paths { get; set; }

        [JsonProperty(nameof(PreScriptResult))]
        public List<ScriptResult> PreScriptResult { get; set; }

        [JsonProperty(nameof(AfterScriptResult))]
        public List<ScriptResult> AfterScriptResult { get; set; }

        [JsonProperty(nameof(Distribute))]
        public JObject Distribute { get; set; }

        [JsonProperty(nameof(Connection))]
        public SerConnection Connection { get; set; }
        #endregion
    }
}