# Serialization + Binding Intro

Your post talks about the loading and saving of the data ([Serialization](https://learn.microsoft.com/en-us/dotnet/standard/serialization/)) but that doesn't sound like the root issue causing trouble. Rather, there's a disconnect where you input some description text and it's not quite connecting back to the object that you're actually saving when you save. You mention being new to C# and if you haven't already heard the term [Binding](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.binding?view=windowsdesktop-7.0) you soon will. As the name suggests it means that the UI control (like a text editor) and the data that is its target are wired to notify each other. Synchronized this way, it's impossible to have the kind of discrepancy you're seeing. The code below will wire changes in the `ListBox` to notify the other controls without having to write all that synchronization by hand.


[![bindings][1]][1]


___

The argument could be made that your app doesn't deal with a "file" at all. It deals with a **roster** of **persons** that can be abstracted as `BindingList<Person>` that will be the `DataSource` of `ListBox` if it's not aleady. You describe a person as a "set of eleven values" but we really want to nail this down.

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
___

`ListBox` will manage a list of `Person` objects where `Name` is the property displayed in the list. As far as the text file, one way or another the aim here would be to turn it into a living breathing `List` in memory, and the reason I mentioned Json is for the ease (1 or 2 lines) with which this is done. The `ListBox` now shows the 5 names, and `listBox.SelectedValue` is a `Person` not a string.

```
public partial class MainForm : Form
{
    public MainForm() => InitializeComponent();

    string _filePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        Assembly.GetEntryAssembly().GetName().Name,
        "roster.json"
        );
    BindingList<Person> Persons { get; set; }
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if(!File.Exists(_filePath))
        {
            makeNewFile();
        }
        // Load the json (or other format) file into a List in memory
        var json = File.ReadAllText(_filePath);
        Persons = JsonConvert.DeserializeObject<BindingList<Person>>(json);

        listBox.DataSource = Persons;
        listBox.DisplayMember = "Name";
        .
        .
        .
    }
}
```

___

Adding some bindings to your UI controls will make Selection Change in the list box automatically reflect in those controls.




```
    .
    .
    .
    labelName.DataBindings.Add("Text", listBox.DataSource, "Name");

    // Two-way binding for Description
    textBoxDescription.DataBindings.Add(
        "Text", 
        listBox.DataSource, 
        "Description",
        true, 
        DataSourceUpdateMode.OnPropertyChanged);
    .
    .
    .
```

As a suggestion, the way I would personally do the Description editing is to turn the description "label" into an editable control with a double click.

[![double-click to edit][2]][2]

```
    .
    .
    .    
    textBoxDescription.MouseDoubleClick += (sender, e) =>
    {
        if(textBoxDescription.ReadOnly)
        {
            textBoxDescription.ReadOnly = false;
            textBoxDescription.BackColor = Color.White;
            textBoxDescription.ForeColor = Color.Black;
        }
    };
    textBoxDescription.KeyDown += (sender, e) =>
    {
        switch(e.KeyData)
        {
            case Keys.Enter:
                e.Handled = e.SuppressKeyPress = true;
                if(!textBoxDescription.ReadOnly)
                {
                    textBoxDescription.ReadOnly = true;
                    textBoxDescription.BackColor = Color.FromArgb(0,0,192);
                    textBoxDescription.ForeColor = Color.White;
                    var json = JsonConvert.SerializeObject(Persons, Formatting.Indented);
                    File.WriteAllText(_filePath, json);
                }
                break;
        }
    };
    .
    .
    .
```

When you reload the app, the changes will persist the way you intended.


  [1]: https://i.stack.imgur.com/csAIQ.png
  [2]: https://i.stack.imgur.com/nfXP4.png