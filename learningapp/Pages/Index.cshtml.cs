using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace learningapp.Pages;

public class IndexModel : PageModel
{
     public List<Course> Courses=new List<Course>();
    private readonly ILogger<IndexModel> _logger;
    private IConfiguration _configuration;
    public IndexModel(ILogger<IndexModel> logger,IConfiguration configuration)
    {
        _logger = logger;
        _configuration=configuration;
    }

    public void OnGet()
    {
        try
        {
            string connectionString = _configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand("SELECT CourseID, CourseName, Rating FROM Course;", sqlConnection))
                {
                    using (var sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            Courses.Add(new Course()
                            {
                                CourseID = int.Parse(sqlDataReader["CourseID"].ToString()),
                                CourseName = sqlDataReader["CourseName"].ToString(),
                                Rating = decimal.Parse(sqlDataReader["Rating"].ToString())
                            });
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {

            throw;
        }
        
        //string connectionString = _configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!;
        //var sqlConnection = new SqlConnection(connectionString);
        //sqlConnection.Open();

        //var sqlcommand = new SqlCommand(
        //"SELECT CourseID,CourseName,Rating FROM Course;",sqlConnection);
        // using (SqlDataReader sqlDatareader = sqlcommand.ExecuteReader())
        // {
        //     while (sqlDatareader.Read())
        //        {
        //            Courses.Add(new Course() {CourseID=Int32.Parse(sqlDatareader["CourseID"].ToString()),
        //            CourseName=sqlDatareader["CourseName"].ToString(),
        //            Rating=Decimal.Parse(sqlDatareader["Rating"].ToString())});
        //        }
        // }
    }
}
