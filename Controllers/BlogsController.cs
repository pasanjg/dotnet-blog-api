using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Data;
using BlogAPI.Models;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly BlogAPIContext _context;

        public BlogsController(BlogAPIContext context)
        {
            _context = context;
        }

        // GET: api/blogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs()
        {
            return await _context.Blogs.Include(blog => blog.Comments).ToListAsync();
        }

        // GET: api/blogs/5
        [HttpGet("{id}")]
        public ActionResult<Blog> GetBlog([FromQuery] int id)
        {

            var blogs = _context.Blogs.Where(blog => blog.Id == id)
                                        .Include(blog => blog.Comments);

            if (blogs == null)
            {
                return NotFound($"No Blog found!");
            }

            return Ok(blogs);
        }

        // POST: api/blogs
        [HttpPost]
        public async Task<ActionResult<Blog>> PostBlog([FromBody] Blog blogs)
        {
            blogs.Date = DateTime.Now;
            _context.Blogs.Add(blogs);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBlogs", new { id = blogs.Id }, blogs);
        }

        // PUT: api/blogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlog([FromQuery] int id, [FromBody] Blog blogs)
        {
            var blog = await _context.Blogs.FindAsync(id);

            if (blogs == null)
            {
                return BadRequest();
            }

            if (blog == null)
            {
                return NotFound($"No Blog found!");
            }

            blog.Title = blogs.Title;
            blog.ImageURL = blogs.ImageURL;
            blog.Description = blogs.Description;

            _context.Blogs.Update(blog);


            try
            {
                await _context.SaveChangesAsync();

                return Ok(blog);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE: api/blogs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Blog>> DeleteBlog([FromQuery] int id)
        {
            var blogs = await _context.Blogs.FindAsync(id);
            

            if (blogs == null)
            {
                return NotFound($"No Blog found!");
            }

            _context.Blogs.Remove(blogs);
            await _context.SaveChangesAsync();

            return Ok(blogs);
        }

        // POST: api/blogs/comment
        [HttpPost("Comment")]
        public async Task<ActionResult<Blog>> PostComment([FromBody] Comment comment)
        {
            var blogs = await _context.Blogs.FindAsync(comment.BlogId);

            comment.Date = DateTime.Now;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBlogs", new { id = comment.BlogId }, blogs);
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}
