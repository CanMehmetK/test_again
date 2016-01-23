using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IdeativeWinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string strCode = textBox1.Text;

            Ideative.Dinamik.CodeCompiler.DebugMode = false;
            Ideative.Dinamik.CodeCompiler.CodeActionsInvoker(txtMyClass.Text, strCode);
            myClass.Execute();

        }

        public static class myClass
        {
            public static void Execute()
            {
                Debug.WriteLine("sdfsfd");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
