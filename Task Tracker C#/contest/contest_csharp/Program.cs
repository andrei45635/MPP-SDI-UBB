using System;
using Application = System.Windows.Forms.Application;
using System.Collections.Generic;
using System.Configuration;
using contest_csharp.repo.users;
using contest_csharp.domain;
using log4net.Config;
using contest_csharp.repo;
using contest_csharp.service;
using contest_csharp.repo.participants;
using contest_csharp.repo.participanttasks;
using contest_csharp.repo.tasks;

namespace contest_csharp
{
    internal class Program
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        [STAThread]
        public static void Main()
        {
            IDictionary<String, string> props = new SortedList<String, String>();
            props.Add("ConnectionString", GetConnectionStringByName("contest.db"));

            ParticipantsRepo _participantsRepo = new ParticipantsDBRepository(props);
            ParticipantTasksRepo _participantTasksRepo = new ParticipantTasksDBRepository(props);
            TaskRepo _tasksRepo = new TaskDBRepository(props);
            UserRepo _userRepo = new UserDBRepository(props);
            
            Service srv = new Service(_participantsRepo, _participantTasksRepo, _tasksRepo, _userRepo);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var Login = new Login(srv);
            Login.Show();
            Application.Run();
            Application.Exit();
        }
        static string GetConnectionStringByName(string name)
        {
            // Assume failure.
            string returnValue = null;

            // Look for the name in the connectionStrings section.
            ConnectionStringSettings settings =ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string.
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }

    }
}