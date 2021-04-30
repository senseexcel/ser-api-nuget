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
        public string JsonJobResults { get; set; }

        /// <summary>
        /// Message by a error
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The task is finish
        /// </summary>
        public bool Finish { get; set; }
        #endregion
    }
}