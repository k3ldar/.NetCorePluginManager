/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Login Plugin
 *  
 *  File: LoginControllerSettings.cs
 *
 *  Purpose:  Login Controller Settings
 *
 *  Date        Name                Reason
 *  28/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using AppSettings;

using SharedPluginFeatures;

namespace LoginPlugin
{
	/// <summary>
	/// Settings that determine how the Login.Plugin module is configured and used.
	/// </summary>
	public sealed class LoginControllerSettings : IPluginSettings
	{
		/// <summary>
		/// Settings name
		/// </summary>
		public string SettingsName => nameof(LoginPlugin);

		/// <summary>
		/// Maximum number of attempts to login before the user is prevented from logging in for several minutes.
		/// </summary>
		/// <value>int</value>
		[SettingDefault(2)]
		public int CaptchaShowFailCount { get; set; }

		/// <summary>
		/// Length of the Captcha word.
		/// 
		/// Default: 7
		/// Minimum: 6
		/// Maximum: 12
		/// </summary>
		[SettingRange(6, 12)]
		[SettingDefault(7)]
		public int CaptchaWordLength { get; set; }

		/// <summary>
		/// The absolute or relative Uri where the user will be redirected to if the login was successful.
		/// </summary>
		/// <value>string</value>
		[SettingDefault("/")]
		public string LoginSuccessUrl { get; set; }

		/// <summary>
		/// Determines whether the Remember Me option is shown or not.
		/// </summary>
		/// <value>bool</value>
		[SettingDefault(true)]
		public bool ShowRememberMe { get; set; }

		/// <summary>
		/// Remember me cookie name.
		/// 
		/// Must be between 6 and 20 characters long.
		/// 
		/// Default: RememberMe
		/// </summary>
		[SettingString(false, 6, 20)]
		[SettingDefault("RememberMe")]
		public string RememberMeCookieName { get; set; }

		/// <summary>
		/// Encryption key used to encrypt cookie values.
		/// 
		/// Must be between 20 and 60 characters long.
		/// </summary>
		/// <value>string</value>
		[SettingString(false, SharedPluginFeatures.Constants.MinimumKeyLength, SharedPluginFeatures.Constants.MaximumKeyLength)]
		[SettingDefault("A^SSDFasdkl;fjanewrun[ca'ekd jf;z4sieurn;fdmmjf")]
		public string EncryptionKey { get; set; }

		/// <summary>
		/// Number of days the user can remain logged in, this is accomplished using cookies.
		/// 
		/// Default: 30
		/// Minimum: 1
		/// Maximum: 360
		/// </summary>
		/// <value>int</value>
		[SettingRange(1, 360)]
		[SettingDefault(30)]
		public int LoginDays { get; set; }

		/// <summary>
		/// Url that the user can be redirected to, in order to change their password.
		/// 
		/// This must be either a relative or absolute Uri.
		/// </summary>
		/// <value>string</value>
		[SettingUri(false, System.UriKind.RelativeOrAbsolute)]
		[SettingDefault("/Account/ChangePassword")]
		public string ChangePasswordUrl { get; set; }

		/// <summary>
		/// The name of the authentication scheme
		/// </summary>
		/// <value>string</value>
		[SettingDefault("DefaultAuthSchemeName")]
		[SettingString(false)]
		public string AuthenticationScheme { get; set; }

		/// <summary>
		/// Unique client id for google logins.
		/// 
		/// By default this will be returned from an environment variable for the user who's account this website is running in, but can be set directly in appsettings.json
		/// </summary>
		[SettingDefault("%GoogleClientId%")]
		public string GoogleClientId { get; set; }

		/// <summary>
		/// Google secret for client.
		/// 
		/// By default this will be returned from an environment variable for the user who's account this website is running in, but can be set directly in appsettings.json
		/// </summary>
		[SettingDefault("%GoogleSecret%")]
		public string GoogleSecret { get; set; }


		/// <summary>
		/// Unique client id for facebook logins.
		/// 
		/// By default this will be returned from an environment variable for the user who's account this website is running in, but can be set directly in appsettings.json
		/// </summary>
		[SettingDefault("%FacebookClientId%")]
		public string FacebookClientId { get; set; }

		/// <summary>
		/// Google secret for client.
		/// 
		/// By default this will be returned from an environment variable for the user who's account this website is running in, but can be set directly in appsettings.json
		/// </summary>
		[SettingDefault("%FacebookSecret%")]
		public string FacebookSecret { get; set; }

		/// <summary>
		/// Determines whether google login is allowed or not, based on current settings
		/// </summary>
		/// <returns>bool</returns>
		public bool IsGoogleLoginEnabled()
		{
			return !String.IsNullOrWhiteSpace(GoogleSecret) && !String.IsNullOrWhiteSpace(GoogleClientId);
		}

		/// <summary>
		/// Determines whether facebook login is allowed or not, based on current settings
		/// </summary>
		/// <returns>bool</returns>
		public bool IsFacebookLoginEnabled()
		{
			return !String.IsNullOrWhiteSpace(FacebookSecret) && !String.IsNullOrWhiteSpace(FacebookClientId);
		}
	}
}
