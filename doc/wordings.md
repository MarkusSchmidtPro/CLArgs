# Wordings

## Args[] = Environment.CommandLine

The `args[]` passed to the `Program`. Basically the command-line itself. *Args* can be anything in the beginning, and the first action on it should be 

```csharp
private static int Main(string[] args)
{
	...
	Arguments arguments = Commander.ParseCommandLine(args);`
	...
```

## Arguments

Arguments is the result when the command-line was parsed. An *Argument* is either an *Option* or a *Verb*. 

### Verb

A *Verb* is initially just a text, that was parsed as a single word from the command-line.

Example `saveFile`

*Verbs* are used to find an appropriate *Command* implementation that can be executed.

### Option

An *Option* is a name-value tag that was parsed from the command-line. Basically the values which are passed as parameters to the command. Options values are always strings!

Example: `--opt1=44 --filename="myfile.xml"`

## Command

A *Command* is the implementation of an action that is finally executed. When an instance of a *Command* is created it gets the *Options*. The Command then validates the *Options* and maps them to the command's *Parameters*.

## Parameters

A *Command* gets *Parameters* which is an object that was build based on the Options. 

