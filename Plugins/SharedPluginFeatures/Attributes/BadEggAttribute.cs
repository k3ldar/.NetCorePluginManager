﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  SharedPluginFeatures
 *  
 *  File: BadEggAttribute.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  08/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class BadEggAttribute : Attribute
    {
        #region Constructors

        public BadEggAttribute()
            : this(true, true)
        {

        }

        public BadEggAttribute(bool validateQuery, bool validateForm)
        {
            ValidateQueryFields = validateQuery;
            ValidateFormFields = validateForm;
        }

        #endregion Constructors

        #region Properties

        public bool ValidateQueryFields { get; private set; }

        public bool ValidateFormFields { get; private set; }

        #endregion Properties
    }
}
