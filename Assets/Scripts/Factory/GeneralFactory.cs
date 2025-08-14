using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerCrafter.Grid;
using TowerCrafter.Grid.Utlis;
using UnityEngine;

public  class GeneralFactory : MonoBehaviour
{
    public static DragDropUI CreateDragDropUI(Transform parent, DragDropUI prefab, ItemBase item, Vector2Int gridPos, InventoryGrid grid)
    {
        DragDropUI drag = Instantiate(prefab, parent);
        drag.SetupItem(item);
        drag.RectTransform.anchoredPosition = GridUtils.AnchorPositionFromGridPosition(grid, gridPos);
        return drag;
    }

    public static UIStash CreateStash(Transform parent, GameObject prefab, int categoryIndex, string name = "")
    {
        var storage = Instantiate(prefab, parent).AddComponent<UIStash>();
        storage.CategoryIndex = categoryIndex;

        return storage;
    }
}



