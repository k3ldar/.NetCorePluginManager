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
 *  Product:  DynamicContent.Plugin
 *  
 *  File: DefaultDynamicContentServices.cs
 *
 *  Purpose:  Default dynamic content services
 *
 *  Date        Name                Reason
 *  13/08/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Middleware;
using Middleware.DynamicContent;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

#pragma warning disable CS1591

namespace DynamicContent.Plugin.Internal
{
	public class DefaultDynamicContentProvider : IDynamicContentProvider
	{
		#region Private Members

		private const int FileVersion1 = 1;
		private const int ReservedSpace1 = 65;
		private const int ReservedSpace2 = 99;
		private static readonly byte[] Header = [44, 43];
		private const int MinimumByteSize = 54;

		private readonly string _rootContentPath;
		private readonly List<IDynamicContentPage> _dynamicContent;
		private readonly IPluginClassesService _pluginClassesService;
		private readonly object _lockObject = new();
		private List<DynamicContentTemplate> _templates;

		#endregion Private Members

		#region Constructors

		public DefaultDynamicContentProvider(IPluginClassesService pluginClassesService, ISettingsProvider settingsProvider)
		{
			_pluginClassesService = pluginClassesService ?? throw new ArgumentNullException(nameof(pluginClassesService));

			if (settingsProvider == null)
				throw new ArgumentNullException(nameof(settingsProvider));

			DynamicContentSettings dynamicContentSettings = settingsProvider.GetSettings<DynamicContentSettings>(Controllers.DynamicContentController.Name);
			_rootContentPath = dynamicContentSettings.DynamicContentLocation;

			_dynamicContent = [];

			InitializeDynamicContent();
		}

		#endregion Constructors

		#region IDynamicContentProvider Methods

		public long CreateCustomPage()
		{
			long Result = 1;

			using (TimedLock tl = TimedLock.Lock(_lockObject))
			{
				while (File.Exists(Path.Combine(_rootContentPath, $"{Result}.page")))
				{
					Result++;
				}

				IDynamicContentPage newPage = new DynamicContentPage(Result)
				{
					Name = $"Page-{Result}",
					ActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
					ActiveTo = new DateTime(2050, 12, 31, 23, 59, 59, DateTimeKind.Utc)
				};

				_dynamicContent.Add(newPage);
				Save(newPage);
			}

			return Result;
		}

		public List<LookupListItem> GetCustomPageList()
		{
			List<LookupListItem> Result = [];

			using (TimedLock tl = TimedLock.Lock(_lockObject))
			{
				_dynamicContent.ForEach(dc => Result.Add(new LookupListItem((int)dc.Id, dc.Name)));
			}

			return Result;
		}

		public IDynamicContentPage GetCustomPage(long id)
		{
			using (TimedLock tl = TimedLock.Lock(_lockObject))
			{
				return _dynamicContent.Find(dc => dc.Id.Equals(id));
			}
		}

		public List<DynamicContentTemplate> Templates()
		{
			if (_templates != null)
				return _templates;

			_templates = _pluginClassesService.GetPluginClasses<DynamicContentTemplate>();

			return _templates;
		}

		public List<IDynamicContentPage> GetCustomPages()
		{
			using (TimedLock tl = TimedLock.Lock(_lockObject))
			{
				return _dynamicContent;
			}
		}

		public bool PageNameExists(long id, string pageName)
		{
			if (String.IsNullOrEmpty(pageName))
				throw new ArgumentNullException(nameof(pageName));

			using (TimedLock tl = TimedLock.Lock(_lockObject))
			{
				return _dynamicContent.Exists(dc => !dc.Id.Equals(id) && dc.Name.Equals(pageName, StringComparison.InvariantCultureIgnoreCase));
			}
		}

		public bool RouteNameExists(long id, string routeName)
		{
			if (String.IsNullOrEmpty(routeName))
				throw new ArgumentNullException(nameof(routeName));

			using (TimedLock tl = TimedLock.Lock(_lockObject))
			{
				return _dynamicContent.Exists(dc => !dc.Id.Equals(id) && dc.RouteName.Equals(routeName, StringComparison.InvariantCultureIgnoreCase));
			}
		}

