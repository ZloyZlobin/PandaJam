using PanndaJamTest.Resources;
using PanndaJamTest.State;
using UnityEngine;
using UnityEngine.UI;

namespace PanndaJamTest.UI
{
	public class GameStateUIController:MonoBehaviour
	{
        [SerializeField]
        private Text goldCounter;

        private void Start()
        {
            GameStateController.OnResourceChanged += OnResourceChanged;
        }

        private void OnDestroy()
        {
            GameStateController.OnResourceChanged -= OnResourceChanged;
        }

        private void OnResourceChanged()
        {
            goldCounter.text = GameStateController.GetResources(ResourceType.Gold).ToString();
        }
	}
}
