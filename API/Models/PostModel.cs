using System.Collections.Generic;

namespace API.Models
{
    public class PostModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public List<CommentModel> Comments { get; set; }
    }
}
