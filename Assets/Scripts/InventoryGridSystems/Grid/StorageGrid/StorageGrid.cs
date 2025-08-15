using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerCrafter.Grid;
using UnityEngine;

namespace TowerCrafter.Grid
{
    [RequireComponent(typeof(StorageFilterManager))]
    public class StorageGrid : CustomGrid
    {
        public override bool CanAccept(ItemBase item)
        {
            return base.CanAccept(item);
        }
    }

}

