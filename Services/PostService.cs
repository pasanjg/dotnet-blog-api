using BlogAPI.Data;
using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Services
{
    public interface IPostService 
    {
        IEnumerable<Post> GetPosts();

        Post GetPost(int Id);

        Post CreatePost(Post post);

        Post UpdatePost(int Id, Post post);

        Post DeletePost(int Id);

        Post PostComment(Comment comment);
    }

    public class PostService : IPostService
    {
        private readonly BlogAPIContext _context;

        Post IPostService.GetPost(int Id)
        {
            return _context.Posts.Where(post => post.Id == Id).Include(post => post.Comments).First();
        }

        IEnumerable<Post> IPostService.GetPosts()
        {
            return _context.Posts.Include(post => post.Comments).ToList();
        }

        public PostService(BlogAPIContext context)
        {
            _context = context;
        }

        Post IPostService.CreatePost(Post post)
        {
            post.Date = DateTime.Now;
            _context.Posts.Add(post);
            _context.SaveChanges();

            return post;
        }

        Post IPostService.UpdatePost(int Id, Post post)
        {

            Post currentPost = _context.Posts.Find(Id);

            currentPost.Title = post.Title;
            currentPost.ImageURL = post.ImageURL;
            currentPost.Description = post.Description;

            _context.Posts.Update(currentPost);

            try
            {
                _context.SaveChanges();

                return currentPost;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(currentPost.Id))
                {
                    return null;
                }

                else
                {
                    throw;
                }
            }
        }

        Post IPostService.DeletePost(int Id)
        {
            Post currentPost = _context.Posts.Find(Id);


            if (currentPost == null)
            {
                return null;
            }

            _context.Posts.Remove(currentPost);

            try
            {
                _context.SaveChanges();

                return currentPost;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (PostExists(currentPost.Id))
                {
                    return null;
                }

                else
                {
                    throw;
                }
            }

        }

        Post IPostService.PostComment(Comment comment)
        {
            var currentPost = _context.Posts.Find(comment.PostId);

            comment.Date = DateTime.Now;

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return currentPost;
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
