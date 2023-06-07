using System;
using System.Windows.Forms;
using contest_csharp.service;
using contest_csharp.domain;

namespace contest_csharp
{
    public partial class Login : Form
    {
        private Service srv;
        public Login(Service srv)
        {
            InitializeComponent();
            this.srv = srv;
        }

        private void Form1_Load(Object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            if (!srv.IsLoggedIn(username, password))
            {
                MessageBox.Show("Invalid credentials!");
            }
            else
            {
                User user = srv.FindLoggedInUser(username, password);
                
                MainView mainView = new MainView(srv);
                mainView.SetLoggedInUser(user);
                mainView.Show();

                this.Close();
                //this.Hide();
            }
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
