## How to use CLArgs

There are four levels of using `CLArgs`

### Level 1 - Basic

Level 1 is a bit more than just `string[] args`.  Simply parse your command-line into `Arguments` and work with *Verbs* and *Options*:

```csharp
c:\> YourApp --fileName="myfile.csv" --target=XML
Arguments arguments = CommandLine.Parse(args);
```

See also 

* [Level 1 Details](Level1/index.md)
* [Verbs and Options](Level1/verbsAndOptions.md)
* [Console *Plugin concept* with Verbs and Microsoft Composition](Level1/verbsWithComposition.md)

### Level 2 - Standard

Parse the command-line into a typed object and pass it to your Command (as shown in the code above). A Command represents the functionality (that is normally  `void Main`).  However, Commands can be bound to *Verbs* so that one console app can support multiple functionalities with different argument sets.

The *Standard Level* supports (multiple) *Verbs* and *Options*, and it is probably the right choice for most of you. I am using `CLArgs` at this level. [More a about Level 2](Level2/index.md)

> Most other command-line solutions I have seen, work between Level 1 and 2.

### Level 3 - Advanced

All those who are still not happy with *Level 2*, who need more flexibility or who have other special requirements: a) let me know, what is missing and then enter b) Level 3. `CLArgs` has several hooks and /or extension points where you can integrate your code, and you can use the `CLArgs` classes and functions as you want it. [More a about Level 3](Level3/index.md)

### Level 4 - Expert

Check out the source-code and use it for your convenience. Don't forget to [let me know](mailto:markus@markusschmidt.pro) what you would make better.
