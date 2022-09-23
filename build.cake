// Addins
#tool MSBuild.SonarQube.Runner.Tool&version=4.6.0
#addin nuget:?package=Cake.Coverlet&version=2.5.4

// Environment variables
var target = Argument("target", EnvironmentVariable("BUILD_TARGET") ?? "Default");
var buildNumber = EnvironmentVariable("BUILD_NUMBER") ?? "0";
var nugetApiKey = EnvironmentVariable("NUGET_API_KEY");
var isStableVersion = !string.IsNullOrEmpty(EnvironmentVariable("NUGET_STABLE"));
string versionSuffix = null;

// Targets

Setup(setupContext => 
{
    if (buildNumber != null && !isStableVersion)
    {
        versionSuffix = "build." + buildNumber;
    }

});

Task("Test")
    .Does(() => DotNetCoreTest(".", new DotNetCoreTestSettings
    {
        Configuration = "Release",
    }, new CoverletSettings
    {
        CollectCoverage = true,
        CoverletOutputFormat = CoverletOutputFormat.opencover,
        CoverletOutputDirectory = coveragePath,
        CoverletOutputName = $"results-{DateTime.UtcNow:dd-MM-yyyy-HH-mm-ss-FFF}"
    }));

Task("Pack")
    .Does(() => 
    {
        var buildSettings = new DotNetCoreBuildSettings
        {
            Configuration = "Release",
            VersionSuffix = versionSuffix
        };

        Information("NuGet version suffix: " + buildSettings.VersionSuffix);
        DotNetCoreBuild(".", buildSettings);
    });

Task("Default")
    .IsDependentOn("Test")
    .IsDependentOn("Pack");

Task("Publish")
    .IsDependentOn("Default")
    .Does(() => 
    {
        var pkgPath = GetFiles($"./**/bin/Release/*{versionSuffix ?? string.Empty}.nupkg").First();
        
        if (string.IsNullOrEmpty(nugetApiKey))
        {
            Error("NuGet API key not set");
            return;
        }

        NuGetPush(pkgPath, new NuGetPushSettings
        {
            ApiKey = nugetApiKey,
            Source = "https://api.nuget.org/v3/index.json"
        });
    });

RunTarget(target);
