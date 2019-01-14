using System.Collections.Generic;
using PanndaJamTest.Bricks;

namespace PanndaJamTest.Behaviours
{
	public class TurnBehaviour: BrickBehaviour
	{
        /// <summary>
        /// Clockwise rotation
        /// </summary>
        /// <param name="table">Table</param>
        public override void ExecuteBehaviour(Brick[][] table)
        {
            var positions = new List<BrickPos>();
            //top
            if (brick.PosY < table.Length - 1)
            {
                //left
                if (brick.PosX > 0)
                    positions.Add(new BrickPos(brick.PosX - 1, brick.PosY + 1));
                //center
                positions.Add(new BrickPos(brick.PosX, brick.PosY + 1));
                //right
                if (brick.PosX < table[brick.PosY + 1].Length - 1)
                    positions.Add(new BrickPos(brick.PosX + 1, brick.PosY + 1));
            }
            //right
            if (brick.PosX < table[brick.PosY].Length - 1)
                positions.Add(new BrickPos(brick.PosX + 1, brick.PosY));
            //bottom
            if (brick.PosY > 0)
            {
                //right
                if (brick.PosX < table[brick.PosY + 1].Length - 1)
                    positions.Add(new BrickPos(brick.PosX + 1, brick.PosY - 1));
                //center
                positions.Add(new BrickPos(brick.PosX, brick.PosY - 1));
                //left
                if (brick.PosX > 0)
                    positions.Add(new BrickPos(brick.PosX - 1, brick.PosY - 1));
            }
            //left
            if (brick.PosX > 0)
                positions.Add(new BrickPos(brick.PosX - 1, brick.PosY));
            if (positions.Count <= 1)
                return;
            if (positions.Count == 2)
            {
                var tmp = table[positions[1].Y][positions[1].X];
                table[positions[1].Y][positions[1].X] = table[positions[0].Y][positions[1].X];
                table[positions[0].Y][positions[0].X] = tmp;
            }
            else
            {
                var bricks = new List<Brick>();
                foreach (var position in positions)
                {
                    bricks.Add(table[position.Y][position.X]);
                }
                //offset
                var offsetBricks = new Brick[bricks.Count];
                for (int i = 0; i < bricks.Count - 2; i++)
                    offsetBricks[i + 2] = bricks[i];
                offsetBricks[0] = bricks[bricks.Count - 2];
                offsetBricks[1] = bricks[bricks.Count - 1];
           
                //setup
                for (int i = 0; i < positions.Count; i++)
                {
                    table[positions[i].Y][positions[i].X] = offsetBricks[i];
                    if (offsetBricks[i])
                    {
                        offsetBricks[i].PosX = positions[i].X;
                        offsetBricks[i].PosY = positions[i].Y;
                    }
                }
            }
        }

	}
}
