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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
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

#pragma warning disable CS0618

namespace AspNetCore.PluginManager
{
#if NET_CORE_2_2 || NET_CORE_2_1 || NET_CORE_2_0 || NET461

    internal class PluginFeatureProvider : IApplicationFeatureProvider<MetadataReferenceFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, MetadataReferenceFeature feature)
        {
            HashSet<String> libraryPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (AssemblyPart assemblyPart in parts.OfType<AssemblyPart>())
            {
                DependencyContext dependencyContext = DependencyContext.Load(assemblyPart.Assembly);
                if (dependencyContext != null)
                {
                    foreach (CompilationLibrary library in dependencyContext.CompileLibraries)
                    {
                        if (string.Equals("reference", library.Type, StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (string libraryAssembly in library.Assemblies)
                            {
                                libraryPaths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, libraryAssembly));
                            }
                        }
                        else
                        {

                            try
                            {
                                foreach (string path in library.ResolveReferencePaths())
                                {
                                    libraryPaths.Add(path);
                                }
                            }
                            catch (InvalidOperationException err)
                            {
                                string libName = library.Name + ".dll";

                                if (String.IsNullOrEmpty(libraryPaths.Where(lp => lp.EndsWith(libName, StringComparison.InvariantCultureIgnoreCase))
                                    .FirstOrDefault()))
                                {
                                    if (PluginManagerService.GetPluginManager().PluginLoaded(libName,
                                        out int _, out string module))
                                    {
                                        libraryPaths.Add(module);
                                    }
                                    else
                                    {
                                        PluginManagerService.GetLogger().AddToLog(LogLevel.Critical, err, libName);
                                        throw;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    libraryPaths.Add(assemblyPart.Assembly.Location);
                }
            }

            foreach (string path in libraryPaths)
            {
                feature.MetadataReferences.Add(CreateMetadataReference(path));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Returned to Net Core, so not fussed")]
        private static MetadataReference CreateMetadataReference(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            {
                ModuleMetadata moduleMetadata = ModuleMetadata.CreateFromStream(stream, PEStreamOptions.PrefetchMetadata);
                AssemblyMetadata assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);

                return assemblyMetadata.GetReference(filePath: path);
            }
        }
    }
#endif
}

#pragma warning restore CS0618
