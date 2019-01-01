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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: PluginFeatureProvider.cs
 *
 *  Purpose:  Ensures loaded modules can be found, if loaded dynamically using plugin manager
 *  Original unmodified source: https://github.com/dotnet/core-setup/issues/2981#issuecomment-322572374
 *
 *  Date        Name                Reason
 *  30/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;

namespace AspNetCore.PluginManager.Classes
{
    public class PluginFeatureProvider : IApplicationFeatureProvider<MetadataReferenceFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, MetadataReferenceFeature feature)
        {
            var libraryPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var assemblyPart in parts.OfType<AssemblyPart>())
            {
                var dependencyContext = DependencyContext.Load(assemblyPart.Assembly);
                if (dependencyContext != null)
                {
                    foreach (var library in dependencyContext.CompileLibraries)
                    {
                        if (string.Equals("reference", library.Type, StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (var libraryAssembly in library.Assemblies)
                            {
                                libraryPaths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, libraryAssembly));
                            }
                        }
                        else
                        {

                            try
                            {
                                foreach (var path in library.ResolveReferencePaths())
                                {
                                    libraryPaths.Add(path);
                                }
                            }
                            catch (InvalidOperationException)
                            {
                                if (PluginManagerService.GetPluginManager().PluginLoaded(library.Name + ".dll", 
                                    out int version, out string module))
                                {
                                    libraryPaths.Add(module);
                                }
                                else
                                    throw;
                            }
                        }
                    }
                }
                else
                {
                    libraryPaths.Add(assemblyPart.Assembly.Location);
                }
            }

            foreach (var path in libraryPaths)
            {
                feature.MetadataReferences.Add(CreateMetadataReference(path));
            }
        }

        private static MetadataReference CreateMetadataReference(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                var moduleMetadata = ModuleMetadata.CreateFromStream(stream, PEStreamOptions.PrefetchMetadata);
                var assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);

                return assemblyMetadata.GetReference(filePath: path);
            }
        }
    }
}