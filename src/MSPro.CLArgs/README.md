# MSPro CLArgs Version History

## 2021-04-11

I have started and migrated some old project, called *CleanFolders* to use `CLArgs`. This project serves as a real world example, that demonstrates how to use `CLArgs` in most cases. Check it out here [CleanSolution CLArgs application](https://github.com/msc4266/CleanSolution).

While migrating this project, I recognized some limitations, which I have fixed immediately:

* *Targets* can now be used in a `CommandContext`.
  (See again the *CleanSolution* example for using *Targets*)
* BETA Support for `OptionTagValue` '  ' to allow blanks as option / value separator, like `/p "Parameter value" /x 5` instead of `/p:"Parameter value" /x:5`.
  Basically, this works; BETA because I haven't extensively tested all scenarios.
* Improved `Settings` to support help-text output for different consoles: `HelpAlignColumn` and `HelpFullWidth` properties.

## 2021-04-09

Released to make some updates available to the public. `CLArgs`is mainly used in my personal project on a daily basis. This project and the daily use of it, gives new ideas, servers as the best proof of concept you can have and it helps me to find and fix issues.

### Changes since last version in January

* Fixed bug with @ file handling
* Introduced *AllowMultiple* functionality
  (see [CleanSolution CLArgs application](https://github.com/msc4266/CleanSolution/blob/main/src/CleanSolution.Command/CommandContext.cs) to see how it is used)
* Refactored to use .net5.0 and C# 9.0
* Support for .net 5.0
* Help output pretty much improved
  * Support for '|' character in help-text to enforce line-breaks.
    (see [CleanSolution CLArgs application](https://github.com/msc4266/CleanSolution/blob/main/src/CleanSolution.Command/CommandContext.cs) to see how it is used)

