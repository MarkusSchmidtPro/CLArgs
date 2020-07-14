# CLArg - A dotnet command-line interpreter

C-Sharp Console Applications are simple:  `static void Main(string[] args)` ..  and go!

Getting arguments from a command-line can be complex. `CLArgs` takes this complex part to keep your console application simple.

> I thought it was time to reinvent the wheel!
>
> Not the (Flint) stone wheel, not the wooden one and not the rubber one. There are so many command-line interpreters out there. I wanted to invent the wheel with air inside, where you can chose the right pressure you need - one fits all. 
>
> Let's reinvent the way how to build command-line apps.

## Four levels of using it

There are four levels of using `CLArgs`

### Level 1 - Basic

A bit more than just `string[] args`. 

Simply parse your command-line into `Arguments` and work with *Verbs* and *Options*:

```
c:\> YourApp --fileName="myfile.csv" --target=XML
Arguments arguments = CommandLine.Parse(args);
```

### Level 2 - Standard

Parse the command-line into a typed object and pass it to your Command. A Command implements the functionality (as `void Main` would do).  However, Commands can be bound to *Verbs* so that one console app can support multiple functionalities.  

The *Standard Level* supports (multiple) *Verbs* and *Options*, and it is probably the right choice for most of you. I am using `CLArgs` at this level. 

> Most other command-line solutions I have seen, work between Level 1 and 2.

### Level 3 - Advanced

All those who are still not happy with *Level 2*, who need more flexibility or who have other special requirements: a) let me know, what is missing and then enter b) Level 3. `CLArgs` has several hooks and /or extension points where you can integrate your code, and you can use the `CLArgs` classes and functions as you want it.

### Level 4 - Expert

Check out the source-code and use it for your convenience. Don't forget to [let me know](mail:markus@markusschmidt.pro) what you would make better.

## Feature List

* Unlimited number of *Verbs*
* Plug-In concept: Automatic Command resolution based on *Verbs*
* Different *argument-sets* for each Command
* Flexible arguments *validation and completion*
* Clean error-handling to report *all* errors instead of only the first one
  * When using a console application, 
    It can be frustrating to see "*Param1 is missing*" and 
    once you fixed it you get: "*Param2 is missing*".
  * `CLArgs` reports all errors in a single run.
* Support for command and argument help-text
  * Help-Texts and argument definitions can be loaded from files, from Resources or they can be build-in by code or a combination of these. 
  * This includes support for localized help-messages.
* Support for custom property types and enums in your parameter classes
* Dynamics default values (not only static, like True, "abc")
  * Including depend default values, e. g. on other values
* ...
<hr/>
<sub>Markus Schmidt (PRO), Munich (DE), 2020-07-10</sub>