using ReadLater.Entities;
using ReadLater.Services;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class BookmarksController : Controller
    {
        IBookmarkService _bookmarkService;
        ICategoryService _categoryService;

        public BookmarksController(
            IBookmarkService bookmarkService,
            ICategoryService categoryService)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            List<Bookmark> bookmarks = _bookmarkService.GetBookmarks(string.Empty);
            return View(bookmarks);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "URL,ShortDescription,Category")] Bookmark bookmark)
        {
            if (ModelState.IsValid)
            {
                CreateEditCategory(bookmark);
                _bookmarkService.CreateBookmark(bookmark);
                return RedirectToAction("Index");
            }

            return View(bookmark);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);

            if (bookmark == null)
            {
                return HttpNotFound();
            }

            return View(bookmark);

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);

            if (bookmark == null)
            {
                return HttpNotFound();
            }

            return View(bookmark);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,URL,ShortDescription,Category,CategoryId,CreateDate")] Bookmark bookmark)
        {
            if (ModelState.IsValid)
            {
                CreateEditCategory(bookmark);
                _bookmarkService.UpdateBookmark(bookmark);
                return RedirectToAction("Index");
            }
            return View(bookmark);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);

            if (bookmark == null)
            {
                return HttpNotFound();
            }

            return View(bookmark);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bookmark bookmark = _bookmarkService.GetBookmark(id);
            _bookmarkService.DeleteBookmark(bookmark);
            return RedirectToAction("Index");
        }

        #region Helpers
        // TODO - would be great to create for this a middlelayer (service)

        public void CreateEditCategory(Bookmark bookmark) 
        {
            Category category = _categoryService.GetCategory(bookmark.Category.Name);

            if (string.IsNullOrEmpty(bookmark.Category.Name))
            {
                bookmark.Category = null;
                bookmark.CategoryId = null;
            }
            else if (bookmark.Category.Name != category?.Name)
            {
                Category newCategory = _categoryService.CreateCategory(new Category { Name = bookmark.Category.Name });
                bookmark.CategoryId = newCategory.ID;
                bookmark.Category = null;
            }
            else
            {
                bookmark.Category = category;
                bookmark.CategoryId = category.ID;
            }
        }

        #endregion
    }
}