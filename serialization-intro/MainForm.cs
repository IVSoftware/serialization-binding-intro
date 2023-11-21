using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml;

namespace serialization_intro
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Persons = new BindingList<Person>
            {
                new Person
                {
                    Name = "The Rock", 
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
            listBox.DataSource = Persons;
            listBox.DisplayMember = "Name";

            labelName.DataBindings.Add("Text", listBox.DataSource, "Name");
            listBox.SelectedValueChanged += (sender, e) =>
            {
            };
        }
        BindingList<Person> Persons { get; set; }
    }

    class Person
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("In Ring Ability")]
        public int InRingAbility { get; set; }

        [JsonPropertyName("Draw Value")]
        public int DrawValue { get; set; }
        public int Charisma { get; set; }
        public int Condition { get; set; }
        public string Alignment { get; set; } = string.Empty;
        public int Weight { get; set; }
        public string Gender { get; set; } = string.Empty;
        public int Morale { get; set; }

        [JsonPropertyName("Ring Style")]
        public string RingStyle { get; set; } = string.Empty;

        [JsonPropertyName("Mic Skills")]
        public int MicSkills { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
