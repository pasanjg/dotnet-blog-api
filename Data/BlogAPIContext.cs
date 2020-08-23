using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Models;

namespace BlogAPI.Data
{
    public class BlogAPIContext : DbContext
    {
        public BlogAPIContext (DbContextOptions<BlogAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Blogs> Blogs { get; set; }
    }
}
