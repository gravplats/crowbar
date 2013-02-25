using System;
using System.IO;

namespace Crowbar
{
    /// <summary>
    /// The default Web.config path provider that will return an AppDomain-relative path to the configuration file.
    /// </summary>
    public class WebConfigPathProvider : IPathProvider
    {
        private readonly string config;

        /// <summary>
        /// Creates a new instance of the <see cref="WebConfigPathProvider" />.
        /// </summary>
        /// <param name="config">The name of the configuration file.</param>
        public WebConfigPathProvider(string config)
        {
            this.config = config;
        }

        /// <inheritdoc />
        public string GetPhysicalPath()
        {
            return config == null ? null : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, config);
        }
    }
}