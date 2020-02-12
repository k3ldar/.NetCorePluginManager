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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
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

namespace Middleware.Search
{
    /// <summary>
    /// Search response for all search types.
    /// </summary>
    public class SearchResponseItem
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseType">User defined response type, i.e. document, webpage, product, tag etc.</param>
        /// <param name="response">The response word or phrase.</param>
        public SearchResponseItem(in string responseType, in string response)
            : this(responseType, response, -1)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseType">User defined response type, i.e. document, webpage, product, tag etc.</param>
        /// <param name="response">The response word or phrase.</param>
        /// <param name="offset">The offset within response where the search term was found, if relevant.</param>
        public SearchResponseItem(in string responseType, in string response, in int offset)
            : this(responseType, response, offset, null)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseType">User defined response type, i.e. document, webpage, product, tag etc.</param>
        /// <param name="response">The response word or phrase.</param>
        /// <param name="offset">The offset within response where the search term was found, if relevant.</param>
        /// <param name="url">If the search returns a url it will be made into a uri otherwise left null.</param>
        public SearchResponseItem(in string responseType, in string response, in int offset, string url)
        {
            if (String.IsNullOrEmpty(response))
                throw new ArgumentNullException(nameof(response));

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

            Properties = new Dictionary<string, object>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// User defined response type i.e. document, webpage, product, tag etc.
        /// </summary>
        public string ResponseType { get; private set; }

        /// <summary>
        /// Response from search result.
        /// </summary>
        public string Response { get; private set; }

        /// <summary>
        /// The offset within response where the search term was found, if relevant.
        /// </summary>
        public int Offset { get; private set; }

        /// <summary>
        /// Url, if provided, returned by the search provider.  If this value is null then the host has to determine how
        /// the search result can be viewed in the host application probably based on the ResponseType
        /// </summary>
        public Uri Url { get; private set; }

        /// <summary>
        /// Dictionary of properties, these are user defined on the premise that the writer of the propery will know.
        /// and check for it's type before using it.
        /// </summary>
        public Dictionary<string, object> Properties { get; private set; }

        #endregion Properties
    }
}
