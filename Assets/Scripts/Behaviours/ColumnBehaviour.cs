using System.Collections.Generic;
using PanndaJamTest.Bricks;

namespace PanndaJamTest.Behaviours
{
    public class ColumnBehaviour : BrickBehaviour
    {
        /// <summary>
        /// Trigger collumn
        /// </summary>
        /// <param name="maxX">Table width</param>
        /// <param name="maxY">Table height</param>
        /// <returns></returns>
        public override IEnumerable<BrickPos> GetTriggered(int maxX, int maxY)
        {
            for (int i = 0; i < maxY; i++)
                yield return new BrickPos(brick.PosX, i);
        }
    }
}
