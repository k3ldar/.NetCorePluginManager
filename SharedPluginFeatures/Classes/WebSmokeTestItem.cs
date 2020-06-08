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
 *  Product:  SharedPluginFeatures
 *  
 *  File: WebSmokeTestItem.cs
 *
 *  Purpose:  Contains web smoke test item data 
 *
 *  Date        Name                Reason
 *  08/06/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Contains web smoke test data pertaining to individual tests that can be run
    /// </summary>
    public sealed class WebSmokeTestItem
    {
        #region Constructors

        /// <summary>
        /// Default constructor for smoke test item
        /// </summary>
        public WebSmokeTestItem()
        {
            ResponseData = new List<string>();
        }

        /// <summary>
        /// Constructor for creating specific instances of smoke test items
        /// </summary>
        /// <param name="route">The route used for the test</param>
        /// <param name="method">The method used when submitting the test, PUT, GET, POST etc</param>
        /// <param name="response">The expected response for the result, 200, 404 etc</param>
        /// <param name="position">The position of the test relative to all other tests</param>
        /// <param name="name">Name of the test, used to identify it</param>
        /// <param name="inputData">The data that will be submitted for the test</param>
        public WebSmokeTestItem(in string route, in string method, in int response,
            in int position, in string name, in string inputData)
            : this(route, method, response, position, name, inputData, new List<string>())
        {
        }

        /// <summary>
        /// Constructor for creating specific instances of smoke test items
        /// </summary>
        /// <param name="route">The route used for the test</param>
        /// <param name="method">The method used when submitting the test, PUT, GET, POST etc</param>
        /// <param name="response">The expected response for the result, 200, 404 etc</param>
        /// <param name="position">The position of the test relative to all other tests</param>
        /// <param name="name">Name of the test, used to identify it</param>
        /// <param name="responseData">A list of string that can be searched for within the response for the request.</param>
        public WebSmokeTestItem(in string route, in string method, in int response,
            in int position, in string name, in List<string> responseData)
            : this(route, method, response, position, name, String.Empty, responseData)
        {
        }

        /// <summary>
        /// Constructor for creating specific instances of smoke test items
        /// </summary>
        /// <param name="route">The route used for the test</param>
        /// <param name="method">The method used when submitting the test, PUT, GET, POST etc</param>
        /// <param name="response">The expected response for the result, 200, 404 etc</param>
        /// <param name="position">The position of the test relative to all other tests</param>
        /// <param name="name">Name of the test, used to identify it</param>
        /// <param name="inputData">The data that will be submitted for the test</param>
        /// <param name="responseData">A list of string that can be searched for within the response for the request.</param>
        public WebSmokeTestItem(in string route, in string method, in int response,
            in int position, in string name, in string inputData,
            in List<string> responseData)
        {
            if (String.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            if (String.IsNullOrEmpty(method))
                throw new ArgumentNullException(nameof(method));

            Route = route;
            Method = method;
            Response = response;
            Position = position;
            Name = name ?? String.Empty;
            InputData = inputData ?? String.Empty;
            ResponseData = responseData ?? throw new ArgumentNullException(nameof(responseData));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The route used for the test
        /// </summary>
        /// <value>string</value>
        public string Route { get; set; }

        /// <summary>
        /// The method used when submitting the test, PUT, GET, POST etc
        /// </summary>
        /// <value>string</value>
        public string Method { get; set; }

        /// <summary>
        /// The expected response for the result, 200, 404 etc
        /// </summary>
        /// <value>int</value>
        public int Response { get; set; }

        /// <summary>
        /// The position of the test relative to all other tests, after being initially loaded
        /// tests are sorted by position, then name
        /// </summary>
        /// <value>int</value>
        public int Position { get; set; }

        /// <summary>
        /// Name of the test, used to identify it.  This can be an empty string, in which
        /// case the method name is used.
        /// </summary>
        /// <value>string</value>
        public string Name { get; set; }

        /// <summary>
        /// The data that will be submitted for the test, this can either be XML or Json data.
        /// The value can be the name of a resource string that contains the data.  Resource names
        /// will always be checked.
        /// </summary>
        /// <value>string</value>
        public string InputData { get; set; }

        /// <summary>
        /// A list of string that can be searched for within the response for the request.  If the 
        /// strings are not found the test would be deemed to fail
        /// </summary>
        /// <value>List&lt;string&gt;</value>
        public List<string> ResponseData { get; set; }

        #endregion Properties
    }
}
