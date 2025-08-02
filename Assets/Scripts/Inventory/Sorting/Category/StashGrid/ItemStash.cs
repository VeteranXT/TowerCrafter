
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemStash : MonoBehaviour 
{
    private string stashName;
    private Sprite stashIcon;
    private InventoryGrid inventoryGrid;

    private void Start()
    {
        inventoryGrid = GetComponentInParent<InventoryGrid>();
    }

    private void OnValidate()
    {
        inventoryGrid = GetComponentInParent<InventoryGrid>();
    }
}