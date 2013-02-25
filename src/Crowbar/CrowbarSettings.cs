using System.Configuration;

namespace Crowbar
{
    /// <summary>
    /// CrowbarSettings is used when sln, web.config is not located in standard mvc settings
    /// </summary>
    public class CrowbarSetting : ConfigurationSection
    {

        /// <summary>
        /// Set the absolute path in the app.config file
        /// </summary>
        public static string MvcProjectAbsolutePath
        {
            get { return ConfigurationManager.AppSettings["MvcProjectAbsolutePath"].ToString(); }
        }

        /// <summary>
        /// Set the relative path in the app.config file
        /// </summary>
        public static string MvcProjectRelativePath
        {
            get { return ConfigurationManager.AppSettings["MvcProjectRelativePath"].ToString(); }
        }

        /// <summary>
        /// Set the mvc solution name in the app.config
        /// </summary>
        public static string MvcSolutionName
        {
            get { return ConfigurationManager.AppSettings["MvcSolutionName"].ToString(); }
        }

        /// <summary>
        /// Set the path to web.config
        /// </summary>
        public static string WebConfigPath
        {
            get { return ConfigurationManager.AppSettings["WebConfigPath"].ToString(); }
        }

        /// <summary>
        /// Set the web.config name
        /// </summary>
        public static string WebConfigName
        {
            get { return ConfigurationManager.AppSettings["WebConfigName"].ToString(); }
        }
    }
}
