using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace MyPortfolio.Core.DTOs
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string Slug { get; set; }
        public DateTime PublishedDate { get; set; }
    }

    public class CreateBlogDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Slug { get; set; }
    }

    public class UpdateBlogDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}