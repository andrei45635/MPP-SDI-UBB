using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using contestService;
using contestDomain;
using Task = contestDomain.Task;
using contestDomain.enums;
using Type = contestDomain.enums.Type;
using contestDomain.dto;
using System.Configuration;
using contestClientIDK;

namespace contestClientIDK
{
    public partial class MainView : Form
    {
        private ContestClientController ctrl;
        private User loggedInUser;

        public MainView(ContestClientController ctrl)
        {
            InitializeComponent();
            this.ctrl = ctrl;
            ctrl.updateEvent += ClientUpdate;
            //taskTypeCB.DataSource = ctrl.GetAllTypes();
            taskTypeCB.Items.Add(contestDomain.enums.Type.PAINTING);
            taskTypeCB.Items.Add(contestDomain.enums.Type.TREASURE);
            taskTypeCB.Items.Add(contestDomain.enums.Type.POETRY);
            ageGroupCB.Items.Add(contestDomain.enums.AgeGroup.PRETEEN);
            ageGroupCB.Items.Add(contestDomain.enums.AgeGroup.YOUNG);
            ageGroupCB.Items.Add(contestDomain.enums.AgeGroup.TEEN);
            //ageGroupCB.DataSource = ctrl.GetAllAgeGroups();
            registerTask1CB.Items.Add(contestDomain.enums.Type.PAINTING);
            registerTask1CB.Items.Add(contestDomain.enums.Type.TREASURE);
            registerTask1CB.Items.Add(contestDomain.enums.Type.POETRY);
            registerTask2CB.Items.Add(contestDomain.enums.Type.PAINTING);
            registerTask2CB.Items.Add(contestDomain.enums.Type.TREASURE);
            registerTask2CB.Items.Add(contestDomain.enums.Type.POETRY);
            //registerTask1CB.DataSource = ctrl.GetAllTypes();
            //registerTask2CB.DataSource = ctrl.GetAllTypes();
            SetupTasksView();
        }

        public void ClientUpdate(object sender, ContestEventArgs e)
        {
            if (e.ClientEventType == ContestClientEvent.Update)
            {
                BeginInvoke(new UpdateListBoxCallback(this.UpdateTables));
            }
        }

        public delegate void UpdateListBoxCallback();

        private void UpdateTables()
        {
            this.SetupTasksView();
        }

        public void SetLoggedInUser(User user)
        {
            this.loggedInUser = user;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.loggedInUser = null;
            Login loginView = new Login(ctrl);
            loginView.Show();
            this.Close();
        }

        private void SetupTasksView()
        {
            List<TaskDTO> tasks = new List<TaskDTO>(ctrl.GetAllTasks());
            tasksView1.DataSource = tasks;
        }

        private void populateFilterTable()
        {
            Type type = (Type)Enum.Parse(typeof(Type), taskTypeCB.SelectedValue.ToString());
            AgeGroup ageGroup = (AgeGroup)Enum.Parse(typeof(AgeGroup), ageGroupCB.SelectedValue.ToString());
            List<Participant> parts = new List<Participant>(ctrl.FilterParticipants(type, ageGroup));
            List<ParticipantDTO> partDTO = new List<ParticipantDTO>();
            foreach(Participant participant in parts)
            {
                partDTO.Add(new ParticipantDTO(participant.Name, participant.Age));
            }
            BindingList<ParticipantDTO> participantsBinding = new BindingList<ParticipantDTO>(partDTO);
            filterTasksView.DataSource = participantsBinding.ToArray<ParticipantDTO>();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            populateFilterTable();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string inputName = registerNameTB.Text.ToString();
            int age = int.Parse(registerAgeTB.Text.ToString());
            if (age < 6 || age > 15)
            {
                MessageBox.Show("Invalid age!");
            }
            else
            {
                Type type1 = (Type)Enum.Parse(typeof(Type), registerTask1CB.GetItemText(registerTask1CB.SelectedItem));
                Type type2 = (Type)Enum.Parse(typeof(Type), registerTask2CB.GetItemText(registerTask2CB.SelectedItem));
                MessageBox.Show(type1.ToString() + " " + type2.ToString());
                ctrl.Register(inputName, age, type1, type2);
                //SetupTasksView();
            }
        }

        private void ContestWindowFormClosing(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine("ContestWindow closing " + e.CloseReason);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                ctrl.Logout();
                ctrl.updateEvent -= ClientUpdate;
                Application.Exit();
            }
        }

        private void MainView_Load(object sender, EventArgs e)
        {
             
        }
    }
}

