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
 *  Product:  Documentation Plugin
 *  
 *  File: DocumentPostProcess.cs
 *
 *  Purpose:  Post process documentation once loaded
 *
 *  Date        Name                Reason
 *  12/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace DocumentationPlugin.Classes
{
	/// <summary>
	/// Class that contains the post processing statistics
	/// </summary>
	public class PostProcessResults
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public PostProcessResults()
		{
			Counts = [];
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Returns the total number of documents processed.
		/// </summary>
		/// <value>int</value>
		public int DocumentsProcessed { get; internal set; }

		/// <summary>
		/// Contains a dictionary of counts, these represent different elements within the post process results.
		/// </summary>
		/// <value>Dictionary&lt;string, int&gt;</value>
		private Dictionary<string, int> Counts { get; set; }

		#endregion Properties

		#region Public Methods

		/// <summary>
		/// Returns the count for the specificd countName
		/// </summary>
		/// <param name="countName">Name of count</param>
		/// <returns>int</returns>
		/// <exception cref="System.ArgumentNullException">Thrown when countName is null or empty.</exception>
		public int GetCountValue(in string countName)
		{
			if (string.IsNullOrEmpty(countName))
				throw new ArgumentNullException(nameof(countName));

			if (!Counts.ContainsKey(countName))
				return 0;

			return Counts[countName];
		}

		#endregion Public Methods

		#region Internal Methods

		/// <summary>
		/// Increments the count value for a specific count type
		/// </summary>
		/// <param name="countName">Name of count to be incremented</param>
		internal void IncrementCount(in string countName)
		{
			if (!Counts.ContainsKey(countName))
			{
				Counts.Add(countName, 0);
			}

			Counts[countName]++;
		}

		#endregion Internal Methods
	}
}
