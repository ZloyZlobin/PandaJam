using System;
using UnityEngine;
using PanndaJamTest.Resources;

namespace PanndaJamTest.Bricks
{
    [RequireComponent(typeof(Collider2D))]
    public class Brick : MonoBehaviour
    {
        /// <summary>
        /// Resources to collect after trigger
        /// </summary>
        [SerializeField]
        private ResourceInfo[] resources;
        /// <summary>
        /// On brick clicked (don't work on locked)
        /// </summary>
        public event Action<Brick> OnClick = (brick) => { };
        /// <summary>
        /// Type (Color)
        /// </summary>
        public BrickType Type { get; set; }
        /// <summary>
        /// X position in table
        /// </summary>
        public int PosY { get; set; }
        /// <summary>
        /// Y position in table
        /// </summary>
        public int PosX { get; set; }
        /// <summary>
        /// Is interactable
        /// </summary>
        public bool IsLocked { get; set; }
        /// <summary>
        /// Resources
        /// </summary>
        public ResourceInfo[] Resources { get { return resources; } }

        private void OnMouseDown()
        {
            if (!IsLocked)
                OnClick(this);
        }
        /// <summary>
        /// Communicate with other brick
        /// </summary>
        /// <param name="target">Other brick</param>
        /// <returns>Is communicate</returns>
        public virtual bool IsTriggerWith(Brick target)
        {
            return Type == target.Type;
        }
    }
}