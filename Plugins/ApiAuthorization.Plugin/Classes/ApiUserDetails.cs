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
 *  Product:  ApiAuthorization.Plugin
 *  
 *  File: ApiUserDetails.cs
 *
 *  Purpose:  Contains user api details received with request
 *
 *  Date        Name                Reason
 *  18/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace ApiAuthorization.Plugin.Classes
{
    internal sealed class ApiUserDetails
    {
        public ApiUserDetails(string apiKey, string merchantId, string authorization, ulong nonce, long epoch, int payloadLength)
        {
            if (String.IsNullOrEmpty(apiKey))
                throw new ArgumentNullException(nameof(apiKey));

            if (String.IsNullOrEmpty(merchantId))
                throw new ArgumentNullException(nameof(merchantId));

            if (String.IsNullOrEmpty(authorization))
                throw new ArgumentNullException(nameof(authorization));

            if (nonce < 1)
                throw new ArgumentOutOfRangeException(nameof(nonce));

            EpochTimeStamp = new DateTime(epoch + DateTime.UnixEpoch.Ticks, DateTimeKind.Utc);
            Nonce = nonce;
            ApiKey = apiKey;
            MerchantId = merchantId;
            Authorization = authorization;
            Created = DateTime.UtcNow;
			PayloadLength = payloadLength;
        }

        public string ApiKey { get; }

        public string MerchantId { get; }

        public ulong Nonce { get; }

        public DateTime EpochTimeStamp { get; }

        public DateTime Created { get; }

        public string Authorization { get; }

		public int PayloadLength { get; }

        public bool WithinTimeParameters(TimeSpan span)
        {
            TimeSpan range = Created - EpochTimeStamp;

            return range.TotalMilliseconds <= span.TotalMilliseconds;
        }
    }
}
