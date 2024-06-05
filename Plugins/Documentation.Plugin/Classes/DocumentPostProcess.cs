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
 *  Product:  Documentation Plugin
 *  
 *  File: DocumentPostProcess.cs
 *
 *  Purpose:  Post process documentation once loaded
 *
 *  Date        Name                Reason
 *  11/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Shared.Docs;

using SharedPluginFeatures;

namespace DocumentationPlugin.Classes
{
    /// <summary>
    /// Documentation cross reference and post process
    /// </summary>
    public sealed class DocumentPostProcess
    {
        #region Private Members

        private static readonly string[] BuiltIntypesAndReferences = { "bool", "boolean", "byte", "sbyte", "char", "decimal",
            "double", "float", "int", "uint", "long", "ulong", "short", "ushort", "object", "string", "dynamic",
            "enum", "struct", "void" };
        private static readonly string[] ValidTags = { "c", "code", "/c", "/code", "para", "/para", "see", "seealso", "example", "exception", "include" };
        private readonly List<Document> _documents;
        private PostProcessResults _processResult;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="documents">List of document files to be post processed</param>
        public DocumentPostProcess(in List<Document> documents)
        {
            _documents = documents ?? throw new ArgumentNullException(nameof(documents));
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Initiates the post process of all the <see cref="Shared.Docs.Document" />s
        /// </summary>
        /// <example>
        /// The following example demonstrates creating a new <see cref="DocumentPostProcess"/> class and 
        /// passing in a list of <see cref="Shared.Docs.Document" /> items and post processing them.
        /// <code>
        /// DocumentPostProcess postProcess = new DocumentPostProcess(GetDocuments());
        /// postProcess.Process();
        /// </code>
        /// </example>
        /// <returns><see cref="PostProcessResults" /></returns>
        public PostProcessResults Process()
        {
            _processResult = new PostProcessResults();

            foreach (Document document in _documents)
            {
                ProcessDocument(document);
            }

            return _processResult;
        }

        /// <summary>
        /// Initiates the post process of a single <see cref="Shared.Docs.Document" />
        /// </summary>
        /// <example>
        /// The following example demonstrates creating a new <see cref="DocumentPostProcess"/> class and 
        /// passing in a list of <see cref="Shared.Docs.Document" /> items and post processing them.
        /// <code>
        /// DocumentPostProcess postProcess = new DocumentPostProcess(GetDocuments());
        /// postProcess.Process();
        /// </code>
        /// </example>
        /// <param name="document"><see cref="Shared.Docs.Document"/> instance to be processed.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when document is null</exception>
        /// <returns><see cref="PostProcessResults" /></returns>
        public PostProcessResults Process(in Document document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            _processResult = new PostProcessResults();

            ProcessDocument(document);

            return _processResult;
        }

        #endregion Public Methods

        #region Private Methods

        private void ProcessDocument(in Document document)
        {
            _processResult.DocumentsProcessed++;

            document.LongDescription = FindReplaceableTags(document, document, document.LongDescription, false, true, false);
            document.ShortDescription = FindReplaceableTags(document, document, document.ShortDescription, false, true, false);
            document.Value = SplitAndFindReplaceableTags(document, document, document.Value, true, false);
            document.Title = FindReplaceableTags(document, document, document.Title, false, true, false);
            document.Summary = FindReplaceableTags(document, document, document.Summary, false, true, false);
            document.Returns = SplitAndFindReplaceableTags(document, document, document.Returns, true, true);
            document.Remarks = FindReplaceableTags(document, document, document.Remarks, false, true, false);
            document.ExampleUseage = FindReplaceableTags(document, document, document.ExampleUseage, false, true, false);

            foreach (DocumentMethod item in document.Constructors)
            {

                item.ExampleUseage = FindReplaceableTags(document, item, item.ExampleUseage, false, true, false);
                item.LongDescription = FindReplaceableTags(document, item, item.LongDescription, false, true, false);
                item.ShortDescription = FindReplaceableTags(document, item, item.ShortDescription, false, true, false);
                item.MethodName = SplitAndFindReplaceableTags(document, item, item.MethodName, false, true);

                ProcessExamples(document, item);
                ProcessExceptions(document, item);
                ProcessParameters(document, item);
            }

            foreach (DocumentMethod item in document.Methods)
            {
                item.ExampleUseage = FindReplaceableTags(document, item, item.ExampleUseage, false, true, false);
                item.LongDescription = FindReplaceableTags(document, item, item.LongDescription, false, true, false);
                item.ShortDescription = FindReplaceableTags(document, item, item.ShortDescription, false, true, false);
                item.Returns = SplitAndFindReplaceableTags(document, item, item.Returns, true, true);
                item.MethodName = SplitAndFindReplaceableTags(document, item, item.MethodName, false, true);
                ProcessExamples(document, item);
                ProcessExceptions(document, item);
                ProcessParameters(document, item);
            }

            foreach (DocumentProperty item in document.Properties)
            {
                item.Summary = FindReplaceableTags(document, item, item.Summary, false, true, false);
                item.Value = SplitAndFindReplaceableTags(document, item, item.Value, true, true);
                item.LongDescription = FindReplaceableTags(document, item, item.LongDescription, false, true, false);
                item.ShortDescription = FindReplaceableTags(document, item, item.ShortDescription, false, true, false);
                item.PropertyName = SplitAndFindReplaceableTags(document, item, item.PropertyName, true, false);
                ProcessExamples(document, item);
                ProcessExceptions(document, item);
            }

            foreach (DocumentField item in document.Fields)
            {
                item.Summary = FindReplaceableTags(document, item, item.Summary, false, true, false);
                item.Value = SplitAndFindReplaceableTags(document, item, item.Value, true, true);
                item.LongDescription = FindReplaceableTags(document, item, item.LongDescription, false, true, false);
                item.ShortDescription = FindReplaceableTags(document, item, item.ShortDescription, false, true, false);
                item.FieldName = SplitAndFindReplaceableTags(document, item, item.FieldName, true, true);
                ProcessExamples(document, item);
                ProcessExceptions(document, item);
            }

            ProcessExceptions(document, document);
            ProcessExamples(document, document);

            DocumentData data = document.Tag as DocumentData;

            if (data != null && data.Contains.Count > 0)
            {
                Dictionary<string, string> contains = new();

                foreach (KeyValuePair<String, String> item in data.Contains)
                {
                    contains.Add(item.Key, FindReplaceableTags(document, document, item.Value, false, false, true));
                }

                data.Contains.Clear();

                foreach (KeyValuePair<string, string> item in contains)
                {
                    data.Contains.Add(item.Key, item.Value);
                }
            }
        }

        private void ProcessParameters(in Document document, in DocumentMethod method)
        {
            foreach (DocumentMethodParameter item in method.Parameters)
            {
                item.Summary = FindReplaceableTags(document, item, item.Summary, false, true, false);
                item.Value = SplitAndFindReplaceableTags(document, item, item.Value, true, true);
                item.LongDescription = FindReplaceableTags(document, item, item.LongDescription, false, true, false);
                item.ShortDescription = FindReplaceableTags(document, item, item.ShortDescription, false, true, false);
                item.MethodName = SplitAndFindReplaceableTags(document, item, item.MethodName, true, true);
                ProcessExamples(document, item);
                ProcessExceptions(document, item);
            }
        }

        private void ProcessExceptions(in Document document, in BaseDocument baseDocument)
        {
            foreach (DocumentException exception in baseDocument.Exception)
            {
                exception.ExceptionName = SplitAndFindReplaceableTags(document, baseDocument, exception.ExceptionName, true, true);
                exception.Summary = FindReplaceableTags(document, baseDocument, exception.Summary, true, true, false);
            }
        }

        private void ProcessExamples(in Document document, in BaseDocument baseDocument)
        {
            foreach (DocumentExample example in baseDocument.Examples)
            {
                example.Text = FindReplaceableTags(document, baseDocument, example.Text, false, true, false);
            }
        }

        private string SplitAndFindReplaceableTags(in Document document, in BaseDocument linkDocument,
            in string text, in bool createHyperlinks, in bool removeNamespace)
        {
            if (String.IsNullOrEmpty(text))
                return text;

            StringBuilder Result = new(text.Length);

            StringBuilder searchWord = new(text.Length);
            bool ignoreWord = false;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (c == '{')
                {
                    if (searchWord.Length > 0)
                    {
                        Result.Append(FindReplaceableTags(document, linkDocument, searchWord.ToString(), true, createHyperlinks, removeNamespace));
                        searchWord.Clear();
                    }

                    Result.Append("&lt;");
                    ignoreWord = false;
                    continue;
                }

                if (c == '}')
                {
                    if (searchWord.Length > 0)
                    {
                        Result.Append(FindReplaceableTags(document, linkDocument, searchWord.ToString(), true, createHyperlinks, removeNamespace));
                        searchWord.Clear();
                    }

                    Result.Append("&gt;");
                    ignoreWord = false;
                    continue;
                }

                if (c == '(')
                {
                    if (searchWord.Length > 0)
                    {
                        Result.Append(FindReplaceableTags(document, linkDocument, searchWord.ToString(), true, createHyperlinks, removeNamespace));
                        searchWord.Clear();
                    }

                    Result.Append(c);
                    ignoreWord = false;
                    continue;
                }

                if (c == ')')
                {
                    if (searchWord.Length > 0)
                    {
                        Result.Append(FindReplaceableTags(document, linkDocument, searchWord.ToString(), true, createHyperlinks, removeNamespace));
                        searchWord.Clear();
                    }

                    Result.Append(c);
                    ignoreWord = false;
                    continue;
                }

                if (ignoreWord && c == ';')
                {
                    Result.Append(c);
                    ignoreWord = false;
                    continue;
                }

                if (ignoreWord)
                {
                    Result.Append(c);
                    continue;
                }

                if (!ignoreWord && c == '&')
                {
                    Result.Append(FindReplaceableTags(document, linkDocument, searchWord.ToString(), true, createHyperlinks, removeNamespace));
                    searchWord.Clear();
                    Result.Append(c);
                    ignoreWord = true;
                    continue;
                }

                switch (c)
                {
                    case ',':
                    case ' ':
                        if (searchWord.Length > 0)
                        {
                            Result.Append(FindReplaceableTags(document, linkDocument, searchWord.ToString(), true, createHyperlinks, removeNamespace));
                            searchWord.Clear();
                        }

                        Result.Append(c);
                        continue;
                }

                searchWord.Append(c);
            }

            if (searchWord.Length > 0)
            {
                Result.Append(FindReplaceableTags(document, linkDocument, searchWord.ToString(), true, createHyperlinks, removeNamespace));
            }

            return FindReplaceableTags(document, linkDocument, Result.ToString(), true, createHyperlinks, removeNamespace);
        }

