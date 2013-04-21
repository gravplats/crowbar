using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Crowbar
{
    /// <summary>
    /// The default Web project path provider that will attempt to return an AppDomain-relative path to the web project.
    /// </summary>
    public class WebProjectPathProvider : IPathProvider
    {
        private readonly string mvcProjectName;

        /// <summary>
        /// Creates a new instance of the <see cref="WebProjectPathProvider"/>.
        /// </summary>
        /// <param name="mvcProjectName">The name of the MVC web project.</param>
        public WebProjectPathProvider(string mvcProjectName)
        {
            this.mvcProjectName = mvcProjectName;
        }

        /// <inheritdoc />
        public string GetPhysicalPath()
        {
            var searchedLocations = new List<string>();

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            while (baseDirectory.Contains("\\"))
            {
                string mvcPath = Path.Combine(baseDirectory, mvcProjectName);
                if (Directory.Exists(mvcPath))
                {
                    return mvcPath;
                }

                searchedLocations.Add(mvcPath);
                baseDirectory = baseDirectory.Substring(0, baseDirectory.LastIndexOf("\\"));
            }

            var locations = new StringBuilder();
            foreach (var searchedLocation in searchedLocations)
            {
                locations.AppendLine();
                locations.Append(searchedLocation);
            }

            throw new ArgumentException(string.Format("The MVC Project '{0}' was not found by Crowbar. The following locations were searched: {1}", mvcProjectName, locations));
        }
    }
}