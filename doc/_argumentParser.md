## Argument Parser

1. 1. The command-line supports different tokens for an option: - -- /
      1. Tokens can be provided and set as needed - whatever token you prefer   
   2. @ options are recognized as [option file](optionFile.md) - this can't be changed!
   3. all tokens that start with a character are *Verbs*







The first action that takes place is parsing the command-line, which is just a string. The *Parser* splits the string into several tokens. A *Token* can become either a *Verb* or an *Option*, and the result is called: `Arguments arguments = Commander.ParseCommandLine( args);`

`Arguments` contains a *List* of `Verbs` and a *Dictionary* of `Options`. All values are of type string: we are parsing, not converting, yet.

> Example: `app.exe convert --src="abc.csv" --lines=5`
>
> Verb[0]:	"convert"
> Option[ "src"]: 	"abc.csv"
> Option[ "lines"] : "5"

Here is your first *hook*