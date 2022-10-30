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
 *  Product:  DynamicContent.Plugin
 *  
 *  File: DeleteControlModel.cs
 *
 *  Purpose:  Delete control model
 *
 *  Date        Name                Reason
 *  20/12/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

#pragma warning disable CS1591

namespace DynamicContent.Plugin.Model
{
    public sealed class DeleteControlModel
    {
        public DeleteControlModel()
        {

        }

        public DeleteControlModel(string cacheId, string controlId)
        {
            if (String.IsNullOrEmpty(cacheId))
                throw new ArgumentNullException(nameof(cacheId));

            if (String.IsNullOrEmpty(controlId))
                throw new ArgumentNullException(nameof(controlId));

            CacheId = cacheId;
            ControlId = controlId;
        }

        public string CacheId { get; set; }

        public string ControlId { get; set; }
    }
}

#pragma warning restore CS1591