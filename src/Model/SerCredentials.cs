namespace Ser.Api.Model
{
    #region Usings
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    #endregion

    /// <summary>
    /// The credentials for the report.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore,
                NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class SerCredentials
    {
        #region Properties
        /// <summary>
        /// The credential type for the connection.
        /// </summary>
        [JsonProperty, JsonConverter(typeof(StringEnumConverter))]
        public QlikCredentialType Type { get; set; }

        /// <summary>
        /// The name of the cookie.
        /// </summary>
        [JsonProperty]
        public string Key { get; set; }

        /// <summary>
        /// The Value of the cookie.
        /// </summary>
        [JsonProperty]
        public string Value { get; set; }

        /// <summary>
        /// The path to the JWT certificate.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Cert { get; set; }

        /// <summary>
        /// The path to the private key.
        /// It's generate the JWT token.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string PrivateKey { get; set; }
        #endregion
    }
}