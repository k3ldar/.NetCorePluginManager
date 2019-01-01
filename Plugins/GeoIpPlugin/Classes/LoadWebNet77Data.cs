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
 *  Product:  GeoIpPlugin
 *  
 *  File: LoadWebNet77Data.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.IO;

using Shared.Classes;

namespace GeoIp.Plugin
{
    /// <summary>
    /// Background thread used to load WebNet77 Geo Ip Data
    /// </summary>
    public class LoadWebNet77Data : ThreadManager
    {
        #region Properties

        private readonly string _webNet77DataFile;

        #endregion Propertes

        #region Constructors

        public LoadWebNet77Data(string webNet77DataFile, List<IpCity> ipRangeData)
            : base (ipRangeData, new TimeSpan(24, 0, 0))
        {
            base.ContinueIfGlobalException = true;

            _webNet77DataFile = webNet77DataFile;
        }

        #endregion Constructors

        #region Overridden Methods

        protected override bool Run(object parameters)
        {
            List<IpCity> rangeData = (List<IpCity>)parameters;
            rangeData.Clear();

            // if available, load Webnet77 data
            if (File.Exists(_webNet77DataFile))
            {
                using (StreamReader stream = new StreamReader(_webNet77DataFile))
                {
                    while (!stream.EndOfStream)
                    {
                        string line = stream.ReadLine();

                        if (line[0] == '#' || line[0] == ' ')
                            continue;

                        if (HasCancelled())
                            return (false);

                        string[] parts = line.Split(',');

                        long startRange = Convert.ToInt64(parts[0].Replace("\"", ""));
                        long endRange = Convert.ToInt64(parts[1].Replace("\"", ""));
                        string country = parts[4];
                        rangeData.Add(new IpCity(startRange, endRange, country));
                    }
                }
            }

            rangeData.Sort();


            return (false);
        }

        #endregion Overridden Methods
    }
}
