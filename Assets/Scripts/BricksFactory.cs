using System.Linq;
using PanndaJamTest.Bricks;
using UnityEngine;

namespace PanndaJamTest
{
	public class BricksFactory:MonoBehaviour
	{
        /// <summary>
        /// Bricks setup
        /// </summary>
        [SerializeField]
        private BrickInfo[] bricks;
        /// <summary>
        /// Target ref
        /// </summary>
        [SerializeField]
        private GameObject target;
        /// <summary>
        /// Total weight of bricks
        /// </summary>
        private int totalWeight;

        private void Start()
        {
            bricks = bricks.OrderByDescending(b => b.weight).ToArray();
            totalWeight = bricks.Sum(b => b.weight);
        }
        /// <summary>
        /// Get random brick (by weight)
        /// </summary>
        /// <returns></returns>
        public Brick GetRandomBrick()
        {
            var rnd = UnityEngine.Random.Range(0, totalWeight);
            int currentWeight = 0;
            foreach (var brick in bricks)
            {
                currentWeight += brick.weight;
                if (rnd <= currentWeight)
                {
                    var resutBrick = GetBrick(brick.prototype);
                    //setup params
                    var sprite = resutBrick.GetComponent<SpriteRenderer>();
                    var brickComp = resutBrick.GetComponent<Brick>();
                    brickComp.Type = brick.type;
                    switch (brick.type)
                    {
                        case BrickType.Blue:
                            sprite.color = Color.blue;
                            break;
                        case BrickType.Red:
                            sprite.color = Color.red;
                            break;
                        case BrickType.Green:
                            sprite.color = Color.green;
                            break;
                        case BrickType.Yellow:
                            sprite.color = Color.yellow;
                            break;
                    }
                    return resutBrick;
                }
            }
            return null;
        }
        /// <summary>
        /// Get brick by prototype
        /// </summary>
        /// <param name="prototype">Prototype</param>
        /// <returns>New brick</returns>
        public Brick GetBrick(GameObject prototype)
        {
            var obj = GameObject.Instantiate(prototype) as GameObject;
            obj.transform.parent = transform;
            return obj.GetComponent<Brick>();
        }
        /// <summary>
        /// Get target brick
        /// </summary>
        /// <returns>Target</returns>
        public Brick GetTarget()
        {
            var targetBrick = GetBrick(target);
            targetBrick.Type = BrickType.Target;
            return targetBrick;
        }
	}
}
