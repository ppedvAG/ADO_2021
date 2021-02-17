using Bogus;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HalloLinq
{
    public partial class Form1 : Form
    {
        List<Person> personen = new List<Person>();

        public Form1()
        {
            InitializeComponent();

            var faker = new Faker<Person>("de")
                            .UseSeed(1234)
                            .RuleFor(x => x.Vorname, f => f.Name.FirstName())
                            .RuleFor(x => x.Nachname, f => f.Name.LastName())
                            .RuleFor(x => x.Stadt, f => f.Address.City())
                            .RuleFor(x => x.Id, f => f.Random.Int(0))
                            .RuleFor(x => x.GebDatum, f => f.Date.Past(50));

            personen.AddRange(faker.Generate(100));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = personen;
        }
    }
}
