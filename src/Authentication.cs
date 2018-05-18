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
    using Reinforced.Typings.Attributes;
    using System;
    using System.IO;
    #endregion

    #region Enumerations
    [TsEnum]
    public enum QlikCredentialType
    {
        WINDOWSAUTH,
        CERTIFICATE,
        SESSION,
        JWT,
        HEADER
    }
    #endregion

    #region Interfaces
#if NET452
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public interface IQlikCredentials
    {
        QlikCredentialType Type { get; }
    }
    #endregion

#if NET452
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public class CertificateAuth : IQlikCredentials
    {
        #region Properties & Variables
        [JsonIgnore]
#if NET452
        [Reinforced.Typings.Attributes.TsIgnore]
#endif
        public QlikCredentialType Type { get; } = QlikCredentialType.CERTIFICATE;

        [JsonProperty(nameof(CertificatePath))]
        public string CertificatePath { get; private set; }

        [JsonProperty(nameof(UserId))]
        public string UserId { get; private set; }

        [JsonProperty(nameof(UserDirectory))]
        public string UserDirectory { get; private set; }

        [JsonProperty(nameof(CertPassword))]
        public string CertPassword { get; private set; }
        #endregion
    }

    public class WindowsAuth : IQlikCredentials
    {
        #region Properties & Variables
        [JsonIgnore]
#if NET452
        [Reinforced.Typings.Attributes.TsIgnore]
#endif
        public QlikCredentialType Type { get; } = QlikCredentialType.WINDOWSAUTH;

        [JsonProperty(nameof(Login))]
        public string Login { get; set; }

        [JsonProperty(nameof(Password))]
        public string Password { get; set; }
        #endregion
    }

#if NET452
    [Reinforced.Typings.Attributes.TsInterface]
#endif
    public class SessionAuth : IQlikCredentials
    {
        #region Properties & Variables
        [JsonIgnore]
#if NET452
        [Reinforced.Typings.Attributes.TsIgnore]
#endif
        public QlikCredentialType Type { get; } = QlikCredentialType.SESSION;

        [JsonProperty(nameof(CookieName))]
        public string CookieName { get; set; }

        [JsonProperty(nameof(CookieValue))]
        public string CookieValue { get; set; }
        #endregion
    }
}