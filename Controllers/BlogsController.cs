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

        // GET: api/Blog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs()
        {
            return await _context.Blogs.Include(blog => blog.Comments).ToListAsync();
        }

        // GET: api/Blog/5
        [HttpGet("{id}")]
        public ActionResult<Blog> GetBlog(int id)
        {
            // var blogs = await _context.Blogs.FindAsync(id);

            var blogs = _context.Blogs.Where(blog => blog.Id == id).Include(blog => blog.Comments);

            if (blogs == null)
            {
                return NotFound($"No Blog found!");
            }

            return Ok(blogs);
        }

        // PUT: api/Blog/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlog(int id, Blog blogs)
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

        // POST: api/Blog
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Blog>> PostBlog(Blog blogs)
        {
            blogs.Date = DateTime.Now;
            _context.Blogs.Add(blogs);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBlogs", new { id = blogs.Id }, blogs);
        }

        // DELETE: api/Blog/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Blog>> DeleteBlog(int id)
        {
            var blogs = await _context.Blogs.FindAsync(id);
            

            if (blogs == null)
            {
                return NotFound($"No Blog found!");
            }

            _context.Blogs.Remove(blogs);
            await _context.SaveChangesAsync();

            return blogs;
        }

        // POST: api/Blogs/Comment
        [HttpPost("Comment")]
        public async Task<ActionResult<Blog>> PostComment(Comment comment)
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
