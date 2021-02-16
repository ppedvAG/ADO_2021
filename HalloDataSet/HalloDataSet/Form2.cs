using HalloDataSet.DataSet1TableAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HalloDataSet
{
    public partial class Form2 : Form
    {
        DataSet1 ds = new DataSet1();
        EmployeesTableAdapter ada = new EmployeesTableAdapter();
        BindingSource bs = new BindingSource();

        public Form2()
        {
            InitializeComponent();

            textBox2.DataBindings.Add("Text", textBox1, "Text");
            textBox2.DataBindings.Add(nameof(textBox1.BackColor), textBox1, "Text", true);

            ada.Fill(ds.Employees);
            bs.DataSource = ds.Employees;
            dataGridView1.DataSource = bs;

            textBox3.DataBindings.Add("Text", bs, "FirstName");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bs.MoveNext();
        }
    }
}