		public bool Save(IDynamicContentPage dynamicContentPage)
		{
			if (dynamicContentPage == null)
				throw new ArgumentNullException(nameof(dynamicContentPage));

			using (TimedLock tl = TimedLock.Lock(_lockObject))
			{
				WriteFileContents(dynamicContentPage);

				_dynamicContent.Remove(GetCustomPage(dynamicContentPage.Id));
				_dynamicContent.Add(dynamicContentPage);
			}

			return true;
		}

		public bool SaveUserInput(string data)
		{
			if (String.IsNullOrEmpty(data))
				return false;

			string inputPath = Path.Combine(_rootContentPath, "Input");

			if (!Directory.Exists(inputPath))
				Directory.CreateDirectory(inputPath);

			inputPath = Path.Combine(inputPath, DateTime.Now.Ticks.ToString() + ".input");

			File.WriteAllText(inputPath, data);

			return File.Exists(inputPath);
		}

		#endregion IDynamicContentProvider Methods

		#region Static Methods

		public static byte[] ConvertFromDynamicContent(IDynamicContentPage dynamicContentPage)
		{
			using (MemoryStream memoryStream = new())
			using (BinaryWriter writer = new(memoryStream))
			{
				writer.Write(Header);
				writer.Write(ReservedSpace1);
				writer.Write(ReservedSpace2);
				writer.Write(FileVersion1);
				writer.Write(dynamicContentPage.Id);
				WriteStringData(writer, dynamicContentPage.Name);
				WriteStringData(writer, dynamicContentPage.RouteName);
				WriteStringData(writer, dynamicContentPage.BackgroundColor);
				WriteStringData(writer, dynamicContentPage.BackgroundImage);
				writer.Write(dynamicContentPage.ActiveFrom.Ticks);
				writer.Write(dynamicContentPage.ActiveTo.Ticks);
				writer.Write(dynamicContentPage.Content.Count);

				foreach (DynamicContentTemplate page in dynamicContentPage.Content)
				{
					WriteStringData(writer, page.UniqueId);
					WriteStringData(writer, page.AssemblyQualifiedName);
					writer.Write(page.ActiveFrom.Ticks);
					writer.Write(page.ActiveTo.Ticks);
					WriteStringData(writer, page.Data);
					writer.Write(page.Height);
					writer.Write((int)page.HeightType);
					writer.Write(page.SortOrder);
					writer.Write(page.Width);
					writer.Write((int)page.WidthType);
				}

				return memoryStream.ToArray();
			}
		}

		public static IDynamicContentPage ConvertFromByteArray(byte[] content)
		{
			if (content == null)
				throw new ArgumentNullException(nameof(content));

			if (content.Length < MinimumByteSize)
				throw new ArgumentException(null, nameof(content));

			using (MemoryStream memoryStream = new(content))
			using (BinaryReader reader = new(memoryStream))
			{
				memoryStream.Position = 0;

				if (reader.ReadByte() == Header[0] && reader.ReadByte() == Header[1])
				{
					// reserved space for future changes
					reader.ReadInt32();
					reader.ReadInt32();

					int version = reader.ReadInt32();

					switch (version)
					{
						case FileVersion1:
							DynamicContentPage Result = new();

							LoadDynamicContentVersion1(reader, Result);
							return Result;

						default:
							throw new InvalidOperationException();
					}
				}
			}

			return null;
		}

		private static void WriteStringData(BinaryWriter writer, string data)
		{
			byte[] byteData;

			if (String.IsNullOrEmpty(data))
				byteData = Array.Empty<byte>();
			else
				byteData = Encoding.UTF8.GetBytes(data);

			writer.Write(byteData.Length);

			if (byteData.Length > 0)
				writer.Write(byteData);
		}

