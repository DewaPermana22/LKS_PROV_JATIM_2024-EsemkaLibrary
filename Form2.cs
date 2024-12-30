using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsemkaLibrary_Dewa
{
    public partial class Form2 : Form
    {
        EsemkaLibraryEntities db = new EsemkaLibraryEntities();
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            dgvBook.DataSource = db.Book.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           var data = db.Book.Where( book => book.title.Contains(textBox1.Text)).ToList();
            dgvBook.DataSource = data;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (returnbtn.Index == e.ColumnIndex)
            {
                e.Value = "Borrow";
            }
        }
    }
}
