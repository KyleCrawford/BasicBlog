using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogMe.Models;

namespace BlogMe.ViewModels
{
    public class ReadWithCommentsViewModel
    {
        public Blog Blog { get; set; }
        public Comment Comment { get; set; }
    }
}