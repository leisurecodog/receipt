using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;

namespace WindowsFormsApp8
{

    public partial class Form1 : Form
    {
        Thread t1;
        string[] number = new string[8];
        public Form1()
        {
            InitializeComponent();
        }
        //***************Global varies********************
        string str;
        Form2 fem= new Form2();
        //************************************************
        private void getNumber()
        {
            WebRequest myrequest =
            WebRequest.Create(@"http://invoice.etax.nat.gov.tw/");
            myrequest.Method = "GET";
            WebResponse respon = myrequest.GetResponse();
            myrequest.Timeout = 1000;
            StreamReader sr = new StreamReader(respon.GetResponseStream());
            str = sr.ReadToEnd();
            sr.Close();
            respon.Close();
        }
        private void HandlingNuber()
        {
            int StrLocation = str.IndexOf("t18Red\">");

            //搜尋特別獎
            number[0] = str.Substring(StrLocation + 8, 8);
            label5.Text = number[0];

            //搜尋特獎
            StrLocation = str.IndexOf("t18Red\">", StrLocation + 10);
            number[1] = str.Substring(StrLocation + 8, 8);
            label6.Text = number[1];

            //搜尋頭獎
            StrLocation = str.IndexOf("t18Red\">", StrLocation + 10);
            StrLocation += 8;
            label7.Text = "";
            for (int i = 0; i < 3; i++)
            {
                number[i + 2]= str.Substring(StrLocation, 8);
                StrLocation+=9;
                if (i > 0)
                    label7.Text += "　" + number[i + 2];
                else
                    label7.Text += number[i + 2];
            }

            //搜尋增開六獎
            StrLocation = str.IndexOf("t18Red\">", StrLocation + 10);
            StrLocation += 8;
            label9.Text = "";
            for (int i = 0; i <3; i++)
            {
                number[i + 5] = str.Substring(StrLocation, 3);
                StrLocation+=4;
                if (i > 0)
                    label9.Text += "　" + number[i + 5];
                else
                    label9.Text += number[i + 5];
            }

            //搜尋兌獎發票月份
            StrLocation = str.IndexOf("<h2>106");

            string one = str.Substring(StrLocation + 4, 10);
            label1.Text = "統一發票兌獎:" + one;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Visible = false;
            //pictureBox1.BackColor = System.Drawing.Color.Transparent;
            CenterToScreen();
            pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\loading-form1 size.gif");
            textBox1.Enabled = false;
            button1.Visible = false;
            button1.Enabled = false;
            button1.Text = "Loading data...";
            t1 = new Thread(getNumber);
            t1.Start();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (t1.IsAlive == false)
            {
                button1.Enabled = true;
                textBox1.Enabled = true;
                label10.Text = "請輸入想要查詢的發票號碼";
                button1.Text = "查詢發票號碼";
                HandlingNuber();
                timer1.Enabled = false;
                button1.Visible = true;
                pictureBox1.Visible = false;
                pictureBox1.Enabled = false;
                //fem.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string num = textBox1.Text;
            int n;
            if (num == "")
            {
                label10.Text= "請輸入想要查詢的發票號碼";
            }
            else if ((Encoding.Default.GetBytes(num).Length != 8) || (!int.TryParse(num, out n)))
            {
                label10.Text = "請輸入正確的發票號碼";
            }
            else
            {
                label10.Text = "MeSoHandSoMe!";
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //fem.Show();
            //fem.Opacity = 0.8;
            
            timer2.Enabled = false;
        }
    }
}
