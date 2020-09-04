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
    public class PostsController : ControllerBase
    {
        private readonly BlogAPIContext _context;

        public PostsController(BlogAPIContext context)
        {
            _context = context;
        }

        // GET: api/posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return await _context.Posts.Include(post => post.Comments).ToListAsync();
        }

        // GET: api/posts/5
        [HttpGet("{id}")]
        public ActionResult<Post> GetPost([FromRoute] int id)
        {

            var post = _context.Posts.Where(post => post.Id == id)
                                        .Include(post => post.Comments);

            if (post == null)
            {
                return NotFound($"No Post found!");
            }

            return Ok(post);
        }

        // POST: api/posts
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost([FromBody] Post posts)
        {
            posts.Date = DateTime.Now;
            _context.Posts.Add(posts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = posts.Id }, posts);
        }

        // PUT: api/posts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost([FromRoute] int id, [FromBody] Post posts)
        {
            var post = await _context.Posts.FindAsync(id);

            if (posts == null)
            {
                return BadRequest();
            }

            if (post == null)
            {
                return NotFound($"No Post found!");
            }

            post.Title = posts.Title;
            post.ImageURL = posts.ImageURL;
            post.Description = posts.Description;

            _context.Posts.Update(post);


            try
            {
                await _context.SaveChangesAsync();

                return Ok(post);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE: api/posts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeletePost([FromRoute] int id)
        {
            var posts = await _context.Posts.FindAsync(id);
            

            if (posts == null)
            {
                return NotFound($"No Post found!");
            }

            _context.Posts.Remove(posts);
            await _context.SaveChangesAsync();

            return Ok(posts);
        }

        // POST: api/posts/comment
        [HttpPost("Comment")]
        public async Task<ActionResult<Post>> PostComment([FromBody] Comment comment)
        {
            var posts = await _context.Posts.FindAsync(comment.PostId);

            comment.Date = DateTime.Now;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = comment.PostId }, posts);
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
