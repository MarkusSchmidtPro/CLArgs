using System;
using System.IO;
using System.Reflection;



namespace MSPro.CLArgs
{
    /// <summary>
    /// The default configuration file resolver.
    /// </summary>
    public class ConfigFileResolver : IConfigFileResolver
    {
        /// <summary>
        /// Set a user defined directory used to search for the configuration file.
        /// </summary>
        public string UserDefinedDirectory { get; set; }

        string IConfigFileResolver.ResolvePath(string fileName)
        {
            if (Path.IsPathRooted(fileName)) return fileName;
            
            string fullPath = Environment.ExpandEnvironmentVariables(fileName);
            if (Path.IsPathRooted(fullPath)) return fullPath;

            if (this.UserDefinedDirectory != null)
            {
                this.UserDefinedDirectory = Environment.ExpandEnvironmentVariables(this.UserDefinedDirectory);
                fullPath = Path.GetFullPath( Path.Combine(UserDefinedDirectory, fullPath));
                if (File.Exists(fullPath)) return fullPath;
            }

            fullPath = Path.GetFullPath( Path.Combine(Assembly.GetEntryAssembly().Location, fullPath));
            if( !File.Exists(fullPath)) 
                throw new FileNotFoundException($"Cannot find config file {fileName}", fileName);

            return fullPath;
        }
    }
}