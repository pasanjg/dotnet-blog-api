﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Models
{
    public class Blogs
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public String Title { get; set; }

        [Required]
        public String Description { get; set; }

        [Required]
        public String Image { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
