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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: DocumentTests.cs
 *
 *  Purpose:  Document Tests
 *
 *  Date        Name                Reason
 *  11/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using AspNetCore.PluginManager.Tests.Documentation;

using DocumentationPlugin.Classes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shared.Classes;
using Shared.Docs;

using consts = SharedPluginFeatures.Constants;
using sl = Shared;

namespace AspNetCore.PluginManager.Tests.Plugins.DocumentationTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class DocumentTests : BaseDocumentTests
    {
        [TestInitialize]
		public void Setup()
		{
			string appSettingsFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "appsettings.json");

			if (!File.Exists(appSettingsFile))
			{
				File.WriteAllText(appSettingsFile, Encoding.UTF8.GetString(Properties.Resources.appsettings));
			}
		}

		[TestMethod]
		public void LoadXmlFile()
        {
            GetDocuments();
            CacheItem cache = MemoryCache.GetCache().Get(consts.DocumentationListCache);

            Assert.IsNotNull(cache);
            Assert.IsInstanceOfType(cache.Value, typeof(List<Document>));
            List<Document> documents = (List<Document>)cache.Value;

            Assert.IsTrue(documents.Count > 10, $"Actual document count: {documents.Count}");
        }

        [TestMethod]
        [Ignore]
        public void FindLongSearchDocument()
        {
            List<Document> docs = GetDocuments();

            Document searchDoc = docs.FirstOrDefault(d => d.LongDescription.Contains("<see cref=") && d.AssemblyName == "SearchPlugin");

            Assert.IsNotNull(searchDoc);
        }

        [TestMethod]
        [Ignore]
        public void CreatePostProcessDocument()
        {
            Document searchDoc = GetDocuments().FirstOrDefault(d => d.LongDescription.Contains("<see cref=") && d.AssemblyName == "SearchPlugin");

            Assert.IsNotNull(searchDoc);

            DocumentPostProcess postProcess = new DocumentPostProcess(GetDocuments());

            Assert.IsNotNull(postProcess);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PostProcessInvalidParameters()
        {
            DocumentPostProcess postProcess = new DocumentPostProcess(GetDocuments());

            Assert.IsNotNull(postProcess);

            postProcess.Process(null);
        }

        [TestMethod]
        [Ignore]
        public void ValidatePostProcessResults()
        {
            DocumentPostProcess postProcess = new DocumentPostProcess(GetDocuments());

            Assert.IsNotNull(postProcess);

            Document searchDoc = GetDocuments().FirstOrDefault(d => d.LongDescription.Contains("<see cref=") && d.AssemblyName == "SearchPlugin");

            Assert.IsNotNull(searchDoc);

            PostProcessResults postProcessResults = postProcess.Process(searchDoc);

            Assert.IsNotNull(postProcessResults);
        }

        [TestMethod]
        public void ValidatePostProcessResultsProcessSingleDocument()
        {
            List<Document> documents = new List<Document>();
            documents.Add(new Document(sl.DocumentType.Class, "Test", "Test", "ClassName", "T:Test.ClassName"));

            DocumentPostProcess postProcess = new DocumentPostProcess(documents);

            Assert.IsNotNull(postProcess);

            Document searchDoc = new Document(sl.DocumentType.Custom, "Custom Type");
            searchDoc.LongDescription = "Test <see cref=\"T:Test.ClassName\" /> class";

            Assert.IsNotNull(searchDoc);

            PostProcessResults postProcessResults = postProcess.Process(searchDoc);

            Assert.IsNotNull(postProcessResults);

            Assert.AreEqual(1, postProcessResults.DocumentsProcessed);
        }

        [TestMethod]
        public void ValidatePostProcessResultsProcessSingleDocumentSeeTag()
        {
            List<Document> documents = new List<Document>();
            documents.Add(new Document(sl.DocumentType.Class, "Test", "Test", "ClassName", "T:Test.ClassName"));

            DocumentPostProcess postProcess = new DocumentPostProcess(documents);

            Assert.IsNotNull(postProcess);

            Document searchDoc = new Document(sl.DocumentType.Custom, "Custom Type");
            searchDoc.ClassName = searchDoc.FullMemberName;
            searchDoc.LongDescription = "Test <see cref=\"T:Test.ClassName\" /> class";

            Assert.IsNotNull(searchDoc);

            PostProcessResults postProcessResults = postProcess.Process(searchDoc);

            Assert.IsNotNull(postProcessResults);

            Assert.AreEqual(1, postProcessResults.DocumentsProcessed);
            Assert.AreEqual(1, postProcessResults.GetCountValue(Constants.CountValueSee));
            Assert.AreEqual("Test <a href=\"/docs/Document/Test/ClassName/\">ClassName</a> class", searchDoc.LongDescription);
        }

        [TestMethod]
        public void ValidatePostProcessResultsProcessSingleDocumentSeeTagInvalid()
        {
            List<Document> documents = new List<Document>();
            documents.Add(new Document(sl.DocumentType.Class, "Test", "Test", "ClassName", "T:Test.ClassName"));

            DocumentPostProcess postProcess = new DocumentPostProcess(documents);

            Assert.IsNotNull(postProcess);

            Document searchDoc = new Document(sl.DocumentType.Custom, "Custom Type");
            searchDoc.LongDescription = "Test <see cref=\"T:Test.ClassName /> class";

            Assert.IsNotNull(searchDoc);

            PostProcessResults postProcessResults = postProcess.Process(searchDoc);

            Assert.IsNotNull(postProcessResults);

            Assert.AreEqual(1, postProcessResults.DocumentsProcessed);
            Assert.AreEqual(1, postProcessResults.GetCountValue(Constants.CountValueSee));
            Assert.AreEqual(1, postProcessResults.GetCountValue(Constants.CountValueSeeNotFound));
        }

        [TestMethod]
        public void ValidatePostProcessResultsProcessSingleDocumentSeeAlsoTag()
        {
            List<Document> documents = new List<Document>();
            documents.Add(new Document(sl.DocumentType.Class, "Test", "Test", "ClassName", "T:Test.ClassName"));

            DocumentPostProcess postProcess = new DocumentPostProcess(documents);

            Assert.IsNotNull(postProcess);

            Document searchDoc = new Document(sl.DocumentType.Custom, "Custom Type");
            searchDoc.ClassName = searchDoc.FullMemberName;
            searchDoc.LongDescription = "Test <seealso cref=\"T:Test.ClassName\" /> class";

            Assert.IsNotNull(searchDoc);

            PostProcessResults postProcessResults = postProcess.Process(searchDoc);

            Assert.IsNotNull(postProcessResults);

            Assert.AreEqual(1, postProcessResults.DocumentsProcessed);
            Assert.AreEqual(1, postProcessResults.GetCountValue(Constants.CountValueSeeAlso));
            Assert.AreEqual("Test <a href=\"/docs/Document/Test/ClassName/\">ClassName</a> class", searchDoc.LongDescription);
        }

        [TestMethod]
        public void ValidatePostProcessResultsProcessSingleDocumentSeeAlsoTagInvalid()
        {
            List<Document> documents = new List<Document>();
            documents.Add(new Document(sl.DocumentType.Class, "Test", "Test", "ClassName", "T:Test.ClassName"));

            DocumentPostProcess postProcess = new DocumentPostProcess(documents);

            Assert.IsNotNull(postProcess);

            Document searchDoc = new Document(sl.DocumentType.Custom, "Custom Type");
            searchDoc.ClassName = searchDoc.FullMemberName;
            searchDoc.LongDescription = "Test <seealso cref=\"T:Test.ClassName /> class";

            Assert.IsNotNull(searchDoc);

            PostProcessResults postProcessResults = postProcess.Process(searchDoc);

            Assert.IsNotNull(postProcessResults);

            Assert.AreEqual(1, postProcessResults.DocumentsProcessed);
            Assert.AreEqual(1, postProcessResults.GetCountValue(Constants.CountValueSeeAlso));
            Assert.AreEqual(1, postProcessResults.GetCountValue(Constants.CountValueSeeAlsoNotFound));
            Assert.AreEqual(0, searchDoc.SeeAlso.Count);
        }

        [TestMethod]
        public void ValidatePostProcessResultsProcessSingleDocumentParaTag()
        {
            List<Document> documents = new List<Document>();
            documents.Add(new Document(sl.DocumentType.Class, "Test", "Test", "ClassName", "T:Test.ClassName"));

            DocumentPostProcess postProcess = new DocumentPostProcess(documents);

            Assert.IsNotNull(postProcess);

            Document searchDoc = new Document(sl.DocumentType.Custom, "Custom Type");
            searchDoc.LongDescription = "Test <para>This will be a seperate paragraph</para> class";

            Assert.IsNotNull(searchDoc);

            PostProcessResults postProcessResults = postProcess.Process(searchDoc);

            Assert.IsNotNull(postProcessResults);

            Assert.AreEqual(1, postProcessResults.DocumentsProcessed);
            Assert.AreEqual(1, postProcessResults.GetCountValue(Constants.CountValueParaOpen));
            Assert.AreEqual(1, postProcessResults.GetCountValue(Constants.CountValueParaClose));
            Assert.AreEqual("Test <p>This will be a seperate paragraph</p> class", searchDoc.LongDescription);
        }

        [TestMethod]
        public void ValidatePostProcessResultsProcessSingleDocumentParaTagInvalidNotClosed()
        {
            List<Document> documents = new List<Document>();
            documents.Add(new Document(sl.DocumentType.Class, "Test", "Test", "ClassName", "T:Test.ClassName"));

            DocumentPostProcess postProcess = new DocumentPostProcess(documents);

            Assert.IsNotNull(postProcess);

            Document searchDoc = new Document(sl.DocumentType.Custom, "Custom Type");
            searchDoc.LongDescription = "Test <para>This will be a seperate paragraph class";

            Assert.IsNotNull(searchDoc);

            PostProcessResults postProcessResults = postProcess.Process(searchDoc);

            Assert.IsNotNull(postProcessResults);

            Assert.AreEqual(1, postProcessResults.DocumentsProcessed);
            Assert.AreEqual(1, postProcessResults.GetCountValue(Constants.CountValueParaOpen));
            Assert.AreEqual(0, postProcessResults.GetCountValue(Constants.CountValueParaClose));
        }

        [TestMethod]
        public void ValidatePostProcessResultsProcessSingleDocumentParaTagInvalidNotOpened()
        {
            List<Document> documents = new List<Document>();
            documents.Add(new Document(sl.DocumentType.Class, "Test", "Test", "ClassName", "T:Test.ClassName"));

            DocumentPostProcess postProcess = new DocumentPostProcess(documents);

            Assert.IsNotNull(postProcess);

            Document searchDoc = new Document(sl.DocumentType.Custom, "Custom Type");
            searchDoc.LongDescription = "Test </para>This will be a seperate paragraph class";

            Assert.IsNotNull(searchDoc);

            PostProcessResults postProcessResults = postProcess.Process(searchDoc);

            Assert.IsNotNull(postProcessResults);

            Assert.AreEqual(1, postProcessResults.DocumentsProcessed);
            Assert.AreEqual(0, postProcessResults.GetCountValue(Constants.CountValueParaOpen));
            Assert.AreEqual(1, postProcessResults.GetCountValue(Constants.CountValueParaClose));
        }

        [TestMethod]
        public void TestSplitString()
        {
            List<Document> documents = new List<Document>();
            documents.Add(new Document(sl.DocumentType.Class, "Test", "Test", "ClassName", "T:Test.ClassName"));
            documents.Add(new Document(sl.DocumentType.Class, "Test", "Test", "AdvancedSearchOptions", "T:Test.AdvancedSearchOptions"));

            DocumentPostProcess postProcess = new DocumentPostProcess(documents);

            Assert.IsNotNull(postProcess);

            Document searchDoc = new Document(sl.DocumentType.Custom, "Custom Type");
            searchDoc.ClassName = searchDoc.FullMemberName;
            searchDoc.Returns = "Dictionary&lt;string, AdvancedSearchOptions&gt;";
            //searchDoc.LongDescription = "Test <see cref=\"T:Test.ClassName\" /> class";

            Assert.IsNotNull(searchDoc);

            PostProcessResults postProcessResults = postProcess.Process(searchDoc);

            Assert.IsNotNull(postProcessResults);

            Assert.AreEqual(1, postProcessResults.DocumentsProcessed);
            Assert.AreEqual("Dictionary&lt;string, <a href=\"/docs/Document/Test/AdvancedSearchOptions/\">AdvancedSearchOptions</a>&gt;", searchDoc.Returns);
        }

        [TestMethod]
        public void TestSplitMethodValue()
        {
            List<Document> documents = new List<Document>();
            documents.Add(new Document(sl.DocumentType.Class, "Test", "Test", "ClassName", "T:Test.ClassName"));
            DocumentMethod documentMethod = new DocumentMethod(sl.DocumentType.Method, "Test", "Test", "Test", "Items()", "M:Blog.Plugin.Classes.BlogSitemapProvider.Items");
            documentMethod.Returns = "List&lt;ISitemapItem&gt;";
            documents[0].Methods.Add(documentMethod);

            DocumentPostProcess postProcess = new DocumentPostProcess(documents);
            postProcess.Process();

            Assert.IsNotNull(postProcess);

            Assert.AreEqual("Items()", documents[0].Methods[0].MethodName);
            Assert.AreEqual("List&lt;ISitemapItem&gt;", documents[0].Methods[0].Returns);
        }

        [TestMethod]
        public void TestSplitMethodValueRemoveNamespace()
        {
            List<Document> documents = new List<Document>();
            documents.Add(new Document(sl.DocumentType.Class, "Test", "Test", "ClassName", "T:Test.ClassName"));
            DocumentMethod documentMethod = new DocumentMethod(sl.DocumentType.Method, "Test", "Test", "Test", "Items()", "M:Blog.Plugin.Classes.BlogSitemapProvider.Items");
            documentMethod.Returns = "List&lt;Some.Namespace.Name.ISitemapItem&gt;";
            documents[0].Methods.Add(documentMethod);

            DocumentPostProcess postProcess = new DocumentPostProcess(documents);
            postProcess.Process();

            Assert.IsNotNull(postProcess);

            Assert.AreEqual("Items()", documents[0].Methods[0].MethodName);
            Assert.AreEqual("List&lt;ISitemapItem&gt;", documents[0].Methods[0].Returns);
        }

        [TestMethod]
        public void TestSplitConstructorValueRemoveNamespace()
        {
            List<Document> documents = new List<Document>();
            documents.Add(new Document(sl.DocumentType.Class, "Test", "Test", "ClassName", "T:Test.ClassName"));
            DocumentMethod documentMethod = new DocumentMethod(sl.DocumentType.Method, "Test", "Test", "Test", "#ctor(Some.NameSpace.Class)", "M:Blog.Plugin.Classes.BlogSitemapProvider.Items");
            documentMethod.Returns = "List&lt;Some.Namespace.Name.ISitemapItem&gt;";
            documents[0].Methods.Add(documentMethod);

            DocumentPostProcess postProcess = new DocumentPostProcess(documents);
            postProcess.Process();

            Assert.IsNotNull(postProcess);

            Assert.AreEqual("#ctor(Class)", documents[0].Methods[0].MethodName);
            Assert.AreEqual("List&lt;ISitemapItem&gt;", documents[0].Methods[0].Returns);
        }

        [TestMethod]
        public void TestSplitConstructorNameWithCurlyBracesRemoveNamespaceConvertBraces()
        {
            List<Document> documents = new List<Document>();
            documents.Add(new Document(sl.DocumentType.Class, "Test", "Test", "ClassName", "T:Middleware.Blog.BlogItem"));
            DocumentMethod documentConstructor = new DocumentMethod(sl.DocumentType.Constructor, "Test", "Test", "Test",
                "M:Middleware.Blog.BlogItem.#ctor(System.Collections.Generic.List{Middleware.Blog.BlogComment}@)",
                "M:Blog.Plugin.Classes.BlogSitemapProvider.Items");
            documents[0].Constructors.Add(documentConstructor);

            DocumentPostProcess postProcess = new DocumentPostProcess(documents);
            postProcess.Process();

            Assert.IsNotNull(postProcess);

            Assert.AreEqual("M:Middleware.Blog.BlogItem.#ctor(List&lt;BlogComment&gt;@)", documents[0].Constructors[0].MethodName);
        }
    }
}
