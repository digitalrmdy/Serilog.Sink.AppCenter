// Environment variables
var target = Argument("target", EnvironmentVariable("BUILD_TARGET") ?? "Default");
var buildNumber = EnvironmentVariable("BITRISE_BUILD_NUMBER") ?? "0";
var nugetApiKey = EnvironmentVariable("NUGET_API_KEY");
var isStableVersion = !string.IsNullOrEmpty(EnvironmentVariable("NUGET_STABLE"));
string versionSuffix = null;

// Targets

Setup(setupContext => 
{
    if (buildNumber != null && !isStableVersion)
    {
        versionSuffix = "ci-" + buildNumber;
    }
});

Task("Test")
    .Does(() => DotNetCoreTest("."));

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