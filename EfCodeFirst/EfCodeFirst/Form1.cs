using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EfCodeFirst
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var con = new EfContext();

            dataGridView1.DataSource = con.Person.OrderBy(x => x.GebDate).ToList();
        }
    }

    public class EfContext : DbContext
    {
        public DbSet<Person> Person { get; set; }

    }



    public class Person //POCO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime GebDate { get; set; }
    }

}
