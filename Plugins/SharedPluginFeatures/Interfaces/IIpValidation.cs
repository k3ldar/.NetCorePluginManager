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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: IIpValidation.cs
 *
 *  Purpose:  Provides interface for Managing Ip connections
 *
 *  Date        Name                Reason
 *  05/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using static SharedPluginFeatures.Enums;

namespace SharedPluginFeatures
{
    public interface IIpValidation
    {
        void ConnectionAdd(in string ipAddress);

        void ConnectionRemove(in string ipAddress, in double hits, in ulong requests, in TimeSpan duration);

        void ConnectionReport(in string ipAddress, in string queryString, in ValidateRequestResult validation);

        bool ConnectionBan(in string ipAddress, in double hits, in ulong requests, in TimeSpan duration); 
    }
}
