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
 *  File: DocumentData.cs
 *
 *  Purpose:  Provides extra information on documents
 *
 *  Date        Name                Reason
 *  24/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using Shared.Docs;

namespace DocumentationPlugin.Classes
{
	internal sealed class DocumentData
	{
		#region Constructors

		public DocumentData()
		{
			KeyNames = new Dictionary<string, string>();
			Contains = new Dictionary<string, string>();
			SeeAlso = new Dictionary<string, string>();
		}

		#endregion Constructors

		#region Properties

		public Dictionary<string, string> KeyNames { get; private set; }

		public Dictionary<string, string> Contains { get; private set; }

		public Dictionary<string, string> SeeAlso { get; private set; }

		public string AllReferences { get; internal set; }

		public string ReferenceData { get; set; }

		public DocumentData Parent { get; internal set; }

		public string FullClassName { get; set; }

		public string ShortClassName { get; set; }

		public Document PreviousDocument { get; set; }

		public Document NextDocument { get; set; }

		#endregion Properties
	}
}
