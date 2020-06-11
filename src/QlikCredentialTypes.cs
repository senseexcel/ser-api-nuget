﻿namespace Ser.Api
{
    #region Usings
    using Newtonsoft.Json;
    #endregion

    /// <summary>
    /// Authentication type to connect to Qlik
    /// </summary>
    #region Enumerations
    public enum QlikCredentialType
    {
        /// <summary>
        /// Connection without Authentication
        /// </summary>
        NONE,

        /// <summary>
        /// Authentication over Windows user name and password
        /// </summary>
        WINDOWSAUTH,

        /// <summary>
        /// Authentication over Qlik client certificate
        /// </summary>
        CERTIFICATE,

        /// <summary>
        /// Authentication over Qlik session cookie
        /// </summary>
        SESSION,

        /// <summary>
        /// Authentication over Vitrual Proxy with method JWT
        /// </summary>
        JWT,

        /// <summary>
        /// Authentication over Vitrual Proxy with JWT Bearer
        /// </summary>
        HEADER
    }
    #endregion
}