using System.Collections.Generic;
using ReadLater.Entities;

namespace ReadLater.Services
{
    public interface IBookmarkService
    {
        Bookmark CreateBookmark(Bookmark bookmark);
        List<Bookmark> GetBookmarks(string category);
        Bookmark GetBookmark(int bookmarkId);
        Bookmark UpdateBookmark(Bookmark bookmark);
        void DeleteBookmark(Bookmark bookmark);
    }
}