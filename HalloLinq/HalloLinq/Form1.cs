﻿using Bogus;
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

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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

        private void button4_Click(object sender, EventArgs e)
        {
            var personMitAllenNachnamen = personen.Aggregate((a, b) => new Person() { Vorname = "NEU", Nachname = $"{a.Nachname}-{b.Nachname}" });

            var count = personen.Count(x => x.GebDatum.Month == 5);
            MessageBox.Show($"Mai gebtag: {count}");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var person = personen.FirstOrDefault(x => x.GebDatum.Month == 3);
            if (person != null)
            {
                MessageBox.Show(person.Nachname);
                bool istDrin = personen.Contains(person);
                bool hatEinerImMaiGebTag = personen.All(x => x.GebDatum.Month == 5);

                IEnumerable<DateTime> alleGebTage = personen.Select(x => x.GebDatum);
                var groups = personen.GroupBy(x => x.GebDatum.Month);


            }
            else
                MessageBox.Show("Nix");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var ano = new { Ding = 7, AnderesDings = "lalala" };

            var query1 = from p in personen
                         select new { Geb = p.GebDatum, Monat = p.GebDatum.Month };

            var query2 = personen.Select(x => new { Geb = x.GebDatum, Monat = x.GebDatum.Month });

            dataGridView1.DataSource = query1.ToList();


                    }

  

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                if( dataGridView1.CurrentRow.DataBoundItem is Person p)
                {
                    MessageBox.Show(p.Nachname);
                }
            }
        }
    }
}
