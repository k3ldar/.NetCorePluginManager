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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles.Tests
 *  
 *  File: MockTableForeignKeyDefaultAllowed.cs
 *
 *  Purpose:  MockTableForeignKeyDefaultAllowed for text based storage
 *
 *  Date        Name                Reason
 *  02/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

namespace PluginManager.DAL.TextFiles.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    [Table("MockTableAddress", cachingStrategy: CachingStrategy.Memory)]
    public class MockTableForeignKeyDefaultAllowed : TableRowDefinition
    {
        private long _userId;

        public MockTableForeignKeyDefaultAllowed()
        {

        }

        public MockTableForeignKeyDefaultAllowed(long userId)
        {
            UserId = userId;
            Description = $"Address {userId}";
        }

        [ForeignKey("MockTableUser", true)]
        public long UserId
        {
            get => _userId;

            set
            {
                _userId = value;
                Update();
            }
        }

        public string Description { get; set; }
    }
}
