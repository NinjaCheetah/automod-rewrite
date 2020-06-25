using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoMod
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Visible = true;
            Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Form3 f = new Form3();
                f.Visible = true;
            }
            else if (radioButton2.Checked == true)
            {

                string message = "The program has encountered a fatal error and will now close.";
                string title = "Fatal Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    System.Windows.Forms.Application.Exit();
                }
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
