namespace Ser.Api
{
    #region Usings
    using System.Linq;
    #endregion

    /// <summary>
    /// Class for work with the qlik userid und userdirectory logic.
    /// </summary>
    public class DomainUser
    {
        #region Properties
        /// <summary>
        /// Qlik user id
        /// </summary>
        public string UserId { get; private set; }

        /// <summary>
        /// Qlik user directory
        /// </summary>
        public string UserDirectory { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Creating a object for DomainUser.
        /// </summary>
        /// <param name="domainUserValue">UserId and UserDirectory as Path.
        /// Sample: 'USERDIRECTORY\\USERID' or 'UserDirectory=USERDIRECTORY; UserId=USERID'
        /// </param>
        public DomainUser(string domainUserValue)
        {
            var split = domainUserValue.Split('\\');
            if (split.Length == 2)
            {
                UserId = split.ElementAtOrDefault(1) ?? null;
                UserDirectory = split.ElementAtOrDefault(0) ?? null;
            }
            else
            {
                split = domainUserValue.Split(';');
                if (split.Length == 2)
                {
                    UserId = split.ElementAtOrDefault(1)?.Split('=').ElementAtOrDefault(1) ?? null;
                    UserDirectory = split.ElementAtOrDefault(0)?.Split('=').ElementAtOrDefault(1) ?? null;
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Format UserId and UserDirectory to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{UserDirectory.ToLowerInvariant()}\\{UserId.ToLowerInvariant()}";
        }
        #endregion
    }
}