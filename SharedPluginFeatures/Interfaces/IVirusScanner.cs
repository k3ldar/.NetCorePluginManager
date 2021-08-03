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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: IVirusScanner.cs
 *
 *  Purpose:  Provides interface for scanning a file or folder for viruses
 *
 *  Date        Name                Reason
 *  04/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SharedPluginFeatures
{
    /// <summary>
    /// Interface for antivirus scanning of files or directories
    /// </summary>
    public interface IVirusScanner
    {
        /// <summary>
        /// Forces a scan of the directory specified, if it exists.
        /// </summary>
        /// <param name="directory">Name of directory that will be scanned.</param>
        void ScanDirectory(in string directory);

        /// <summary>
        /// Forces a scan of an individual file
        /// </summary>
        /// <param name="fileName">Name of file to be scanned, must include the full path to the file</param>
        void ScanFile(in string fileName);

        /// <summary>
        /// Forces a scan of an array of files
        /// </summary>
        /// <param name="fileNames">Name of files to be scanned, must include the full path to the file</param>
        void ScanFile(in string[] fileNames);
    }
}
