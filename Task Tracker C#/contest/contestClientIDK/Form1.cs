using System;
using System.Windows.Forms;
using contestService;
using contestDomain;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace contestClientIDK
{
    public partial class Login : Form
    {
        private ContestClientController controller;
        public Login(ContestClientController controller)
        {
            InitializeComponent();
            this.controller = controller;
        }

        private void Form1_Load(Object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            try
            {
                controller.Login(username, password);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid credentials!");
            }
            //User loggedUser = controller.FindLoggedUser(username, password);
            User loggedUser = new User(new Random().Next(500, 1488), username, password);
            MainView mainView = new MainView(controller);
            mainView.SetLoggedInUser(loggedUser);
            mainView.Show();

            //this.Close();
            this.Hide();
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
