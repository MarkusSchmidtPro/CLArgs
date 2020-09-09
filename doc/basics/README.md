---
description: How Options become Properties become Parameters
---

# Options

An _Option_ is a tag-value \(or name-value\) token that is parsed from the command-line:

`--filename=Input.xml --out='outDir\' --forceOverride`

{% hint style="info" %}
An Option without an explicit value is resolved to _boolean: true_  
like _--forceOverride_ in the example above.
{% endhint %}

For each _Option_ provided in the command-line the `Commander` tries to find an appropriate `OptionDescriptor`. _OptionDescriptors_ are automatically resolved by searching for the corresponding Annotations, or they can be added manually to the `Commander`\(see [How Commander uses Annotations](../the-commander/how-commander-uses-annotations.md)\) .

An `OptionDescriptor` describes an _Option_. An _OptionDescriptor_ determines if an _Option_ is required, the recognized _Option_ names and tokens, its data type, the help text and its [static default value](dynamic-default-values.md). 

A group of options is called _Parameters_ class which is finally passed to a _Command_.

```csharp
class XmlConverterParameters
{
    [OptionDescriptor("filename", "f", Required = true)]
    public string Filename { get; set; }

    [OptionDescriptor("out", "o", Required = false, Default = "out")]
    public string OutDir  { get; set; }

    [OptionDescriptor("forceOverride", "fo", 
                      Required = false, Default = true)]
    public bool ForceOverride { get; set; }
}
```

{% hint style="info" %}
\_\_[_Parameters_ ](../command-parameters.md)classes do not need annotations. A _Parameters_ class simply acts as a container for all _Options_ which belong to a Command. The **Properties** in a _Parameters_ class **take the** _**Option**_ **values** from command-line.
{% endhint %}