		private static string ReadStringData(BinaryReader reader)
		{
			int len = reader.ReadInt32();

			bool isValid = len + reader.BaseStream.Position < reader.BaseStream.Length && len > 0;

			if (isValid)
				return Encoding.UTF8.GetString(reader.ReadBytes(len));

			return String.Empty;
		}

		private static void LoadDynamicContentVersion1(BinaryReader reader, DynamicContentPage dynamicContentPage)
		{
			dynamicContentPage.Id = reader.ReadInt64();
			dynamicContentPage.Name = ReadStringData(reader);
			dynamicContentPage.RouteName = ReadStringData(reader);
			dynamicContentPage.BackgroundColor = ReadStringData(reader);
			dynamicContentPage.BackgroundImage = ReadStringData(reader);
			dynamicContentPage.ActiveFrom = new DateTime(reader.ReadInt64(), DateTimeKind.Utc);
			dynamicContentPage.ActiveTo = new DateTime(reader.ReadInt64(), DateTimeKind.Utc);
			int itemCount = reader.ReadInt32();
			int item = 0;

			while (item < itemCount)
			{
				string uniqueId = ReadStringData(reader);
				string className = ReadStringData(reader);

				string[] classParts = className.Split(",");

				if (classParts.Length < 2)
					throw new InvalidOperationException();

				DynamicContentTemplate instance = CreateTemplateItem(classParts[1].Trim(), classParts[0].Trim(), uniqueId, out bool templateClassFound);
				instance.ActiveFrom = new DateTime(reader.ReadInt64(), DateTimeKind.Utc);
				instance.ActiveTo = new DateTime(reader.ReadInt64(), DateTimeKind.Utc);
				string data = ReadStringData(reader);

				if (templateClassFound)
					instance.Data = data;
				else
					instance.Data = "<p>Content template not found</p>";

				instance.Height = reader.ReadInt32();
				instance.HeightType = (DynamicContentHeightType)reader.ReadInt32();
				instance.SortOrder = reader.ReadInt32();
				instance.Width = reader.ReadInt32();
				instance.WidthType = (DynamicContentWidthType)reader.ReadInt32();
				dynamicContentPage.Content.Add(instance);
				item++;
			}
		}

		private static DynamicContentTemplate CreateTemplateItem(string assemblyName, string className, string uniqueId, out bool templateClassFound)
		{
			DynamicContentTemplate baseInstance;

			try
			{
				Assembly classAssembly = Assembly.Load(assemblyName);
				Type t = classAssembly.GetType(className);

				if (t == null)
				{
					baseInstance = new Templates.HtmlTextTemplate();
					templateClassFound = false;
				}
				else
				{
					baseInstance = (DynamicContentTemplate)Activator.CreateInstance(t);
					templateClassFound = true;
				}
			}
			catch
			{
				baseInstance = new Templates.HtmlTextTemplate();
				templateClassFound = false;
			}

			return baseInstance.Clone(uniqueId);
		}

		#endregion Static Methods

		#region Private Methods

		private void WriteFileContents(IDynamicContentPage dynamicContentPage)
		{
			byte[] contents = ConvertFromDynamicContent(dynamicContentPage);
			string filename = Path.Combine(_rootContentPath, $"{dynamicContentPage.Id}.page");
			File.WriteAllBytes(filename, contents);
		}

		private static IDynamicContentPage ReadFileContents(string filename)
		{
			return ConvertFromByteArray(File.ReadAllBytes(filename));
		}

		private void InitializeDynamicContent()
		{
			if (!Directory.Exists(_rootContentPath))
				Directory.CreateDirectory(_rootContentPath);

			using (TimedLock tl = TimedLock.Lock(_lockObject))
			{
				string[] pages = Directory.GetFiles(_rootContentPath, "*.page");

				foreach (string page in pages)
				{
					try
					{
						IDynamicContentPage convertedPage = ReadFileContents(page);

						if (convertedPage != null)
							_dynamicContent.Add(convertedPage);
					}
					catch
					{
						// ignore as couldn't load
					}
				}
			}
		}

		#endregion Private Methods
	}
}

#pragma warning restore CS1591