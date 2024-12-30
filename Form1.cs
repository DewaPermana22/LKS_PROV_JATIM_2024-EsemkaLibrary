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
    public partial class Form1 : Form
    {
        int totalDenda = 2000;
        int hariTela;
        private readonly EsemkaLibraryEntities db = new EsemkaLibraryEntities();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "Irwin Acheson";
            dgvBorrow.Clear();
            label2.Text = DateTime.Today.ToString("dddd-MMMM-yyyy");
            timer1.Start();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var user = db.Member.FirstOrDefault(m => m.name == textBox1.Text && m.deleted_at == null).id;
                if (user != 0)
                {
                    var data = db.Borrowing.Where(b => b.member_id == user).ToList();
                    dgvBorrow.DataSource = data;
                    var pinjam = db.Borrowing.Where(p => p.member_id == user && p.return_date == null).Select(s => DateTime.Today - s.borrow_date).FirstOrDefault();
                    int bolehGak = db.Borrowing.Count(m => m.member_id == user && m.return_date == null);
                    button2.Enabled = bolehGak < 3;
                }
                else
                {
                    MessageBox.Show("Kesalahan");
                }
            }
            catch (Exception)
            {

            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is Borrowing borrow)
            {
                if (bookidDataGridViewTextBoxColumn.Index == e.ColumnIndex)
                {
                    e.Value = borrow.Book.title;
                }
                if (returnbtn.Index == e.ColumnIndex)
                {
                    e.Value = "return";
                }
                if (returndateDataGridViewTextBoxColumn.Index == e.ColumnIndex)
                {
                    if (e.Value != null)
                    {
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                    }
                    else
                    {
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                if (overdue_days.Index == e.ColumnIndex)
                {
                    if (borrow.return_date == null)
                    {
                        hariTela = (DateTime.Today - borrow.borrow_date).Days;
                        e.Value = hariTela;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is Borrowing borrow)
            {
                if (e.ColumnIndex == returnbtn.Index)
                {
                    int totalan = hariTela * totalDenda;
                    DialogResult res = MessageBox.Show("Succes Return : " + borrow.Book.title + "\n" + "Member Need To pay : "+totalan.ToString("C2", new System.Globalization.CultureInfo("id-ID")));
                    if (res == DialogResult.OK)
                    {
                        borrow.return_date = DateTime.Today;
                        db.SaveChanges();
                        MessageBox.Show("Succes!");
                        OnLoad(null);
                    }
                }
            }
            
        }
    }
}
