using BlogApi.Models;
using BlogApi.Models.DTOs;
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
                    Content= dataReader.GetString(2),
                    postTime = dataReader.GetDateTime(3),
                    updateTime = dataReader.GetDateTime(4),
                    blogId = dataReader.GetInt32(5),

                };
                bloggers.Add(blogger);
            }


            connector.Close();
            return bloggers;
        }
    


    [HttpPost]
        public object AddNewBlogPost(AddBlogPostDto blogger)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var blg = new BlogPost
            {
                Title = blogger.Title,
                Content = blogger.Content,
                postTime = DateTime.Now,
                updateTime = DateTime.Now
            };

            var sql = $"INSERT INTO `blogpost`(`Title`, `Content`, `postTime`, `updateTime`) VALUES (@title,@content,@posttime,@updatetime)";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@title", blg.Title);
            cmd.Parameters.AddWithValue("@content", blg.Content);
            cmd.Parameters.AddWithValue("@posttime", blg.postTime);
            cmd.Parameters.AddWithValue("@updatetime", blg.updateTime);

            cmd.ExecuteNonQuery();
            connector.Close();
            return blg;
        }

        [HttpPut]
        public object UpdateBlogPost([FromQuery] int id, [FromBody] UpdateBlogPostDto updateBlogPostDto)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"UPDATE `blogpost` SET `title`=@title,`content`=@content,`posttime`=@posttime,`updatetime`=@updatetime 
                WHERE `id`= @id;";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@title", updateBlogPostDto.Title);
            cmd.Parameters.AddWithValue("@content", updateBlogPostDto.Content);
            cmd.Parameters.AddWithValue("@posttime", updateBlogPostDto.postTime);
            cmd.Parameters.AddWithValue("@updatetime", updateBlogPostDto.updateTime);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            var updatedBlogPost = new UpdateBlogPostDto
            {
                Title = updateBlogPostDto.Title,
                Content = updateBlogPostDto.Content,
                postTime = updateBlogPostDto.postTime,
                updateTime = updateBlogPostDto.updateTime
            };

            connector.Close();

            return new { message = "Sikeres frissítés.", result = updatedBlogPost};
        }

        [HttpDelete]
        public object DeleteBlogPost(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = $"DELETE FROM `blogpost` WHERE id = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("id", id);

            cmd.ExecuteNonQuery();

            connector.Close();


            return new { message = "Sikeres törlés" };
        }

    }

}
