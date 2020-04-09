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
 *  File: TrimLine.cs
 *
 *  Purpose:  Trims lines of leading/trailing spaces
 *
 *  Date        Name                Reason
 *  23/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Text;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Classes.Minify
{
    internal sealed class TrimLine : MinifyOperation
    {
        public override IMinifyResult Process(in MinificationFileType fileType, ref string data, in List<PreserveBlock> preserveBlocks)
        {
            MinifyResult Result = new MinifyResult(nameof(TrimLine), data.Length);

            using (StopWatchTimer.Initialise(_timings))
            {
                switch (fileType)
                {
                    case MinificationFileType.Htm:
                    case MinificationFileType.Html:
                    case MinificationFileType.Razor:
                    case MinificationFileType.CSS:
                    case MinificationFileType.Js:
                    case MinificationFileType.Less:
                        data = TrimAllLines(data, preserveBlocks);
                        break;
                }
            }

            Result.Finalise(data.Length, _timings.Fastest);

            return Result;
        }

        private string TrimAllLines(string data, in List<PreserveBlock> preserveBlocks)
        {
            StringBuilder Result = new StringBuilder(data.Length);

            for (int i = 0; i < data.Length; i++)
            {
                char currentChar = data[i];
                bool peekBack = i > 0;
                bool peekForward = i < data.Length - 1;

                if (IsInPreBlock(i, preserveBlocks, out MinificationPreserveBlock _))
                {
                    Result.Append(currentChar);
                    continue;
                }

                if (currentChar == '\r')
                {
                    continue;
                }

                if (((currentChar == '\t' || currentChar == ' ') && peekBack && Result[Result.Length -1] == '\n') ||
                    (currentChar == ' ' && peekForward && (data[i + 1] == ' ' || data[i + 1] == '\n')))
                {
                    continue;
                }

                Result.Append(currentChar);
            }

            return Result.ToString().Trim();
        }
    }
}
