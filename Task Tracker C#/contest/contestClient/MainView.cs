using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using contest_csharp.service;
using contest_csharp.domain;
using Task = contest_csharp.domain.Task;
using contest_csharp.domain.enums;
using Type = contest_csharp.domain.enums.Type;
using contest_csharp.mapper;
using contest_csharp.dto;
using contest_csharp.repo.tasks;
using System.Configuration;

namespace contest_csharp
{
    public partial class MainView : Form
    {
        private Service srv;
        private Participant2ParticipantDTO participantMapper = new Participant2ParticipantDTO();
        private static IDictionary<String, string> ReadProps()
        {
            IDictionary<String, string> props = new SortedList<String, String>();
            props.Add("ConnectionString", GetConnectionStringByName("contest.db"));
            return props;
        }
        static string GetConnectionStringByName(string name)
        {
            // Assume failure.
            string returnValue = null;

            // Look for the name in the connectionStrings section.
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string.
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }
        private Task2TaskDTO taskMapper = new Task2TaskDTO(new TaskDBRepository(ReadProps()));
        private User loggedInUser;
        public MainView(Service srv)
        {
            this.srv = srv;
            InitializeComponent();
            taskTypeCB.DataSource = srv.GetAllTypes();
            ageGroupCB.DataSource = srv.GetAllAgeGroups();
            registerTask1CB.DataSource = srv.GetAllTypes();
            registerTask2CB.DataSource = srv.GetAllTypes();
            SetupTasksView();
        }

        public void SetLoggedInUser(User user)
        {
            this.loggedInUser = user;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.loggedInUser = null;
            Login loginView = new Login(srv);
            loginView.Show();
            this.Close();
        }

        private void SetupTasksView()
        {
            List<Task> allTasks = new List<Task>(srv.GetAllTasks());
            IList<TaskDTO> tasks = taskMapper.ConvertList(allTasks);
            tasksView1.DataSource = tasks;
        }

        private void populateFilterTable()
        {
            Type type = (Type)Enum.Parse(typeof(Type), taskTypeCB.SelectedValue.ToString());
            AgeGroup ageGroup = (AgeGroup)Enum.Parse(typeof(AgeGroup), ageGroupCB.SelectedValue.ToString());
            IList<ParticipantDTO> participants = participantMapper.ConvertList(srv.FindParticipantsByAgeType(type, ageGroup));
            BindingList<ParticipantDTO> participantsBinding = new BindingList<ParticipantDTO>(participants);
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
                Participant participant = new Participant(srv.GetParticipantsSize() + 1, inputName, age);
                Type type1 = (Type)Enum.Parse(typeof(Type), registerTask1CB.SelectedValue.ToString());
                Type type2 = (Type)Enum.Parse(typeof(Type), registerTask2CB.SelectedValue.ToString());
                srv.RegisterUser(inputName, age, type1, type2);
                SetupTasksView();
            }
        }

        private void MainView_Load(object sender, EventArgs e)
        {
             
        }
    }
}

