/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  File: PreserveBlock.cs
 *
 *  Purpose:  Data blocks to be preserved.
 *
 *  Date        Name                Reason
 *  26/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Classes.Minify
{
	/// <summary>
	/// Represents a block of preserved data that should not be modified during minification
	/// </summary>
	public sealed class PreserveBlock
	{
		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="blockType">Type of minification block.</param>
		/// <param name="startBlock">Start of block within the data.</param>
		public PreserveBlock(in MinificationPreserveBlock blockType, in int startBlock)
		{
			if (startBlock < 0)
				throw new ArgumentOutOfRangeException(nameof(startBlock));

			BlockStart = startBlock;
			BlockType = blockType;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Starting index of the block of data.
		/// </summary>
		/// <value>int</value>
		public int BlockStart { get; private set; }

		/// <summary>
		/// Ending index of the block of data.
		/// </summary>
		/// <value>int</value>
		public int BlockEnd { get; private set; }

		/// <summary>
		/// Type of preserved block
		/// </summary>
		/// <value>MinificationPreserveBlock</value>
		public MinificationPreserveBlock BlockType { get; private set; }

		#endregion Properties

		#region Internal Methods

		internal void SetBlockEnd(in int endBlock)
		{
			if (endBlock < 0)
				throw new ArgumentOutOfRangeException(nameof(endBlock));

			BlockEnd = endBlock;
		}

		#endregion Internal Methods
	}
}
