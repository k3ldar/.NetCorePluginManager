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
 *  File: DocumentLoadThread.cs
 *
 *  Purpose:  Loads documentation when the plugin is initialised
 *
 *  Date        Name                Reason
 *  19/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Shared.Classes;

using SharedPluginFeatures;

namespace DocumentationPlugin.Classes
{
	/// <summary>
	/// Thread that loads all documentation data when the plugin is initialised, preventing
	/// any delay in the showing of doucmentation which can take a little while to load depending on the 
	/// quantity and composition of document files.
	/// </summary>
	internal sealed class DocumentLoadThread : ThreadManager
	{
		#region Private Members

		private readonly IDocumentationService _documentationService;

		#endregion Private members

		#region Constructors

		public DocumentLoadThread(IDocumentationService documentationService)
			: base(null, new TimeSpan(0, 5, 0), null, 10000, 200, false, true)
		{
			HangTimeout = 0;
			_documentationService = documentationService ?? throw new ArgumentNullException(nameof(documentationService));
		}

		#endregion Constructors

		#region Overridden Methods

		protected override bool Run(object parameters)
		{
			DocumentPostProcess documentPostProcess = new(
				_documentationService.GetDocuments());

			PostProcessResults results = documentPostProcess.Process();

			return results.DocumentsProcessed == 0;
		}

		#endregion Overridden Methods
	}
}
