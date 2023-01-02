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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: MinifyResult.cs
 *
 *  Purpose:  Results of minification
 *
 *  Date        Name                Reason
 *  23/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Classes.Minify
{
    /// <summary>
    /// Indicates the individual timings, start and end length of data and name of minification process.
    /// </summary>
    internal sealed class MinifyResult : IMinifyResult
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="processName">Name of individual minification process name.</param>
        /// <param name="startLength">Starting length of the data to be minified.</param>
        public MinifyResult(in string processName, in int startLength)
        {
            if (String.IsNullOrEmpty(processName))
                throw new ArgumentNullException(nameof(processName));

            if (startLength < 0)
                throw new ArgumentOutOfRangeException(nameof(startLength));

            ProcessName = processName;
            StartLength = startLength;
        }

        #endregion Constructors

        #region IMInifyResult Properties

        /// <summary>
        /// Name of process within the minification, if the minification is not split into individual elements then the name of the minification engine.
        /// </summary>
        /// <value>string</value>
        public string ProcessName { get; private set; }

        /// <summary>
        /// Length of data before minification has taken place.
        /// </summary>
        /// <value>int</value>
        public int StartLength { get; private set; }

        /// <summary>
        /// Length of data after minification has completed.
        /// </summary>
        /// <value>int</value>
        public int EndLength { get; private set; }

        /// <summary>
        /// Time taken to complete the minification
        /// </summary>
        /// <value>decimal</value>
        public decimal TimeTaken { get; private set; }

        #endregion IMInifyResult Properties

        #region Public Methods

        /// <summary>
        /// Called to indicate the minification process is complete and store the final length of data and time taken in 1000ths ms.
        /// </summary>
        /// <param name="endLength">Length of data after minification.</param>
        /// <param name="timeTaken">Time taken to complete the minification.</param>
        public void Finalise(int endLength, decimal timeTaken)
        {
            EndLength = endLength;
            TimeTaken = timeTaken;
        }

        #endregion Public Methods
    }
}
