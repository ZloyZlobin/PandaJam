using System.Collections.Generic;
using PanndaJamTest.Bricks;
using UnityEngine;

namespace PanndaJamTest.Behaviours
{
    public class IceBehaviour : BrickBehaviour
    {
        /// <summary>
        /// Ice sprite (remove when unfreeze)
        /// </summary>
        [SerializeField]
        private GameObject iceSprite;

        protected override void Start()
        {
            base.Start();
            brick.IsLocked = true;//ice brick lock on start
        }
        /// <summary>
        /// Don't trigger if frozen
        /// </summary>
        /// <param name="maxX">Table width</param>
        /// <param name="maxY"></param>
        /// <returns></returns>
        public override IEnumerable<BrickPos> GetTriggered(int maxX, int maxY)
        {
            return new BrickPos[0];
        }
        /// <summary>
        /// Unfreeze brick
        /// </summary>
        /// <param name="table">Table</param>
        public override void ExecuteBehaviour(Brick[][] table)
        {
            brick.IsLocked = false;
            if(iceSprite != null)
                GameObject.DestroyImmediate(iceSprite);
            GameObject.DestroyImmediate(this);
        }
    }
}
