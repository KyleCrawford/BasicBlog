using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BlogMe.Models
{
    public class Comment
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Comment")]
        public string CommentText { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Left at:")]
        public DateTime TimeCommented { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }
}