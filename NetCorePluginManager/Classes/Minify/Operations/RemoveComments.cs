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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: RemoveComments.cs
 *
 *  Purpose:  Removes code comments from files
 *
 *  Date        Name                Reason
 *  23/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Text;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Classes.Minify
{
    internal class RemoveComments : MinifyOperation
    {
        #region Private Members

        private readonly string _commentStart;
        private readonly string _commentEnd;
        private readonly char _commentEndChar;

        #endregion Private Members

        #region Constructors

        public RemoveComments(string commentStart, string commentEnd)
        {
            if (String.IsNullOrEmpty(commentStart))
                throw new ArgumentNullException(nameof(commentStart));

            if (String.IsNullOrEmpty(commentEnd))
                throw new ArgumentNullException(nameof(commentEnd));

            _commentStart = commentStart;
            _commentEnd = commentEnd;
            _commentEndChar = commentEnd[commentEnd.Length - 1];
        }

        #endregion Constructors

        public override IMinifyResult Process(in MinificationFileType fileType, ref string data, in List<PreserveBlock> preserveBlocks)
        {
            MinifyResult Result = new MinifyResult(GetType().Name, data.Length);

            using (StopWatchTimer.Initialise(_timings))
            {
                switch (fileType)
                {
                    case MinificationFileType.Htm:
                    case MinificationFileType.Html:
                    case MinificationFileType.Razor:
                    case MinificationFileType.CSS:
                        data = RemoveCommentsFromFile(data, preserveBlocks);
                        break;
                }
            }

            Result.Finalise(data.Length, _timings.Fastest);

            return Result;
        }

        private string RemoveCommentsFromFile(string data, in List<PreserveBlock> preserveBlocks)
        {
            StringBuilder Result = new StringBuilder(data.Length);

            int startPos = data.IndexOf(_commentStart, StringComparison.Ordinal);

            if (startPos > -1)
            {
                bool isInComment = false;

                for (int i = 0; i < data.Length; i++)
                {
                    bool canPeekForward = i < data.Length - _commentStart.Length;
                    bool canPeekBack = i > _commentEnd.Length - 1;
                    char currentChar = data[i];

                    if (IsInPreBlock(i, preserveBlocks, out MinificationPreserveBlock _))
                    {
                        Result.Append(currentChar);
                        continue;
                    }

                    if (!canPeekBack && (!canPeekForward && !isInComment))
                    {
                        Result.Append(currentChar);
                        continue;
                    }

                    if (!isInComment &&
                        canPeekForward &&
                        data.Substring(i, _commentStart.Length) == _commentStart)
                    {
                        isInComment = true;
                    }

                    if (!isInComment)
                        Result.Append(currentChar);

                    if (isInComment &&
                        currentChar == _commentEndChar &&
                        canPeekBack &&
                        data.Substring(i - (_commentEnd.Length - 1), _commentEnd.Length) == _commentEnd)
                    {
                        isInComment = false;
                    }
                }
            }
            else
            {
                Result.Append(data);
            }

            return Result.ToString();
        }
    }
}
