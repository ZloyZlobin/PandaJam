using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PanndaJamTest.UI
{
	public class TableUIController: MonoBehaviour
	{
        [SerializeField]
        private Table table;

        public void ForceAddLinne()
        {
            table.ForceAddLine();
        }
	}
}
