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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockActionDescriptorCollectionProvider.cs
 *
 *  Purpose:  Mock IActionDescriptorCollectionProvider class
 *
 *  Date        Name                Reason
 *  23/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace AspNetCore.PluginManager.Tests
{
    [ExcludeFromCodeCoverage]
    public class MockActionDescriptorCollectionProvider : IActionDescriptorCollectionProvider
    {
        #region Private Members

        private readonly ActionDescriptorCollection _actionDescriptorCollection;

        #endregion Private Members

        #region Constructors

        public MockActionDescriptorCollectionProvider(in ActionDescriptorCollection actionDescriptorCollection)
        {
            _actionDescriptorCollection = actionDescriptorCollection ?? throw new ArgumentNullException(nameof(actionDescriptorCollection));
        }

        #endregion Constructors

        #region IActionDescriptorCollectionProvider Properties
        public ActionDescriptorCollection ActionDescriptors
        {
            get
            {
                return _actionDescriptorCollection;
            }
        }

        #endregion IActionDescriptorCollectionProvider Properties
    }
}
