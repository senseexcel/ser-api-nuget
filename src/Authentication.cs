#region License
/*
Copyright (c) 2018 Konrad Mattheis und Martin Berthold
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
#endregion

namespace Ser.Api
{
    #region Usings
    using Newtonsoft.Json;
    using System;
    using System.IO;
    #endregion

    /// <summary>
    /// Authentication type to connect to Qlik
    /// </summary>
    #region Enumerations
#if NET45
    [Reinforced.Typings.Attributes.TsEnum]
#endif
    public enum QlikCredentialType
    {
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

    #region Interfaces
    /// <summary>
    /// Qlik authentication type
    /// </summary>
#if NET45
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public interface IQlikCredentials
    {
        /// <summary>
        /// Qlik authentication type
        /// </summary>
        QlikCredentialType Type { get; }
    }
    #endregion

    /// <summary>
    /// Logic for certificate authentication
    /// </summary>
#if NET45
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public class CertificateAuth : IQlikCredentials
    {
        #region Properties & Variables
        /// <summary>
        /// Qlik authentication type
        /// </summary>
        [JsonIgnore]
#if NET45
        [Reinforced.Typings.Attributes.TsIgnore]
#endif
        public QlikCredentialType Type { get; } = QlikCredentialType.CERTIFICATE;

        /// <summary>
        /// Path to the qlik server certificates
        /// </summary>
        [JsonProperty(nameof(CertificatePath))]
        public string CertificatePath { get; private set; }
        
        /// <summary>
        /// Qlik UserId
        /// </summary>
        [JsonProperty(nameof(UserId))]        
        public string UserId { get; private set; }

        /// <summary>
        /// Qlik UserDirectory
        /// </summary>
        [JsonProperty(nameof(UserDirectory))]
        public string UserDirectory { get; private set; }

        /// <summary>
        /// Password of the certificate which can be specified when generating
        /// </summary>
        [JsonProperty(nameof(CertPassword))]
        public string CertPassword { get; private set; }
#endregion
    }

    /// <summary>
    /// Logic for windows authentication
    /// </summary>
    public class WindowsAuth : IQlikCredentials
    {
        #region Properties & Variables
        /// <summary>
        /// Qlik authentication type
        /// </summary>
        [JsonIgnore]
#if NET45
        [Reinforced.Typings.Attributes.TsIgnore]
#endif
        public QlikCredentialType Type { get; } = QlikCredentialType.WINDOWSAUTH;

        /// <summary>
        /// windows account name
        /// </summary>
        [JsonProperty(nameof(Login))]
        public string Login { get; set; }


        /// <summary>
        /// windows account password
        /// </summary>
        [JsonProperty(nameof(Password))]
        public string Password { get; set; }
        #endregion
    }

    /// <summary>
    /// The Session authentication class
    /// </summary>
#if NET45
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public class SessionAuth : IQlikCredentials
    {
        #region Properties & Variables
        /// <summary>
        /// Qlik authentication type
        /// </summary>
        [JsonIgnore]
#if NET45
        [Reinforced.Typings.Attributes.TsIgnore]
#endif
        public QlikCredentialType Type { get; } = QlikCredentialType.SESSION;

        /// <summary>
        /// The name of the cookie (Default: XQlik-Session)
        /// </summary>
        [JsonProperty(nameof(CookieName))]
        public string CookieName { get; set; }

        /// <summary>
        /// The value of the cookie
        /// </summary>
        [JsonProperty(nameof(CookieValue))]
        public string CookieValue { get; set; }
        #endregion
    }
}