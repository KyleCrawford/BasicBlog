using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogMe.Models;

namespace BlogMe.ViewModels
{
    public class ViewAllBlogsViewModel
    {
        public PagedList.IPagedList<BlogMe.Models.Blog> BlogList { get; set; }
        public ApplicationUser CurrentUser { get; set; }
    }
}