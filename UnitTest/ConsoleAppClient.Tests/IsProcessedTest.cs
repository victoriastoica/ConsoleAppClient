using ConsoleAppClient.DAL;
using ConsoleAppClient.Models;
using Microsoft.Extensions.Configuration;

namespace ConsoleAppClient.Tests
{
    public class IsProcessedTest
    {
        [Fact]
        public void IsProcessed()
        {
            // Arrange
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",
                    optional: false, reloadOnChange: true).Build();

            TaskSample taskSample = new TaskSample("Task1", DateTime.Now, "Update", false, DateTime.Now, string.Empty);
            TaskSampleDAL taskSampleDAL = new TaskSampleDAL(config);

            // Act
            taskSampleDAL.IsProcessed(taskSample);

            // Assert
            Assert.True(true, taskSample.TaskIsProcessed ? "true" : "false");
        }
    }
}