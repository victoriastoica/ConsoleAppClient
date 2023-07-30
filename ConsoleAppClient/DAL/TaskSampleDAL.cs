using ConsoleAppClient.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ConsoleAppClient.DAL
{
    public class TaskSampleDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="configuration"></param>
        public TaskSampleDAL(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");
        }

        /// <summary>
        /// Gets all the tasks from db
        /// </summary>
        /// <returns></returns>
        public List<TaskSample> GetAllTaskSamples()
        {
            var lstTaskSamples = new List<TaskSample>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM [APIIntegrationDb].[dbo].[Task] where IsProcessed = 0;", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        lstTaskSamples.Add(new TaskSample
                        {
                            TaskId = rdr.GetInt32("Id"),
                            TaskName = rdr.GetString("Name"),
                            TaskDate = rdr.GetDateTime("Date"),
                            TaskType = rdr.GetString("Type"),
                            TaskIsProcessed = rdr.GetBoolean("IsProcessed")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstTaskSamples;
        }

        /// <summary>
        /// Updates the record when is successfully processed
        /// </summary>
        /// <param name="taskSample"></param>
        public void IsProcessed(TaskSample taskSample)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (con)
                    {
                        string query = @"   UPDATE [APIIntegrationDb].[dbo].[Task] 
                                            SET IsProcessed = 1, ModifyDate = GETDATE(), ModifyBy = 'admin'
                                            WHERE Id = @taskId; ";

                        using (SqlCommand cmd = new(query, con))
                        {
                            cmd.Parameters.Add("@taskId", SqlDbType.Int).Value = taskSample.TaskId;
                            cmd.ExecuteNonQuery();
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}