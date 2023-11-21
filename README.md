# Serialization Intro

Your post talks about the loading and saving of the data ([Serialization](https://learn.microsoft.com/en-us/dotnet/standard/serialization/)) but reading your issue carefully that doesn't sound like what's causing trouble. Rather, there's a disconnect where you input some description text and it's not quite connecting back to the object that you're actually saving when you save. You mention being new to C# and if you haven't already heard the term "Binding" you soon will. As the name suggests it means that the UI control (like a text editor) and the data that is its target are wired to notify each other. Synchronized this way, it's impossible to have the kind of discrepancy you're seeing.

We could make an argument that your app doesn't deal with a "file" at all. It deals with a roster of persons that can be abstracted as `BindingList<Person>` that will be the `DataSource` of `listBox` if it's not aleady. You describe a person as a "set of eleven values" but we really want to nail this down.

```
class Person
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [Description("In Ring Ability")]
    public int InRingAbility { get; set; }

    [Description("Draw Value")]
    public int DrawValue { get; set; }
    public int Charisma { get; set; }
    public int Condition { get; set; }
    public string Alignment { get; set; } = string.Empty;
    public int Weight { get; set; }
    public string Gender { get; set; } = string.Empty;
    public int Morale { get; set; }

    [Description("Ring Style")]
    public string RingStyle { get; set; } = string.Empty;

    [Description("Mic Skills")]
    public int MicSkills { get; set; }
}
```

