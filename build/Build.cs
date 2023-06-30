using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build : NukeBuild
{
	/// Support plugins are available for:
	///   - JetBrains ReSharper        https://nuke.build/resharper
	///   - JetBrains Rider            https://nuke.build/rider
	///   - Microsoft VisualStudio     https://nuke.build/visualstudio
	///   - Microsoft VSCode           https://nuke.build/vscode
	public static int Main() => Execute<Build>(x => x.Pack);

	[Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
	readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

	[Solution] readonly Solution Solution;

	[GitRepository] readonly GitRepository GitRepository;

	[GitVersion(NoFetch = true)] readonly GitVersion GitVersion;

	AbsolutePath SourceDirectory => RootDirectory / "source";
	AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

	Target Clean => _ => _
		.Before(Restore)
		.Executes(() =>
		{
			SourceDirectory
				.GlobDirectories("**/bin", "**/obj")
				.ForEach(ap => ap.DeleteDirectory());
			ArtifactsDirectory.CreateOrCleanDirectory();
		});

	Target Restore => _ => _
		.Executes(() =>
		{
			DotNetRestore(s => s
				.SetProjectFile(Solution)
				.EnableDeterministic()
				.EnableContinuousIntegrationBuild());
		});

	Target Compile => _ => _
		.DependsOn(Restore)
		.Executes(() =>
		{
			DotNetBuild(s => s
				.EnableNoRestore()
				.SetProjectFile(Solution)
				.SetConfiguration(Configuration)
				.SetDeterministic(IsServerBuild)
				.SetContinuousIntegrationBuild(IsServerBuild)
				.SetVersion(GitVersion.FullSemVer)
				.SetAssemblyVersion(GitVersion.AssemblySemVer)
				.SetFileVersion(GitVersion.AssemblySemFileVer)
				.SetInformationalVersion(GitVersion.InformationalVersion));
		});

	Target RunTests => _ => _
		.DependsOn(Compile)
		.Executes(() =>
		{
			DotNetTest(s => s
				.SetProjectFile(Solution)
				.SetConfiguration(Configuration)
				.EnableNoRestore()
				.EnableNoBuild());
		});

	Target Pack => _ => _
		.DependsOn(Clean, RunTests)
		.Produces(ArtifactsDirectory / "*.nupkg")
		.Executes(() =>
		{
			DotNetPack(s => s
				.EnableNoRestore()
				.EnableNoBuild()
				.SetProject(Solution)
				.SetConfiguration(Configuration)
				.SetVersion(GitVersion.NuGetVersion)
				.SetProperty("RepositoryBranch", GitRepository.Branch)
				.SetProperty("RepositoryCommit", GitRepository.Commit)
				.SetOutputDirectory(ArtifactsDirectory));
		});

	Target Publish => _ => _
		.DependsOn(Pack, PublishToNuGet);
}