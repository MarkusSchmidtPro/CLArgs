using System;
using System.Collections.Generic;
using JetBrains.Annotations;



namespace MSPro.CLArgs;

[PublicAPI]
public class Settings2
{
    /// <summary>
    ///     Get or set a list of characters that mark the end of an option's name.
    /// </summary>
    public string[] OptionValueTags { get; set; } = { ":", "=" };

    public bool IgnoreCase { get; set; } = true;

    public StringComparison StringComparison =>
        this.IgnoreCase
            ? StringComparison.InvariantCultureIgnoreCase
            : StringComparison.InvariantCulture;


    /// <summary>
    ///     Get or set if unknown option tags provided in the command-line should be ignored.
    /// </summary>
    /// <remarks>
    ///     If set to <c>true</c> unknown options are ignored.<br />
    ///     Otherwise an error is added to the <see cref="ErrorDetailList">error collection</see>.
    /// </remarks>
    public bool IgnoreUnknownOptions { get; set; }



    /// <summary>
    ///     Get or set tags which identify an option.
    /// </summary>
    /// <remarks>
    ///     A command-line argument that starts with any of these character
    ///     is considered to be an <c>Option</c>.
    /// </remarks>
    public string[] OptionsTags { get; set; } = { "--", "-", "/" };


    /// <summary>
    ///     The width that is used to print a help text before a line break is inserted.
    /// </summary>
    public int HelpFullWidth { get; set; } = 80;

    /// <summary>
    ///     First column where help text starts.
    /// </summary>
    public int HelpAlignColumn { get; set; } = 20;



    public IEqualityComparer<string> GetStringComparer() =>
        this.IgnoreCase
            ? StringComparer.InvariantCultureIgnoreCase
            : StringComparer.InvariantCulture;
}