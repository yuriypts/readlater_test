using MVC.DTO;
using ReadLater.Entities;
using ReadLater.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace MVC.Controllers.API
{
    [Authorize]
    public class ApiBookmarksController : ApiController
    {
        private IBookmarkService _bookmarkService;
        private ICategoryService _categoryService;

        public ApiBookmarksController(
            IBookmarkService bookmarkService,
            ICategoryService categoryService)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
        }

        [Route("api/bookmark")]
        [HttpGet]
        public IHttpActionResult GetBookmark()
        {
            IEnumerable<Bookmark> bookmarks = _bookmarkService.GetBookmarks(string.Empty);
            return Ok(bookmarks);
        }

        [Route("api/bookmark")]
        [HttpPost]
        public IHttpActionResult CreateBookmark([FromBody] BookmarkDTO bookmarkDTO)
        {
            Bookmark bookmark = new Bookmark();
            Bookmark modifiedBookmark = CreateEditCategory(bookmark, bookmarkDTO);

            return Ok(_bookmarkService.CreateBookmark(modifiedBookmark));
        }

        [Route("api/bookmark/{id:int}")]
        [HttpPut]
        public IHttpActionResult EditBookmark(int id, [FromBody] BookmarkDTO bookmarkDTO)
        {
            Bookmark bookmark = _bookmarkService.GetBookmark(id);
           
            if (bookmark == null) 
            {
                return NotFound();
            }

            Bookmark modifiedBookmark = CreateEditCategory(bookmark, bookmarkDTO);
            return Ok(_bookmarkService.UpdateBookmark(modifiedBookmark));
        }

        [Route("api/bookmark/{id:int}")]
        [HttpDelete]
        public IHttpActionResult DeleteBookmark(int id)
        {
            Bookmark bookmark = _bookmarkService.GetBookmark(id);

            if (bookmark == null)
            {
                return NotFound();
            }

            _bookmarkService.DeleteBookmark(bookmark);

            return Ok();
        }

        #region Helpers

        public Bookmark CreateEditCategory(Bookmark bookmark, BookmarkDTO bookmarkDTO)
        {
            bookmark.URL = bookmarkDTO.URL;
            bookmark.ShortDescription = bookmarkDTO.ShortDescription;

            Category category = _categoryService.GetCategory(bookmarkDTO.CategoryName);

            if (string.IsNullOrEmpty(bookmarkDTO.CategoryName))
            {
                bookmark.Category = null;
                bookmark.CategoryId = null;
            }
            else if (bookmarkDTO.CategoryName != category?.Name)
            {
                Category newCategory = _categoryService.CreateCategory(new Category { Name = bookmarkDTO.CategoryName });
                bookmark.CategoryId = newCategory.ID;
                bookmark.Category = null;
            }
            else
            {
                bookmark.Category = category;
                bookmark.CategoryId = category.ID;
            }

            return bookmark;
        }

        #endregion
    }
}
