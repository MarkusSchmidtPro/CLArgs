using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    public interface IConfigFileResolver
    {
        /// <summary>
        /// Resolve a relative configuration file name to a full path.
        /// </summary>
        /// <remarks>
        /// Resolution looks in the preferred directory first. If the file exists there, the
        /// full path is returned. Otherwise the full path is build based on the entry Assembly's path.<br/>
        /// You can use %AppData% and other environment variables.</remarks>
        public string ResolvePath([NotNull] string fileName);
    }
}