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
 *  File: SmokeTestAttribute.cs
 *
 *  Purpose:  Attribute for discovering tests
 *
 *  Date        Name                Reason
 *  08/06/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Attribute for generating automated smoke test, tests.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class SmokeTestAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Default constructor, this attribute needs to be applied agains
        /// a function that returns an <see cref="WebSmokeTestItem"/> instance
        /// containing test data
        /// </summary>
        public SmokeTestAttribute()
        {
            IsFromControllerAction = false;
        }

        /// <summary>
        /// Constructor for attribute applied against a controller action method.  This
        /// will create test data using the values of the action.
        /// </summary>
        /// <param name="position">The position of the test relative to all other tests</param>
        /// <param name="response">The expected response for the result, 200, 404 etc</param>
        /// <param name="method">The web method used when submitting the test, GET, POST, PUT etc</param>
        /// <param name="inputData">The data that will be submitted for the test, this must be a colon seperated list of strings representing parameter values to be submitted.</param>
        /// <param name="searchData">A colon seperated list of string that can be searched for within the response for the test.</param>
        /// <param name="name">Name of the test, used to identify it, if not specified then the controller and action name will be used.</param>
        public SmokeTestAttribute(int response, int position = 10000, string method = "", string inputData = "", string searchData = "", string name = "")
        {
            IsFromControllerAction = true;
            Response = response;
            Position = position;
            Method = method;
            InputData = inputData;
            SearchData = searchData;
            Name = name;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Indicates that the attribute was on a controller action or not
        /// </summary>
        public bool IsFromControllerAction { get; private set; }


        public int Response { get; private set; }


        public int Position { get; private set; }


        public string Method { get; private set; }


        public string InputData { get; private set; }


        public string SearchData { get; private set; }


        public string Name { get; private set; }

        #endregion Properties
    }
}
