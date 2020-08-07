### Using statements

`using CommandLine` becomes `using MSPro.CLArgs`

### Option Attributes

In *CLArgs* `[Option()]` attributes become `[OptionDescriptor()]` attributes. Simply rename those attributes. *OptionDescriptor* starts with a name followed by the tag - switch parameter sequence - and the *Tag* is a string, not a character.

### Verb Attributes

Verb attributes annotate parameter classes with *CommandLine*, where a *Command* attribute annotates the *Command*.

**Old**

```csharp
[Verb( "CredentialManager", HelpText = "Manage credentials")]
class CredentialManagerArgs
{
	[Option('k', "key", HelpText = "The key...")]
	public string Key { get; set; }
```

**New - MsPro.CLArgs**

```csharp
[Command( "CredentialManager", HelpText = "Manage credentials")]
class CredentialManagerCommand : CommandBase<CredentialManagerArgs>
{
}
```



