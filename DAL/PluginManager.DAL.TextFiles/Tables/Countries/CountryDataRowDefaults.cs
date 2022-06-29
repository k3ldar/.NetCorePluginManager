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
 *  File: CountryDataRowDefaults.cs
 *
 *  Purpose:  Default initialisation values for country table
 *
 *  Date        Name                Reason
 *  18/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace PluginManager.DAL.TextFiles.Tables
{
    internal class CountryDataRowDefaults : ITableDefaults<CountryDataRow>
    {
        public long PrimarySequence => 0;

        public long SecondarySequence => 0;

        public ushort Version => 1;

        List<CountryDataRow> ITableDefaults<CountryDataRow>.InitialData(ushort version)
        {
            if (version == 1)
                return GetVersion1Data();

            return null;
        }

        private List<CountryDataRow> GetVersion1Data()
        {
            List<CountryDataRow> initialData = new List<CountryDataRow>();

            initialData.Add(new CountryDataRow() { Code = "ZZ", Name = "Unknown Country", Visible = false });
            initialData.Add(new CountryDataRow() { Code = "GB", Name = "United Kingdom" });
            initialData.Add(new CountryDataRow() { Code = "US", Name = "United States of America" });
            initialData.Add(new CountryDataRow() { Code = "AF", Name = "Afghanistan" });
            initialData.Add(new CountryDataRow() { Code = "AL", Name = "Albania" });
            initialData.Add(new CountryDataRow() { Code = "DZ", Name = "Algeria" });
            initialData.Add(new CountryDataRow() { Code = "AS", Name = "American Samoa" });
            initialData.Add(new CountryDataRow() { Code = "AD", Name = "Andorra" });
            initialData.Add(new CountryDataRow() { Code = "AO", Name = "Angola" });
            initialData.Add(new CountryDataRow() { Code = "AI", Name = "Anguilla" });
            initialData.Add(new CountryDataRow() { Code = "AQ", Name = "Antarctica" });
            initialData.Add(new CountryDataRow() { Code = "AG", Name = "Antigua and Barbuda" });
            initialData.Add(new CountryDataRow() { Code = "AR", Name = "Argentina" });
            initialData.Add(new CountryDataRow() { Code = "AM", Name = "Armenia" });
            initialData.Add(new CountryDataRow() { Code = "AW", Name = "Aruba" });
            initialData.Add(new CountryDataRow() { Code = "AU", Name = "Australia" });
            initialData.Add(new CountryDataRow() { Code = "AT", Name = "Austria" });
            initialData.Add(new CountryDataRow() { Code = "AZ", Name = "Azerbaijan" });
            initialData.Add(new CountryDataRow() { Code = "BS", Name = "Bahamas" });
            initialData.Add(new CountryDataRow() { Code = "BH", Name = "Bahrain" });
            initialData.Add(new CountryDataRow() { Code = "BD", Name = "Bangladesh" });
            initialData.Add(new CountryDataRow() { Code = "BB", Name = "Barbados" });
            initialData.Add(new CountryDataRow() { Code = "BY", Name = "Belarus" });
            initialData.Add(new CountryDataRow() { Code = "BE", Name = "Belgium" });
            initialData.Add(new CountryDataRow() { Code = "BZ", Name = "Belize" });
            initialData.Add(new CountryDataRow() { Code = "BJ", Name = "Benin" });
            initialData.Add(new CountryDataRow() { Code = "BM", Name = "Bermuda" });
            initialData.Add(new CountryDataRow() { Code = "BT", Name = "Bhutan" });
            initialData.Add(new CountryDataRow() { Code = "BO", Name = "Bolivia" });
            initialData.Add(new CountryDataRow() { Code = "BA", Name = "Bosnia and Herzegovina" });
            initialData.Add(new CountryDataRow() { Code = "BW", Name = "Botswana" });
            initialData.Add(new CountryDataRow() { Code = "BV", Name = "Bouvet Island" });
            initialData.Add(new CountryDataRow() { Code = "BR", Name = "Brazil" });
            initialData.Add(new CountryDataRow() { Code = "IO", Name = "British Indian Ocean Territory" });
            initialData.Add(new CountryDataRow() { Code = "BN", Name = "Brunei Darussalam" });
            initialData.Add(new CountryDataRow() { Code = "BG", Name = "Bulgaria" });
            initialData.Add(new CountryDataRow() { Code = "BF", Name = "Burkina Faso" });
            initialData.Add(new CountryDataRow() { Code = "BI", Name = "Burundi" });
            initialData.Add(new CountryDataRow() { Code = "KH", Name = "Cambodia" });
            initialData.Add(new CountryDataRow() { Code = "CM", Name = "Cameroon" });
            initialData.Add(new CountryDataRow() { Code = "CA", Name = "Canada" });
            initialData.Add(new CountryDataRow() { Code = "CV", Name = "Cape Verde" });
            initialData.Add(new CountryDataRow() { Code = "KY", Name = "Cayman Islands" });
            initialData.Add(new CountryDataRow() { Code = "CF", Name = "Central African Republic" });
            initialData.Add(new CountryDataRow() { Code = "TD", Name = "Chad" });
            initialData.Add(new CountryDataRow() { Code = "CL", Name = "Chile" });
            initialData.Add(new CountryDataRow() { Code = "CN", Name = "China" });
            initialData.Add(new CountryDataRow() { Code = "CX", Name = "Christmas Island" });
            initialData.Add(new CountryDataRow() { Code = "CC", Name = "Cocos (Keeling) Islands" });
            initialData.Add(new CountryDataRow() { Code = "CO", Name = "Colombia" });
            initialData.Add(new CountryDataRow() { Code = "KM", Name = "Comoros" });
            initialData.Add(new CountryDataRow() { Code = "CG", Name = "Congo" });
            initialData.Add(new CountryDataRow() { Code = "CK", Name = "Cook Islands" });
            initialData.Add(new CountryDataRow() { Code = "CR", Name = "Costa Rica" });
            initialData.Add(new CountryDataRow() { Code = "CI", Name = "Côte d'Ivoire" });
            initialData.Add(new CountryDataRow() { Code = "HR", Name = "Croatia" });
            initialData.Add(new CountryDataRow() { Code = "CU", Name = "Cuba" });
            initialData.Add(new CountryDataRow() { Code = "CY", Name = "Cyprus" });
            initialData.Add(new CountryDataRow() { Code = "CZ", Name = "Czech Republic" });
            initialData.Add(new CountryDataRow() { Code = "DK", Name = "Denmark" });
            initialData.Add(new CountryDataRow() { Code = "DJ", Name = "Djibouti" });
            initialData.Add(new CountryDataRow() { Code = "DM", Name = "Dominica" });
            initialData.Add(new CountryDataRow() { Code = "DO", Name = "Dominican Republic" });
            initialData.Add(new CountryDataRow() { Code = "TP", Name = "East Timor" });
            initialData.Add(new CountryDataRow() { Code = "EC", Name = "Ecuador" });
            initialData.Add(new CountryDataRow() { Code = "EG", Name = "Egypt" });
            initialData.Add(new CountryDataRow() { Code = "SV", Name = "El salvador" });
            initialData.Add(new CountryDataRow() { Code = "GQ", Name = "Equatorial Guinea" });
            initialData.Add(new CountryDataRow() { Code = "ER", Name = "Eritrea" });
            initialData.Add(new CountryDataRow() { Code = "EE", Name = "Estonia" });
            initialData.Add(new CountryDataRow() { Code = "ET", Name = "Ethiopia" });
            initialData.Add(new CountryDataRow() { Code = "FK", Name = "Falkland Islands" });
            initialData.Add(new CountryDataRow() { Code = "FO", Name = "Faroe Islands" });
            initialData.Add(new CountryDataRow() { Code = "FJ", Name = "Fiji" });
            initialData.Add(new CountryDataRow() { Code = "FI", Name = "Finland" });
            initialData.Add(new CountryDataRow() { Code = "FR", Name = "France" });
            initialData.Add(new CountryDataRow() { Code = "GF", Name = "French Guiana" });
            initialData.Add(new CountryDataRow() { Code = "PF", Name = "French Polynesia" });
            initialData.Add(new CountryDataRow() { Code = "TF", Name = "French Southern Territories" });
            initialData.Add(new CountryDataRow() { Code = "GA", Name = "Gabon" });
            initialData.Add(new CountryDataRow() { Code = "GM", Name = "Gambia" });
            initialData.Add(new CountryDataRow() { Code = "GE", Name = "Georgia" });
            initialData.Add(new CountryDataRow() { Code = "DE", Name = "Germany" });
            initialData.Add(new CountryDataRow() { Code = "GH", Name = "Ghana" });
            initialData.Add(new CountryDataRow() { Code = "GI", Name = "Gibraltar" });
            initialData.Add(new CountryDataRow() { Code = "GR", Name = "Greece" });
            initialData.Add(new CountryDataRow() { Code = "GL", Name = "Greenland" });
            initialData.Add(new CountryDataRow() { Code = "GD", Name = "Grenada" });
            initialData.Add(new CountryDataRow() { Code = "GP", Name = "Guadeloupe" });
            initialData.Add(new CountryDataRow() { Code = "GU", Name = "Guam" });
            initialData.Add(new CountryDataRow() { Code = "GT", Name = "Guatemala" });
            initialData.Add(new CountryDataRow() { Code = "GN", Name = "Guinea" });
            initialData.Add(new CountryDataRow() { Code = "GW", Name = "Guinea-Bissau" });
            initialData.Add(new CountryDataRow() { Code = "GY", Name = "Guyana" });
            initialData.Add(new CountryDataRow() { Code = "HT", Name = "Haiti" });
            initialData.Add(new CountryDataRow() { Code = "HM", Name = "Heard Island and McDonald Islands" });
            initialData.Add(new CountryDataRow() { Code = "VA", Name = "Holy See (Vatican City State)" });
            initialData.Add(new CountryDataRow() { Code = "HN", Name = "Honduras" });
            initialData.Add(new CountryDataRow() { Code = "HK", Name = "Hong Kong" });
            initialData.Add(new CountryDataRow() { Code = "HU", Name = "Hungary" });
            initialData.Add(new CountryDataRow() { Code = "IS", Name = "Iceland" });
            initialData.Add(new CountryDataRow() { Code = "IN", Name = "India" });
            initialData.Add(new CountryDataRow() { Code = "ID", Name = "Indonesia" });
            initialData.Add(new CountryDataRow() { Code = "IR", Name = "Iran" });
            initialData.Add(new CountryDataRow() { Code = "IQ", Name = "Iraq" });
            initialData.Add(new CountryDataRow() { Code = "IE", Name = "Ireland" });
            initialData.Add(new CountryDataRow() { Code = "IL", Name = "Israel" });
            initialData.Add(new CountryDataRow() { Code = "IT", Name = "Italy" });
            initialData.Add(new CountryDataRow() { Code = "JM", Name = "Jamaica" });
            initialData.Add(new CountryDataRow() { Code = "JP", Name = "Japan" });
            initialData.Add(new CountryDataRow() { Code = "JO", Name = "Jordan" });
            initialData.Add(new CountryDataRow() { Code = "KZ", Name = "Kazakstan" });
            initialData.Add(new CountryDataRow() { Code = "KE", Name = "Kenya" });
            initialData.Add(new CountryDataRow() { Code = "KI", Name = "Kiribati" });
            initialData.Add(new CountryDataRow() { Code = "KW", Name = "Kuwait" });
            initialData.Add(new CountryDataRow() { Code = "KG", Name = "Kyrgystan" });
            initialData.Add(new CountryDataRow() { Code = "LA", Name = "Lao" });
            initialData.Add(new CountryDataRow() { Code = "LV", Name = "Latvia" });
            initialData.Add(new CountryDataRow() { Code = "LB", Name = "Lebanon" });
            initialData.Add(new CountryDataRow() { Code = "LS", Name = "Lesotho" });
            initialData.Add(new CountryDataRow() { Code = "LR", Name = "Liberia" });
            initialData.Add(new CountryDataRow() { Code = "LY", Name = "Libyan Arab Jamahiriya" });
            initialData.Add(new CountryDataRow() { Code = "LI", Name = "Liechtenstein" });
            initialData.Add(new CountryDataRow() { Code = "LT", Name = "Lithuania" });
            initialData.Add(new CountryDataRow() { Code = "LU", Name = "Luxembourg" });
            initialData.Add(new CountryDataRow() { Code = "MO", Name = "Macau" });
            initialData.Add(new CountryDataRow() { Code = "MK", Name = "Macedonia (FYR)" });
            initialData.Add(new CountryDataRow() { Code = "MG", Name = "Madagascar" });
            initialData.Add(new CountryDataRow() { Code = "MW", Name = "Malawi" });
            initialData.Add(new CountryDataRow() { Code = "MY", Name = "Malaysia" });
            initialData.Add(new CountryDataRow() { Code = "MV", Name = "Maldives" });
            initialData.Add(new CountryDataRow() { Code = "ML", Name = "Mali" });
            initialData.Add(new CountryDataRow() { Code = "MT", Name = "Malta" });
            initialData.Add(new CountryDataRow() { Code = "MH", Name = "Marshall Islands" });
            initialData.Add(new CountryDataRow() { Code = "MQ", Name = "Martinique" });
            initialData.Add(new CountryDataRow() { Code = "MR", Name = "Mauritania" });
            initialData.Add(new CountryDataRow() { Code = "MU", Name = "Mauritius" });
            initialData.Add(new CountryDataRow() { Code = "YT", Name = "Mayotte" });
            initialData.Add(new CountryDataRow() { Code = "MX", Name = "Mexico" });
            initialData.Add(new CountryDataRow() { Code = "FM", Name = "Micronesia" });
            initialData.Add(new CountryDataRow() { Code = "MD", Name = "Moldova" });
            initialData.Add(new CountryDataRow() { Code = "MC", Name = "Monaco" });
            initialData.Add(new CountryDataRow() { Code = "MN", Name = "Mongolia" });
            initialData.Add(new CountryDataRow() { Code = "MS", Name = "Montserrat" });
            initialData.Add(new CountryDataRow() { Code = "MA", Name = "Morocco" });
            initialData.Add(new CountryDataRow() { Code = "MZ", Name = "Mozambique" });
            initialData.Add(new CountryDataRow() { Code = "MM", Name = "Myanmar" });
            initialData.Add(new CountryDataRow() { Code = "NA", Name = "Namibia" });
            initialData.Add(new CountryDataRow() { Code = "NR", Name = "Nauru" });
            initialData.Add(new CountryDataRow() { Code = "NP", Name = "Nepal" });
            initialData.Add(new CountryDataRow() { Code = "NL", Name = "Netherlands" });
            initialData.Add(new CountryDataRow() { Code = "AN", Name = "Netherlands Antilles" });
            initialData.Add(new CountryDataRow() { Code = "NT", Name = "Neutral Zone" });
            initialData.Add(new CountryDataRow() { Code = "NC", Name = "New Caledonia" });
            initialData.Add(new CountryDataRow() { Code = "NZ", Name = "New Zealand" });
            initialData.Add(new CountryDataRow() { Code = "NI", Name = "Nicaragua" });
            initialData.Add(new CountryDataRow() { Code = "NE", Name = "Niger" });
            initialData.Add(new CountryDataRow() { Code = "NG", Name = "Nigeria" });
            initialData.Add(new CountryDataRow() { Code = "NU", Name = "Niue" });
            initialData.Add(new CountryDataRow() { Code = "NF", Name = "Norfolk Island" });
            initialData.Add(new CountryDataRow() { Code = "KP", Name = "North Korea" });
            initialData.Add(new CountryDataRow() { Code = "MP", Name = "Northern Mariana Islands" });
            initialData.Add(new CountryDataRow() { Code = "NO", Name = "Norway" });
            initialData.Add(new CountryDataRow() { Code = "OM", Name = "Oman" });
            initialData.Add(new CountryDataRow() { Code = "PK", Name = "Pakistan" });
            initialData.Add(new CountryDataRow() { Code = "PW", Name = "Palau" });
            initialData.Add(new CountryDataRow() { Code = "PA", Name = "Panama" });
            initialData.Add(new CountryDataRow() { Code = "PG", Name = "Papua New Guinea" });
            initialData.Add(new CountryDataRow() { Code = "PY", Name = "Paraguay" });
            initialData.Add(new CountryDataRow() { Code = "PE", Name = "Peru" });
            initialData.Add(new CountryDataRow() { Code = "PH", Name = "Philippines" });
            initialData.Add(new CountryDataRow() { Code = "PN", Name = "Pitcairn" });
            initialData.Add(new CountryDataRow() { Code = "PL", Name = "Poland" });
            initialData.Add(new CountryDataRow() { Code = "PT", Name = "Portugal" });
            initialData.Add(new CountryDataRow() { Code = "PR", Name = "Puerto Rico" });
            initialData.Add(new CountryDataRow() { Code = "QA", Name = "Qatar" });
            initialData.Add(new CountryDataRow() { Code = "RE", Name = "Reunion" });
            initialData.Add(new CountryDataRow() { Code = "RO", Name = "Romania" });
            initialData.Add(new CountryDataRow() { Code = "RU", Name = "Russian Federation" });
            initialData.Add(new CountryDataRow() { Code = "RW", Name = "Rwanda" });
            initialData.Add(new CountryDataRow() { Code = "SH", Name = "Saint Helena" });
            initialData.Add(new CountryDataRow() { Code = "KN", Name = "Saint Kitts and Nevis" });
            initialData.Add(new CountryDataRow() { Code = "LC", Name = "Saint Lucia" });
            initialData.Add(new CountryDataRow() { Code = "PM", Name = "Saint Pierre and Miquelon" });
            initialData.Add(new CountryDataRow() { Code = "VC", Name = "Saint Vincent and the Grenadines" });
            initialData.Add(new CountryDataRow() { Code = "WS", Name = "Samoa" });
            initialData.Add(new CountryDataRow() { Code = "SM", Name = "San Marino" });
            initialData.Add(new CountryDataRow() { Code = "ST", Name = "Sao Tome and Principe" });
            initialData.Add(new CountryDataRow() { Code = "SA", Name = "Saudi Arabia" });
            initialData.Add(new CountryDataRow() { Code = "SN", Name = "Senegal" });
            initialData.Add(new CountryDataRow() { Code = "SC", Name = "Seychelles" });
            initialData.Add(new CountryDataRow() { Code = "SL", Name = "Sierra Leone" });
            initialData.Add(new CountryDataRow() { Code = "SG", Name = "Singapore" });
            initialData.Add(new CountryDataRow() { Code = "SK", Name = "Slovakia" });
            initialData.Add(new CountryDataRow() { Code = "SI", Name = "Slovenia" });
            initialData.Add(new CountryDataRow() { Code = "SB", Name = "Solomon Islands" });
            initialData.Add(new CountryDataRow() { Code = "SO", Name = "Somalia" });
            initialData.Add(new CountryDataRow() { Code = "ZA", Name = "South Africa" });
            initialData.Add(new CountryDataRow() { Code = "GS", Name = "South Georgia" });
            initialData.Add(new CountryDataRow() { Code = "KR", Name = "South Korea" });
            initialData.Add(new CountryDataRow() { Code = "ES", Name = "Spain" });
            initialData.Add(new CountryDataRow() { Code = "LK", Name = "Sri Lanka" });
            initialData.Add(new CountryDataRow() { Code = "SD", Name = "Sudan" });
            initialData.Add(new CountryDataRow() { Code = "SR", Name = "Suriname" });
            initialData.Add(new CountryDataRow() { Code = "SJ", Name = "Svalbard and Jan Mayen Islands" });
            initialData.Add(new CountryDataRow() { Code = "SZ", Name = "Swaziland" });
            initialData.Add(new CountryDataRow() { Code = "SE", Name = "Sweden" });
            initialData.Add(new CountryDataRow() { Code = "CH", Name = "Switzerland" });
            initialData.Add(new CountryDataRow() { Code = "SY", Name = "Syria" });
            initialData.Add(new CountryDataRow() { Code = "TW", Name = "Taiwan" });
            initialData.Add(new CountryDataRow() { Code = "TJ", Name = "Tajikistan" });
            initialData.Add(new CountryDataRow() { Code = "TZ", Name = "Tanzania" });
            initialData.Add(new CountryDataRow() { Code = "TH", Name = "Thailand" });
            initialData.Add(new CountryDataRow() { Code = "TG", Name = "Togo" });
            initialData.Add(new CountryDataRow() { Code = "TK", Name = "Tokelau" });
            initialData.Add(new CountryDataRow() { Code = "TO", Name = "Tonga" });
            initialData.Add(new CountryDataRow() { Code = "TT", Name = "Trinidad and Tobago" });
            initialData.Add(new CountryDataRow() { Code = "TN", Name = "Tunisia" });
            initialData.Add(new CountryDataRow() { Code = "TR", Name = "Turkey" });
            initialData.Add(new CountryDataRow() { Code = "TM", Name = "Turkmenistan" });
            initialData.Add(new CountryDataRow() { Code = "TC", Name = "Turks and Caicos Islands" });
            initialData.Add(new CountryDataRow() { Code = "TV", Name = "Tuvalu" });
            initialData.Add(new CountryDataRow() { Code = "UG", Name = "Uganda" });
            initialData.Add(new CountryDataRow() { Code = "UA", Name = "Ukraine" });
            initialData.Add(new CountryDataRow() { Code = "AE", Name = "United Arab Emirates" });
            initialData.Add(new CountryDataRow() { Code = "UM", Name = "United States Minor Outlying Islands" });
            initialData.Add(new CountryDataRow() { Code = "UY", Name = "Uruguay" });
            initialData.Add(new CountryDataRow() { Code = "UZ", Name = "Uzbekistan" });
            initialData.Add(new CountryDataRow() { Code = "VU", Name = "Vanuatu" });
            initialData.Add(new CountryDataRow() { Code = "VE", Name = "Venezuela" });
            initialData.Add(new CountryDataRow() { Code = "VN", Name = "Viet Nam" });
            initialData.Add(new CountryDataRow() { Code = "VG", Name = "Virgin Islands (British)" });
            initialData.Add(new CountryDataRow() { Code = "VI", Name = "Virgin Islands (U.S.)" });
            initialData.Add(new CountryDataRow() { Code = "WF", Name = "Wallis and Futuna Islands" });
            initialData.Add(new CountryDataRow() { Code = "EH", Name = "Western Sahara" });
            initialData.Add(new CountryDataRow() { Code = "YE", Name = "Yemen" });
            initialData.Add(new CountryDataRow() { Code = "YU", Name = "Yugoslavia" });
            initialData.Add(new CountryDataRow() { Code = "ZR", Name = "Zaire" });
            initialData.Add(new CountryDataRow() { Code = "ZM", Name = "Zambia" });
            initialData.Add(new CountryDataRow() { Code = "ZW", Name = "Zimbabwe" });
            initialData.Add(new CountryDataRow() { Code = "AX", Name = "Aland Islands" });
            initialData.Add(new CountryDataRow() { Code = "EU", Name = "European Union" });
            initialData.Add(new CountryDataRow() { Code = "GG", Name = "Guernsey" });
            initialData.Add(new CountryDataRow() { Code = "IM", Name = "Isle of Man" });
            initialData.Add(new CountryDataRow() { Code = "JE", Name = "Jersey" });
            initialData.Add(new CountryDataRow() { Code = "ME", Name = "Montenegro" });
            initialData.Add(new CountryDataRow() { Code = "MF", Name = "Saint Martin" });
            initialData.Add(new CountryDataRow() { Code = "PS", Name = "Palestinian Territory Occupied" });
            initialData.Add(new CountryDataRow() { Code = "RS", Name = "Serbia" });
            initialData.Add(new CountryDataRow() { Code = "TL", Name = "Timor-leste" });
            initialData.Add(new CountryDataRow() { Code = "CW", Name = "Curaçao" });


            return initialData;
        }
    }
}