        private string FindReplaceableTags(in Document document, BaseDocument linkDocument, string text,
            bool attemptToFindClass, in bool createHyperlinks, in bool removeNamespace)
        {
            if (String.IsNullOrEmpty(text))
                return text;

            if (linkDocument == null)
            {
                linkDocument = document;
            }

            StringBuilder builder = new(text.Length + 2048);
            StringBuilder currentTag = null;

            bool inTag = false;
            int firstSpace = -1;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (!inTag && c == '<')
                {
                    inTag = true;
                    currentTag = new StringBuilder(1024);
                    currentTag.Append(c);
                    continue;
                }

                if (!inTag)
                {
                    builder.Append(c);
                    continue;
                }

                if (inTag && c == '>')
                {
                    inTag = false;
                    currentTag.Append(c);
                    builder.Append(ProcessTag(document, linkDocument, currentTag, firstSpace, createHyperlinks));
                    currentTag = null;
                    firstSpace = -1;
                    continue;
                }

                if (inTag && firstSpace == -1 && c == ' ')
                {
                    firstSpace = currentTag.Length;
                }

                if (inTag)
                {
                    currentTag.Append(c);
                }
            }

            if (currentTag != null)
                builder.Append(currentTag);

            if (attemptToFindClass && builder.ToString().Equals(text))
            {
                builder = AttemptFindClass(text, createHyperlinks, removeNamespace);
            }

