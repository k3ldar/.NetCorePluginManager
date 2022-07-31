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
 *  File: CountryProvider.cs
 *
 *  Purpose:  ICountryProvider for text based storage
 *
 *  Date        Name                Reason
 *  25/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Middleware;

using PluginManager.DAL.TextFiles.Tables;
using PluginManager.SimpleDB;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal class CountryProvider : ICountryProvider
    {
        private readonly ITextTableOperations<CountryDataRow> _countries;

        public CountryProvider(ITextTableOperations<CountryDataRow> countries)
        {
            _countries = countries ?? throw new ArgumentNullException(nameof(countries));
        }

        public Country CountryCreate(in string name, in string code, in bool visible)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));

            if (code.Length > 3)
                throw new ArgumentException(nameof(code));

            CountryDataRow tableCountries = new CountryDataRow()
            {
                Name = name,
                Code = code,
                Visible = visible
            };

            _countries.Insert(tableCountries);
            return new Country(name, code, visible);
        }

        public bool CountryDelete(in Country country)
        {
            string code = country.Code;
            CountryDataRow tableCountry = _countries.Select().Where(c => c.Code.Equals(code)).FirstOrDefault();

            if (tableCountry == null)
                return false;

            tableCountry.Code = country.Code;
            tableCountry.Visible = country.Visible;
            tableCountry.Name = country.Name;
            _countries.Delete(tableCountry);

            return true;
        }

        public bool CountryUpdate(in Country country)
        {
            string code = country.Code;
            CountryDataRow tableCountry = _countries.Select().Where(c => c.Code.Equals(code)).FirstOrDefault();

            if (tableCountry == null)
                return false;

            tableCountry.Code = country.Code;
            tableCountry.Visible = country.Visible;
            tableCountry.Name = country.Name;
            _countries.Update(tableCountry);

            return true;
        }

        public List<Country> GetAllCountries()
        {
            return ConvertTableCountriesToCountries(_countries.Select());
        }

        public List<Country> GetVisibleCountries()
        {
            return ConvertTableCountriesToCountries(_countries.Select().Where(c => c.Visible).ToList());
        }

        private List<Country> ConvertTableCountriesToCountries(IReadOnlyList<CountryDataRow> tableCountries)
        {
            List<Country> Result = new List<Country>();

            foreach (CountryDataRow tableCountry in tableCountries)
            {
                Result.Add(new Country(tableCountry.Name, tableCountry.Code, tableCountry.Visible));
            }

            return Result;
        }
    }
}
