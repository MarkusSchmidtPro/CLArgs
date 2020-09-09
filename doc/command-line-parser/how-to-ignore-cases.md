---
description: Case-sensitive or ignore case behavior
---

# How to ignore cases

When the command-line is parsed the results are always case-sensitive \(see [Command Line Parser](./)\). Finally it depends how you want to use the parsed results. If you use the `Commander` object, you can simply provide a `Settings` object, to tell the `Commander` to ignore cases, when resolving _Verbs/Commands_ and _Options_.

```csharp
Commander commander = new Commander( new Settings {IgnoreCase = true, ..
```

The `Commander` and `Command.Execute()` will then ignore cases, and _Verbs_ and _Option_ tags will be case-ignorant \(see [Sample03.Options](https://github.com/msc4266/CLArgs/tree/master/samples/Sample03.Options/Program.cs)\). 

{% hint style="danger" %}
While Option _tags can be 'this or that',_  Option _values_ and _Targets_ are always case-sensitive!
{% endhint %}



