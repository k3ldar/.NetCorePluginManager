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
 *  Copyright (c) 2012 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatues
 *  
 *  File: ErrorInformation.cs
 *
 *  Purpose:  Contains information about exceptions raised within a website
 *
 *  Date        Name                Reason
 *  17/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Used extensively by ErrorManager.Plugin module to store information about errors that have been generated within the system.
    /// 
    /// See also SystemAdmin.Plugin where error information is displayed.
    /// </summary>
    public sealed class ErrorInformation
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="error">Exception that was raised.</param>
        /// <param name="errorIdentifier">Unique identifier for the error.</param>
        public ErrorInformation(in Exception error, in string errorIdentifier)
        {
            Error = error ?? throw new ArgumentNullException(nameof(error));

            if (String.IsNullOrEmpty(errorIdentifier))
                throw new ArgumentNullException(nameof(errorIdentifier));

            ErrorIdentifier = errorIdentifier;

            Date = DateTime.Now;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The date and time the exception was first raised.
        /// </summary>
        /// <value>DateTime</value>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Indicates that the exception has expired.
        /// </summary>
        /// <value>bool</value>
        public bool Expired { get; set; }

        /// <summary>
        /// Exception that was raised.
        /// </summary>
        /// <value>System.Exception</value>
        public Exception Error { get; private set; }

        /// <summary>
        /// The number of times the exception has been raised.
        /// </summary>
        /// <value>uint</value>
        public uint ErrorCount { get; private set; }

        /// <summary>
        /// The identifier for the exception that was raised.
        /// </summary>
        /// <value>string</value>
        public string ErrorIdentifier { get; private set; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Used when the same exception is subsequently raised.
        /// </summary>
        public void IncrementError()
        {
            ErrorCount++;
        }

        #endregion Public Methods
    }
}
