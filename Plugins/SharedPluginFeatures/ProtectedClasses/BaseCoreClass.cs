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
 *  Product:  SharedPluginFeatures
 *  
 *  File: BaseCoreClass.cs
 *
 *  Purpose:  Base Core Class
 *
 *  Date        Name                Reason
 *  24/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.Extensions.Configuration;

namespace SharedPluginFeatures
{
    public class BaseCoreClass
    {
        #region Protected Methods

        protected T GetSettings<T>(in string jsonFile, in string sectionName)
        {
            if (String.IsNullOrEmpty(jsonFile))
                throw new ArgumentNullException(nameof(jsonFile));

            if (String.IsNullOrEmpty(sectionName))
                throw new ArgumentNullException(nameof(sectionName));

            ConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationBuilder configBuilder = builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
            configBuilder.AddJsonFile(jsonFile);
            IConfigurationRoot config = builder.Build();
            T Result = (T)Activator.CreateInstance(typeof(T));
            config.GetSection(sectionName).Bind(Result);

            return (Result);
        }

        protected T GetSettings<T>(in string sectionName)
        {
            return (GetSettings<T>("appsettings.json", sectionName));
        }

        #endregion Protected Methods
    }
}
