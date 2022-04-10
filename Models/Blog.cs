using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BlogMe.Models
{
    public class Blog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }


        public DateTime CreatedOn { get; set; }

        [Display(Name = "Your Blog Text")]
        [Required]
        public string BlogText { get; set; }

        //public string Comments { get; set; }

        public string BlogOwnerId { get; set; }
        public ApplicationUser BlogOwner { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}