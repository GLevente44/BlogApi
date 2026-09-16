using BlogApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly string ConnectionString = "server=localhost;database=blog;uid=root;password=";

        [HttpGet]
        public List<BlogPost> GetAllBlogPost()
        {
            List<BlogPost> bloggers = new List<BlogPost>();

            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = "SELECT * FROM blogpost;";
            var cmd = new MySqlCommand(sql, connector);
            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                var blogger = new BlogPost
                {
                    Id = dataReader.GetInt32(0),
                    Title = dataReader.GetString(1),
                    Context = dataReader.GetString(2),
                    postTime = dataReader.GetDateTime(3),
                    updateTime = dataReader.GetDateTime(4),
                    blogId = dataReader.GetInt32(5),

                };
                bloggers.Add(blogger);
            }


            connector.Close();
            return bloggers;



        }
    }
}
