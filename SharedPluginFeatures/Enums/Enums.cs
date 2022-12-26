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
 *  Product:  SharedPluginFeatures
 *  
 *  File: Enums.cs
 *
 *  Purpose:  Shared Enum Values
 *
 *  Date        Name                Reason
 *  19/10/2018  Simon Carter        Initially Created
 *  04/11/2018  Simon Carter        Add Sieradelta GeoIp options to geoip 
 *  21/11/2018  Simon Carter        Add Login Enums
 *  20/01/2020  Simon Carter        Add minification enums
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
	/// <summary>
	/// Standard Enum values shared across all plugin modules.
	/// </summary>
	public static class Enums
	{
		/// <summary>
		/// Geo Ip provider types.
		/// 
		/// GeoIpPlugin module can use a variety of methods to implement Geo Ip lookup functionality via the IGeoIpProvider interface.
		/// </summary>
		public enum GeoIpProvider
		{
			/// <summary>
			/// No Geo Ip Provider
			/// </summary>
			None = 0,

			/// <summary>
			/// Geo Ip data provided by IpStack https://ipstack.com/
			/// </summary>
			IpStack = 1,

			/// <summary>
			/// Geo Ip data is provided by a MySql Database 
			/// </summary>
			MySql = 2,

			/// <summary>
			/// Geo Ip data is provided by a MS Sql Server Database
			/// </summary>
			MSSql = 3,

			/// <summary>
			/// Geo Ip data is provided by a Firebird database
			/// </summary>
			Firebird = 4,
		}

		/// <summary>
		/// System Admin menu types
		/// </summary>
		public enum SystemAdminMenuType
		{
			/// <summary>
			/// Not used at present time.
			/// </summary>
			FirstChild = 0,

			/// <summary>
			/// Data to be shown within SystemAdmin.Plugin is raw data.
			/// </summary>
			Text = 1,

			/// <summary>
			/// Data to be shown within SystemAdmin.Plugin is grid based data.
			/// </summary>
			Grid = 2,

			/// <summary>
			/// Data to be shown within SystemAdmin.Plugin comes from a partial view provided by the implementor.
			/// </summary>
			PartialView = 3,

			/// <summary>
			/// Data to be shown within SystemAdmin.Plugin is map related data.
			/// </summary>
			Map = 4,

			/// <summary>
			/// Data to be shown within SystemAdmin.Plugin is raw text formatted using html.
			/// </summary>
			FormattedText = 5,

			/// <summary>
			/// Data to be shown within the plugin comes from a view
			/// </summary>
			View = 6,

			/// <summary>
			/// Data to be shown will be a chart
			/// </summary>
			Chart = 7,

			/// <summary>
			/// Contains settings for a plugin
			/// </summary>
			Settings = 8,
		}

		/// <summary>
		/// Validate Request Results
		/// </summary>
		[Flags]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1714:Flags enums should have plural names", Justification = "yeak ok, I hear you but not this time!")]
		public enum ValidateRequestResult
		{
			/// <summary>
			/// State unknown
			/// </summary>
			Undetermined = 1,

			/// <summary>
			/// Ip has too many requests
			/// </summary>
			TooManyRequests = 2,

			/// <summary>
			/// Enough keywords to suggest may be a SQL injection attack
			/// </summary>
			PossibleSQLInjectionAttack = 4,

			/// <summary>
			/// Enough keywords to determine this is a SQL injection attack
			/// </summary>
			SQLInjectionAttack = 8,

			/// <summary>
			/// Determines that the request is probably generated from a spider or bot
			/// </summary>
			PossibleSpiderBot = 16,

			/// <summary>
			/// Determines that the request is generated from a spider or bot
			/// </summary>
			SpiderBot = 32,

			/// <summary>
			/// Enough keywords to suggest this maybe a hack attempt
			/// </summary>
			PossibleHackAttempt = 64,

			/// <summary>
			/// Enough keywords to determine this is a hack attempt
			/// </summary>
			HackAttempt = 128,

			/// <summary>
			/// IP Address is white listed
			/// </summary>
			IpWhiteListed = 256,

			/// <summary>
			/// IP Address is black listed
			/// </summary>
			IpBlackListed = 512,

			/// <summary>
			/// IP address is a search engine
			/// </summary>
			SearchEngine = 1024,

			/// <summary>
			/// A Ban has been requested on the IP Address
			/// </summary>
			BanRequested = 2048,
		}
	}

	/// <summary>
	/// Type of dynamic content template type
	/// </summary>
	public enum DynamicContentTemplateType
	{
		/// <summary>
		/// Standard template type
		/// </summary>
		Default,

		/// <summary>
		/// Template is a form input type
		/// </summary>
		Input
	}

	/// <summary>
	/// Type of data to be posted to a controller action
	/// </summary>
	public enum PostType
	{
		/// <summary>
		/// The post is a web form
		/// </summary>
		Form,

		/// <summary>
		/// The post is json data
		/// </summary>
		Json,

		/// <summary>
		/// The post data is xml
		/// </summary>
		Xml,

		/// <summary>
		/// Other type of custom data
		/// </summary>
		Other
	}

	/// <summary>
	/// The frequency of which a sitemap item is updated.
	/// </summary>
	public enum SitemapChangeFrequency
	{
		/// <summary>
		/// The item is continually updated
		/// </summary>
		Always,

		/// <summary>
		/// The item is updated on an hourly basis
		/// </summary>
		Hourly,

		/// <summary>
		/// The item is updated daily
		/// </summary>
		Daily,

		/// <summary>
		/// The item is updated on a weekly basis
		/// </summary>
		Weekly,

		/// <summary>
		/// The item is updated on a monthly basis
		/// </summary>
		Monthly,

		/// <summary>
		/// The item is updated very rarely
		/// </summary>
		Yearly,

		/// <summary>
		/// The item is archived and will never be updated again
		/// </summary>
		Never,
	}

	/// <summary>
	/// Type of file to be minified
	/// </summary>
	public enum MinificationFileType
	{
		/// <summary>
		/// Unknown file type, not known to the minification process.
		/// </summary>
		Unknown,

		/// <summary>
		/// The type of file is a Razor file
		/// </summary>
		Razor,

		/// <summary>
		/// The type of file is a HTML file.
		/// </summary>
		Html,

		/// <summary>
		/// The type of file is a HTM file.
		/// </summary>
		Htm,

		/// <summary>
		/// The type of file is a JavaScript file.
		/// </summary>
		Js,

		/// <summary>
		/// The type of file is a CSS file.
		/// </summary>
		CSS,

		/// <summary>
		/// The type of file is a Less file.
		/// </summary>
		Less,

		/// <summary>
		/// The type of file is a Gif image.
		/// </summary>
		ImageGif,

		/// <summary>
		/// The type of file is a Jpeg file.
		/// </summary>
		ImageJpeg,

		/// <summary>
		/// The type of file is a PNG file.
		/// </summary>
		ImagePng
	}

	/// <summary>
	/// Block of data to be preserved during minification
	/// </summary>
	public enum MinificationPreserveBlock
	{
		/// <summary>
		/// Undefined preserve block
		/// </summary>
		Undefined,

		/// <summary>
		/// The contents represent an Html &lt;pre&gt; block
		/// </summary>
		HtmlPreBlock,

		/// <summary>
		/// The contents represent Razor code within a line
		/// </summary>
		RazorLine,

		/// <summary>
		/// The contents represent a preservable block of Razor code.
		/// </summary>
		RazorBlock
	}


	/// <summary>
	/// Data type used for a chart
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "String is the name of the data type")]
	public enum ChartDataType
	{
		/// <summary>
		/// The chart data type has not been set, this value indicates the <see cref="SharedPluginFeatures.ChartModel"/> is invalid
		/// </summary>
		NotSet = 0,

		/// <summary>
		/// String chart data type
		/// </summary>
		String,

		/// <summary>
		/// Number chart data type
		/// </summary>
		Number,

		/// <summary>
		/// Bool chart data type
		/// </summary>
		Boolean,

		/// <summary>
		/// Date chart data type
		/// </summary>
		Date,

		/// <summary>
		/// Date/Time chart data type
		/// </summary>
		DateTime,

		/// <summary>
		/// Time of day chart data type
		/// </summary>
		TimeOfDay
	}

	/// <summary>
	/// Enum with width options for dynamic content templates
	/// </summary>
	public enum DynamicContentWidthType : byte
	{
		/// <summary>
		/// Value is defined in terms of columns (for use in bootstrap etc)
		/// </summary>
		Columns = 0,

		/// <summary>
		/// Value is defined in terms of pixels
		/// </summary>
		Pixels = 1,

		/// <summary>
		/// The value is defined as a percentage
		/// </summary>
		Percentage = 2
	}

	/// <summary>
	/// Enum with height options for dynamic content templates
	/// </summary>
	public enum DynamicContentHeightType : byte
	{
		/// <summary>
		/// Value is defined in terms of pixels
		/// </summary>
		Pixels = 0,

		/// <summary>
		/// The value is defined as a percentage
		/// </summary>
		Percentage = 1,

		/// <summary>
		/// The control fits the size of the internal data
		/// </summary>
		Automatic = 2
	}

	/// <summary>
	/// Type of discount that has been applied to an Invoice/Order
	/// </summary>
	public enum DiscountType
	{
		/// <summary>
		/// Invoice/Order has no discount.
		/// </summary>
		None = 0,

		/// <summary>
		/// Invoice/Order has a percentage discount on the total.
		/// </summary>
		PercentageTotal = 1,

		/// <summary>
		/// Invoice/Order has a percentage discount on the sub total.
		/// </summary>
		PercentageSubTotal = 2,

		/// <summary>
		/// Invoice/Order has a monetary value discount (i.e. GBP10)
		/// </summary>
		Value = 3
	}

	/// <summary>
	/// Type of validation
	/// </summary>
	public enum ValidationType
	{
		/// <summary>
		/// Unknown strict validation
		/// </summary>
		Other,

		/// <summary>
		/// Email validation
		/// </summary>
		Email,

		/// <summary>
		/// Filename validation
		/// </summary>
		FileName,

		/// <summary>
		/// Path validation
		/// </summary>
		Path,

		/// <summary>
		/// Name validation
		/// </summary>
		Name,

		/// <summary>
		/// Password validation
		/// </summary>
		Password,

		/// <summary>
		/// Route name validation
		/// </summary>
		RouteName,

		/// <summary>
		/// Local Redirect uri
		/// </summary>
		RedirectUriLocal,
	}
}
