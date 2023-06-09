﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Asp_Net_FinalProject.Attributes;
using Asp_Net_FinalProject.Models;

namespace Asp_Net_FinalProject.Controllers
{
    public class PostsController : Controller
    {
        private dbEntities db = new dbEntities();

        // GET: Posts


        public ActionResult Index()
        {
            var post = db.Post.Include(p => p.User);
            return View(post.ToList());
        }

        // GET: Posts/Details/5


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DetailsAsync([Bind(Include = "Content")] Comment comment,int? id)
        {

            if (ModelState.IsValid)
            {
                string userEmail = User.Identity.Name;
                User user = db.User.FirstOrDefault(u => u.Email == userEmail);

                if (user != null)
                {
                    comment.Post_id = (int)id;
                    comment.Comment_date = DateTime.Now;
                    comment.User_id = user.Id;
                    db.Comment.Add(comment);
                    db.SaveChanges();
                    await Task.Delay(TimeSpan.FromSeconds(5));

                    return RedirectToAction("Details", new {id = id});
                }
                

            }
            return RedirectToAction("Details", new { id = id });
        }


        // GET: Posts/Create

        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create([Bind(Include = "Id,Title,Content,User_id")] Post post)
        {
            if (ModelState.IsValid)
            {
                // 當前登入用戶的用戶 ID
                string userEmail = User.Identity.Name;
                User user = db.User.FirstOrDefault(u => u.Email == userEmail);
                if (user != null)
                {
                    post.User_id = user.Id; // 正確的用戶 ID
                    post.Post_date = DateTime.Now;
                    db.Post.Add(post);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(post);
        }


        // GET: Posts/Edit/5
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_id = new SelectList(db.User, "Id", "UserName", post.User_id);
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        

        public ActionResult Edit([Bind(Include = "Id,Title,Content,User_id,Post_date")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User_id = new SelectList(db.User, "Id", "UserName", post.User_id);
            return View(post);
        }

        // GET: Posts/Delete/5
        

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        

        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Post.Find(id);
            db.Post.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
