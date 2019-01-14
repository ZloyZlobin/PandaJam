using System;
using UnityEngine;

namespace PanndaJamTest.Bricks
{
    [Serializable]
	public class BrickInfo
	{
        /// <summary>
        /// Weight to radom creation
        /// </summary>
        public int weight;
        /// <summary>
        /// Color
        /// </summary>
        public BrickType type;
        /// <summary>
        /// Reference on prototype gameObject/prefab
        /// </summary>
        public GameObject prototype;
	}
}
