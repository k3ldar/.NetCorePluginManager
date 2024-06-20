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
 *  Product:  SharedPluginFeatures
 *  
 *  File: HmacGenerator.cs
 *
 *  Purpose:  Contains methods for generating a valid hmac value
 *
 *  Date        Name                Reason
 *  20/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace SharedPluginFeatures
{
	/// <summary>
	/// Helper class providing methods that can generate a hmac value to be used with HmacApiAuthorizationService
	/// </summary>
	public static class HmacGenerator
	{
		private readonly static Random _random = new((int)(EpochDateTime() / TimeSpan.TicksPerSecond));

		private const int MininumNonceValue = 1;
		private const int MinimumEpochTicks = 0;

		/// <summary>
		/// Generates a hmac value based on parameter values
		/// </summary>
		/// <param name="apiKey">Users api key</param>
		/// <param name="apiSecret">Users api secret</param>
		/// <param name="epochTicks">Number of ticks since 1/1/1970</param>
		/// <param name="nonce">User generated nonce value</param>
		/// <param name="token">Users token</param>
		/// <param name="payload">Payload being validated using user details</param>
		/// <returns>string</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="apiKey"/> is null or empty</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="apiSecret"/> is null or empty</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="token"/> is null or empty</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="payload"/> is null or empty</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="epochTicks"/> is less than zero</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="nonce"/>is less than 1</exception>
		public static string GenerateHmac(string apiKey, string apiSecret, long epochTicks, ulong nonce, string token, string payload)
		{
			if (String.IsNullOrEmpty(apiKey))
				throw new ArgumentNullException(nameof(apiKey));

			if (String.IsNullOrEmpty(apiSecret))
				throw new ArgumentNullException(nameof(apiSecret));

			if (epochTicks < MinimumEpochTicks)
				throw new ArgumentOutOfRangeException(nameof(epochTicks));

			if (nonce < MininumNonceValue)
				throw new ArgumentOutOfRangeException(nameof(nonce));

			if (String.IsNullOrEmpty(token))
				throw new ArgumentNullException(nameof(token));

			if (payload == null)
				throw new ArgumentNullException(nameof(payload));

			string hmacData = $"{apiKey}{nonce}{epochTicks}{token}{payload}";

			using (HMACSHA256 hmac = new(Encoding.UTF8.GetBytes(apiSecret)))
			{
				byte[] encBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(hmacData));
				string encString = ByteArrayToHexString(encBytes);
				return Convert.ToBase64String(Encoding.UTF8.GetBytes(encString));
			}
		}

		/// <summary>
		/// Generates the current date time as epoch date time returning the number of ticks
		/// </summary>
		/// <returns>long</returns>
		public static long EpochDateTime()
		{
			return (DateTime.UtcNow - DateTime.UnixEpoch).Ticks;
		}

		/// <summary>
		/// Generates a random nonce value
		/// </summary>
		/// <returns>ulong</returns>
		public static ulong GenerateNonce()
		{
			return UInt64.MaxValue - (ulong)_random.Next(0, Int32.MaxValue);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static string ByteArrayToHexString(byte[] ba)
		{
			StringBuilder hex = new(ba.Length * 2);

			foreach (byte b in ba)
				hex.AppendFormat("{0:x2}", b);

			return hex.ToString();
		}
	}
}
