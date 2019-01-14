using System.Collections.Generic;
using PanndaJamTest.Bricks;

namespace PanndaJamTest.Behaviours
{
	public class RowBehaviour: BrickBehaviour
	{
        /// <summary>
        /// Trigger all row
        /// </summary>
        /// <param name="maxX">Table width</param>
        /// <param name="maxY">Table hight</param>
        /// <returns></returns>
        public override IEnumerable<BrickPos> GetTriggered(int maxX, int maxY)
        {
            for (int i = 0; i < maxX; i++)
                yield return new BrickPos(i, brick.PosY);
        }
	}
}
