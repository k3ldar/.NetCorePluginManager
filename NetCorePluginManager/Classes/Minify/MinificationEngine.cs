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
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: MinificationEngine.cs
 *
 *  Purpose:  Basic internal minification engine from minifying source files
 *
 *  Date        Name                Reason
 *  23/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Text;

using SharedPluginFeatures;

#pragma warning disable IDE0028

namespace AspNetCore.PluginManager.Classes.Minify
{
    internal sealed class MinificationEngine : IMinificationEngine
    {
        #region Private Members

        private readonly List<MinifyOperation> _operations;

        #endregion Private Members

        #region Constructor

        public MinificationEngine()
        {
            _operations = new List<MinifyOperation>();

            _operations.Add(new RemoveBlankLines());
            _operations.Add(new RemoveCarriageReturn());
            _operations.Add(new TrimLine());
            _operations.Add(new RemoveCSSComments());
            _operations.Add(new RemoveHtmlComments());
            _operations.Add(new RemoveRazorComments());
            _operations.Add(new RemoveDoubleSpaces());
            _operations.Add(new RemoveWhiteSpace());
        }

        #endregion Constructor

        #region IMinifyFileContents Methods

        public List<IMinifyResult> MinifyData(in MinificationFileType fileType, in string contents, out string result)
        {
            if (String.IsNullOrEmpty(contents))
                throw new ArgumentNullException(nameof(contents));

            List<IMinifyResult> Result;

            Result = MinifyData(Encoding.UTF8, fileType, Encoding.UTF8.GetBytes(contents), out byte[] returnData);

            result = Encoding.UTF8.GetString(returnData);

            return Result;
        }

        public List<IMinifyResult> MinifyData(in MinificationFileType fileType, in byte[] contents, out byte[] result)
        {
            return MinifyData(Encoding.UTF8, fileType, contents, out result);
        }

        public List<IMinifyResult> MinifyData(in Encoding encoding, in MinificationFileType fileType, in byte[] contents, out byte[] result)
        {
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            if (contents == null)
                throw new ArgumentNullException(nameof(contents));

            List<IMinifyResult> Result = new List<IMinifyResult>();

            switch (fileType)
            {
                case MinificationFileType.Razor:
                case MinificationFileType.CSS:
                case MinificationFileType.Htm:
                case MinificationFileType.Html:
                case MinificationFileType.Js:
                case MinificationFileType.Less:
                    string data = encoding.GetString(contents);

                    foreach (MinifyOperation operation in _operations)
                    {
                        Result.Add(operation.Process(fileType, ref data, PreserveBlocks(data, fileType == MinificationFileType.Razor)));
                    }

                    result = encoding.GetBytes(data);

                    break;

                case MinificationFileType.Unknown:
                default:
                    result = contents;
                    break;
            }

            return Result;
        }

        #endregion IMinifyFileContents Methods

        #region Private Methods

        private List<PreserveBlock> PreserveBlocks(string data, in bool isRazor)
        {
            const string preStart = "<pre>";
            const string preEnd = "</pre>";

            List<PreserveBlock> Result = new List<PreserveBlock>();

            int startPos = isRazor ? 0 : data.IndexOf(preStart, StringComparison.OrdinalIgnoreCase);

            if (startPos > -1)
            {
                // costly but...
                data = data.ToLower();

                bool isInPreBlock = false;

                for (int i = 0; i < data.Length; i++)
                {
                    bool canPeekForward = i < data.Length - preStart.Length;
                    bool canPeekBack = i > preEnd.Length - 1;
                    char currentChar = data[i];

                    if (!canPeekBack & !canPeekForward && !isInPreBlock)
                    {
                        continue;
                    }

                    if (!isInPreBlock && canPeekForward && data.Substring(i, preStart.Length) == preStart)
                    {
                        isInPreBlock = true;
                        MinificationPreserveBlock blockType = MinificationPreserveBlock.HtmlPreBlock;

                        Result.Add(new PreserveBlock(blockType, i));
                    }

                    if (isInPreBlock && currentChar == '>' && canPeekBack && data.Substring(i - 5, 6) == preEnd)
                    {
                        isInPreBlock = false;
                        Result[Result.Count - 1].SetBlockEnd(i);
                    }
                }
            }

            return Result;
        }

        #endregion Private Methods
    }
}

#pragma warning restore IDE0028
