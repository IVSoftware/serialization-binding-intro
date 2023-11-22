using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using IVSoftware.Portable.Csv;

namespace serialization_intro
{
    public partial class MainForm : Form
    {
        BindingList<Person> Persons { get; } = new BindingList<Person>();
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
                        typeof(Person).GetCsvHeader(),
                    };
                    foreach (var person in Persons)
                    {
                        builder.Add(person.ToCsvLine());
                    }
                    Directory.CreateDirectory(Path.GetDirectoryName(_filePathCsv));
                    File.WriteAllLines(_filePathCsv, builder.ToArray());
                    Process.Start("notepad.exe", _filePathCsv);
                }
            };
            buttonFromCsv.Click += async (sender, e) =>
            {
                UseWaitCursor = true;
                await localClearAndWait();
                if (File.Exists(_filePathCsv))
                {
                    string[] lines = File.ReadAllLines(_filePathCsv);
                    if (lines.FirstOrDefault() is string header && header == typeof(Person).GetCsvHeader())
                    {
                        for (int i = 1; i < lines.Length; i++)
                        {
                            var line = lines[i];
                            if (!string.IsNullOrEmpty(line))
                            {
                                Persons.Add(typeof(Person).FromCsvLine<Person>(line));
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
                await localClearAndWait();
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
            async Task localClearAndWait()
            {
                if(Persons.Any())
                {
                    Persons.Clear();
                    await Task.Delay(1000); // Observe the cleared list
                }
            }
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

        string _filePathCsv = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            Assembly.GetEntryAssembly().GetName().Name,
            "roster.csv"
            );

        string _filePathJson = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            Assembly.GetEntryAssembly().GetName().Name,
            "roster.json"
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
