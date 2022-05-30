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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: ReaderWriterInitializer.cs
 *
 *  Purpose:  ReaderWriterInitializer for text based storage
 *
 *  Date        Name                Reason
 *  23/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PluginManager.DAL.TextFiles.Interfaces;

namespace PluginManager.DAL.TextFiles
{
    public sealed class ReaderWriterInitializer : IReaderWriterInitializer
    {
        public const uint DefaultMinimumVersion = 1;

        private uint _minimumVersion = DefaultMinimumVersion;

        public ReaderWriterInitializer(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (!Directory.Exists(path))
                throw new ArgumentException($"Path does not exist: {path}", nameof(path));

            Path = path;
        }


        public string Path { get; private set; }

        public uint MinimumVersion 
        { 
            get
            {
                return _minimumVersion;
            }

            set
            {
                if (value < DefaultMinimumVersion)
                    value = DefaultMinimumVersion;

                _minimumVersion = value;
            }
        }
    }
}
