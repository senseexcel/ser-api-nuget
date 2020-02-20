namespace Ser.Api
{
    #region Usings
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;
    #endregion

    /// <summary>
    /// Class with general methods for SER
    /// </summary>
    public class SerUtilities
    {
        #region Logger
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        /// <summary>
        /// Convert a Path to a full path.
        /// </summary>
        /// <param name="path">path</param>
        /// <returns>full path</returns>
        public static string GetFullPathFromApp(string path)
        {
            try
            {
                if (String.IsNullOrEmpty(path))
                    return null;
                if (path.StartsWith("/"))
                    return path;
                if (!path.StartsWith("\\\\") && !path.Contains(":") && !path.StartsWith("%"))
                    path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).TrimEnd('\\'), path.TrimStart('\\'));
                path = Environment.ExpandEnvironmentVariables(path);
                return Path.GetFullPath(path);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }
    }
}
