using UnityEditor;

namespace Editor
{
	public class JenkinsPerformBuild 
	{
		public static void PerformBuild()
		{
			var scenes = new[] {"Assets/Main.unity"};
			BuildPipeline.BuildPlayer(scenes, "iOSBuild", BuildTarget.iOS, BuildOptions.None);
		}
		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update () {
		
		}
	}
}
