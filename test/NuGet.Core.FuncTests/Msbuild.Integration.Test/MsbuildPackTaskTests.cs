// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Xml.Linq;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Xunit;

namespace Msbuild.Integration.Test
{
    [Collection("Msbuild Integration Tests")]
    public class MsbuildPackTaskTests
    {
        private MsbuildIntegrationTestFixture _msbuildFixture;

        public MsbuildPackTaskTests(MsbuildIntegrationTestFixture fixture)
        {
            _msbuildFixture = fixture;
        }

        [PlatformFact(Platform.Windows)]
        public void MsbuildPack_RequireLicenseAcceptanceNotEmittedWhenUnspecified()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot)
                {
                    Projects =
                    {
                        SimpleTestProjectContext.CreateNETCoreWithSDK("a", pathContext.SolutionRoot, "net461")
                    }
                };

                solution.Create(pathContext.SolutionRoot);

                var result = _msbuildFixture.RunMsBuild(pathContext.WorkingDirectory, $"/t:Pack {pathContext.SolutionRoot} /p:PackageOutputPath={pathContext.WorkingDirectory}", ignoreExitCode: true);
                Assert.True(result.ExitCode == 0, result.AllOutput);

                using (var package = new PackageArchiveReader(Path.Combine(pathContext.WorkingDirectory, "a.0.0.0.nupkg")))
                {
                    var document = XDocument.Load(package.GetNuspec());
                    var ns = document.Root.GetDefaultNamespace();

                    Assert.Null(document.Root.Element(ns + "metadata").Element(ns + "requireLicenseAcceptance"));
                }
            }
        }
    }
}
