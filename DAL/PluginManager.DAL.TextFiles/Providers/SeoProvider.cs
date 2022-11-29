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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: ISeoProvider.cs
 *
 *  Purpose:  ISeoProvider for text based storage
 *
 *  Date        Name                Reason
 *  25/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SharedPluginFeatures;

using PluginManager.DAL.TextFiles.Tables;
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal sealed class SeoProvider : ISeoProvider
    {
        #region Private Members

        private readonly ISimpleDBOperations<SeoDataRow> _seoData;


        #endregion Private Members

        #region Constructors

        public SeoProvider(ISimpleDBOperations<SeoDataRow> seoData)
        {
            _seoData = seoData ?? throw new ArgumentNullException(nameof(seoData));
        }

        #endregion Constructors

        #region ISeoProvider Methods

        public bool AddKeyword(in string route, in string keyword)
        {
            if (String.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            if (String.IsNullOrEmpty(keyword))
                throw new ArgumentNullException(nameof(keyword));

            string routeName = route;

            SeoDataRow seoData = _seoData.Select().FirstOrDefault(r => r.Route.Equals(routeName, StringComparison.InvariantCultureIgnoreCase));

            if (seoData == null)
            {
                seoData = new SeoDataRow()
                {
                    Route = routeName,
                };
            }

            if (!seoData.Keywords.Contains(keyword))
            {
                seoData.Keywords.Add(keyword);
                _seoData.InsertOrUpdate(seoData);
                return true;
            }

            return false;
        }

        public bool AddKeywords(in string route, in List<string> keywords)
        {
            if (String.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            if (keywords == null)
                return false;

            string routeName = route;

            SeoDataRow seoData = _seoData.Select().FirstOrDefault(r => r.Route.Equals(routeName, StringComparison.InvariantCultureIgnoreCase));

            if (seoData == null)
            {
                seoData = new SeoDataRow()
                {
                    Route = routeName,
                };
            }

            keywords.ForEach(kw => 
            {
                if (!seoData.Keywords.Contains(kw))
                    seoData.Keywords.Add(kw);
            });

            _seoData.InsertOrUpdate(seoData);
            return true;
        }

        public bool GetSeoDataForRoute(in string route, out string title,
            out string description, out string author, out List<string> keywords)
        {
            if (String.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            string routeName = route;

            SeoDataRow seoData = _seoData.Select().FirstOrDefault(r => r.Route.Equals(routeName, StringComparison.InvariantCultureIgnoreCase));

            if (seoData == null)
            {
                title = String.Empty;
                description = String.Empty;
                author = String.Empty;
                keywords = new();
                return false;
            }

            title = seoData.Title;
            description = seoData.Description;
            author = seoData.Author;
            keywords = seoData.Keywords;
            return true;
        }

        public bool RemoveKeyword(in string route, in string keyword)
        {
            if (route == null)
                throw new ArgumentNullException(nameof(route));

            if (String.IsNullOrEmpty(keyword))
                return false;

            string routeName = route;

            SeoDataRow seoData = _seoData.Select().FirstOrDefault(r => r.Route.Equals(routeName, StringComparison.InvariantCultureIgnoreCase));

            if (seoData == null)
                return false;

            if (!seoData.Keywords.Contains(keyword))
                return false;

            seoData.Keywords.Remove(keyword);
            _seoData.Update(seoData);

            return true;
        }

        public bool RemoveKeywords(in string route, in List<string> keywords)
        {
            if (route == null)
                throw new ArgumentNullException(nameof(route));

            if (keywords == null)
                throw new ArgumentNullException(nameof(keywords));

            string routeName = route;

            SeoDataRow seoData = _seoData.Select().FirstOrDefault(r => r.Route.Equals(routeName, StringComparison.InvariantCultureIgnoreCase));

            if (seoData == null)
                return false;

            keywords.ForEach(kw => seoData.Keywords.Remove(kw));
            _seoData.Update(seoData);

            return true;
        }

        public bool UpdateDescription(in string route, in string description)
        {
            if (String.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            if (String.IsNullOrEmpty(description))
                return false;

            string routeName = route;

            SeoDataRow seoData = _seoData.Select().FirstOrDefault(r => r.Route.Equals(routeName, StringComparison.InvariantCultureIgnoreCase));

            if (seoData == null)
            {
                seoData = new SeoDataRow()
                {
                    Route = routeName,
                };
            }

            seoData.Description = description;
            _seoData.InsertOrUpdate(seoData);
            return true;
        }

        public bool UpdateTitle(in string route, in string title)
        {
            if (String.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            if (String.IsNullOrEmpty(title))
                return false;

            string routeName = route;

            SeoDataRow seoData = _seoData.Select().FirstOrDefault(r => r.Route.Equals(routeName, StringComparison.InvariantCultureIgnoreCase));

            if (seoData == null)
            {
                seoData = new SeoDataRow()
                {
                    Route = routeName,
                };
            }

            seoData.Title = title;
            _seoData.InsertOrUpdate(seoData);
            return true;
        }

        public bool UpdateAuthor(in string route, in string author)
        {
            if (String.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            if (String.IsNullOrEmpty(author))
                return false;

            string routeName = route;

            SeoDataRow seoData = _seoData.Select().FirstOrDefault(r => r.Route.Equals(routeName, StringComparison.InvariantCultureIgnoreCase));

            if (seoData == null)
            {
                seoData = new SeoDataRow()
                {
                    Route = routeName,
                };
            }

            seoData.Author = author;
            _seoData.InsertOrUpdate(seoData);
            return true;
        }

        #endregion ISeoProvider Methods
    }
}
