using PanndaJamTest.State;
using UnityEngine;

namespace PanndaJamTest.UI
{
    public class GameResultUIController : MonoBehaviour
    {
        public void RestartGame()
        {
            GameStateController.ClearState();
            GameStateController.SetState(GameState.StartGame);
            GameObject.DestroyImmediate(gameObject);//close self
        }
    }
}
