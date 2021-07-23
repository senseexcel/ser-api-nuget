namespace Ser.Api.Model
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Ser.Api.JsonConverters;
    #endregion

    /// <summary>
    /// The connection class of the report.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore,
                NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class SerConnection
    {
        #region Variables && Properties
        private Uri privateURI;

        /// <summary>
        /// The Connection uri to Qlik for TypeScript (optional).
        /// Note:
        /// Desktop: "Qlik Sense Desktop"
        /// Server: "https://NAME_OF_SERVER/VIRTUALPROXY"
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Uri ServerUri
        {
            get
            {
                try
                {
                    var newUri = new UriBuilder(privateURI);
                    switch (newUri.Scheme.ToLowerInvariant())
                    {
                        case "ws":
                            newUri.Scheme = "http";
                            break;
                        case "wss":
                            newUri.Scheme = "https";
                            break;
                    }
                    return newUri.Uri;
                }
                catch (Exception ex)
                {
                    throw new Exception($"The server uri {privateURI} is invalid.", ex);
                }
            }
            set
            {
                if (value != privateURI)
                {
                    privateURI = value;
                    var uri = privateURI.OriginalString.TrimEnd('/');
                    privateURI = new Uri(uri);
                }
            }
        }

        /// <summary>
        /// Qlik app name or id
        /// </summary>
        [JsonProperty]
        public string App { get; set; }

        /// <summary>
        /// The reload timeout for InAppMode.
        /// Wait for data reload of the app (optional).
        /// Wait for other apps or processes
        /// </summary>
        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RetryTimeout { get; set; } = 0;

        /// <summary>
        /// Use the ssl validation (optional).
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(true)]
        public bool SslVerify { get; set; } = true;

        /// <summary>
        /// The thumb print from qlik client ssl certificate (optional).
        /// </summary>
        public List<SerThumbprint> SslValidThumbprints { get; set; }

        /// <summary>
        /// Share the qlik session or create a new session with a identity (optional).
        /// </summary>
        [JsonProperty, JsonConverter(typeof(SingleValueArrayConverter))]
        public List<string> Identities { get; set; }

        /// <summary>
        /// The credentials for the qlik communication for TypeScript (optional).
        /// </summary>
        [JsonProperty]
        public SerCredentials Credentials { get; set; }

        /// <summary>
        /// The lisence keys (optional).
        /// </summary>
        [JsonProperty, JsonConverter(typeof(SingleValueArrayConverter))]
        public List<string> Lefs { get; set; }

        /// <summary>
        /// The lisence server (optional).
        /// </summary>
        [JsonProperty]
        public List<SerServer> LicenseServers { get; set; } = new List<SerServer>();

        /// <summary>
        /// The renderer server (optional).
        /// </summary>
        [JsonProperty]
        public List<SerServer> RendererServers { get; set; } = new List<SerServer>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Get the server uri.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{ServerUri}";
        }
        #endregion
    }

    /// <summary>
    /// Server info for license Server
    /// </summary>
    public class SerServer
    {
        #region Properties
        /// <summary>
        /// Server uri with port and protocol
        /// </summary>
        public Uri ServerUri { get; set; }

        /// <summary>
        /// Location/Country short
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Prefer server x
        /// </summary>
        public int Priority { get; set; }
        #endregion
    }

    /// <summary>
    /// The thumb print for the report.
    /// Note:
    /// Copy the thumb print from thw qlik client certificate.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore,
                NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class SerThumbprint
    {
        #region Properties
        /// <summary>
        /// The server url to the qlilk certificate
        /// </summary>
        [JsonProperty]
        public string Url { get; set; }

        /// <summary>
        /// The thumb print from the qlik clint certificate
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Thumbprint { get; set; }
        #endregion
    }
}