// Addins & tools

// Variables
// Names and keys

// Environment variables
var target = Argument("target", EnvironmentVariable("BUILD_TARGET") ?? "Default");
var buildNumber = EnvironmentVariable("BITRISE_BUILD_NUMBER");
var nugetApiKey = EnvironmentVariable("NUGET_API_KEY");
var isStableVersion = !string.IsNullOrEmpty("NUGET_STABLE");

// Targets

Task("Test")
    .Does(() => DotNetCoreTest("."));

Task("Pack")
    .Does(() => 
    {
        var buildSettings = new DotNetCoreBuildSettings
        {
            Configuration = "Release",
        };
        if (buildNumber != null && !isStableVersion)
        {
            buildSettings.VersionSuffix = "ci-" + buildNumber;
        }
        DotNetCoreBuild(".", buildSettings);
    });

Task("Default")
    .IsDependentOn("Test")
    .IsDependentOn("Pack");

Task("Publish")
    .IsDependentOn("Default")
    .Does(() => 
    {
        var pkgPath = GetFiles("./**/bin/Release/*.nupkg").First();
        
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