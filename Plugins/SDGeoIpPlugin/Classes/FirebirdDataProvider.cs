﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  SieraDeltaGeoIpPlugin
 *  
 *  File: FirebirdDataProvider.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  04/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Text;

using FirebirdSql.Data.FirebirdClient;

using Shared.Classes;

using SharedPluginFeatures;

namespace SieraDeltaGeoIp.Plugin
{
    /// <summary>
    /// Background thread used to load Geo Ip Data from Firebird Database
    /// </summary>
    internal class FirebirdDataProvider : ThreadManager, IGeoIpProvider
    {
        #region Properties

        private readonly GeoIpPluginSettings _settings;

        #endregion Propertes

        #region Constructors

        public FirebirdDataProvider(GeoIpPluginSettings settings, List<IpCity> ipRangeData)
            : base(ipRangeData, new TimeSpan(24, 0, 0))
        {
            base.ContinueIfGlobalException = true;
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        #endregion Constructors

        #region Overridden Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "take a chill pill man, it's ok...")]
        protected override bool Run(object parameters)
        {
            List<IpCity> rangeData = (List<IpCity>)parameters;
            rangeData.Clear();

            FbConnection db = new(_settings.DatabaseConnectionString);
			try
			{
				db.Open();
				string SQL = "SELECT ipc.WD$ID, COALESCE(c.WD$COUNTRY_CODE, 'Unknown'), COALESCE(ipc.WD$REGION, ''), " +
					"COALESCE(ipc.WD$CITY, ''), COALESCE(ipc.WD$LATITUDE, 0.0), COALESCE(ipc.WD$LONGITUDE, 0.0), " +
					"COALESCE(c.WD$FROM_IP, 0), COALESCE(c.WD$TO_IP, 0) FROM WD$IPTOCOUNTRY c " +
					"LEFT JOIN WD$IPCITY ipc ON (ipc.WD$ID = c.WD$CITY_ID) ";

				StringBuilder whereClause = new();

				foreach (string countryCode in _settings.CountryList)
				{
					if (String.IsNullOrEmpty(countryCode))
						continue;

					if (whereClause.Length > 0)
						whereClause.Append(", ");

					whereClause.Append($"'{countryCode}'");
				}

				if (whereClause.Length > 0)
					SQL += $"WHERE c.WD$COUNTRY_CODE IN ({whereClause})";

				FbTransaction tran = db.BeginTransaction();
				try
				{
					FbCommand cmd = new(SQL, db, tran);
					try
					{
						FbDataReader rdr = cmd.ExecuteReader();
						try
						{
							while (rdr.Read())
							{
								rangeData.Add(new IpCity(rdr.GetInt64(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3),
									rdr.GetDecimal(4), rdr.GetDecimal(5), rdr.GetInt64(6), rdr.GetInt64(7)));
							}
						}
						finally
						{
							rdr.Close();
						}
					}
					finally
					{
						cmd.Dispose();
					}
				}
				finally
				{
					tran.Rollback();
					tran.Dispose();
				}
			}
			finally
			{
				db.Dispose();
			}

            rangeData.Sort();

            return false;
        }

        #endregion Overridden Methods

        #region IGeoIpProvider Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "take a chill pill man, it's ok...")]
        public bool GetIpAddressDetails(in string ipAddress, out string countryCode, out string region,
            out string cityName, out decimal latitude, out decimal longitude, out long uniqueId,
            out long ipFrom, out long ipTo)
        {
            countryCode = "ZZ";
            region = String.Empty;
            cityName = String.Empty;
            latitude = 0;
            longitude = 0;
            uniqueId = -1;
            ipFrom = 0;
            ipTo = 0;

            FbConnection db = new(_settings.DatabaseConnectionString);
            try
            {
                db.Open();
                string SQL = "SELECT p.OPCOUNTRY, p.OPCITY, p.OPREGION, COALESCE(p.OPLONGITUDE, 0), COALESCE(p.OPLATITUDE, 0), " +
                    "p.OPID, p.OPSTARTBLOCK, p.OPENDBLOCK " +
                    $"FROM WD$GEO_DECODE_IP('{ipAddress}') p ";

                FbTransaction tran = db.BeginTransaction();
                try
                {
                    FbCommand cmd = new(SQL, db, tran);
                    try
                    {
                        FbDataReader rdr = cmd.ExecuteReader();
                        try
                        {
                            if (rdr.Read())
                            {
                                countryCode = rdr.GetString(0);
                                region = rdr.GetString(2);
                                cityName = rdr.GetString(1);
                                latitude = rdr.GetDecimal(4);
                                longitude = rdr.GetDecimal(3);
                                uniqueId = rdr.GetInt64(5);
                                ipFrom = rdr.GetInt64(6);
                                ipTo = rdr.GetInt64(7);

                                return true;
                            }
                        }
                        finally
                        {
                            rdr.Close();
                        }
                    }
                    finally
                    {
                        cmd.Dispose();
                    }
                }
                finally
                {
                    tran.Rollback();
                    tran.Dispose();
                }
            }
            finally
            {
                db.Close();
                db.Dispose();
            }

            return false;
        }

        #endregion IGeoIpProvider Methods
    }
}
