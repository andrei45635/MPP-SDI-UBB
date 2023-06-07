using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MyTask = contestRestClient.TaskTwo;
using MyTaskOG = contestDomain.Task;
using System.Collections.Generic;


namespace contestRestClient
{
    class MainClass
    {
        static HttpClient client = new HttpClient();

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:8080/contest/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            int Id = 193;
            Console.WriteLine("Getting the task with the id {0}", Id);
            string URL = "http://localhost:8080/contest/tasks/" + Id;
            MyTask Task = await GetTaskAsync(URL);
            Console.WriteLine(Task.Id.ToString() + Task.Type.ToString() + Task.AgeGroup.ToString());
            Console.WriteLine("Received {0}", Task);
            string getAllURL = "http://localhost:8080/contest/tasks/";
            List<MyTask> Tasks = new List<MyTask>();
            Tasks = await GetTasksAsync(getAllURL);
            Console.WriteLine("Received {0} tasks", Tasks.Count);
            foreach(var t in Tasks)
            {
                Console.WriteLine(t.ToString());
            }
            //MyTaskOG testTask2 = new MyTaskOG(198, contestDomain.enums.Type.PAINTING, contestDomain.enums.AgeGroup.PRETEEN);
            //TaskTwo testTask = new TaskTwo("POETRY", "PRETEEN", 198);
            //Console.WriteLine("Adding task " + testTask2.ToString());
            //var res = await CreateTask(getAllURL, testTask2);
            //Console.WriteLine(res);
            Console.ReadLine();
        }

        async static Task<MyTaskOG> CreateTask(string path, MyTaskOG task)
        {
            MyTaskOG? Result = null;
            HttpResponseMessage Response = await client.PostAsJsonAsync(path, task);
            Console.WriteLine($"Response: {Response.StatusCode}");  
            if (Response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Response: {Response.StatusCode}");
                Result = await Response.Content.ReadAsAsync<MyTaskOG>();
            }
            return Result;
        }

        async static Task<MyTask> GetTaskAsync(string path)
        {
            MyTask? Result = null;
            HttpResponseMessage Response = await client.GetAsync(path);
            if (Response.IsSuccessStatusCode)
            {
                Result = await Response.Content.ReadAsAsync<MyTask>();
            }
            return Result;
        }

        async static Task<List<MyTask>> GetTasksAsync(string path)
        {
            List<MyTask> Result = null;
            HttpResponseMessage Response = await client.GetAsync(path);
            if (Response.IsSuccessStatusCode)
            {
                Result = await Response.Content.ReadAsAsync<List<MyTask>>();
            }
            return Result;
        }

        static async Task<String> GetTextAsync(string path)
        {
            String product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsStringAsync();
            }
            return product;
        }
    }

    public class TaskTwo
    {
        public string Type { get; set; }
        public string AgeGroup { get; set; }
        public int Id { get; set; }

        public TaskTwo(string type, string ageGroup, int id)
        {
            Type = type;
            AgeGroup = ageGroup;
            Id = id;
        }

        public override string ToString()
        {
            return string.Format("[Task: ID = {0}, Type = {1}, Age Group = {2}]", Id, Type, AgeGroup);
        }
    }
}
