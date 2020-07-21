# Verbs and Options

If you call `Arguments arguments = CommandLine.Parse(args);` you get 

* a list of *Verbs* and 
* a dictionary of *[Options](options.md)*. 

## Verbs 

- *Verbs* are single words found in the command line. 

- A *Verb* starts with a character.

- A *Verb* determines which functionality should be executed.


**Example:**

```
c:\> MyApp word1 text2 verb3

>>> Start Functionality
Command-Line: word1 text2 verb3
Verb[0] = 'word1'
Verb[1] = 'text2'
Verb[2] = 'verb3'
<<< End Functionality
```

There is also a `VerbPath` property that would return `word1.text2.verb3`.

`CLArgs` does not worry about what you are doing with these verbs. It is your application that makes use of what you have got.

```csharp
private static void Main(string[] args)
{
	Arguments arguments = CommandLine.Parse(args);
    switch (arguments.VerbPath)
    {
        if (arguments.VerbPath == "word1")
            word();
        else if (arguments.VerbPath == "word1.text2")
            text();
        else if (arguments.VerbPath == "word1.text2.verb3")
            verb();
        else
            Debug.Fail("Don't know what to do!");
    }
    ...
}
static void word() => Console.WriteLine("Function Word");
static void text() => Console.WriteLine("Function Text");
static void verb() => Console.WriteLine("Function Verb");
```

See [source-code](../../samples/Level1/Sample1.Verbs/Program.cs) / [sample project](../../samples/Level1/Sample1.Verbs) / [all samples](../../samples).

### What's next

* [Plugin concept with Verbs and Microsoft Composition](verbsWithComposition.md)
* [Options](options.md)
* [Level 2](../Level2/index.md)
