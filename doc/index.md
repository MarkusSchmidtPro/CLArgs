# Modern Console Application Design

`dotnet` itself is a command-line application and a great example how modern console applications should work: `c:\> dotnet build --help`

```
Usage: dotnet build [options] <PROJECT | SOLUTION>

Arguments:
  <PROJECT | SOLUTION>   The project or solution file to operate on. If a file is not specified, the command will search the current directory for one.

Options:
  -h, --help               	Show command line help.
  -o, --output <OUTPUT_DIR> The output directory to place built artifacts in.
	...
```

* Support --help.
* There is one or more Verb(s): *build*
* There are several *Options*, which have a name and a value: *output=<OUTPUT_DIR>*
  * Options are mandatory or optional
  * Different verbs may require different option "sets"
* Options can be represented by different Tags: *-o, --output*
* etc., etc., etc.

`CLArgs` supports all these features and it provides a very flexible and extensible model to work with the command-line. 

## What's next

* [The Mission - what I wanted to accomplish is **what you get**](mission.md)
* [The four levels of using CLArgs](howToUse.md)


