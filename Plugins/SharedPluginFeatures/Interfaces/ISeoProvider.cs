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
 *  File: ISeoProvider.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  12/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

namespace SharedPluginFeatures
{
    public interface ISeoProvider
    {
        bool GetSeoDataForRoute(in string route, out string title, out string metaDescription, 
            out string author, out List<string> keywords);

        bool UpdateTitle(in string route, in string title);

        bool UpdateDescription(in string route, in string title);

        bool UpdateAuthor(in string route, in string author);

        bool AddKeyword(in string route, in string keyword);

        bool RemoveKeyword(in string route, in string keyword);

        bool AddKeywords(in string route, in List<string> keyword);

        bool RemoveKeywords(in string route, in List<string> keyword);
    }
}
