using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server_Support;

namespace ServerGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        TCP_Server_Asynch moj;
        private bool uruchomiony = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = "127.0.0.1";
            this.textBox2.Text = "2048";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(uruchomiony==false)
            {
                uruchomiony = true;
                moj = new TCP_Server_Asynch(IPAddress.Parse(this.textBox1.Text), Int32.Parse(this.textBox2.Text));
                Thread MyThread = null;
                try
                {
                    ThreadStart ThreadMethod = new ThreadStart(moj.Start);
                    MyThread = new Thread(ThreadMethod);
                }
                catch (Exception d)
                {
                    return;
                }
                try
                {
                    MyThread.Start();
                }
                catch (Exception d)
                {

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = listView1.Items.Count - 1; i >= 0; i--)
            {
                listView1.Items[i].Remove();
            }
            for (int i=0; i<moj.lista_zalogowanych.Count();i++)
            {
                ListViewItem item1 = new ListViewItem(moj.lista_zalogowanych[i].ToString());
                if(!listView1.Items.ContainsKey(moj.lista_zalogowanych[i].ToString()))
                    listView1.Items.Add(item1);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
