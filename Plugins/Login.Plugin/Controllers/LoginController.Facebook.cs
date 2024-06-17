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
using System.Text;
using System.Text.Json;

using LoginPlugin.Classes;

using Microsoft.AspNetCore.Mvc;

using Middleware;

using Shared.Classes;
using Shared.Communication;

using static Shared.Utilities;

#pragma warning disable CS1591, S1075

namespace LoginPlugin.Controllers
{
	public partial class LoginController
	{
		private const string Facebook = "Facebook";
		private const string FacebookCallBackUrl = "{0}://{1}/Login/FacebookCallback";
		private const string FacebookFailedToRetrieveLoginDetails = "unable to get FB login details";
		private const string OAuthParamAccessToken = "?access_token=";
		private const string FacebookScope = "&scope=public_profile,email";
		private const string FacebookOAuthUri = "https://www.facebook.com/v12.0/dialog/oauth";
		private const string FacebookOAuthAccessTokenUri = "https://graph.facebook.com/v12.0/oauth/access_token";
		private const string FacebookGraphMeUri = "https://graph.facebook.com/v12.0/me";
		private const string FacebookGraphUri = "https://graph.facebook.com/v12.0/";
		private const string FacebookProfileUri = "?fields=id,name,email&access_token=";
		private const string FacebookUserConfirmationUri = "{0}://{1}/Login/FacebookUser/{2}";
		private const string FacebookOAuthParamRedirectUri = "?redirect_uri=";

		[HttpPost]
		public IActionResult FacebookLogin(string returnUrl)
		{
			UserSession userSession = GetUserSession();
			string externalLoginCacheId = String.Format(ExternalLoginCacheItem, userSession.SessionID);
			_memoryCache.GetShortCache().Add(externalLoginCacheId, new CacheItem(externalLoginCacheId, returnUrl));

			string Facebookurl = FacebookOAuthUri +
				FacebookOAuthParamRedirectUri + String.Format(FacebookCallBackUrl, HttpContext.Request.Scheme, HttpContext.Request.Host) +
				FacebookScope +
				OAuthParamClientId + _settings.FacebookClientId;

			return Redirect(Facebookurl);
		}

		public IActionResult FacebookCallback(string code)
		{
			NVPCodec parameters = new();
			parameters.Add(OAuthCode, code);
			parameters.Add(OAuthClientId, _settings.FacebookClientId);
			parameters.Add(OAuthClientSecret, _settings.FacebookSecret);
			parameters.Add(OAuthRedirectUri, String.Format(FacebookCallBackUrl, HttpContext.Request.Scheme, HttpContext.Request.Host));
			parameters.Add(OAuthGrantType, OAuthAuthCode);
			string response = HttpPost.Post(FacebookOAuthAccessTokenUri, parameters);

			TokenResponse facebookAccessToken = JsonSerializer.Deserialize<TokenResponse>(response, GetSerializerOptions());

			TokenUserDetails userDetails = GetFacebookUserDetails(facebookAccessToken) ?? throw new InvalidOperationException(FacebookFailedToRetrieveLoginDetails);
			userDetails.Provider = Facebook;
			UserSession userSession = GetUserSession();

			if (userSession == null)
				return RedirectToAction(nameof(Index));

			UserLoginDetails loginDetails = null;
			LoginResult loginResult = _loginProvider.Login(userDetails, ref loginDetails);

			if (loginResult == LoginResult.Success)
			{
				userSession.Login(loginDetails.UserId, loginDetails.Username, loginDetails.Email);

				CookieAdd(_settings.RememberMeCookieName, Encrypt(loginDetails.UserId.ToString(),
					_settings.EncryptionKey), _settings.LoginDays, true);

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

		[HttpPost]
		public IActionResult FacebookRemoveUser(string signed_request)
		{
			if (!String.IsNullOrEmpty(signed_request))
			{
				string[] requestParts = signed_request.Split('.');

				string signatureRaw = Base64Decode(requestParts[0]);
				string dataRaw = Base64Decode(requestParts[1]);

				// the decoded signature
				byte[] signature = Convert.FromBase64String(signatureRaw);
				byte[] dataBuffer = Convert.FromBase64String(dataRaw);

				// JSON object
				string json = Encoding.UTF8.GetString(dataBuffer);

				byte[] appSecretBytes = Encoding.UTF8.GetBytes("SecretKey");
				System.Security.Cryptography.HMAC hmac = new System.Security.Cryptography.HMACSHA256(appSecretBytes);
				byte[] expectedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(requestParts[1]));

				if (!BytesEqual(expectedHash, signature))
				{
					throw new InvalidOperationException("Invalid signature");
				}

				FacebookRemoveUser fbUser = JsonSerializer.Deserialize<FacebookRemoveUser>(json, GetSerializerOptions());
				TokenUserDetails tokenUserDetails = new(fbUser);
				_loginProvider.RemoveExternalUser(tokenUserDetails);
				string confirmationUrl = String.Format(FacebookUserConfirmationUri, HttpContext.Request.Scheme, HttpContext.Request.Host, fbUser.UserId);

				return Ok(new { url = confirmationUrl, confirmation_code = fbUser.UserId });
			}


			return null;
		}

		public IActionResult FacebookUser(string id)
		{
			TokenUserDetails tokenUserDetails = new()
			{
				Id = id,
				Provider = Facebook,
				Verify = true,
				Email = id,
				Name = id
			};

			UserLoginDetails loginDetails = null;
			LoginResult loginResult = _loginProvider.Login(tokenUserDetails, ref loginDetails);

			if (loginResult == LoginResult.InvalidCredentials)
				return Content("Not found");

			return Content("Valid User");
		}

		private static string Base64Decode(string data)
		{
			byte[] convertedData = Convert.FromBase64String(data);
			return Encoding.UTF8.GetString(convertedData);
		}

		private static bool BytesEqual(byte[] value1, byte[] value2)
		{
			if (value1.Length != value2.Length)
				return false;

			for (int i = 0; i < value1.Length; i++)
			{
				if (value1[i] != value2[i])
					return false;
			}

			return true;
		}

		private TokenUserDetails GetFacebookUserDetails(TokenResponse facebookPlusAccessToken)
		{
			using (HttpClient httpClient = new())
			{
				string userIdUrl = FacebookGraphMeUri +
					OAuthParamAccessToken + facebookPlusAccessToken.access_token;

				HttpResponseMessage output = httpClient.GetAsync(userIdUrl).Result;

				if (output.IsSuccessStatusCode)
				{
					TokenUserDetails idData = JsonSerializer.Deserialize<TokenUserDetails>(output.Content.ReadAsStringAsync().Result, GetSerializerOptions());

					httpClient.CancelPendingRequests();

					string profileUrl = FacebookGraphUri + idData.Id + FacebookProfileUri + facebookPlusAccessToken.access_token;

					output = httpClient.GetAsync(profileUrl).Result;

					if (output.IsSuccessStatusCode)
					{
						return JsonSerializer.Deserialize<TokenUserDetails>(output.Content.ReadAsStringAsync().Result, GetSerializerOptions());
					}

				}
			}

			return null;
		}
	}
}

#pragma warning restore CS1591, S1075