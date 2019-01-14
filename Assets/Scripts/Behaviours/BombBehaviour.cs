using System;
using System.Collections.Generic;
using PanndaJamTest.Bricks;

namespace PanndaJamTest.Behaviours
{
	public class BombBehaviour: BrickBehaviour
	{
        /// <summary>
        /// Trigger 9 bricks around
        /// </summary>
        /// <param name="maxX">Table width</param>
        /// <param name="maxY">Table height</param>
        /// <returns></returns>
        public override IEnumerable<BrickPos> GetTriggered(int maxX, int maxY)
        {
            for (int i = Math.Max(0, brick.PosX - 1); i < Math.Min(maxX, brick.PosX + 2); i++)
            {
                for (int j = Math.Max(0, brick.PosY - 1); j < Math.Min(maxY, brick.PosY + 2); j++)
                {
                    yield return new BrickPos(i, j);
                }
            }
        }
	}
}
