﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: RemoveDoubleSpaces.cs
 *
 *  Purpose:  Removes double spaces from source files
 *
 *  Date        Name                Reason
 *  23/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System.Text;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Classes.Minify
{
	internal class RemoveDoubleSpaces : MinifyOperation
	{
		public override IMinifyResult Process(in MinificationFileType fileType, ref string data, in List<PreserveBlock> preserveBlocks)
		{
			MinifyResult Result = new(nameof(RemoveCarriageReturn), data.Length);

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
						data = RemoveCarriageReturns(data, preserveBlocks);
						break;
				}
			}

			Result.Finalise(data.Length, _timings.Fastest);

			return Result;
		}

		private static string RemoveCarriageReturns(string data, in List<PreserveBlock> preserveBlocks)
		{
			StringBuilder Result = new(data.Length);

			for (int i = 0; i < data.Length; i++)
			{
				bool peekForward = i < data.Length - 1;

				char currentChar = data[i];

				if (IsInPreBlock(i, preserveBlocks, out MinificationPreserveBlock _))
				{
					Result.Append(currentChar);
					continue;
				}

				if (currentChar == ' ' &&
					peekForward && data[i + 1] == ' ')
				{
					continue;
				}

				Result.Append(currentChar);
			}

			return Result.ToString();
		}
	}
}
