using TowerCrafter.Grid;
using TowerCrafter.Grid.Utlis;
using UnityEngine;
public static class GeneralFactory
{

    public static DragDropUI CreateDragDropUI(GameObject prefab, Transform parent, ItemBase item, Vector2Int gridPos, CustomGrid grid)
    {
        var drag = DragDropUI.Instantiate(prefab.GetComponent<DragDropUI>());
        drag.SetupItem(item);
        drag.RectTransform.anchoredPosition = GridUtils.AnchorPositionFromGridPosition(grid, gridPos);
        return drag;
    }

    public static UIStash CreateStash(GameObject prefab, Transform parent)
    {
        var storage = UIStash.Instantiate(prefab).AddComponent<UIStash>();
        return storage;
    }

    public static UICategory CreateCategory(GameObject prefab, Transform parent)
    {
        var category = UICategory.Instantiate(prefab).AddComponent<UICategory>();
        return category;
    }
}



