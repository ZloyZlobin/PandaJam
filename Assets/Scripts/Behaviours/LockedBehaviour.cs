namespace PanndaJamTest.Behaviours
{
	public class LockedBehaviour: BrickBehaviour
	{
        protected override void Start()
        {
            base.Start();
            brick.IsLocked = true;
        }
	}
}
