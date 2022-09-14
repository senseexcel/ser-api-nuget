namespace AgApi
{
    #region Usings
    using System;
    using System.Text.Json.Serialization;
    #endregion

    /// <summary>
    /// Encryption types
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EncryptionType
    {
        /// <summary>
        /// RSA 256 Bit encryption
        /// </summary>
        RSA256 = 0,

        /// <summary>
        /// AES 256 Bit encryption
        /// </summary>
        AES256 = 1
    }
}