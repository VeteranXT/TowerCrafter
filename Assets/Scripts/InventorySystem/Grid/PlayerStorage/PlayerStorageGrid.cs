using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerCrafter.Grid;
using UnityEngine;

namespace TowerCrafter.Grid
{
    [RequireComponent(typeof(StorageFilterHandler))]
    public class PlayerStorageGrid : InventoryGrid
    {
        private int categoryIndex = 0;
        private int stashIndex = 0;


        private void Awake()
        {
            StorageFilterHandler.EventCategoryChanged += UpdateCategory;
            StorageFilterHandler.EventStashChanged += UpdateStash;
        }

        private void UpdateCategory(int category)
        {
            categoryIndex = category;
        }

        private void UpdateStash(int stash)
        {
            stashIndex = stash;
        }
    }

}

