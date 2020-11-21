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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: MinifyOperation.cs
 *
 *  Purpose:  Basic minification engine
 *
 *  Date        Name                Reason
 *  23/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Classes.Minify
{
    internal abstract class MinifyOperation
    {
        protected Timings _timings = new Timings();

        public abstract IMinifyResult Process(in MinificationFileType fileType, ref string data, in List<PreserveBlock> preserveBlocks);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Intended for dev not public use.")]
        protected bool IsInPreBlock(in int currentPosition, in List<PreserveBlock> preserveBlocks, out MinificationPreserveBlock blockType)
        {
            blockType = MinificationPreserveBlock.Undefined;

            if (currentPosition < 0)
                throw new ArgumentOutOfRangeException(nameof(currentPosition), "Must be greater or equal to 0");

            if (preserveBlocks == null)
                throw new ArgumentNullException(nameof(preserveBlocks));


            for (int i = 0; i < preserveBlocks.Count; i++)
            {
                if (currentPosition >= preserveBlocks[i].BlockStart && currentPosition <= preserveBlocks[i].BlockEnd)
                {
                    blockType = preserveBlocks[i].BlockType;
                    return true;
                }
            }

            return false;
        }

    }
}
