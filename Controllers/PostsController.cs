using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BlogAPI.Models;
using BlogAPI.Services;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        // GET: api/posts
        [HttpGet]
        public IEnumerable<Post> GetPosts()
        {
            return _postService.GetPosts();
        }

        // GET: api/posts/5
        [HttpGet("{id}")]
        public ActionResult<Post> GetPost([FromRoute] int Id)
        {

            Post post = _postService.GetPost(Id);

            if (post == null)
            {
                return NotFound($"No Post found!");
            }

            return Ok(post);
        }

        // POST: api/posts
        [HttpPost]
        public ActionResult<Post> CreatePost([FromBody] Post post)
        {
            post = _postService.CreatePost(post);
            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }

        // PUT: api/posts/5
        [HttpPut("{id}")]
        public IActionResult UpdatePost([FromRoute] int Id, [FromBody] Post post)
        {
            if (post == null)
            {
                return BadRequest();
            }

            Post updatedPost = _postService.UpdatePost(Id, post);

            if (updatedPost == null)
            {
                return NotFound($"No Post found!");
            }

            return Ok(updatedPost);
        }

        // DELETE: api/posts/5
        [HttpDelete("{id}")]
        public ActionResult<Post> DeletePost([FromRoute] int Id)
        {
            Post deletedPost = _postService.DeletePost(Id);

            return Ok(deletedPost);
        }

        // POST: api/posts/comment
        [HttpPost("Comment")]
        public ActionResult<Post> PostComment([FromBody] Comment comment)
        {
            Post post = _postService.PostComment(comment);

            return CreatedAtAction("GetPost", new { id = comment.PostId }, post);
        }

    }
}
