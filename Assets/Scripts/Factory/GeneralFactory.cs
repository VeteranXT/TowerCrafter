using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerCrafter.Grid;
using TowerCrafter.Grid.Utlis;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public  class GeneralFactory : MonoBehaviour
{
    public static DragDropUI CreateDragDropUI(Transform parent, DragDropUI prefab, ItemBase item, Vector2Int gridPos, InventoryGrid grid)
    {
        DragDropUI drag = Instantiate(prefab, parent);
        drag.SetupItem(item);
        drag.RectTransform.anchoredPosition = GridUtils.AnchorPositionFromGridPosition(grid, gridPos);
        return drag;
    }

    public static UIStash CreateStash(Transform parent, GameObject prefab, UnityAction call)
    {
        UIStash storage = Instantiate(prefab, parent).AddComponent<UIStash>();
        storage.GetButton.onClick.AddListener(call);
        return storage;
    }

    public static UICategory CreateCategory(Transform parent, GameObject prefab, UnityAction call)
    {
        UICategory cat = Instantiate(prefab, parent.transform).AddComponent<UICategory>();
        cat.GetButton.onClick.AddListener(call);
        return cat;
    }
}



