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
 *  Copyright (c) 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Shopping Cart Plugin
 *  
 *  File: PaypointHelper.cs
 *
 *  Purpose:  Helper for paypoint provider
 *
 *  Date        Name                Reason
 *  24/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Text;
using System.IO;
using System.Web;
using System.Net;
using System.Collections.Specialized;

using static Shared.Utilities;

namespace ShoppingCartPlugin.Classes.Paypoint
{
	/// <summary>
	/// determines what type of post to perform.
	/// Get: Does a get against the source.
	/// Post: Does a post against the source.
	/// </summary>
	public enum PostType { Get, Post }

	/// <summary>
	/// Summary description for ValCard.
	/// </summary>
	public class PaypointHelper
	{
		#region Private Members

		private readonly string _transID;
		private readonly string _amount;
		private string _digest;

		#endregion Private Members

		#region Constants

		private const int Timeout = 10000;

		#endregion Constants

		#region Constructors

		public PaypointHelper(string orderReference, decimal cost, string currency, string merchantId, 
			string remotePassword, string urlSuccess)
		{
			PostItems = new NameValueCollection();
			Url = "https://www.secpay.com/java-bin/ValCard";
			PostType = PostType.Post;
			Currency = currency;
			_transID = orderReference;
			_amount = Convert.ToString(cost);
			UrlSuccess = urlSuccess;
			MerchantID = merchantId;
			RemotePassword = remotePassword;
			UrlSuccess = urlSuccess;
			CreateDigest();
			BuildParamList();
		}

		#endregion Constructors

		#region Properties

		public string UrlSuccess { get; private set; }

		public string RemotePassword { get; private set; }

		public string MerchantID { get; private set; }

		public string Currency { get; private set; }

		/// <summary>
		/// Gets or sets the url to submit the post to.
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Gets or sets the name value collection of items to post.
		/// </summary>
		public NameValueCollection PostItems { get; private set; }

		/// <summary>
		/// Gets or sets the type of action to perform against the url.
		/// </summary>
		public PostType PostType { get; set; }

		#endregion Properties

		#region Public Methods

		public string GetURL()
		{
			StringBuilder parameters = new StringBuilder();

			for (int i = 0; i < PostItems.Count; i++)
			{
				EncodeAndAddItem(ref parameters, PostItems.GetKey(i), PostItems[i]);
			}

			string result = Url + "?" + parameters.ToString();
			return result;
		}

		/// <summary>
		/// Posts the supplied data to specified url.
		/// </summary>
		/// <returns>a string containing the result of the post.</returns>
		public string Post(string url)
		{
			StringBuilder parameters = new StringBuilder();

			for (int i = 0; i < PostItems.Count; i++)
			{
				EncodeAndAddItem(ref parameters, PostItems.GetKey(i), PostItems[i]);
			}


			string result = PostData(url, parameters.ToString());
			return result;
		}

		/// <summary>
		/// Posts the supplied data to specified url.
		/// </summary>
		/// <returns>a string containing the result of the post.</returns>
		public string Post()
		{
			return Post(Url);
		}

		#endregion Public Methods

		#region Private Methods

		private void CreateDigest()
		{
			_digest =  HashStringMD5(_transID + _amount + RemotePassword);
		}

		private void BuildParamList()
		{
			PostItems.Clear();
			PostItems.Add("merchant", MerchantID);
			PostItems.Add("trans_id", _transID);
			PostItems.Add("amount", _amount);
			PostItems.Add("callback", UrlSuccess);
			PostItems.Add("digest", _digest);
			PostItems.Add("currency", Currency);
		}

		/// <summary>
		/// Posts data to a specified url. Note that this assumes that you have already url encoded the post data.
		/// </summary>
		/// <param name="postData">The data to post.</param>
		/// <param name="url">the url to post to.</param>
		/// <returns>Returns the result of the post.</returns>
		private string PostData(string url, string postData)
		{
			HttpWebRequest request = null;
			
			if (PostType == PostType.Post)
			{
				Uri uri = new Uri(url);
				request = (HttpWebRequest)WebRequest.Create(uri);
				request.Method = "POST";
				request.Timeout = Timeout;
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = postData.Length;
				using (Stream writeStream = request.GetRequestStream())
				{
					UTF8Encoding encoding = new UTF8Encoding();
					byte[] bytes = encoding.GetBytes(postData);
					writeStream.Write(bytes, 0, bytes.Length);
				}
			}
			else
			{
				Uri uri = new Uri(url + "?" + postData);
				request = (HttpWebRequest)WebRequest.Create(uri);
				request.Method = "GET";
			}

			string result = string.Empty;

			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			{
				using (Stream responseStream = response.GetResponseStream())
				{
					using (StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8))
					{
						result = readStream.ReadToEnd();
					}
				}
			}
			return result;
		}

		private void EncodeAndAddItem(ref StringBuilder baseRequest, string key, string dataItem)
		{
			if (baseRequest == null)
			{
				baseRequest = new StringBuilder();
			}

			if (baseRequest.Length != 0)
			{
				baseRequest.Append("&");
			}

			baseRequest.Append(key);
			baseRequest.Append("=");
			baseRequest.Append(HttpUtility.UrlEncode(dataItem));
		}

		#endregion Private Methods
	}
}
