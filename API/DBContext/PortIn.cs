using API.DBContext;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

public class PortIn
{

    public static async void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApiDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApiDbContext>>()))
        {
            #region Post Port In
            // Look for existing Post table in database.
            if (!context.Post.Any())
            {
                // Port In Posts
                // Retrieve Data from posts endpoint
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            List<PostModel> deserializedResponse = JsonConvert.DeserializeObject<List<PostModel>>(responseContent);

                            context.Post.AddRange(deserializedResponse);

                            context.SaveChanges();
                        }
                    }
                }
            }
            #endregion

            #region Comment Port In
            // Look for existing Comment table in database.
            if (!context.Comment.Any())
            {
                // Port In Comments
                // Retrieve Data from comments endpoint
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/comments"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            List<CommentModel> deserializedResponse = JsonConvert.DeserializeObject<List<CommentModel>>(responseContent);

                            context.Comment.AddRange(deserializedResponse);

                            context.SaveChanges();
                        }
                    }
                }
            }
            #endregion
        }
    }
}