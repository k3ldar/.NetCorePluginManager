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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: ProductGroupDataTriggers.cs
 *
 *  Purpose:  Triggers for product group table
 *
 *  Date        Name                Reason
 *  28/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
    internal class ProductGroupDataTriggers : ITableTriggers<ProductGroupDataRow>
    {
        private const int MinimumDescriptionLength = 5;
        private const int MaximumDescriptionLength = 50;

        public int Position => 0;

        public TriggerType TriggerTypes => TriggerType.BeforeInsert | TriggerType.BeforeUpdate;

        public void AfterDelete(List<ProductGroupDataRow> records)
        {

        }

        public void AfterInsert(List<ProductGroupDataRow> records)
        {

        }

        public void AfterUpdate(List<ProductGroupDataRow> records)
        {

        }

        public void BeforeDelete(List<ProductGroupDataRow> records)
        {

        }

        public void BeforeInsert(List<ProductGroupDataRow> records)
        {
            records.ForEach(r => ValidateData(r));
        }

        public void BeforeUpdate(List<ProductGroupDataRow> records)
        {
            records.ForEach(r => ValidateData(r));
        }

        public void BeforeUpdate(ProductGroupDataRow newRecord, ProductGroupDataRow oldRecord)
        {
            
        }

        private void ValidateData(ProductGroupDataRow row)
        {
            if (String.IsNullOrEmpty(row.Description))
                throw new InvalidDataRowException(nameof(ProductGroupDataRow), nameof(row.Description), "Can not be null or empty");

            if (row.Description.Length < MinimumDescriptionLength)
                throw new InvalidDataRowException(nameof(ProductGroupDataRow), nameof(row.Description), $"Minimum length for description is {MinimumDescriptionLength} characters");

            if (row.Description.Length > MaximumDescriptionLength)
                throw new InvalidDataRowException(nameof(ProductGroupDataRow), nameof(row.Description), $"Maximum length for description is {MaximumDescriptionLength} characters");
        }
    }
}
