using UnityEditor;


class JenkinsPerformBuild
{
	static void PerformBuild()
	{
		var scenes = new[] {"Assets/Scenes/Main.unity"};
		BuildPipeline.BuildPlayer(scenes, "iOSBuild", BuildTarget.iOS, BuildOptions.None);
	}
}