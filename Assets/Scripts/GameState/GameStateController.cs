using System;
using PanndaJamTest.Resources;
using System.Collections.Generic;

namespace PanndaJamTest.State
{
    public static class GameStateController
    {
        public static event Action<GameState> OnGameStateChanged = (state) => { };
        public static event Action OnResourceChanged = () => { };

        public static GameState GameState { get; private set; }

        private static Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
        /// <summary>
        /// Change game state
        /// </summary>
        /// <param name="state">new state</param>
        public static void SetState(GameState state)
        {
            GameState = state;
            OnGameStateChanged(state);
        }
        /// <summary>
        /// Collect resources
        /// </summary>
        /// <param name="resourcesToCollect">Resources to collect</param>
        public static void CollectResources(ResourceInfo[] resourcesToCollect)
        {
            if (resourcesToCollect == null)
                return;
            foreach(var resourceToCollect in resourcesToCollect)
            {
                if (resources.ContainsKey(resourceToCollect.Type))
                    resources[resourceToCollect.Type] += resourceToCollect.Amount;
                else
                    resources.Add(resourceToCollect.Type, resourceToCollect.Amount);
            }
            OnResourceChanged();
        }
        /// <summary>
        /// Get resources amount by type
        /// </summary>
        /// <param name="type">Resource type</param>
        /// <returns>Amount</returns>
        public static int GetResources(ResourceType type)
        {
            int amount = 0;
            resources.TryGetValue(type, out amount);
            return amount;
        }

        /// <summary>
        /// Reset game state
        /// </summary>
        public static void ClearState()
        {
            resources.Clear();
            OnResourceChanged();
        }
    }
}
