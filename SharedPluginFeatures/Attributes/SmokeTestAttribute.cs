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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
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
        /// Default constructor, this attribute needs to be applied against
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
        /// <param name="formId">The name of the form that will be submitted</param>
        /// <param name="postType">The type of data to be posted to the controller action</param>
        /// <param name="response">The expected response for the result, 200, 404 etc</param>
        /// <param name="method">The web method used when submitting the test, GET, POST, PUT etc</param>
        /// <param name="inputData">The data that will be submitted for the test, this must be a colon seperated list of strings representing parameter values to be submitted or can also be raw json or XML.</param>
        /// <param name="searchData">A colon seperated list of string that can be searched for within the response after either a get or prior to data being submitted via a form.</param>
        /// <param name="submitSearchData">A colon seperated list of strings that can be searched for in the response after data has been submitted.</param>
        /// <param name="name">Name of the test, used to identify it, if not specified then the controller and action name will be used.</param>
        /// <param name="parameters">Name value pair of valid parameter values for this test</param>
        /// <param name="redirectUrl">The expected url where the response will be redirected to.</param>
        public SmokeTestAttribute(int response,
            PostType postType = PostType.Form,
            string formId = "",
            int position = 10000,
            string method = "",
            string inputData = "",
            string searchData = "",
            string submitSearchData = "",
            string name = "",
            string parameters = "",
            string redirectUrl = "/")
        {
            IsFromControllerAction = true;
            Response = response;
            PostType = postType;
            Position = position;
            Method = method;
            InputData = inputData;
            SubmitSearchData = submitSearchData;
            SearchData = searchData;
            Name = name;
            FormId = formId;
            Parameters = parameters;
            RedirectUrl = redirectUrl;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Indicates that the attribute was on a controller action or not
        /// </summary>
        /// <value>bool</value>
        public bool IsFromControllerAction { get; private set; }

        /// <summary>
        /// The response expected after submitting the test
        /// </summary>
        /// <value>int</value>
        public int Response { get; private set; }

        /// <summary>
        /// The type of data to be posted
        /// </summary>
        public PostType PostType { get; private set; }

        /// <summary>
        /// The relative position of this test compared to other tests.
        /// </summary>
        /// <value>int</value>
        public int Position { get; private set; }

        /// <summary>
        /// Method used to submit data, GET, POST etc
        /// </summary>
        /// <value>string</value>
        public string Method { get; private set; }

        /// <summary>
        /// Data to be input, if a FormId is specified this directly relates to the fields in the mode
        /// that will be submitted, as name value pairs.
        /// 
        /// If FormId is not specified this can be json or xml data that will be submitted as part of the body
        /// </summary>
        /// <value>string</value>
        public string InputData { get; private set; }

        /// <summary>
        /// Data to be searched for in the response returned by the test, if the test involves
        /// submitting data then SearchData will be sought from the get request when the form 
        /// is identified.
        /// </summary>
        /// <value>string</value>
        public string SearchData { get; private set; }

        /// <summary>
        /// Data to be searched for in the response returned by the test, after the form
        /// has been submitted.  To search for data prior to submitting data use <see cref="SearchData" />
        /// </summary>
        /// <value>string</value>
        public string SubmitSearchData { get; private set; }

        /// <summary>
        /// Name of the test
        /// </summary>
        /// <value>string</value>
        public string Name { get; private set; }

        /// <summary>
        /// The name of the form to be submitted
        /// </summary>
        /// <value>string</value>
        public string FormId { get; private set; }

        /// <summary>
        /// Parameters values for the method being implemented
        /// </summary>
        /// <value>string</value>
        public string Parameters { get; private set; }

        /// <summary>
        /// The expected url where the response will be redirected to
        /// </summary>
        /// <value>string</value>
        public string RedirectUrl { get; private set; }

        #endregion Properties
    }
}
