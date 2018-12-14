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
 *  Copyright (c) 2012 - 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatues
 *  
 *  File: IAccountProvider.cs
 *
 *  Purpose:  User Account provider
 *
 *  Date        Name                Reason
 *  09/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Text;

using static SharedPluginFeatures.Enums;

namespace SharedPluginFeatures
{
    public interface IAccountProvider
    {
        #region Change Password

        bool ChangePassword(in Int64 userId, in string newPassword);

        #endregion Change Password

        #region Address Options

        AddressOptions GetAddressOptions();

        #endregion Address Options

        #region User Contact Details

        bool GetUserAccountDetails(in Int64 userId, out string firstName, out string lastName, 
            out string email, out bool emailConfirmed, out string telephone, out bool telephoneConfirmed);

        bool SetUserAccountDetails(in Int64 userId, in string firstName, in string lastName, 
            in string email, in string telephone);

        bool ConfirmEmailAddress(in Int64 userId, in string confirmationCode);

        bool ConfirmTelephoneNumber(in Int64 userId, in string confirmationCode);

        #endregion User Contact Details
    }
}
