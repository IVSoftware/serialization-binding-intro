using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;

namespace serialization_intro
{
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
            // Load the json file into a list in memory
            var json = File.ReadAllText(_filePath);
            Persons = JsonConvert.DeserializeObject<BindingList<Person>>(json);

            listBox.DataSource = Persons;
            listBox.DisplayMember = "Name";

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
                            File.WriteAllText(_filePath, json);
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
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
            var json = JsonConvert.SerializeObject(personsList, Formatting.Indented);
            File.WriteAllText(_filePath, json);
            System.Diagnostics.Process.Start("notepad.exe", _filePath);
        }
    }

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
}
