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
 *  Product:  Plugin Middleware
 *  
 *  File: SearchResponse.cs
 *
 *  Purpose:  Base search response
 *
 *  Date        Name                Reason
 *  02/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Text;

namespace Middleware.Search
{
    /// <summary>
    /// Search response for all search types.
    /// </summary>
    public class SearchResponseItem
    {
        #region Private Members

        private bool _highlighted = false;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SearchResponseItem()
        {
            Properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseType">User defined response type, i.e. document, webpage, product, tag etc.</param>
        /// <param name="response">The response word or phrase.</param>
        public SearchResponseItem(in string responseType, in string response)
            : this(responseType, response, -1, response)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseType">User defined response type, i.e. document, webpage, product, tag etc.</param>
        /// <param name="response">The response word or phrase.</param>
        /// <param name="offset">The offset within response where the search term was found, if relevant.</param>
        /// <param name="displayName">Name that will be displayed with the search result url, should no custom view be supplied.</param>
        public SearchResponseItem(in string responseType, in string response, in int offset, string displayName)
            : this(responseType, response, offset, null, displayName, null)
        {

        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseType">User defined response type, i.e. document, webpage, product, tag etc.</param>
        /// <param name="response">The response word or phrase.</param>
        /// <param name="offset">The offset within response where the search term was found, if relevant.</param>
        /// <param name="url">If the search returns a url it will be made into a uri otherwise left null.</param>
        /// <param name="displayName">Name that will be displayed with the search result url, should no custom view be supplied.</param>
        /// <param name="viewName">Name and path of the view that will display search results</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "It's param names so ok with me!")]
        public SearchResponseItem(in string responseType, in string response, in int offset, string url, string displayName, string viewName)
            : this()
        {
            if (String.IsNullOrEmpty(response))
                throw new ArgumentNullException(nameof(response));

            if (String.IsNullOrEmpty(displayName) && String.IsNullOrEmpty(viewName))
                throw new ArgumentException($"Either {nameof(displayName)} or {nameof(viewName)} must be set");

            ResponseType = responseType ?? String.Empty;
            Response = response;

            if (!String.IsNullOrEmpty(url))
            {
                if (Uri.TryCreate(url, UriKind.Relative, out Uri uriResult))
                {
                    Url = uriResult;
                }
            }

            Offset = offset;

            DisplayName = displayName;
            ViewName = viewName;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// User defined response type i.e. document, webpage, product, tag etc.
        /// </summary>
        /// <value>string</value>
        public string ResponseType { get; set; }

        /// <summary>
        /// Response from search result.
        /// </summary>
        /// <value>string</value>
        public string Response { get; set; }

        /// <summary>
        /// The offset within response where the search term was found, if relevant.
        /// </summary>
        /// <value>int</value>
        public int Offset { get; set; }

        /// <summary>
        /// Url, if provided, returned by the search provider.  If this value is null then the host has to determine how
        /// the search result can be viewed in the host application probably based on the ResponseType
        /// </summary>
        /// <value>Uri</value>
        public Uri Url { get; set; }

        /// <summary>
        /// Dictionary of properties, these are user defined on the premise that the writer of the propery will know.
        /// and check for it's type before using it.
        /// </summary>
        /// <value>Dictionary&lt;string, object&gt;</value>
        public Dictionary<string, object> Properties { get; private set; }

        /// <summary>
        /// Indicates the relevance of the search item in relation to the search
        /// </summary>
        /// <value>int</value>
        public int Relevance { get; set; }

        /// <summary>
        /// Name of the view that will be used to display the search results
        /// </summary>
        /// <value>string</value>
        public string ViewName { get; set; }

        /// <summary>
        /// Name that will be displayed with the search result url, should no custom view be supplied.
        /// </summary>
        public string DisplayName { get; set; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Highlights the keyword search
        /// </summary>
        /// <param name="keywordLength">Length of keyword being searched.</param>
        public void HighlightKeywords(in int keywordLength)
        {
            if (_highlighted || Offset == -1)
                return;

            StringBuilder Result = new StringBuilder(Response, Response.Length + 20);

            Result.Insert(keywordLength + Offset, "</strong>");
            Result.Insert(Offset, "<strong>");

            Response = Result.ToString();
            _highlighted = true;
        }

        #endregion Public Methods
    }
}
