using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace serialization_intro
{
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
                Persons.Clear();
                await Task.Delay(1000); // See the cleared list
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
                Persons.Clear();
                await Task.Delay(1000); // See the cleared list
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

        string _filePathJson = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            Assembly.GetEntryAssembly().GetName().Name,
            "roster.json"
            );

        string _filePathCsv = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            Assembly.GetEntryAssembly().GetName().Name,
            "roster.csv"
            );

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if(!File.Exists(_filePathJson))
            {
                makeNewFile();
            }
            // Load the json file into a list in memory
            var json = File.ReadAllText(_filePathJson);

            labelName.DataBindings.Add("Text", listBox.DataSource, "Name");
            // Two-way binding for Description
            textBoxDescription.DataBindings.Add(
                "Text", 
                listBox.DataSource, 
                "Description",
                true, 
                DataSourceUpdateMode.OnPropertyChanged);
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
                            File.WriteAllText(_filePathJson, json);
                            listBox.Focus();
                        }
                        break;
                }
            };
        }

        private void makeNewFile()
        {
            var personsList = new List<Person>
            {
                new Person
                {
                    Name = "The Rock",
                    Description = "He is the most electrifying name in sports entertainment.",
                    InRingAbility = 80,
                    Charisma = 90,
                    Gender = "Male",
                    RingStyle = "Basic",
                    DrawValue = 100,
                    Condition = 80,
                    Weight = 220,
                    Morale = 100,
                    MicSkills = 100,
                },
                new Person
                {
                    Name = "MJF",
                    Description = "First-ever World Middleweight Champion at the Battle Riot special.",
                    Gender = "Male",
                },
                new Person
                {
                    Name = "David Flair",
                    Gender = "Male",
                },
                new Person
                {
                    Name = "Charlotte Flair",
                    Gender = "Female",
                },
                new Person
                {
                    Name = "Braden Putt",
                    Gender = "Male",
                },
            };
            Directory.CreateDirectory(Path.GetDirectoryName(_filePathJson));
            var json = JsonConvert.SerializeObject(personsList, Formatting.Indented);
            File.WriteAllText(_filePathJson, json);
            System.Diagnostics.Process.Start("notepad.exe", _filePathJson);
        }
    }

    class Person
    {
        public Person() { }

        public static implicit operator Person?(string csvLine)
        {
            if(csvLine == CsvHeader)
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
}
