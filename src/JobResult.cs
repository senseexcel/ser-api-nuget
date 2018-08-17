#region License
/*
Copyright (c) 2018 Konrad Mattheis und Martin Berthold
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
#endregion;

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
    /// <summary>
    /// The status of a finished script
    /// </summary>
#if NET45
    [Reinforced.Typings.Attributes.TsEnum]
#endif
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
#if NET45
    [Reinforced.Typings.Attributes.TsEnum]
#endif
    public enum TaskStatusInfo
    {
        /// <summary>
        /// The Task was abort
        /// </summary>
        ABORT,

        /// <summary>
        /// The Task was finished successfull
        /// </summary>
        SUCCESS,

        /// <summary>
        /// The Task was finished with warnings
        /// </summary>
        WARNING,

        /// <summary>
        /// The Task was finished with an error
        /// </summary>
        ERROR,

        /// <summary>
        /// The Task was finished with an error, but is repeated at the next call
        /// </summary>
        RETRYERROR
    }
    #endregion

    /// <summary>
    /// The result of a finished job.
    /// This information is needed for the result file.
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
#if NET45
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public class JobResult
    {
        #region Variables & Properties
        /// <summary>
        /// The id of the task.
        /// </summary>
        [JsonProperty]
        public Guid TaskId { get; private set; }

        /// <summary>
        /// The start time of the task.
        /// </summary>
        [JsonProperty]
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// The runtime of the task.
        /// </summary>
        [JsonProperty]
        public TimeSpan RunTime { get; private set; }

        /// <summary>
        /// The status information of the finished task
        /// </summary>
        [JsonProperty]
        public TaskStatusInfo Status { get; private set; }

        /// <summary>
        /// The current version of aspose cells.
        /// </summary>
        [JsonProperty]
        public string AsposeVersion { get; private set; }

        /// <summary>
        /// The current version of the ser engine.
        /// </summary>
        [JsonProperty]
        public string EngineVersion { get; private set; }

        /// <summary>
        /// The count of the reports.
        /// </summary>
        [JsonProperty]
        public int Count { get; private set; }

        /// <summary>
        /// The list of the generated reports.
        /// </summary>
        [JsonProperty]
        public IList<Report> Reports { get; private set; }
        #endregion
    }

    /// <summary>
    /// This class is a part of JobResult.
    /// It maps the individual report in the job result.
    /// </summary>
    /// <seealso cref="JobResult"/>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
#if NET45
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public class Report
    {
        #region Properties and Variables
        /// <summary>
        /// The individually name of the report.
        /// </summary>
        [JsonProperty]
        public string Name { get; private set; }

        /// <summary>
        /// The path or paths of the Report.
        /// The PDF can save multiple pages, in this case is it one path.
        /// The CSV save multiple files, in this case is it many paths.
        /// </summary>
        [JsonProperty]
        public List<string> Paths { get; set; }

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
#if NET45
        [Reinforced.Typings.Attributes.TsIgnore]
#endif
        public JObject Distribute { get; set; }

        /// <summary>
        /// The used connection to create the report.
        /// </summary>
        [JsonProperty]
        public SerConnection Connection { get; set; }
        #endregion
    }
}