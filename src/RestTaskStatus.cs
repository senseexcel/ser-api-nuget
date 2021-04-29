namespace Ser.Api
{
    #region Usings
    using System;
    #endregion

    /// <summary>
    /// Task information
    /// </summary>
    public class RestTaskStatus
    {
        #region Properties
        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Task State Message
        /// </summary>
        public string ProcessMessage { get; set; }

        /// <summary>
        /// DistributeResult
        /// </summary>
        public string DistributeResult { get; set; }

        /// <summary>
        /// Job result list as Json
        /// </summary>
        public string JobResultJson { get; set; }
        #endregion
    }
}