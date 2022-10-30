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
 *  Product:  SharedPluginFeatures
 *  
 *  File: IMinifyFileContents.cs
 *
 *  Purpose:  Interface to be used by the minification engine
 *
 *  Date        Name                Reason
 *  23/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System.Text;

namespace SharedPluginFeatures
{
    /// <summary>
    /// This interface is implemented by a third party minification engine.
    /// 
    /// The minification engine should add a transient entry to the IServiceCollection during initialisation.  If no plugin registers
    /// a new minification engine, then a default engine is supplied.
    /// </summary>
    public interface IMinificationEngine
    {
        /// <summary>
        /// Minify the contents of a file, this will use UTF8 encoding.
        /// </summary>
        /// <param name="fileType">Type of file to be minified.</param>
        /// <param name="contents">The contents of the file that should be minified.</param>
        /// <param name="result">List of minification results, the number of items depends on the implementation of the minificaiton engine.</param>
        /// <returns></returns>
        List<IMinifyResult> MinifyData(in MinificationFileType fileType, in string contents, out string result);

        /// <summary>
        /// Minify the contents of a file, this will use the UTF8 encoding.
        /// </summary>
        /// <param name="fileType">Type of file to be minified.</param>
        /// <param name="contents">The contents of the file that should be minified.</param>
        /// <param name="result">List of minification results, the number of items depends on the implementation of the minificaiton engine.</param>
        /// <returns></returns>
        List<IMinifyResult> MinifyData(in MinificationFileType fileType, in byte[] contents, out byte[] result);

        /// <summary>
        /// Minify the contents of a file.
        /// </summary>
        /// <param name="encoding">The encoding that should be used to decode and encode teh contents.</param>
        /// <param name="fileType">Type of file to be minified.</param>
        /// <param name="contents">The contents of the file that should be minified.</param>
        /// <param name="result">List of minification results, the number of items depends on the implementation of the minificaiton engine.</param>
        /// <returns></returns>
        List<IMinifyResult> MinifyData(in Encoding encoding, in MinificationFileType fileType, in byte[] contents, out byte[] result);
    }
}
