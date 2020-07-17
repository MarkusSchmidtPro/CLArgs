# Verbs with Microsoft.Composition

Normally, I use Microsoft's `System.Composition` NuGet package to realize a *Plug-In* concept for *Verbs*. This allows me to annotate a class like follows:

```csharp
// The exported name is the verb!

[Export("HelloWorld", typeof(IVerbRunner))]
class HelloWorldCommand : IVerbRunner
{
	public void Run(Arguments arguments) => Console.WriteLine("This is my HelloWorld command.");
}   

[Export("HelloGermany", typeof(IVerbRunner))]
class HelloGermanyCommand : IVerbRunner
{
	public void Run(Arguments arguments) => Console.WriteLine("This is my HelloGermany command.");
}
```

I can then use a `serviceContainer` to resolve any functionality by name (by a Verb).

Et voilà: simply create new classes, give them a name and you can use them via a Verb. 

```csharp
private static void Main(string[] args)
{
    Arguments arguments = CommandLine.Parse(args);
    Console.WriteLine($"Command-Line: {arguments.CommandLine}");
    
    var configuration // build the container, collect [Exports]
        = new ContainerConfiguration().WithAssembly(Assembly.GetExecutingAssembly());
    var serviceContainer = configuration.CreateContainer();

    Console.WriteLine(">>Start Functionality");
    var command= serviceContainer.GetExport<IVerbRunner>( arguments.Verbs[0]);
    command.Run( arguments);
    Console.WriteLine("<<< End Functionality");
}
```

Output will be 

```
Command-Line: HelloWorld
>>Start Functionality
This is my HelloWorld command.
<<< End Functionality
```

See also Sample Project: `Level1\Sample1.Composition`. 

> Please refer to Microsoft's documentation to get used to `System.Composition`. 
>
> Basically all classes annotated with the `Export` attribute will be collected in the service container.

## What's next

* [Level 2](doc/level2.md)