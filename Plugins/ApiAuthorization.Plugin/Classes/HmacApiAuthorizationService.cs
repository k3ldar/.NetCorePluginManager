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
 *  File: ApiAuthorizationService.cs
 *
 *  Purpose:  IApiAuthorizationService implementation using HMAC security
 *
 *  Date        Name                Reason
 *  18/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Linq;
using System.Text;

using Microsoft.AspNetCore.Http;

using Middleware;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace ApiAuthorization.Plugin.Classes
{
    public sealed class HmacApiAuthorizationService : IApiAuthorizationService
    {
        #region Private Members

        private const string AuthCode = "authcode";
        private const string ApiKey = "apikey";
        private const string MerchantId = "merchantId";
        private const string Nonce = "nonce";
        private const string TimeStamp = "timestamp";
		private const string PayloadLength = "payloadLength";

        private static readonly Timings _timings = new Timings();

        private readonly IUserApiQueryProvider _apiQueryProvider;
        private readonly TimeSpan _maximumRequestAge;

        #endregion Private Members

        #region Constructors

        public HmacApiAuthorizationService(IUserApiQueryProvider apiQueryProvider)
            : this(apiQueryProvider, new TimeSpan(0, 0, 0, 180, 0))
        {

        }

        public HmacApiAuthorizationService(IUserApiQueryProvider apiQueryProvider, TimeSpan maximumRequestAge)
        {
            _apiQueryProvider = apiQueryProvider ?? throw new ArgumentNullException(nameof(apiQueryProvider));
            _maximumRequestAge = maximumRequestAge;
        }

        #endregion Constructors

        #region IApiAuthorizationService Methods

        public bool ValidateApiRequest(HttpRequest httpRequest, string policyName, out int responseCode)
        {
            using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
            {
                ApiUserDetails apiDetails = ValidateHeaders(httpRequest);

                if (apiDetails == null || !apiDetails.WithinTimeParameters(_maximumRequestAge))
                {
                    responseCode = SharedPluginFeatures.Constants.HtmlResponseBadRequest;
                    return false;
                }

                if (!_apiQueryProvider.ApiSecret(apiDetails.MerchantId, apiDetails.ApiKey, out string userSecret))
                {
                    responseCode = SharedPluginFeatures.Constants.HtmlResponseUnauthorized;
                    return false;
                }

                string payload = String.Empty;

                if (apiDetails.PayloadLength > 0 && httpRequest.Body.CanSeek && httpRequest.ContentLength > 0)
                {
                    byte[] requestBytes = new byte[httpRequest.Body.Length];
                    httpRequest.Body.Position = 0;
                    httpRequest.Body.Read(requestBytes, 0, (int)httpRequest.Body.Length);
                    payload = Encoding.ASCII.GetString(requestBytes);
                    httpRequest.Body.Position = 0;
                }

                string hmacValidation = HmacGenerator.GenerateHmac(apiDetails.ApiKey, userSecret,
                    apiDetails.EpochTimeStamp.Ticks - DateTime.UnixEpoch.Ticks,
                    apiDetails.Nonce, apiDetails.MerchantId, payload);

                if (hmacValidation.Equals(apiDetails.Authorization))
                {
                    responseCode = SharedPluginFeatures.Constants.HtmlResponseSuccess;
                    return true;
                }

                responseCode = SharedPluginFeatures.Constants.HtmlResponseUnauthorized;
                return false;
            }
        }

        #endregion IApiAuthorizationService Methods

        #region Properties

        internal static Timings GetTimings => _timings.Clone();

        #endregion Properties

        #region Private Methods

        private static ApiUserDetails ValidateHeaders(HttpRequest request)
        {
            if (!request.Headers.ContainsKey(ApiKey) ||
                !request.Headers.ContainsKey(MerchantId) ||
                !request.Headers.ContainsKey(Nonce) ||
                !request.Headers.ContainsKey(TimeStamp) ||
                !request.Headers.ContainsKey(AuthCode))
            {
                return null;
            }

            string apiKey = request.Headers[ApiKey];

            if (String.IsNullOrEmpty(apiKey))
                return null;

            string merchantId = request.Headers[MerchantId];

            if (String.IsNullOrEmpty(merchantId))
                return null;

            string nonce = request.Headers[Nonce];

            if (String.IsNullOrEmpty(nonce) || !UInt64.TryParse(nonce, out ulong numericNonce))
                return null;

            string timestamp = request.Headers[TimeStamp];

            if (String.IsNullOrEmpty(timestamp) || !Int64.TryParse(timestamp, out long numericTimestamp))
                return null;

            string authorization = request.Headers[AuthCode];

            if (String.IsNullOrEmpty(authorization))
                return null;

			int payloadLength = 0;

			if (request.Headers.Keys.Contains(PayloadLength))
			{
				Int32.TryParse(request.Headers[PayloadLength], out payloadLength);
			}

            return new ApiUserDetails(apiKey, merchantId, authorization, numericNonce, numericTimestamp, payloadLength);
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591