using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using BlogMe.Models;

namespace BlogMe.ViewModels
{
    public class ShowAllBlogsViewModel
    {
        // I want the list of blogs
        public PagedList.IPagedList<Blog> BlogList { get; set; }
        // and all the users
        public List<ApplicationUser> Users { get; set; }
        
    }
}