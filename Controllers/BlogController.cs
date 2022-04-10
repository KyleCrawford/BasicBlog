using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogMe.Models;
using PagedList;
using BlogMe.ViewModels;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using AuthorizeAttribute = System.Web.Mvc.AuthorizeAttribute;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;

namespace BlogMe.Controllers
{
    public class BlogController : Controller
    {
        // Our database context
        private ApplicationDbContext _context;

        public BlogController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Blogs
        // Show all blogs (paginated)
        public ActionResult Index(int? page)
        {
            List<Blog> blogs = _context.Blogs.ToList();

            return View(PrepareViewModel(blogs, "View", page));
        }

        [Authorize]
        public ActionResult ViewOwnBlogs(int? page)
        {
            string userId = User.Identity.GetUserId();
            List<Blog> blogs = _context.Blogs.Where(b => b.BlogOwnerId == userId).ToList();

            return View("Index", PrepareViewModel(blogs, "Edit", page));
        }
        private ShowAllBlogsViewModel PrepareViewModel(List<Blog> blogs, string viewOrEdit, int? page)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            var pageBlogs = blogs.ToPagedList(pageNumber, pageSize);

            ShowAllBlogsViewModel viewModel = new ShowAllBlogsViewModel
            {
                BlogList = pageBlogs,
                Users = _context.Users.ToList(),
                ViewOrEdit = viewOrEdit
            };
            return viewModel;
        }

        // View Blog
        // Show one blog in reading format
        public ActionResult Read(int id)
        {
            Blog blog = _context.Blogs.Find(id);
            return View(blog);
        }

        // New Blog
        // Create a blog, must be logged in
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // Edit
        // Edit an existing blog
        [Authorize]
        public ActionResult Edit(int id)
        {
            Blog test = null;
            if (id == 0)
            {
                // we are creating a new blog
            }
            else
            {
                // We are editing an existing blog
                
                test = _context.Blogs.SingleOrDefault(b => b.Id == id);
                if (test is null)
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
                }
                if (test.BlogOwnerId != User.Identity.GetUserId())
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
                    //return HttpNotFound();
                }
            }
            return View(test);
        }

        [Authorize]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Save(Blog blog)
        {
            
            if (!ModelState.IsValid)
            {
                return View("Edit", blog);
            }

            if (blog.Id == 0)
            {
                // create new blog
                blog.CreatedOn = DateTime.Now;
                blog.BlogOwnerId = User.Identity.GetUserId();
                _context.Blogs.Add(blog);
            }
            else
            {
                // edit existing blog
                Blog oldBlog = _context.Blogs.SingleOrDefault(b => b.Id == blog.Id);
                if (oldBlog is null)
                {
                    // something went wrong
                    return View("Edit", blog);
                }

                oldBlog.Title = blog.Title;
                oldBlog.BlogText = blog.BlogText;
                //oldBlog.CreatedOn = blog.CreatedOn;
            }
            _context.SaveChanges();

            return RedirectToAction("Read", blog); //View("Read", blog);
        }

        // Delete - Can just put this on the Edit page. Might need a function

    }
}