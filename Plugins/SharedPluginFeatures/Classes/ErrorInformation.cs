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
 *  Copyright (c) 2012 - 2018 Simon Carter.  All Rights Reserved.
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
    public sealed class ErrorInformation
    {
        #region Constructors

        public ErrorInformation(in Exception error, in string errorIdentifier)
        {
            Error = error ?? throw new ArgumentNullException(nameof(error));

            if (String.IsNullOrEmpty(errorIdentifier))
                throw new ArgumentNullException(nameof(errorIdentifier));

            ErrorIdentifier = errorIdentifier;
        }

        #endregion Constructors

        #region Properties

        public bool Expired { get; set; }

        public Exception Error { get; private set; }

        public uint ErrorCount { get; private set; }

        public string ErrorIdentifier { get; private set; }

        #endregion Properties

        #region Internal Methods

        public void IncrementError()
        {
            ErrorCount++;
        }

        #endregion Internal Methods
    }
}
