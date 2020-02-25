// Addins
#tool MSBuild.SonarQube.Runner.Tool&version=4.6.0
#addin Cake.Sonar&version=1.1.22
#addin Cake.Coverlet&version=2.3.4

// Environment variables
var target = Argument("target", EnvironmentVariable("BUILD_TARGET") ?? "Default");
var buildNumber = EnvironmentVariable("BITRISE_BUILD_NUMBER") ?? "0";
var nugetApiKey = EnvironmentVariable("NUGET_API_KEY");
var isStableVersion = !string.IsNullOrEmpty(EnvironmentVariable("NUGET_STABLE"));
var sonarLogin = EnvironmentVariable("SONAR_LOGIN");
var sonarOrganization = EnvironmentVariable("SONAR_ORGANIZATION");
var sonarKey = EnvironmentVariable("SONAR_KEY");
var sonarHost = EnvironmentVariable("SONAR_HOST");
string versionSuffix = null;
var coveragePath = ".coverage/";

// Targets

Setup(setupContext => 
{
    if (buildNumber != null && !isStableVersion)
    {
        versionSuffix = "build." + buildNumber;
    }

    CreateDirectory(coveragePath);

    if (!string.IsNullOrEmpty(sonarLogin))
    {
        SonarBegin(new SonarBeginSettings
        {
            Login = sonarLogin,
            Organization = sonarOrganization,
            Key = sonarKey,
            Url = sonarHost,
            OpenCoverReportsPath = coveragePath + "*.xml"
        });
    }
});

Teardown(context =>
{
    if (!string.IsNullOrEmpty(sonarLogin))
    {
        SonarEnd(new SonarEndSettings
        {
            Login = sonarLogin
        });
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
