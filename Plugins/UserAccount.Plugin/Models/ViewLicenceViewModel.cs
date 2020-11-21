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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  UserAccount.Plugin
 *  
 *  File: ViewLicenceViewModel.cs
 *
 *  Purpose:  View Licence view model, contains details on individual licences
 *
 *  Date        Name                Reason
 *  06/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

namespace UserAccount.Plugin.Models
{
#pragma warning disable CS1591

    public class ViewLicenceViewModel : BaseModel
    {
        #region Constructors

        public ViewLicenceViewModel()
        {

        }

        public ViewLicenceViewModel(in BaseModelData baseModelData,
            in int id, in string domain, in string licenceType, in bool active,
            in bool trial, in DateTime expires, in byte updates, in string licence)
            : base(baseModelData)
        {
            if (String.IsNullOrEmpty(licenceType))
                throw new ArgumentNullException(nameof(licenceType));

            Id = id;
            Domain = domain ?? throw new ArgumentNullException(nameof(domain));
            LicenceType = licenceType;
            Active = active;
            Trial = trial;
            Expires = expires;
            Licence = licence ?? throw new ArgumentNullException(nameof(licence));
        }

        #endregion Constructors

        #region Properties

        public int Id { get; set; }

        public string Domain { get; set; }

        public string LicenceType { get; set; }

        public bool Active { get; set; }

        public DateTime Expires { get; set; }

        public byte Updates { get; set; }

        public string Licence { get; set; }

        public bool Trial { get; set; }

        public string AvailableUpdates
        {
            get
            {
                return Convert.ToString(3 - Updates);
            }
        }

        #endregion Properties
    }

#pragma warning restore CS1591
}
