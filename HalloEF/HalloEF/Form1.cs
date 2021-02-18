using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace HalloEF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var context = new NORTHWNDEntities();

            dataGridView1.DataSource = context.Orders.Include(x=>x.Employee).OrderBy(x => x.OrderDate).ToList();
        }
    }
}
