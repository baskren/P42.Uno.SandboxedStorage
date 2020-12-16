using System;
using System.Collections.Generic;
using Foundation;

namespace P42.Uno.SandboxedStorage
{
    public static class BookmarkExtensions
    {
        static readonly NSString BookmarksKey = (NSString)"Bookmarks";

        public static (NSUrl NewUrl, NSData Bookmark) GetBookmark(this NSUrl url)
        {
            var bookmarksObj = NSUserDefaults.StandardUserDefaults.ValueForKey(BookmarksKey) as NSDictionary;
            var nsBookmarks = bookmarksObj?.MutableCopy() as NSMutableDictionary ?? new NSMutableDictionary();
            foreach (var key in nsBookmarks.Keys)
            {
                var bookmark = nsBookmarks[key] as NSData;
                var bookmarkUrl = NSUrl.FromBookmarkData(bookmark,
                    NSUrlBookmarkResolutionOptions.WithoutUI |
                    NSUrlBookmarkResolutionOptions.WithSecurityScope,
                    null,
                    out bool isStale,
                    out NSError error1
                    );
                if (bookmarkUrl != null && error1 == null)
                {
                    if (bookmarkUrl.Path.TrimEnd('/') == url.Path.TrimEnd('/'))
                    {
                        if (isStale)
                        {
                            if (url.CreateBookmark() is NSData freshBookmark)
                            {
                                return (bookmarkUrl, freshBookmark);
                            }
                            return (null,null);
                        }
                        return (bookmarkUrl, bookmark);
                    }
                }
                else
                {
                    if (error1 != null)
                        Console.WriteLine("Bookmark error: Bookmark for url [" + url + "] gave error [" + error1.Description + "] when trying to convert to URL.");
                    nsBookmarks.Remove(key);
                    NSUserDefaults.StandardUserDefaults.SetValueForKey(nsBookmarks, BookmarksKey);
                }
            }
            return (null, null);
        }

        public static NSData CreateBookmark(this NSUrl url)
        {
            var newBookmark = url.CreateBookmarkData(
                NSUrlBookmarkCreationOptions.WithSecurityScope,
                new string[] { }, null, out NSError error2);
            if (error2 != null)
            {
                Console.WriteLine("Can not get bookmark for url path [" + url.Path + "].");
                Console.WriteLine("ERROR: " + error2);
                return null;
            }
            var bookmarksObj = NSUserDefaults.StandardUserDefaults.ValueForKey(BookmarksKey) as NSDictionary;
            var nsBookmarks = bookmarksObj?.MutableCopy() as NSMutableDictionary ?? new NSMutableDictionary();
            nsBookmarks[url.Path] = newBookmark;
            NSUserDefaults.StandardUserDefaults.SetValueForKey(nsBookmarks, BookmarksKey);
            return newBookmark;
        }

        public static (NSUrl NewUrl,NSData Bookmark) GetOrCreateBookmark(this NSUrl url)
        {
            var existingBookmark = url.GetBookmark();
            if (existingBookmark.Bookmark != null)
                return existingBookmark;
            return (url,url.CreateBookmark());
        }

    }
}
