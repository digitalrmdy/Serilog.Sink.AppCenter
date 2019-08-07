// Addins & tools

// Variables
// Names and keys

// Environment variables
var target = Argument("target", EnvironmentVariable("BUILD_TARGET") ?? "Default");

// Targets

Task("Test")
    .Does(() => DotNetCoreTest("."));

Task("Pack")
    .Does(() => DotNetCoreBuild(".", new DotNetCoreBuildSettings
    {
        Configuration = "Release",
        VersionSuffix = "ci-1"
    }));

Task("Default")
    .IsDependentOn("Test")
    .IsDependentOn("Pack");

RunTarget(target);