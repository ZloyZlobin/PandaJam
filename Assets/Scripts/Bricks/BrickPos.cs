namespace PanndaJamTest.Bricks
{
    public class BrickPos
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public BrickPos(int posX, int PosY)
        {
            X = posX;
            Y = PosY;
        }
    }
}