            return builder.ToString();
        }

        private StringBuilder AttemptFindClass(string text, in bool createHyperlinks, in bool removeNamespace)
        {
            foreach (string value in BuiltIntypesAndReferences)
            {
                if (value.Equals(text, StringComparison.InvariantCultureIgnoreCase))
                {
                    return new StringBuilder(value);
                }
            }

            Document memberDoc = _documents.Find(d => d.DocumentType == Shared.DocumentType.Class && d.FullMemberName.EndsWith(text));

            if (memberDoc != null)
            {
                if (createHyperlinks)
                {
                    return new StringBuilder($"<a href=\"/docs/Document/{HtmlHelper.RouteFriendlyName(memberDoc.AssemblyName)}" +
                        $"/{HtmlHelper.RouteFriendlyName(memberDoc.ClassName)}/\">{memberDoc.ClassName}</a>");
                }
                else
                {
                    return new StringBuilder(memberDoc.ClassName);
                }
            }
            else
            {
                if (removeNamespace && StringCouldBeNamespace(text))
                {
                    string[] parts = text.Split('.');
                    return new StringBuilder(parts[parts.Length - 1]);
                }
                else
                {
                    return new StringBuilder(text);
                }
            }
        }

        private static bool StringCouldBeNamespace(in string text)
        {
            bool sepFound = false;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                switch (c)
                {
                    case '.':
                        if (sepFound)
                            continue;

                        sepFound = i > 0 && i < text.Length;
                        continue;
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'i':
                    case 'j':
                    case 'k':
                    case 'l':
                    case 'm':
                    case 'n':
                    case 'o':
                    case 'p':
                    case 'q':
                    case 'r':
                    case 's':
                    case 't':
                    case 'u':
                    case 'v':
                    case 'w':
                    case 'x':
                    case 'y':
                    case 'z':
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                    case 'G':
                    case 'H':
                    case 'I':
                    case 'J':
                    case 'K':
                    case 'L':
                    case 'M':
                    case 'N':
                    case 'O':
                    case 'P':
                    case 'Q':
                    case 'R':
                    case 'S':
                    case 'T':
                    case 'U':
                    case 'V':
                    case 'W':
                    case 'X':
                    case 'Y':
                    case 'Z':
                        continue;

                    default:
                        return false;
                }
            }

