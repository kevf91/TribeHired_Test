using API.DBContext;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

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

        [HttpPost("searchcomments")]
        public JsonResult SearchComments([FromBody] CommentSearchModel searchModel)
        {
            try
            {
                var response = _context.Comment.AsQueryable();

                // Get all the propertyInfo of searchModel
                PropertyInfo[] propInfo = searchModel.GetType().GetProperties();
                foreach (var prop in propInfo)
                {
                    // String type
                    if(prop.PropertyType == typeof(string))
                    {
                        if (prop.GetValue(searchModel) != null)
                        {
                            // Generate query
                            string query = string.Format("{0}.Contains(@0)", prop.Name.ToString());
                            // Get Value base on this property
                            var searchValue = prop.GetValue(searchModel).ToString();
                            // execute
                            response = response.Where(query, searchValue)
;
                        }
                    }
                    // Number Type
                    else
                    {
                        if(prop.GetValue(searchModel) != null)
                        {
                            // Generate query
                            string query = string.Format("{0} == @0", prop.Name.ToString());
                            // Get Value base on this property
                            var searchValue = prop.GetValue(searchModel).ToString();
                            // execute
                            response = response.Where(query, searchValue);
                        }
                    }

                }

                return Json(response.ToList());
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
    }
}
