using System.Collections.Generic;
using PanndaJamTest.Bricks;
using UnityEngine;

namespace PanndaJamTest.Behaviours
{
    [RequireComponent(typeof(Brick))]
    public class BrickBehaviour : MonoBehaviour
    {
        protected Brick brick;

        protected virtual void Start()
        {
            brick = GetComponent<Brick>();
        }
        /// <summary>
        /// Return triggered bricks by trigger this one
        /// </summary>
        /// <param name="maxX">Table width</param>
        /// <param name="maxY">Table height</param>
        /// <returns></returns>
        public virtual IEnumerable<BrickPos> GetTriggered(int maxX, int maxY)
        {
            return new[] { new BrickPos(brick.PosX, brick.PosY) };
        }
        /// <summary>
        /// Execute over behaviour on trigger
        /// </summary>
        /// <param name="table">Table</param>
        public virtual void ExecuteBehaviour(Brick[][] table)
        {
        }
    }
}
