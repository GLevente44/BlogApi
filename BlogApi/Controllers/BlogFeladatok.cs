using BlogApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogFeladatok : ControllerBase
    {
        private readonly string ConnectionString = "server=localhost;database=blog;uid=root;password=";


        // 4. Feladat
        [HttpGet]
        public List<Blogger> GetBloggerNameAndEmail()
        {
            List<Blogger> bloggers = new List<Blogger>();

            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = "SELECT Name,Email FROM blogger;";
            var cmd = new MySqlCommand(sql, connector);
            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                var blogger = new Blogger
                {
                    Name = dataReader.GetString(0),
                    Email = dataReader.GetString(1)
                };
                bloggers.Add(blogger);
            }


            connector.Close();
            return bloggers;
        }

    }
}