            return sepFound;
        }

        private string ProcessTag(in Document document, in BaseDocument linkDocument, in StringBuilder builder,
            in int firstSpace, in bool createHyperlinks)
        {
            string tag = firstSpace == -1 ?
                builder.ToString(1, builder.Length - 2) :
                builder.ToString(1, firstSpace - 1);

            if (!ValidTags.Contains(tag))
                return builder.ToString();

            bool containsCRef = firstSpace > -1 &&
                firstSpace + 8 < builder.Length &&
                builder.ToString(firstSpace + 1, 6).Equals("cref=\"");
            string cRefValue = null;

            if (containsCRef)
            {
                StringBuilder cRefBuilder = new(builder.Length);

                for (int i = firstSpace + 7; i < builder.Length; i++)
                {
                    if (builder[i] == '"')
                        break;

                    cRefBuilder.Append(builder[i]);
                }

                cRefValue = cRefBuilder.ToString();
            }

            switch (tag)
            {
                case "c":
                case "code":
                    _processResult.IncrementCount(Constants.CountValueCodeOpen);
                    return "<pre style=\"font-family:Consolas;font-size:13px;color:black;background:white;\">";

                case "/c":
                case "/code":
                    _processResult.IncrementCount(Constants.CountValueCodeClose);
                    return "</pre>";

                case "example":
                case "exception":
                case "include":
                    break;

                case "para":
                    _processResult.IncrementCount(Constants.CountValueParaOpen);
                    return "<p>";

                case "/para":
                    _processResult.IncrementCount(Constants.CountValueParaClose);
                    return "</p>";

                case "seealso":
                    if (containsCRef && !String.IsNullOrEmpty(cRefValue))
                    {
                        _processResult.IncrementCount(Constants.CountValueSeeAlso);
                        return ProcessLinkedTag(document, linkDocument, builder, cRefValue, Constants.CountValueSeeAlsoNotFound, createHyperlinks);
                    }

                    break;

                case "see":
                    if (containsCRef && !String.IsNullOrEmpty(cRefValue))
                    {
                        _processResult.IncrementCount(Constants.CountValueSee);
                        return ProcessLinkedTag(document, linkDocument, builder, cRefValue, Constants.CountValueSeeNotFound, createHyperlinks);
                    }

                    break;
            }

            // if we get here we can't process the tag, it is probably invalid, replace the 
            // start/end brackets so that it appears in a page
            _processResult.IncrementCount(Constants.CountValueSeeNotFound);
            builder.Remove(0, 1);
            builder.Remove(builder.Length - 1, 1);
            builder.Insert(0, "&lt;");
            builder.Append("&gt;");

            return builder.ToString();
        }

        private string ProcessLinkedTag(in Document document, in BaseDocument linkDocument, in StringBuilder builder,
            string cRefValue, in string countName, in bool createHyperlinks)
        {
            Document xRefDoc = _documents.Find(d => d.FullMemberName == cRefValue);

            if (xRefDoc != null && !String.IsNullOrEmpty(xRefDoc.ClassName))
            {
                if (xRefDoc.FullMemberName.Equals(document.FullMemberName))
                {
                    return xRefDoc.ClassName;
                }

                string link = $"<a href=\"/docs/Document/{HtmlHelper.RouteFriendlyName(xRefDoc.AssemblyName)}/" +
                    $"{HtmlHelper.RouteFriendlyName(xRefDoc.ClassName)}/\">{xRefDoc.ClassName}</a>";

                // add a link to the see also
                if (!String.IsNullOrEmpty(document.ClassName) && !xRefDoc.SeeAlso.ContainsKey(document.ClassName))
                {
                    xRefDoc.SeeAlso.Add(document.ClassName,
                        $"<a href=\"/docs/Document/{HtmlHelper.RouteFriendlyName(document.AssemblyName)}/" +
                        $"{HtmlHelper.RouteFriendlyName(document.ClassName)}/\">{document.ClassName}</a>");
                    document.SeeAlso.Add(xRefDoc.ClassName,
                        $"<a href=\"/docs/Document/{HtmlHelper.RouteFriendlyName(xRefDoc.AssemblyName)}/" +
                        $"{HtmlHelper.RouteFriendlyName(xRefDoc.ClassName)}/\">{xRefDoc.ClassName}</a>");
                }

                if (!linkDocument.SeeAlso.ContainsKey(xRefDoc.ClassName))
                {
                    linkDocument.SeeAlso.Add(xRefDoc.ClassName, link);
                }

                //"docs/Document/{documentName}/"
                //"docs/Document/{className}/Type/{classType}/{typeName}"
                switch (cRefValue[0])
                {
                    case 'T':
                        if (createHyperlinks)
                        {
                            return link;
                        }
                        else
                        {
                            return xRefDoc.ClassName;
                        }
                    default:
                        break;
                }
            }

            _processResult.IncrementCount(countName);
            builder.Remove(0, 1);
            builder.Remove(builder.Length - 1, 1);
            builder.Insert(0, "&lt;");
            builder.Append("&gt;");

            return builder.ToString();
        }

        #endregion Private Methods
    }
}
