using PanndaJamTest.State;
using UnityEngine;

namespace PanndaJamTest.UI
{
	public class UIController: MonoBehaviour
	{
        [SerializeField]
        private GameObject winWnd;
        [SerializeField]
        private GameObject loseWnd;

        private void Start()
        {
            GameStateController.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            GameStateController.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState gameState)
        {
            GameObject wnd = null;
            if (gameState == GameState.Win)
                wnd = GameObject.Instantiate(winWnd) as GameObject;
            else if (gameState == GameState.Lose)
                wnd = GameObject.Instantiate(loseWnd) as GameObject;

            if (wnd != null)
            {
                wnd.transform.SetParent(transform, false);
            }
        }
	}
}