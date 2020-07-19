// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.Test.Apex.VisualStudio.Solution;
using Newtonsoft.Json;
using NuGet.Test.Utility;
using NuGet.Tests.Apex.Apex;
using NuGet.VisualStudio.Contracts;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.Tests.Apex.NuGetEndToEndTests
{
    public class NuGetProjectServiceTestCase : SharedVisualStudioHostTestClass, IClassFixture<VisualStudioHostFixtureFactory>
    {
        public NuGetProjectServiceTestCase(VisualStudioHostFixtureFactory visualStudioHostFixtureFactory, ITestOutputHelper output)
            : base(visualStudioHostFixtureFactory, output)
        {
        }

        [StaFact]
        public async Task GetInstalledPackagesAsync_ProjectWithOnePackageInstalled_Succeds()
        {
            using (var testContext = new SimpleTestPathContext())
            {
                // Arrange
                EnsureVisualStudioHost();

                var solutionService = VisualStudio.Get<SolutionService>();
                var nugetService = GetNuGetTestService();

                solutionService.CreateEmptySolution(solutionName: "projectServiceTest", path: testContext.SolutionRoot);
                var project = solutionService.CreateProject(ProjectLanguage.CSharp, ProjectTemplate.NetCoreClassLib);
                var projectGuid = project.ProjectGuid;

                var nugetProjectService = VisualStudio.Get<NuGetProjectServiceService>();

                var json = await nugetProjectService.GetInstalledPackagesAsync(projectGuid);
                var result = JsonConvert.DeserializeObject<InstalledPackagesResult>(json);
            }
        }
    }
}
