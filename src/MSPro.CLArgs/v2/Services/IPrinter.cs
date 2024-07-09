using System;



namespace MSPro.CLArgs
{
    public interface IPrinter
    {
        /// <summary>
        ///     Print a Debug message
        /// </summary>
        /// <remarks>Debug message will normally not be printed, but logged, only.</remarks>
        /// <param name="message"></param>
        public void Debug(string message);
        public void Info(string message, bool cr = true);
        public void Warn(string message);
        public void Error(string message);
        public void Error(Exception e);


        /// <summary>
        /// Print a message without logging it.
        /// </summary>
        public void Write(string message);
    
        /// <summary>
        /// Print a message without logging it.
        /// </summary>
        public void WriteLine(string? message=null);
    }
}