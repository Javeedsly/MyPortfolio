using System;
using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Core.DTOs
{
    public class FeedbackDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime SubmittedDate { get; set; }
    }

    public class CreateFeedbackDto
    {
        [Required(ErrorMessage = "Name required.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-mail required.")]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Subject required.")]
        [StringLength(100)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message required.")]
        [StringLength(1000)]
        public string Message { get; set; }
    }
}