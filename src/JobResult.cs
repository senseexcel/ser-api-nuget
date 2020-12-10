namespace Ser.Api
{
    #region Usings
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    #region Enumerations
    /// <summary>
    /// The status of a finished script
    /// </summary>
    public enum ScriptResult
    {
        /// <summary>
        /// Unkown result
        /// </summary>
        UNKOWN,

        /// <summary>
        /// Success result
        /// </summary>
        SUCCESS,

        /// <summary>
        /// The .net script has an error
        /// </summary>
        SCRIPT_ERROR,

        /// <summary>
        /// No access to the sandbox.
        /// </summary>
        PERMISSION_ERROR,

        /// <summary>
        /// General error
        /// </summary>
        ERROR,

        /// <summary>
        /// General warning
        /// </summary>
        WARNING
    }

    /// <summary>
    /// The stauts of a finished task
    /// </summary>
    public enum TaskStatusInfo
    {
        /// <summary>
        /// The Task was abort
        /// </summary>
        ABORT = 0,

        /// <summary>
        /// The Task was finished successfull
        /// </summary>
        SUCCESS = 1,

        /// <summary>
        /// The Task was finished with warnings
        /// </summary>
        WARNING = 2,

        /// <summary>
        /// The Task was finished with an error
        /// </summary>
        ERROR = 4,

        /// <summary>
        /// The Task was finished with an error, but is repeated at the next call
        /// </summary>
        RETRYERROR = 8,

        /// <summary>
        /// The Task was tagged as inactive
        /// </summary>
        INACTIVE = 16
    }
    #endregion

    /// <summary>
    /// The result of a finished job.
    /// This information is needed for the result file.
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class JobResult
    {
        #region Variables & Properties
        /// <summary>
        /// The id of the task.
        /// </summary>
        [JsonProperty]
        public Guid TaskId { get; set; }

        /// <summary>
        /// The start time of the task.
        /// </summary>
        [JsonProperty]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// The runtime of the task.
        /// </summary>
        [JsonProperty]
        public TimeSpan RunTime { get; set; }

        /// <summary>
        /// The current version of aspose cells.
        /// </summary>
        [JsonProperty]
        public string AsposeVersion { get; set; }

        /// <summary>
        /// The current version of the ser engine.
        /// </summary>
        [JsonProperty]
        public string EngineVersion { get; set; }

        /// <summary>
        /// The status information of the finished task
        /// </summary>
        [JsonProperty, JsonConverter(typeof(StringEnumConverter))]
        public TaskStatusInfo Status { get; set; }

        /// <summary>
        /// Set the first Error
        /// </summary>
        [JsonProperty]
        public ReportException Exception { get; set; }

        /// <summary>
        /// The count of the reports.
        /// </summary>
        [JsonProperty]
        public int Count { get; set; }

        /// <summary>
        /// The list of the generated reports.
        /// </summary>
        [JsonProperty]
        public IList<Report> Reports { get; set; }
        #endregion
    }

    /// <summary>
    /// This class is a part of JobResult.
    /// It maps the individual report in the job result.
    /// </summary>
    /// <seealso cref="JobResult"/>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Report
    {
        #region Properties and Variables
        /// <summary>
        /// The individually name of the report.
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// The path or paths of the Report.
        /// The PDF can save multiple pages, in this case is it one path.
        /// The CSV save multiple files, in this case is it many paths.
        /// </summary>
        [JsonProperty]
        public List<string> Paths { get; set; } = new List<string>();

        /// <summary>
        /// The value of the current dynamic filter.
        /// </summary>
        [JsonProperty]
        public string DynamicPathValue { get; set; }

        /// <summary>
        /// The list of script results that were executed before the calculation was running.
        /// </summary>
        [JsonProperty]
        public List<ScriptResult> PreScriptResult { get; set; }

        /// <summary>
        /// The list of script results that were executed after the calculation was running.
        /// </summary>
        [JsonProperty]
        public List<ScriptResult> AfterScriptResult { get; set; }

        /// <summary>
        /// This property includes the distribute options.
        /// It is a json structure.
        /// </summary>
        [JsonProperty]
        public JObject Distribute { get; set; }

        /// <summary>
        /// The used connection to create the report.
        /// </summary>
        [JsonProperty]
        public SerConnection Connection { get; set; }

        /// <summary>
        /// Download data from restservice.
        /// </summary>
        [JsonIgnore]
        public List<ReportData> Data { get; set; } = new List<ReportData>();
        #endregion
    }

    /// <summary>
    /// Reporting data from rest service
    /// </summary>
    public class ReportData
    {
        /// <summary>
        /// File path of the report.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Data as byte array
        /// </summary>
        public byte[] DownloadData { get; set; }
    }

    /// <summary>
    /// If the engine has an error.
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ReportException
    {
        /// <summary>
        /// All messages in excpetion recursion
        /// </summary>
        [JsonProperty]
        public string FullMessage { get; set; }

        /// <summary>
        /// All messages and the stacktrace
        /// </summary>
        [JsonProperty]
        public string StackTrace { get; set; }

        private static string GetCompleteMessage(Exception exception)
        {
            var x = exception?.InnerException ?? null;
            var msg = new StringBuilder(exception?.Message);
            while (x != null)
            {
                msg.Append($"{Environment.NewLine}{x.Message}");
                x = x.InnerException;
            }
            return msg.ToString();
        }

        /// <summary>
        /// Convert a full exception to serialize exception.
        /// </summary>
        /// <param name="exception">full exception</param>
        /// <returns></returns>
        public static ReportException GetException(Exception exception)
        {
            if (exception == null)
                return null;

            return new ReportException()
            {
                FullMessage = GetCompleteMessage(exception),
                StackTrace = exception.ToString()
            };
        }
    }
}