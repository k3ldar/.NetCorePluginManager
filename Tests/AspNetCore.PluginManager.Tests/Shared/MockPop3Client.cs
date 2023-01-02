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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockPop3Client.cs
 *
 *  Purpose:  Mock pop 3 client
 *
 *  Date        Name                Reason
 *  16/12/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shared.Communication;

namespace AspNetCore.PluginManager.Tests.Shared
{
	[ExcludeFromCodeCoverage]
	public sealed class MockPop3Client : IPop3Client
	{
		public bool IsConnected => throw new NotImplementedException();

		public string DeleteMessage(int messageNumber)
		{
			Messages[messageNumber -1] = "removed";
			return DeleteResponse;
		}

		public void Dispose()
		{
			DisposeCalled = true;
		}

		public int GetMailCount(out int sizeInOctets)
		{
			sizeInOctets = 32;
			return Messages.Count;
		}

		public void Initialize(string uri, string userName, string password, ushort port)
		{
			
		}

		public string RetrieveMessage(int messageNumber, out string readResponse)
		{
			readResponse = ReadResponse;
			return Messages[messageNumber -1];
		}

		public List<string> Messages { get; set; } = new();

		public string ReadResponse { get; set; } = "+OK";

		public string DeleteResponse { get; set; } = "+OK";

		public bool DisposeCalled { get; private set; } = false;
	}
}
