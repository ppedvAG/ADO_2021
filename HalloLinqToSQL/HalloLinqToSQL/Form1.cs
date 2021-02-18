using System;
using System.Data;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace HalloLinqToSQL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //DataClasses1DataContext context = new DataClasses1DataContext();//nicht so lange leben lassen wie das Programmm läuft
        DataClasses1DataContext context = null;

        private void button1_Click(object sender, EventArgs e)
        {
            context = new DataClasses1DataContext(); //Unit-of-Work wird mit den laden gestartet (cache dauer von LazyLoading)

            var query = context.Employees
                               .Where(x => x.FirstName.Length > 3)
                               .OrderBy(x => x.BirthDate)
                               .Take(100);

            Debug.WriteLine(query.ToString());

            dataGridView1.DataSource = query.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            context.SubmitChanges();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow.DataBoundItem is Employee emp)
            {
                MessageBox.Show($"{emp.LastName}");
                //MessageBox.Show($"{emp.LastName} {string.Join(", ", emp.Orders.Select(x => x.OrderDate.Value.ToString("d")))}"); //wenn order == DbNull, dann NullRef Exception

                var orderDates = string.Join(", ", emp.Orders.Where(x => x.OrderDate.HasValue).Select(x => x.OrderDate.Value.ToString("d")));
                MessageBox.Show($"{emp.LastName} {orderDates}");
                //foreach (var order in emp.Orders)
                //{
                //    MessageBox.Show(order.OrderDate.ToString());
                //}
            }
        }

        //public  Nullable<int> NullableInt { get; set; }
        public int? NullableInt { get; set; }

        public DateTime Datum { get; set; } = DateTime.MinValue;
        public int Zahl { get; set; }
        public bool Bool { get; set; }

        private void button3_Click(object sender, EventArgs e)
        {
            var newEmp = new Employee()
            {
                FirstName = "Wilma",
                LastName = "Feuerstein",
                BirthDate = DateTime.Now.AddYears(-30)
            };

            context.Employees.InsertOnSubmit(newEmp);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            context = new DataClasses1DataContext();
            //context.DeferredLoadingEnabled = false; //lazy loading deaktivieren
            
            var op = new DataLoadOptions();         //eager Loading
            op.LoadWith<Order>(x => x.Employee);    //eager Loading
            op.LoadWith<Order>(x => x.Customer);    //eager Loading
            op.LoadWith<Order>(x => x.Shipper);     //eager Loading
            context.LoadOptions = op;               //eager Loading
            dataGridView1.DataSource = context.Orders.ToList();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value is Employee emp)
            {
                e.Value = $"{emp.FirstName} {emp.LastName}";
            }
        }
    }
}
