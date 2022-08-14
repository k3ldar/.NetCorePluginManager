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
 *  Product:  SimpleDB
 *  
 *  File: ITableTriggers.cs
 *
 *  Purpose:  Triggers interface for user table
 *
 *  Date        Name                Reason
 *  17/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */


namespace SimpleDB
{
    public interface ITableTriggers<T>
        where T : TableRowDefinition
    {
        int Position { get; }

        TriggerType TriggerTypes { get; }

        void BeforeInsert(List<T> records);

        void AfterInsert(List<T> records);

        void BeforeDelete(List<T> records);

        void AfterDelete(List<T> records);

        void BeforeUpdate(List<T> records);

        void BeforeUpdate(T newRecord, T oldRecord);

        void AfterUpdate(List<T> records);
    }
}
