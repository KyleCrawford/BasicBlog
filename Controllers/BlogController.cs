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

using System.Data.Entity.Validation;

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
            //blog.Comments = _context.Comments.Where(c => c.BlogId == id).ToList();
            ReadWithCommentsViewModel viewModel = new ReadWithCommentsViewModel
            {
                Blog = blog,
                Comment = new Comment()
            };
            return View(viewModel);
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
            Blog blog = null;
            if (id == 0)
            {
                // we are creating a new blog
                blog = new Blog();
            }
            else
            {
                // We are editing an existing blog
                
                blog = _context.Blogs.SingleOrDefault(b => b.Id == id);
                if (blog is null)
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
                }
                if (blog.BlogOwnerId != User.Identity.GetUserId())
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
                    //return HttpNotFound();
                }
            }
            return View(blog);
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
            }
            _context.SaveChanges();

            return RedirectToAction("Read", blog); //View("Read", blog);
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            Blog blog = _context.Blogs.SingleOrDefault(b => b.Id == id);
            if (blog is null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
            if (blog.BlogOwnerId != User.Identity.GetUserId())
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
                //return HttpNotFound();
            }
            _context.Blogs.Remove(blog);
            _context.SaveChanges();
            return RedirectToAction("ViewOwnBlogs");
        }

        [Authorize]
        public ActionResult Comment(ReadWithCommentsViewModel viewModel)
        {
            Comment comment = viewModel.Comment;
            comment.TimeCommented = DateTime.Now;
            comment.Username = User.Identity.GetUserName();
            comment.BlogId = viewModel.Blog.Id;
            _context.Comments.Add(comment);
            _context.SaveChanges();

            return RedirectToAction("Read", new { id= viewModel.Blog.Id });
        }
        
    }
}