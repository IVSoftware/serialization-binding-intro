# Csv Serialization

A key precursor to **populate a ListBox with values from a custom class** is the ability of your `Person` class to save and load ([Serialize](https://learn.microsoft.com/en-us/dotnet/standard/serialization/)) with respect to a given a comma delimited string _(then once you do this see **Usage** section below for loading the `ListBox`)_. 

There are two important considerations, however.

- How to know with precision "what value goes where"
- What if the actual value contains a comma?

To address these concerns, an assumption will be made that we're dealing with a properly-formatted **.csv** file that includes a **header** row. This provides an index of how to deserialize it, and without it there is no assurance that we're putting the puzzle pieces back together in the right order. In addition, values that contain commas as part of the value will need to be  [_escaped_](https://hawq.apache.org/docs/userguide/2.3.0.0-incubating/datamgmt/load/g-escaping-in-csv-formatted-files.html#:~:text=By%20default%2C%20the%20escape%20character,declare%20a%20different%20escape%20character.).

**CSV File with Header and Escaping**
```
Name,Description,InRingAbility,DrawValue,Charisma,Condition,Alignment,Weight,Gender,Morale,RingStyle,MicSkills
The Rock,"He is the most electrifying name in sports entertainment. Movie star, too. ",80,100,90,80,,220,Male,100,Basic,100
MJF,First-ever World Middleweight Champion at the Battle Riot special.,0,0,0,0,,0,Male,0,,0
David Flair,,0,0,0,0,,0,Male,0,,0
Charlotte Flair,,0,0,0,0,,0,Female,0,,0
Braden Putt,The rising star that everyone is watching.,0,0,0,0,,0,Male,0,,0
```


I'm familiar with your other [post](https://stackoverflow.com/q/77519303/5438626) showing that there's nothing to stop the user from including commas when editing the `Description` field. This is why escaping is necessary in **.csv**.
___
_Using JSON would mean 'not' having to worry about the order or about escaping commas_. 
___

[![screenshot][1]][1]

___


**Provide Header**

First, converting in either direction will benefit if `Person` can generate the file header with the property names (as a **static** property of the class).

```
class Person
{
    /// <summary>
    /// Converter to CSV string for Property Names
    /// </summary>
    public static string[] CsvHeaderNames
    {
        get
        {
            if (_csvHeaderNames is null)
            {
                _csvHeaderNames =
                    typeof(Person)
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .Select(_ => _.Name)
                        .ToArray();
            }
            return _csvHeaderNames;
        }
    }
    static string[]? _csvHeaderNames = null;

    public static string CsvHeader => string.Join(",", CsvHeaderNames);
    .
    .
    .
}
```
___

**Import from CSV single line**

This block gives the `Person` class the ability to construct an instance given a _single line_ of the **.csv** file. We'll need a [RegEx](https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=net-8.0) expression to ignore the escaped commas, and an additional helper to remove double-quotes that have been appended in the process.
```
class Person
{
    public Person() { }

    public static implicit operator Person?(string csvLine)
    {
        if (csvLine == CsvHeader)
        {
            // Can't make a person from the header row.
            return null;
        }
        else
        {
            var person = new Person();
            var values = Regex.Split(csvLine, IgnoreEscapedCommas);
            for (int i = 0; i < CsvHeaderNames.Length; i++)
            {
                var propertyName = CsvHeaderNames[i];
                var value = localRemoveOutsideQuotes(values[i]);
                if (typeof(Person).GetProperty(propertyName) is PropertyInfo pi)
                {
                    switch (pi.PropertyType.Name)
                    {
                        case nameof(Int32):
                            pi.SetValue(person, Int32.Parse(value));
                            break;
                        case nameof(String):
                            pi.SetValue(person, value);
                            break;
                        default:
                            Debug.Assert(false, "An unhandled type has been added to this class.");
                            break;
                    }
                }
                string localRemoveOutsideQuotes(string s)
                {
                    if (s.Contains(',') && s.StartsWith("\"") && s.EndsWith("\""))
                    {
                        return s.Substring(1, s.Length - 2);
                    }
                    else return s;
                }
            }
            return person;
        }
    }
    .
    .
    .
}
```
___

**Export to single CSV line**

This block overrides the `ToString()` method so that `Person` enumerates its own properties. This is where we need to make sure that "values containing commas" are surrounded by double quotes to escape them.

```
   ...
    const string IgnoreEscapedCommas = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";

    /// <summary>
    /// Converter to CSV string for values
    /// </summary>
    public override string ToString()
    {
        return
            string.Join(
                ",",
                typeof(Person)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Select(_ => localEscape(_.GetValue(this)?.ToString()??string.Empty)));
        string localEscape(string mightHaveCommas)
        {
            if (mightHaveCommas.Contains(","))
            {
                return $@"""{mightHaveCommas}""";
            }
            else return mightHaveCommas;
        }
    }
    ...
```


___

**Usage**

Here are some serialize methods for you to experiment with.

```
public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        listBox.DataSource = Persons;
        listBox.DisplayMember = "Name";
        buttonToCsv.Click += (sender, e) =>
        {
            if (Persons.Any() || warnEmpty())
            {
                List<string> builder = new List<string>
                {
                    Person.CsvHeader,
                };
                foreach (var person in Persons)
                {
                    builder.Add(person.ToString());
                }
                Directory.CreateDirectory(Path.GetDirectoryName(_filePathCsv));
                File.WriteAllLines(_filePathCsv, builder.ToArray());
                Process.Start("notepad.exe", _filePathCsv);
            }
        };
        buttonFromCsv.Click += async (sender, e) =>
        {
            UseWaitCursor = true;
            await Task.Delay(1000);
            Persons.Clear();
            if (File.Exists(_filePathCsv))
            {
                string[] lines = File.ReadAllLines(_filePathCsv);
                if (lines.FirstOrDefault() is string header)
                {
                    for (int i = 1; i < lines.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(lines[i]))
                        {
                            Persons.Add(lines[i]);
                        }
                    }
                }
                else
                {
                    Debug.Assert(false, "Expecting header");
                }
            }
            UseWaitCursor = false;
        };
        buttonFromJson.Click += async(sender, e) =>
        {
            UseWaitCursor = true;
            await Task.Delay(1000);
            Persons.Clear();
            if (File.Exists(_filePathJson))
            {
                var persons = 
                JsonConvert
                .DeserializeObject<BindingList<Person>>(
                    File.ReadAllText( _filePathJson)
                );
                foreach (var person in persons)
                {
                    Persons.Add(person);
                }
            }
            UseWaitCursor = false;
        };
        buttonToJson.Click += (sender, e) =>
        {
            if (Persons.Any() || warnEmpty())
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filePathJson));
                File.WriteAllText(_filePathJson, JsonConvert.SerializeObject(Persons, Formatting.Indented));
                Process.Start("notepad.exe", _filePathJson);
            }
        };
    }
    bool warnEmpty()
    {
        return !DialogResult.Cancel.Equals(
            MessageBox.Show(
                "Person list is empty. Save anyway?",
                "Confirm",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question
            ));
    }
    BindingList<Person> Persons { get; } = new BindingList<Person>();
}
```


  [1]: https://i.stack.imgur.com/Z5x79.png