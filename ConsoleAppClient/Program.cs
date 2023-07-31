using ConsoleAppClient.DAL;
using ConsoleAppClient.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ConsoleAppClient
{
    class Program
    {
        static HttpClient client = new();
        private static IConfiguration _iconfiguration;

        static void Main()
        {
            GetAppSettingsFile();
            RunAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets the appsettings file
        /// </summary>
        static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",
                    optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();
        }

        /// <summary>
        /// RunAsync
        /// </summary>
        /// <returns></returns>
        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("https://localhost:7049/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                Console.WriteLine($"Command started.");

                int count = 0;
                List<TaskSample> taskSamplesList = GetAllTasks();
                while (taskSamplesList.Count > 0)
                {
                    foreach(TaskSample item in taskSamplesList)
                    {
                        var response = await CommandTaskSampleAsync(item);

                        Console.WriteLine($"{item.TaskName} > {item.TaskType}");

                        if (response == "OK")
                        {
                            SetIsProcessed(item);
                            Console.WriteLine("The record was successfully processed.");
                        }
                        else
                            Console.WriteLine("The record could not be processed.");

                        count++;
                    }

                    Thread.Sleep(1000);
                    taskSamplesList = GetAllTasks();
                }

                Console.WriteLine($"Command completed. Affected {count} rows.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Gets all tasks from db
        /// </summary>
        /// <returns></returns>
        static List<TaskSample> GetAllTasks()
        {
            var tasksDAL = new TaskSampleDAL(_iconfiguration);
            var lstDepartment = tasksDAL.GetAllTaskSamples();
            return lstDepartment;
        }

        /// <summary>
        /// Sets is processed to True when API response is OK
        /// </summary>
        /// <param name="item"></param>
        static void SetIsProcessed(TaskSample item)
        {
            var tasksDAL = new TaskSampleDAL(_iconfiguration);
            tasksDAL.IsProcessed(item);
        }

        /// <summary>
        /// Sends data to API
        /// </summary>
        /// <param name="taskSample">task</param>
        /// <returns>response from API</returns>
        static async Task<string> CommandTaskSampleAsync(TaskSample taskSample)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/{taskSample.TaskType.ToLower()}/{taskSample.TaskId}", taskSample);
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result.Replace("\"", "");
        }
    }
}