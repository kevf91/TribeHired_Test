using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.DBContext
{
    public class ApiDbContext : DbContext
    {
        public DbSet<PostModel> Post { get; set; }

        public DbSet<CommentModel> Comment { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        { }
    }
}
