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
 *  File: TableCountryDefaults.cs
 *
 *  Purpose:  Default initialisation values for country table
 *
 *  Date        Name                Reason
 *  18/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using PluginManager.DAL.TextFiles.Interfaces;

namespace PluginManager.DAL.TextFiles.Tables
{
    public class TableCountryDefaults : ITableDefaults<TableCountry>
    {
        public long PrimarySequence => 0;

        public long SecondarySequence => 0;

        public List<TableCountry> InitialData
        {
            get
            {
                List<TableCountry> initialData = new List<TableCountry>();

                initialData.Add(new TableCountry() { Code = "ZZ", Name = "Unknown Country", Visible = false });
                initialData.Add(new TableCountry() { Code = "GB", Name = "United Kingdom" });
                initialData.Add(new TableCountry() { Code = "US", Name = "United States of America" });
                initialData.Add(new TableCountry() { Code = "AF", Name = "Afghanistan" });
                initialData.Add(new TableCountry() { Code = "AL", Name = "Albania" });
                initialData.Add(new TableCountry() { Code = "DZ", Name = "Algeria" });
                initialData.Add(new TableCountry() { Code = "AS", Name = "American Samoa" });
                initialData.Add(new TableCountry() { Code = "AD", Name = "Andorra" });
                initialData.Add(new TableCountry() { Code = "AO", Name = "Angola" });
                initialData.Add(new TableCountry() { Code = "AI", Name = "Anguilla" });
                initialData.Add(new TableCountry() { Code = "AQ", Name = "Antarctica" });
                initialData.Add(new TableCountry() { Code = "AG", Name = "Antigua and Barbuda" });
                initialData.Add(new TableCountry() { Code = "AR", Name = "Argentina" });
                initialData.Add(new TableCountry() { Code = "AM", Name = "Armenia" });
                initialData.Add(new TableCountry() { Code = "AW", Name = "Aruba" });
                initialData.Add(new TableCountry() { Code = "AU", Name = "Australia" });
                initialData.Add(new TableCountry() { Code = "AT", Name = "Austria" });
                initialData.Add(new TableCountry() { Code = "AZ", Name = "Azerbaijan" });
                initialData.Add(new TableCountry() { Code = "BS", Name = "Bahamas" });
                initialData.Add(new TableCountry() { Code = "BH", Name = "Bahrain" });
                initialData.Add(new TableCountry() { Code = "BD", Name = "Bangladesh" });
                initialData.Add(new TableCountry() { Code = "BB", Name = "Barbados" });
                initialData.Add(new TableCountry() { Code = "BY", Name = "Belarus" });
                initialData.Add(new TableCountry() { Code = "BE", Name = "Belgium" });
                initialData.Add(new TableCountry() { Code = "BZ", Name = "Belize" });
                initialData.Add(new TableCountry() { Code = "BJ", Name = "Benin" });
                initialData.Add(new TableCountry() { Code = "BM", Name = "Bermuda" });
                initialData.Add(new TableCountry() { Code = "BT", Name = "Bhutan" });
                initialData.Add(new TableCountry() { Code = "BO", Name = "Bolivia" });
                initialData.Add(new TableCountry() { Code = "BA", Name = "Bosnia and Herzegovina" });
                initialData.Add(new TableCountry() { Code = "BW", Name = "Botswana" });
                initialData.Add(new TableCountry() { Code = "BV", Name = "Bouvet Island" });
                initialData.Add(new TableCountry() { Code = "BR", Name = "Brazil" });
                initialData.Add(new TableCountry() { Code = "IO", Name = "British Indian Ocean Territory" });
                initialData.Add(new TableCountry() { Code = "BN", Name = "Brunei Darussalam" });
                initialData.Add(new TableCountry() { Code = "BG", Name = "Bulgaria" });
                initialData.Add(new TableCountry() { Code = "BF", Name = "Burkina Faso" });
                initialData.Add(new TableCountry() { Code = "BI", Name = "Burundi" });
                initialData.Add(new TableCountry() { Code = "KH", Name = "Cambodia" });
                initialData.Add(new TableCountry() { Code = "CM", Name = "Cameroon" });
                initialData.Add(new TableCountry() { Code = "CA", Name = "Canada" });
                initialData.Add(new TableCountry() { Code = "CV", Name = "Cape Verde" });
                initialData.Add(new TableCountry() { Code = "KY", Name = "Cayman Islands" });
                initialData.Add(new TableCountry() { Code = "CF", Name = "Central African Republic" });
                initialData.Add(new TableCountry() { Code = "TD", Name = "Chad" });
                initialData.Add(new TableCountry() { Code = "CL", Name = "Chile" });
                initialData.Add(new TableCountry() { Code = "CN", Name = "China" });
                initialData.Add(new TableCountry() { Code = "CX", Name = "Christmas Island" });
                initialData.Add(new TableCountry() { Code = "CC", Name = "Cocos (Keeling) Islands" });
                initialData.Add(new TableCountry() { Code = "CO", Name = "Colombia" });
                initialData.Add(new TableCountry() { Code = "KM", Name = "Comoros" });
                initialData.Add(new TableCountry() { Code = "CG", Name = "Congo" });
                initialData.Add(new TableCountry() { Code = "CK", Name = "Cook Islands" });
                initialData.Add(new TableCountry() { Code = "CR", Name = "Costa Rica" });
                initialData.Add(new TableCountry() { Code = "CI", Name = "Côte d'Ivoire" });
                initialData.Add(new TableCountry() { Code = "HR", Name = "Croatia" });
                initialData.Add(new TableCountry() { Code = "CU", Name = "Cuba" });
                initialData.Add(new TableCountry() { Code = "CY", Name = "Cyprus" });
                initialData.Add(new TableCountry() { Code = "CZ", Name = "Czech Republic" });
                initialData.Add(new TableCountry() { Code = "DK", Name = "Denmark" });
                initialData.Add(new TableCountry() { Code = "DJ", Name = "Djibouti" });
                initialData.Add(new TableCountry() { Code = "DM", Name = "Dominica" });
                initialData.Add(new TableCountry() { Code = "DO", Name = "Dominican Republic" });
                initialData.Add(new TableCountry() { Code = "TP", Name = "East Timor" });
                initialData.Add(new TableCountry() { Code = "EC", Name = "Ecuador" });
                initialData.Add(new TableCountry() { Code = "EG", Name = "Egypt" });
                initialData.Add(new TableCountry() { Code = "SV", Name = "El salvador" });
                initialData.Add(new TableCountry() { Code = "GQ", Name = "Equatorial Guinea" });
                initialData.Add(new TableCountry() { Code = "ER", Name = "Eritrea" });
                initialData.Add(new TableCountry() { Code = "EE", Name = "Estonia" });
                initialData.Add(new TableCountry() { Code = "ET", Name = "Ethiopia" });
                initialData.Add(new TableCountry() { Code = "FK", Name = "Falkland Islands" });
                initialData.Add(new TableCountry() { Code = "FO", Name = "Faroe Islands" });
                initialData.Add(new TableCountry() { Code = "FJ", Name = "Fiji" });
                initialData.Add(new TableCountry() { Code = "FI", Name = "Finland" });
                initialData.Add(new TableCountry() { Code = "FR", Name = "France" });
                initialData.Add(new TableCountry() { Code = "GF", Name = "French Guiana" });
                initialData.Add(new TableCountry() { Code = "PF", Name = "French Polynesia" });
                initialData.Add(new TableCountry() { Code = "TF", Name = "French Southern Territories" });
                initialData.Add(new TableCountry() { Code = "GA", Name = "Gabon" });
                initialData.Add(new TableCountry() { Code = "GM", Name = "Gambia" });
                initialData.Add(new TableCountry() { Code = "GE", Name = "Georgia" });
                initialData.Add(new TableCountry() { Code = "DE", Name = "Germany" });
                initialData.Add(new TableCountry() { Code = "GH", Name = "Ghana" });
                initialData.Add(new TableCountry() { Code = "GI", Name = "Gibraltar" });
                initialData.Add(new TableCountry() { Code = "GR", Name = "Greece" });
                initialData.Add(new TableCountry() { Code = "GL", Name = "Greenland" });
                initialData.Add(new TableCountry() { Code = "GD", Name = "Grenada" });
                initialData.Add(new TableCountry() { Code = "GP", Name = "Guadeloupe" });
                initialData.Add(new TableCountry() { Code = "GU", Name = "Guam" });
                initialData.Add(new TableCountry() { Code = "GT", Name = "Guatemala" });
                initialData.Add(new TableCountry() { Code = "GN", Name = "Guinea" });
                initialData.Add(new TableCountry() { Code = "GW", Name = "Guinea-Bissau" });
                initialData.Add(new TableCountry() { Code = "GY", Name = "Guyana" });
                initialData.Add(new TableCountry() { Code = "HT", Name = "Haiti" });
                initialData.Add(new TableCountry() { Code = "HM", Name = "Heard Island and McDonald Islands" });
                initialData.Add(new TableCountry() { Code = "VA", Name = "Holy See (Vatican City State)" });
                initialData.Add(new TableCountry() { Code = "HN", Name = "Honduras" });
                initialData.Add(new TableCountry() { Code = "HK", Name = "Hong Kong" });
                initialData.Add(new TableCountry() { Code = "HU", Name = "Hungary" });
                initialData.Add(new TableCountry() { Code = "IS", Name = "Iceland" });
                initialData.Add(new TableCountry() { Code = "IN", Name = "India" });
                initialData.Add(new TableCountry() { Code = "ID", Name = "Indonesia" });
                initialData.Add(new TableCountry() { Code = "IR", Name = "Iran" });
                initialData.Add(new TableCountry() { Code = "IQ", Name = "Iraq" });
                initialData.Add(new TableCountry() { Code = "IE", Name = "Ireland" });
                initialData.Add(new TableCountry() { Code = "IL", Name = "Israel" });
                initialData.Add(new TableCountry() { Code = "IT", Name = "Italy" });
                initialData.Add(new TableCountry() { Code = "JM", Name = "Jamaica" });
                initialData.Add(new TableCountry() { Code = "JP", Name = "Japan" });
                initialData.Add(new TableCountry() { Code = "JO", Name = "Jordan" });
                initialData.Add(new TableCountry() { Code = "KZ", Name = "Kazakstan" });
                initialData.Add(new TableCountry() { Code = "KE", Name = "Kenya" });
                initialData.Add(new TableCountry() { Code = "KI", Name = "Kiribati" });
                initialData.Add(new TableCountry() { Code = "KW", Name = "Kuwait" });
                initialData.Add(new TableCountry() { Code = "KG", Name = "Kyrgystan" });
                initialData.Add(new TableCountry() { Code = "LA", Name = "Lao" });
                initialData.Add(new TableCountry() { Code = "LV", Name = "Latvia" });
                initialData.Add(new TableCountry() { Code = "LB", Name = "Lebanon" });
                initialData.Add(new TableCountry() { Code = "LS", Name = "Lesotho" });
                initialData.Add(new TableCountry() { Code = "LR", Name = "Liberia" });
                initialData.Add(new TableCountry() { Code = "LY", Name = "Libyan Arab Jamahiriya" });
                initialData.Add(new TableCountry() { Code = "LI", Name = "Liechtenstein" });
                initialData.Add(new TableCountry() { Code = "LT", Name = "Lithuania" });
                initialData.Add(new TableCountry() { Code = "LU", Name = "Luxembourg" });
                initialData.Add(new TableCountry() { Code = "MO", Name = "Macau" });
                initialData.Add(new TableCountry() { Code = "MK", Name = "Macedonia (FYR)" });
                initialData.Add(new TableCountry() { Code = "MG", Name = "Madagascar" });
                initialData.Add(new TableCountry() { Code = "MW", Name = "Malawi" });
                initialData.Add(new TableCountry() { Code = "MY", Name = "Malaysia" });
                initialData.Add(new TableCountry() { Code = "MV", Name = "Maldives" });
                initialData.Add(new TableCountry() { Code = "ML", Name = "Mali" });
                initialData.Add(new TableCountry() { Code = "MT", Name = "Malta" });
                initialData.Add(new TableCountry() { Code = "MH", Name = "Marshall Islands" });
                initialData.Add(new TableCountry() { Code = "MQ", Name = "Martinique" });
                initialData.Add(new TableCountry() { Code = "MR", Name = "Mauritania" });
                initialData.Add(new TableCountry() { Code = "MU", Name = "Mauritius" });
                initialData.Add(new TableCountry() { Code = "YT", Name = "Mayotte" });
                initialData.Add(new TableCountry() { Code = "MX", Name = "Mexico" });
                initialData.Add(new TableCountry() { Code = "FM", Name = "Micronesia" });
                initialData.Add(new TableCountry() { Code = "MD", Name = "Moldova" });
                initialData.Add(new TableCountry() { Code = "MC", Name = "Monaco" });
                initialData.Add(new TableCountry() { Code = "MN", Name = "Mongolia" });
                initialData.Add(new TableCountry() { Code = "MS", Name = "Montserrat" });
                initialData.Add(new TableCountry() { Code = "MA", Name = "Morocco" });
                initialData.Add(new TableCountry() { Code = "MZ", Name = "Mozambique" });
                initialData.Add(new TableCountry() { Code = "MM", Name = "Myanmar" });
                initialData.Add(new TableCountry() { Code = "NA", Name = "Namibia" });
                initialData.Add(new TableCountry() { Code = "NR", Name = "Nauru" });
                initialData.Add(new TableCountry() { Code = "NP", Name = "Nepal" });
                initialData.Add(new TableCountry() { Code = "NL", Name = "Netherlands" });
                initialData.Add(new TableCountry() { Code = "AN", Name = "Netherlands Antilles" });
                initialData.Add(new TableCountry() { Code = "NT", Name = "Neutral Zone" });
                initialData.Add(new TableCountry() { Code = "NC", Name = "New Caledonia" });
                initialData.Add(new TableCountry() { Code = "NZ", Name = "New Zealand" });
                initialData.Add(new TableCountry() { Code = "NI", Name = "Nicaragua" });
                initialData.Add(new TableCountry() { Code = "NE", Name = "Niger" });
                initialData.Add(new TableCountry() { Code = "NG", Name = "Nigeria" });
                initialData.Add(new TableCountry() { Code = "NU", Name = "Niue" });
                initialData.Add(new TableCountry() { Code = "NF", Name = "Norfolk Island" });
                initialData.Add(new TableCountry() { Code = "KP", Name = "North Korea" });
                initialData.Add(new TableCountry() { Code = "MP", Name = "Northern Mariana Islands" });
                initialData.Add(new TableCountry() { Code = "NO", Name = "Norway" });
                initialData.Add(new TableCountry() { Code = "OM", Name = "Oman" });
                initialData.Add(new TableCountry() { Code = "PK", Name = "Pakistan" });
                initialData.Add(new TableCountry() { Code = "PW", Name = "Palau" });
                initialData.Add(new TableCountry() { Code = "PA", Name = "Panama" });
                initialData.Add(new TableCountry() { Code = "PG", Name = "Papua New Guinea" });
                initialData.Add(new TableCountry() { Code = "PY", Name = "Paraguay" });
                initialData.Add(new TableCountry() { Code = "PE", Name = "Peru" });
                initialData.Add(new TableCountry() { Code = "PH", Name = "Philippines" });
                initialData.Add(new TableCountry() { Code = "PN", Name = "Pitcairn" });
                initialData.Add(new TableCountry() { Code = "PL", Name = "Poland" });
                initialData.Add(new TableCountry() { Code = "PT", Name = "Portugal" });
                initialData.Add(new TableCountry() { Code = "PR", Name = "Puerto Rico" });
                initialData.Add(new TableCountry() { Code = "QA", Name = "Qatar" });
                initialData.Add(new TableCountry() { Code = "RE", Name = "Reunion" });
                initialData.Add(new TableCountry() { Code = "RO", Name = "Romania" });
                initialData.Add(new TableCountry() { Code = "RU", Name = "Russian Federation" });
                initialData.Add(new TableCountry() { Code = "RW", Name = "Rwanda" });
                initialData.Add(new TableCountry() { Code = "SH", Name = "Saint Helena" });
                initialData.Add(new TableCountry() { Code = "KN", Name = "Saint Kitts and Nevis" });
                initialData.Add(new TableCountry() { Code = "LC", Name = "Saint Lucia" });
                initialData.Add(new TableCountry() { Code = "PM", Name = "Saint Pierre and Miquelon" });
                initialData.Add(new TableCountry() { Code = "VC", Name = "Saint Vincent and the Grenadines" });
                initialData.Add(new TableCountry() { Code = "WS", Name = "Samoa" });
                initialData.Add(new TableCountry() { Code = "SM", Name = "San Marino" });
                initialData.Add(new TableCountry() { Code = "ST", Name = "Sao Tome and Principe" });
                initialData.Add(new TableCountry() { Code = "SA", Name = "Saudi Arabia" });
                initialData.Add(new TableCountry() { Code = "SN", Name = "Senegal" });
                initialData.Add(new TableCountry() { Code = "SC", Name = "Seychelles" });
                initialData.Add(new TableCountry() { Code = "SL", Name = "Sierra Leone" });
                initialData.Add(new TableCountry() { Code = "SG", Name = "Singapore" });
                initialData.Add(new TableCountry() { Code = "SK", Name = "Slovakia" });
                initialData.Add(new TableCountry() { Code = "SI", Name = "Slovenia" });
                initialData.Add(new TableCountry() { Code = "SB", Name = "Solomon Islands" });
                initialData.Add(new TableCountry() { Code = "SO", Name = "Somalia" });
                initialData.Add(new TableCountry() { Code = "ZA", Name = "South Africa" });
                initialData.Add(new TableCountry() { Code = "GS", Name = "South Georgia" });
                initialData.Add(new TableCountry() { Code = "KR", Name = "South Korea" });
                initialData.Add(new TableCountry() { Code = "ES", Name = "Spain" });
                initialData.Add(new TableCountry() { Code = "LK", Name = "Sri Lanka" });
                initialData.Add(new TableCountry() { Code = "SD", Name = "Sudan" });
                initialData.Add(new TableCountry() { Code = "SR", Name = "Suriname" });
                initialData.Add(new TableCountry() { Code = "SJ", Name = "Svalbard and Jan Mayen Islands" });
                initialData.Add(new TableCountry() { Code = "SZ", Name = "Swaziland" });
                initialData.Add(new TableCountry() { Code = "SE", Name = "Sweden" });
                initialData.Add(new TableCountry() { Code = "CH", Name = "Switzerland" });
                initialData.Add(new TableCountry() { Code = "SY", Name = "Syria" });
                initialData.Add(new TableCountry() { Code = "TW", Name = "Taiwan" });
                initialData.Add(new TableCountry() { Code = "TJ", Name = "Tajikistan" });
                initialData.Add(new TableCountry() { Code = "TZ", Name = "Tanzania" });
                initialData.Add(new TableCountry() { Code = "TH", Name = "Thailand" });
                initialData.Add(new TableCountry() { Code = "TG", Name = "Togo" });
                initialData.Add(new TableCountry() { Code = "TK", Name = "Tokelau" });
                initialData.Add(new TableCountry() { Code = "TO", Name = "Tonga" });
                initialData.Add(new TableCountry() { Code = "TT", Name = "Trinidad and Tobago" });
                initialData.Add(new TableCountry() { Code = "TN", Name = "Tunisia" });
                initialData.Add(new TableCountry() { Code = "TR", Name = "Turkey" });
                initialData.Add(new TableCountry() { Code = "TM", Name = "Turkmenistan" });
                initialData.Add(new TableCountry() { Code = "TC", Name = "Turks and Caicos Islands" });
                initialData.Add(new TableCountry() { Code = "TV", Name = "Tuvalu" });
                initialData.Add(new TableCountry() { Code = "UG", Name = "Uganda" });
                initialData.Add(new TableCountry() { Code = "UA", Name = "Ukraine" });
                initialData.Add(new TableCountry() { Code = "AE", Name = "United Arab Emirates" });
                initialData.Add(new TableCountry() { Code = "UM", Name = "United States Minor Outlying Islands" });
                initialData.Add(new TableCountry() { Code = "UY", Name = "Uruguay" });
                initialData.Add(new TableCountry() { Code = "UZ", Name = "Uzbekistan" });
                initialData.Add(new TableCountry() { Code = "VU", Name = "Vanuatu" });
                initialData.Add(new TableCountry() { Code = "VE", Name = "Venezuela" });
                initialData.Add(new TableCountry() { Code = "VN", Name = "Viet Nam" });
                initialData.Add(new TableCountry() { Code = "VG", Name = "Virgin Islands (British)" });
                initialData.Add(new TableCountry() { Code = "VI", Name = "Virgin Islands (U.S.)" });
                initialData.Add(new TableCountry() { Code = "WF", Name = "Wallis and Futuna Islands" });
                initialData.Add(new TableCountry() { Code = "EH", Name = "Western Sahara" });
                initialData.Add(new TableCountry() { Code = "YE", Name = "Yemen" });
                initialData.Add(new TableCountry() { Code = "YU", Name = "Yugoslavia" });
                initialData.Add(new TableCountry() { Code = "ZR", Name = "Zaire" });
                initialData.Add(new TableCountry() { Code = "ZM", Name = "Zambia" });
                initialData.Add(new TableCountry() { Code = "ZW", Name = "Zimbabwe" });
                initialData.Add(new TableCountry() { Code = "AX", Name = "Aland Islands" });
                initialData.Add(new TableCountry() { Code = "EU", Name = "European Union" });
                initialData.Add(new TableCountry() { Code = "GG", Name = "Guernsey" });
                initialData.Add(new TableCountry() { Code = "IM", Name = "Isle of Man" });
                initialData.Add(new TableCountry() { Code = "JE", Name = "Jersey" });
                initialData.Add(new TableCountry() { Code = "ME", Name = "Montenegro" });
                initialData.Add(new TableCountry() { Code = "MF", Name = "Saint Martin" });
                initialData.Add(new TableCountry() { Code = "PS", Name = "Palestinian Territory Occupied" });
                initialData.Add(new TableCountry() { Code = "RS", Name = "Serbia" });
                initialData.Add(new TableCountry() { Code = "TL", Name = "Timor-leste" });
                initialData.Add(new TableCountry() { Code = "CW", Name = "Curaçao" });


                return initialData;
            }
        }
    }
}
