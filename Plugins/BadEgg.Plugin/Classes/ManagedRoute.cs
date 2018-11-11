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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  BadEgg.Plugin
 *  
 *  File: ManagedRoutes.cs
 *
 *  Purpose:  Manages routes which are checked for sql injection attacks
 *
 *  Date        Name                Reason
 *  08/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace BadEgg.Plugin
{
    internal sealed class ManagedRoute
    {
        #region Constructors

        internal ManagedRoute(in string route, in bool validateQueryFields, in bool validateFormFields)
        {
            if (String.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            Route = route;
            ValidateFormFields = validateFormFields;
            ValidateQueryFields = validateQueryFields;
        }

        #endregion Constructors

        #region Properties

        internal string Route { get; set; }

        internal bool ValidateQueryFields { get; set; }

        internal bool ValidateFormFields { get; set; }

        #endregion Properties
    }
}
