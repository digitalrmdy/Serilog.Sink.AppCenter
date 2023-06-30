using System.Collections.Generic;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build
{
	const string NUGET_SOURCE_NAME = "nuget.org";

	[Secret, Parameter]
	readonly string NuGetApiKey;

	Target PublishToNuGet => _ => _
		.DependsOn(Pack)
		.Requires(() => !string.IsNullOrEmpty(NuGetApiKey))
		.Executes(() =>
		{
			IEnumerable<AbsolutePath> artifactPackages = ArtifactsDirectory.GlobFiles("*.nupkg");

			DotNetNuGetPush(s => s
				.SetSource(NUGET_SOURCE_NAME)
				.SetApiKey(NuGetApiKey)
				.EnableSkipDuplicate()
				.CombineWith(artifactPackages, (_, v) => _
					.SetTargetPath(v)));
		});
}