# Sample Commands

This Assembly contains some example commands, used by the several Demos.

It is good practice to place each command in a folder that represents the command's verb. For example: **MATH MULT** -> `.\Math\Mult\MultCommand.cs`. Of course, use a corresponding namespace:

```csharp
namespace SampleCommands.Math.Mult;

[Command("MATH.MULT", "Multiply two decimal values.")]
public class MultCommand : CommandBase2<MultContext>
{
```