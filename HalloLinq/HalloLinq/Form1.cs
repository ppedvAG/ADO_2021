using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tools;

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

        private void button2_Click(object sender, EventArgs e)
        {
            var query = from p in personen
                        where p.GebDatum.Year >= 1990 && p.Stadt.StartsWith("B")
                        orderby p.GebDatum.Year descending, p.Nachname
                        select p;

            dataGridView1.DataSource = query.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int kw = DateTime.Now.GetKW(); //erweiterungsmethode

            dataGridView1.DataSource = personen.Where(p => p.GebDatum.Year >= 1990 && p.Stadt.StartsWith("B"))
                                               .OrderByDescending(nichtP => nichtP.GebDatum.Year)
                                               .ThenBy(x => x.Nachname)
                                               .ToList();

        }
    }
}
