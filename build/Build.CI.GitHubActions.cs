using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;

[GitHubActions(
	"pr",
	GitHubActionsImage.UbuntuLatest,
	AutoGenerate = false,
	CacheKeyFiles = new string[0],
	EnableGitHubToken = false,
	FetchDepth = 0, // Only a single commit is fetched by default, for the ref/SHA that triggered the workflow. Make sure to fetch whole git history, in order to get GitVersion to work.
	InvokedTargets = new[] { nameof(Pack) },
	OnPullRequestBranches = new[] { "develop" },
	PublishArtifacts = true)]
[GitHubActions(
	"push",
	GitHubActionsImage.UbuntuLatest,
	AutoGenerate = false,
	CacheKeyFiles = new string[0],
	EnableGitHubToken = false,
	FetchDepth = 0, // Only a single commit is fetched by default, for the ref/SHA that triggered the workflow. Make sure to fetch whole git history, in order to get GitVersion to work.
	InvokedTargets = new[] { nameof(Pack) },
	OnPushBranches = new[] { "develop" },
	PublishArtifacts = true)]
[GitHubActions(
	"publish",
	GitHubActionsImage.UbuntuLatest,
	AutoGenerate = false,
	CacheKeyFiles = new string[0],
	EnableGitHubToken = false,
	FetchDepth = 0, // Only a single commit is fetched by default, for the ref/SHA that triggered the workflow. Make sure to fetch whole git history, in order to get GitVersion to work.
	ImportSecrets = new []{nameof(NuGetApiKey)},
	InvokedTargets = new[] { nameof(Publish) },
	OnPushTags = new[] { "*.*.*" },
	PublishArtifacts = true)]
partial class Build
{
	[CI] readonly GitHubActions GitHubActions;
}