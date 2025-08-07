using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;



namespace MSPro.CLArgs;

public static class Helper
{
    /// <summary>
    ///     Get the current process directory.
    /// </summary>
    /// <remarks>
    ///     Uses <see cref="Process.GetCurrentProcess()" /> to get
    ///     the <see cref="Process.MainModule" />'s directory.
    /// </remarks>
    public static string BinDir => Path.GetDirectoryName(GetExecutableFileName())!;



    /// <summary>
    ///     Split a command-line into arguments.
    /// </summary>
    /// <remarks>
    ///     Simulates Win32 CommandLineToArgvW tokenization.
    /// </remarks>
    public static string[] SplitCommandLine(string commandLine)
    {
        List<string> result = [];
        var i = 0;
        var quotation = false;
        StringBuilder currentToken = new(4096);
        while (i < commandLine.Length)
        {
            if (quotation)
            {
                if (commandLine[i] == '\"') quotation = false;
                else currentToken.Append(commandLine[i]);
            }
            else
                switch (commandLine[i])
                {
                    case ' ':
                        result.Add(currentToken.ToString());
                        currentToken.Clear();
                        break;
                    case '\"':
                        quotation = true;
                        break;
                    default:
                        currentToken.Append(commandLine[i]);
                        break;
                }

            i++;
        }

        if (quotation)
            throw new ApplicationException("Unmatched quotation - missing quote.");

        result.Add(currentToken.ToString());
        return result.ToArray();
    }



    /// <summary>
    ///     Get the name of the current executable.
    /// </summary>
    /// <remarks>
    ///     To support self-contained Assemblies, the method uses <see cref="Environment.GetCommandLineArgs()" /> to
    ///     get the name of the executable.<br />
    ///     <b>Be careful:</b> The exe of a .net portable Assembly cannot be loaded using
    ///     <see cref="Assembly.LoadFile(string)" />.
    ///     If you use ILSpy you will see 'PEFileNotSupportedException: PE file does not contain any managed metadata.'.<br />
    ///     Interesting to see, <see cref="Assembly.GetEntryAssembly()" /> returns an ExeFileName.DLL as the entry Assembly.
    ///     This DLL can be used
    ///     The same is true for a single executable, which cannot be loaded
    ///     <br />
    ///     Compilation shows this warning: <see cref="System.Reflection.Assembly.Location" /> always returns an empty
    ///     string for assemblies embedded in a single-file app. If the path to the app directory is needed,
    ///     consider calling <see cref="System.AppDomain.BaseDirectory" />".
    /// </remarks>
    /// <seealso href="https://docs.microsoft.com/de-de/dotnet/core/deploying/single-file" />
    public static string GetExecutableFileName()
    {
        ProcessModule processModule = Process.GetCurrentProcess().MainModule!;
        if (processModule == null)
            throw new InvalidOperationException("Unexpected: Call to Process.GetCurrentProcess().MainModule returned null!");

        return Path.GetFullPath(processModule.FileName);
    }



    /// <summary>
    ///     Resolves the path of an existing file.
    /// </summary>
    /// <remarks>
    ///     Tries to find and existing file by probing certain directories.<br />.
    /// </remarks>
    /// <param name="fileName">The name of the file that should be found.</param>
    /// <param name="addDirs">An optional list of additional directories where to look for the file.</param>
    /// <param name="workDir">The current working directory, which is normally <see cref="Environment.CurrentDirectory" />.</param>
    /// <param name="binDir">The directory of the executable, which is normally, <see cref="BinDir" />.</param>
    /// <param name="throwIfNotFound">
    ///     Set to <c>true</c> [Default] when you want a <see cref="FileNotFoundException" />
    ///     to be thrown when the file is not found. Set to <c>false</c> when you want to have
    ///     <c>null</c> as the return value instead.
    /// </param>
    /// <returns>
    ///     The file's full path, if the file could be found.<br />
    ///     Otherwise, <c>null</c> if the file wasn't found and if
    ///     <paramref name="throwIfNotFound" /> is set to <c>false</c>.
    /// </returns>
    public static string? FindFile(string fileName
        , IEnumerable<string>? addDirs = null
        , string workDir
        , string binDir
        , bool throwIfNotFound = true)
    {
        if (!string.IsNullOrWhiteSpace(fileName))
        {
            fileName = Environment.ExpandEnvironmentVariables(fileName);
            if (Path.IsPathRooted(fileName)) return Path.GetFullPath(fileName);
            string fullPath;

            // 1. addDirs
            if (addDirs != null)
            {
                foreach (string addDir in addDirs)
                {
                    Debug.Assert(Path.IsPathRooted(addDir));
                    fullPath = Path.Combine(Environment.ExpandEnvironmentVariables(addDir), fileName);
                    if (File.Exists(fullPath)) return Path.GetFullPath(fullPath);
                }
            }


            // 2. PreferredDir
            Debug.Assert(Path.IsPathRooted(workDir));
            // Search in preferred directory
            fullPath = Path.Combine(Environment.ExpandEnvironmentVariables(workDir), fileName);
            if (File.Exists(fullPath)) return Path.GetFullPath(fullPath);


            // 3. BinDir
            fullPath = Path.Combine(binDir, fileName);
            if (File.Exists(fullPath)) return Path.GetFullPath(fullPath);
        }

        if (throwIfNotFound)
            throw new FileNotFoundException($"The requested file '{fileName} could not be found!", fileName);
        return null;
    }



    /// <summary>
    ///     Wrap a given text (insert meaningful line breaks) as a specified column.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="atColumn"></param>
    /// <returns></returns>
    public static WrappedText Wrap(string text, int atColumn)
    {
        if (string.IsNullOrWhiteSpace(text)) return new WrappedText();
        StringBuilder sb = new(text.Replace(Environment.NewLine, "|"));
        sb.Replace("\r\n", "|");
        sb.Replace("\r", "|");
        sb.Replace("\n", "|");
        string[] lines = sb.ToString().Split('|');
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length > atColumn) lines[i] = insertLineBreaks(lines[i], atColumn);
        }

        return new WrappedText(string.Join(Environment.NewLine, lines));
    }



    /// <summary>
    ///     Wraps long line by using sb.AppendLine for each new line.
    /// </summary>
    private static string insertLineBreaks(string line, int lineBreakColumn)
    {
        char[] lineBreaker = [' ', '-'];
        StringBuilder sb = new();
        var pStartSegment = 0;
        do
        {
            int pStartFind = pStartSegment + lineBreakColumn;
            int pEnd = line.LastIndexOfAny(lineBreaker, pStartFind);

            // cannot break line BEFORE lineBreakColumn --> insert hard-break
            if (pEnd < pStartSegment) pEnd = pStartFind;

            sb.AppendLine(line.Substring(pStartSegment, pEnd - pStartSegment + 1).TrimEnd());
            pStartSegment = pEnd + 1;
        } while (pStartSegment < line.Length - lineBreakColumn);

        // add last Segment w/o NewLine
        sb.Append(line.Substring(pStartSegment, line.Length - pStartSegment));
        return sb.ToString();
    }



    public class WrappedText
    {
        public WrappedText(string? allText = null, string defaultText = "---")
        {
            AllText = allText ?? defaultText;

            int firstNewLine = AllText.IndexOf(Environment.NewLine, StringComparison.Ordinal);
            AllLines = firstNewLine < 0
                ? [AllText]
                : AllText.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }



        public string[] AllLines { get; }
        public string AllText { get; }
    }
}