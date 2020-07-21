# How it works in detail

This chapter is for all those, who want to understand how it works, to make the best out of it. All others who need a tool to parse a command-line go ahead and check [Use-Cases and examples](UseCases/index.md).

## Introduction

`CLArgs` does a lot of things behind the scenes from:
`void Main( string[] args)`

1. to parse the command-line text,
2. to create an instance of your parameter object from it and 
3. to finally call the right command for the given verb(s)

`Command.OnExecute(MyParams p)`

Behind the scenes `CLArgs` performs different steps, and you can take over control whenever you want it.

### The steps

1. [Parse the command-line string](argumentParser.md) (args[]) 

   2. The tokens in a command-line are called *command-line arguments*.
   2. They are split into *Verbs* and *OptionTags*

   > An Option has a *Name* and one or more *Tags*. 
   >
   > For example: `Name="SourceFile", Tags=source, f, sf`. In a command-line you can provide either the option's name or any of the tags.
   >
   > When parsing the command-line in the first instance, you get option-tags. In the seconds instance, based on *Option Descriptors* those *Tags* are aggregated into an Option (by Name). 

### Command Base

For your convenience, `CommandBase` implements the following steps. However, you can provide your own implementation and 'step-in' whenever you need it.

2. Take a [list of option descriptors](optionDescriptors.md) 

   1. Check and validate the provided arguments against
      the known and expected options

> This is, for example, an *OptionDescriptorAttribute* to decorate properties: `OptionDescriptor("SourceFile", Required = false, Tag = "src" )]`

3. Consolidate the option tags, as provided in the command-line,
and [build the option-list](optionList.md) (by name)
  
   > For example, command-line arguments like `--f, -sf, --source` will be collected and aggregated into a single option, named: *SourceFile*. 

4. Create a new instance of the command parameter object

5. Take all Options (by name)
   1. find their matching property on the command parameter object
   2. convert the option's value (which still string) to the property's type
      1. [Type converter](convertValues.md) for standard types (string, int, enum, DateTime) are available
      2. You can provide your own or override the default implementation
   3. and assign it to the property

6. Now, that we have got a typed parameter object, it could happen, some properties were not provided in the command-line and we have to [resolve missing arguments](resolveArguments.md) to provide dynamic defaults or default that depends on other values.

7. Finally, we check if we [collected any error](errorList.md)
  
   1. to throw an *AggregateException*
   
8. or to [execute the Command](executeCommand.md)

### CommandBase implementation

   ``` csharp
void ICommand.Execute(Arguments arguments, bool throwIf)
{
   // Step 2
	var options = CommandLineOptions.FromArguments<TCommandParameters>(arguments);

   this.Errors.Add(options.Errors);
   if (!this.Errors.HasErrors())
   {
   	// Step 3, 4, 5
       CommandLineOptionsConverter converter = new CommandLineOptionsConverter();
       var commandParameters = converter.ToCommandParameters<TCommandParameters>(options, out List<string> unresolvedProperties);

       this.Errors.Add(converter.Errors);
       if (!this.Errors.HasErrors())
       {
           // Step 6
       	OnResolveProperties(commandParameters, unresolvedProperties);
           if (!this.Errors.HasErrors())
           {
               // Step 7
               OnExecute(commandParameters);
           }
        }
    }

    if (!this.Errors.HasErrors() || !throwIf) return;

    throw new AggregateException(this.Errors.Details.Select(
        e => new ArgumentException(e.ErrorMessages[0], e.AttributeName)));
}
   ```

   