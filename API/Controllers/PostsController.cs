using API.DBContext;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : Controller
    {
        private readonly ApiDbContext _context;

        public PostsController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet("getpostlist")]
        public JsonResult GetPostList()
        {
            try
            {

                var response = _context.Post
                                    .Include(p => p.Comments)
                                    .Select(p =>
                                        new {
                                            post_id = p.Id,
                                            post_title = p.Title,
                                            post_body = p.Body,
                                            total_number_of_comments = p.Comments.Count(),
                                        })
                                    .OrderBy(p => p.total_number_of_comments)
                                    .ToList();

                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        [HttpPost("getcommentlist")]
        public JsonResult GetCommentList([FromBody] CommentModel searchModel)
        {
            try
            {
                var response = _context.Comment.ToList();

                // Id filter
                if (searchModel.Id > 0)
                {
                    response = response.Where(c => c.Id == searchModel.Id).ToList();
                }

                // PostId filter
                if (searchModel.PostId > 0)
                {
                    response = response.Where(c => c.PostId == searchModel.PostId).ToList();
                }

                // Name filter
                if (!string.IsNullOrEmpty(searchModel.Name))
                {
                    response = response.Where(c => c.Name.Contains(searchModel.Name)).ToList();
                }

                // Email filter
                if (!string.IsNullOrEmpty(searchModel.Email))
                {
                    response = response.Where(c => c.Email.Contains(searchModel.Email)).ToList();
                }

                // Body filter
                if (!string.IsNullOrEmpty(searchModel.Body))
                {
                    response = response.Where(c => c.Body.Contains(searchModel.Body)).ToList();
                }

                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
    }
}
