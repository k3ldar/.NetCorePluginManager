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
 *  Product:  Login Plugin
 *  
 *  File: LoginController.cs
 *
 *  Purpose:  Google Controler login options
 *
 *  Date        Name                Reason
 *  05/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;

using LoginPlugin.Classes;

using Microsoft.AspNetCore.Mvc;

using Middleware;

using Shared.Classes;
using Shared.Communication;

using static Shared.Utilities;

#pragma warning disable IDE0060, S1075, S6967

namespace LoginPlugin.Controllers
{
	public partial class LoginController
	{
		private const string Google = "Google";
		private const string GoogleCallBackUrl = "{0}://{1}/Login/GoogleCallback";
		private const string GoogleTokenUrl = "https://www.googleapis.com/oauth2/v1/userinfo?access_token={0}";
		private const string GoogleOAuthTokenUrl = "https://accounts.google.com/o/oauth2/token";
		private const string GoogleFailedToRetrieveLoginDetails = "unable to get Google login details";
		private const string GoogleScope = "&scope=https://www.googleapis.com/auth/userinfo.email%20https://www.googleapis.com/auth/userinfo.profile";
		private const string GoogleOAuthUri = "https://accounts.google.com/o/oauth2/auth";

		/// <summary>
		/// Google login action
		/// </summary>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult GoogleLogin(string returnUrl)
		{
			UserSession userSession = GetUserSession();
			string externalLoginCacheId = String.Format(ExternalLoginCacheItem, userSession.SessionID);
			_memoryCache.GetShortCache().Add(externalLoginCacheId, new CacheItem(externalLoginCacheId, returnUrl));

			string Googleurl = GoogleOAuthUri +
				OAuthParamResponseTypeCode +
				OAuthParamRedirectUri + String.Format(GoogleCallBackUrl, HttpContext.Request.Scheme, HttpContext.Request.Host) +
				GoogleScope +
				OAuthParamClientId + _settings.GoogleClientId;

			return Redirect(Googleurl);
		}

		/// <summary>
		/// Google login callback action
		/// </summary>
		/// <param name="code"></param>
		/// <param name="scope"></param>
		/// <param name="authuser"></param>
		/// <param name="prompt"></param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public IActionResult GoogleCallback(string code, string scope, string authuser, string prompt)
		{
			NVPCodec parameters = new()
			{
				{ OAuthCode, code },
				{ OAuthClientId, _settings.GoogleClientId },
				{ OAuthClientSecret, _settings.GoogleSecret },
				{ OAuthRedirectUri, String.Format(GoogleCallBackUrl, HttpContext.Request.Scheme, HttpContext.Request.Host) },
				{ OAuthGrantType, OAuthAuthCode }
			};
			string response = HttpPost.Post(GoogleOAuthTokenUrl, parameters);

			TokenResponse googlePlusAccessToken = JsonSerializer.Deserialize<TokenResponse>(response);

			TokenUserDetails userDetails = GetGoogleUserDetails(googlePlusAccessToken);

			if (googlePlusAccessToken == null)
				throw new InvalidOperationException(GoogleFailedToRetrieveLoginDetails);

			userDetails.Provider = Google;
			UserSession userSession = GetUserSession();

			if (userSession == null)
				return RedirectToAction(nameof(Index));

			UserLoginDetails loginDetails = null;
			LoginResult loginResult = _loginProvider.Login(userDetails, ref loginDetails);

			if (loginResult == LoginResult.Success)
			{
				userSession.Login(loginDetails.UserId, loginDetails.Username, loginDetails.Email);

				CookieAdd(_settings.RememberMeCookieName,
					Encrypt(loginDetails.UserId.ToString(), _settings.EncryptionKey),
					_settings.LoginDays, true);

				GetAuthenticationService().SignInAsync(HttpContext,
					_settings.AuthenticationScheme,
					new ClaimsPrincipal(_claimsProvider.GetUserClaims(loginDetails.UserId)),
					_claimsProvider.GetAuthenticationProperties());

				string externalLoginCacheId = String.Format(ExternalLoginCacheItem, userSession.SessionID);

				CacheItem redirectCache = _memoryCache.GetShortCache().Get(externalLoginCacheId);
				return LocalRedirect(redirectCache == null ? SharedPluginFeatures.Constants.ForwardSlash : (string)redirectCache.Value);
			}

			return RedirectToAction(nameof(Index));
		}

		private static TokenUserDetails GetGoogleUserDetails(TokenResponse googlePlusAccessToken)
		{
			using (HttpClient httpClient = new())
			{
				string profileUrl = String.Format(GoogleTokenUrl, googlePlusAccessToken.access_token);

				HttpResponseMessage output = httpClient.GetAsync(profileUrl).Result;

				if (output.IsSuccessStatusCode)
				{
					string responseData = output.Content.ReadAsStringAsync().Result;
					return JsonSerializer.Deserialize<TokenUserDetails>(responseData);
				}
			}

			return null;
		}
	}
}

#pragma warning disable IDE0060, S1075, S6967