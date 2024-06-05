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
 *  File: BaseCoreClass.cs
 *
 *  Purpose:  Base Core Class
 *
 *  Date        Name                Reason
 *  24/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

#pragma warning disable CS1591

namespace SharedPluginFeatures
{
    public class BaseCoreClass
    {
		#region Private Members

		private const string AtoZUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const string AtoZLower = "abcdefghijklmnopqrstuvwxyz";
		private const string Numbers = "01234567890";
		private const string EmailChars = "\".@-";
		private const string PasswordChars = "!£$%^&}{@~:}{<>*()_+-=[]'#;";
		private const string Space = " ";
		private const string Dash = "-";
		private const string FSlash = "/";

		private const string EmailCharacters = AtoZLower + AtoZUpper + Numbers + EmailChars;
		private const string Password = EmailCharacters + PasswordChars + Space;
		private const string Name = AtoZUpper + AtoZLower + Numbers + Space;
		private const string FileOrPath = AtoZLower + AtoZUpper + Numbers + Space + Dash + FSlash;

		#endregion Private Members

		#region Constructors

		protected BaseCoreClass()
		{

		}

		#endregion Constructors

		#region Protected Methods

		public static string ValidateUserInput(string userInput, ValidationType validationType)
		{
			if (string.IsNullOrEmpty(userInput))
				return userInput;

			switch (validationType)
			{
				case ValidationType.Path:
					return ValidateUserPathInput(userInput);

				case ValidationType.Email:
					EmailAddressAttribute emailAddressAttribute = new();
					if (emailAddressAttribute.IsValid(userInput))
						return userInput;

					return RemoveInvalidCharacters(userInput, EmailCharacters);

				case ValidationType.Password:
					return RemoveInvalidCharacters(userInput, Password);

				case ValidationType.Name:
					return RemoveInvalidCharacters(userInput, Name);

				case ValidationType.FileName:
				case ValidationType.RouteName:
					return RemoveInvalidCharacters(userInput, FileOrPath);

				case ValidationType.RedirectUriLocal:
					if (!userInput.StartsWith(FSlash))
						userInput = $"{FSlash}{userInput}";

					return RemoveInvalidCharacters(userInput, FileOrPath);
			}

			return userInput;
		}

		#endregion Protected Methods

		#region Static Methods

		private static string ValidateUserPathInput(string path)
		{
			if (path.Contains(Constants.InvalidPathPreviousDirectory))
				throw new InvalidOperationException($"Invalid path: {path}");

			if (PathContainsInvalidChars(path))
				throw new InvalidOperationException($"Path contains invalid characters: {path}");

			return path;
		}

		private static string RemoveInvalidCharacters(string input, string validChars)
		{
			StringBuilder stringBuilder = new(input.Length);

			foreach (char c in input)
			{
				if (validChars.Contains(c))
					stringBuilder.Append(c);
			}

			return stringBuilder.ToString();
		}

		private static bool PathContainsInvalidChars(string path)
		{
			char[] invalidChars = System.IO.Path.GetInvalidPathChars();

			foreach (char c in path)
			{
				if (invalidChars.Contains(c))
					return true;
			}

			return false;
		}

		#endregion Static Methods
	}
}

#pragma warning restore CS1591