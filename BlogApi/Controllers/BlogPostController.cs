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
            List<BlogPost> blogposts = new List<BlogPost>();

            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = "SELECT * FROM blogpost;";
            var cmd = new MySqlCommand(sql, connector);
            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                var blogpost = new BlogPost
                {
                    Id = dataReader.GetInt32(0),
                    Title = dataReader.GetString(1),
                    Content= dataReader.GetString(2),
                    postTime = dataReader.GetDateTime(3),
                    updateTime = dataReader.GetDateTime(4),
                    blogId = dataReader.GetInt32(5),

                };
                blogposts.Add(blogpost);
            }


            connector.Close();
            return blogposts;
        }
    


    [HttpPost]
        public object AddNewBlogPost(AddBlogPostDto blogpost)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var blgp = new BlogPost
            {
                Title = blogpost.Title,
                Content = blogpost.Content,
                postTime = DateTime.Now,
                updateTime = DateTime.Now,
                blogId = blogpost.Id
            };

            var sql = $"INSERT INTO `blogpost`(`Title`, `Content`, `postTime`, `updateTime`, `blogId`) VALUES (@title,@content,@posttime,@updatetime,@blogid)";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@title", blgp.Title);
            cmd.Parameters.AddWithValue("@content", blgp.Content);
            cmd.Parameters.AddWithValue("@posttime", blgp.postTime);
            cmd.Parameters.AddWithValue("@updatetime", blgp.updateTime);
            cmd.Parameters.AddWithValue("blogid", blgp.blogId);

            cmd.ExecuteNonQuery();
            connector.Close();
            return blgp;
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
