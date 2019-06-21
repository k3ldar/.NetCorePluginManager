/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Demo Website
 *  
 *  File: MockBlogProvider.cs
 *
 *  Purpose:  Mock IBlogProvider for tesing purpose
 *
 *  Date        Name                Reason
 *  21/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

using Middleware;
using Middleware.Blog;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    public sealed class MockBlogProvider : IBlogProvider
    {
        #region Private Members

        private readonly List<BlogEntry> _blogEntries;

        #endregion Private Members

        #region Constructors

        public MockBlogProvider()
        {
            _blogEntries = new List<BlogEntry>()
            {
                new BlogEntry(1, 123, "My First Blog Entry", "This is about my first blog entry", "Making blogs is easy", "Test User", true,
                    DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-9),
                    new List<string>() { "Blogs", "First", "Test" },
                    new List<BlogComment>()
                    {
                        new BlogComment(1, null, DateTime.Now.AddDays(-8), 2, "A User", true, "This is the first comment"),
                        new BlogComment(2, null, DateTime.Now.AddDays(-7), 2, "Another User", true, "This is the second comment")
                    }),
                new BlogEntry(2, 123, "Test", "Lorem Ipsum", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor " +
                    "incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut " +
                    "aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat " +
                    "nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", 
                    "Test User", true,
                    DateTime.Now.AddDays(-7), DateTime.Now.AddDays(-7), DateTime.Now.AddDays(-7),
                    new List<string>() { "Lorem", "Ipsum" },
                    new List<BlogComment>()
                    {
                        new BlogComment(1, null, DateTime.Now.AddDays(-7), 2, "A User", true, "honi soit qui mal y pense")
                    })
        };
        }

        #endregion Constructors

        #region IBlogProvider Methods

        public List<BlogEntry> GetRecentPosts(in int recentCount)
        {
            int count = recentCount;
            return _blogEntries.OrderBy(o => o.PublishDateTime).Take(count).ToList();
        }

        #endregion IBlogProvider Methods
    }
}
