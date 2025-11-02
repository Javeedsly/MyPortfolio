using System;
using System.Collections.Generic;

namespace MyPortfolio.Core.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string Slug { get; set; } 
        public DateTime PublishedDate { get; set; }
    }
}