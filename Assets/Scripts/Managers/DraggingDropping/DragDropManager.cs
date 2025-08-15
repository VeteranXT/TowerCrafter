// 8/15/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using System.Linq;
using TowerCrafter.Grid;
using TowerCrafter.Grid.Utlis;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DragDropManager : BaseMonoBehaviour
{
    #region Fields and Properties

    [SerializeField] private CustomGrid hoveredGrid;
    [SerializeField] private CustomGrid oldGrid;
    [SerializeField] private DragDropUI selectedUI;
    [SerializeField] private Canvas canvas;

    private int currentCategoryIndex = 0;
    private int currentStashIndex = 0;

    #endregion

    protected override void Subscribe()
    {
        CustomGrid.EventOnGridChanged += HandleGridChange;
        CustomGrid.EventDragedDroped += HandleDrop;
        DragDropUI.EventOnClick += HandleClick;
        DragDropUI.EventBeginDrag += HandleBeginDragging;
        DragDropUI.EventOnDraged += HandleDragging;
        DragDropUI.EventEndDrag += HandleEndDragging;
        StorageFilterManager.EventCategoryChanged += UpdateCategoryIndex;
        StorageFilterManager.EventStashChanged += UpdateStashIndex;
    }


    protected override void FindReferences()
    {
        canvas = FindAnyObjectByType<Canvas>();
    }
    #region Unity Lifecycle



    private  void Update()
    {
        // Update the position of the selected UI element
        if (selectedUI == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            selectedUI.RectTransform.parent as RectTransform,
            Input.mousePosition,
            null,
            out Vector2 localPoint
        );

        // Center the UI element based on its size
        Vector2 size = selectedUI.RectTransform.rect.size;
        Vector2 centerOffset = new(size.x * 0.5f, -size.y * 0.5f);
        selectedUI.RectTransform.anchoredPosition = localPoint - centerOffset;
    }

    protected override void OnDestroy()
    {
        // Unsubscribe from events
        CustomGrid.EventOnGridChanged -= HandleGridChange;
        CustomGrid.EventDragedDroped -= HandleDrop;
        DragDropUI.EventOnClick -= HandleClick;
        DragDropUI.EventBeginDrag -= HandleBeginDragging;
        DragDropUI.EventOnDraged -= HandleDragging;
        DragDropUI.EventEndDrag -= HandleEndDragging;
    }

    #endregion

    #region Event Handlers

    private void UpdateStashIndex(int newStashIndex)
    {
        currentStashIndex = newStashIndex;
    }

    private void UpdateCategoryIndex(int newCategoryIndex)
    {
        currentCategoryIndex = newCategoryIndex;
    }

    private void HandleBeginDragging(DragDropUI drop)
    {
        drop.CanvasGroup.blocksRaycasts = false;
        drop.CanvasGroup.alpha = 0.7f;

        // Clear the slots from the old position
        if (hoveredGrid != null)
        {
            GridUtils.MarkSlots(hoveredGrid, drop.ItemData, null, drop.ItemData.GridPosition);
        }

        oldGrid = hoveredGrid;
        selectedUI = drop;
    }

    private void HandleDragging(PointerEventData data)
    {
        if (canvas == null) return;

        // Update the position of the dragged UI element
        data.pointerDrag.GetComponent<RectTransform>().anchoredPosition += data.delta / canvas.scaleFactor;
    }

    private void HandleEndDragging(DragDropUI drop)
    {
        drop.CanvasGroup.blocksRaycasts = true;
        drop.CanvasGroup.alpha = 1f;

        // Return to the original position if no valid grid is hovered
        if (hoveredGrid == null || !IsDropValid(drop, hoveredGrid))
        {
            ReturnToOriginalPosition(drop,oldGrid);
            return;
        }

        // The drop will be handled by HandleDrop
    }

    private void HandleClick(DragDropUI data)
    {
        if (selectedUI == null)
        {
            // Select the clicked item
            selectedUI = data;
            hoveredGrid.MarkSlots(selectedUI.ItemData, null, selectedUI.ItemData.GridPosition);
            return;
        }

        // Handle dropping the currently selected item
        HandleDrop(selectedUI);
    }

    private void HandleDrop(DragDropUI data)
    {
        if (data == null || data.ItemData == null)
        {
            Debug.LogError("Created UI without assigned Item!");
            return;
        }

        if (hoveredGrid == null) return;
        if (!IsDropValid(data, hoveredGrid)) return;

        var item = data.ItemData;

        bool canGridAccept = hoveredGrid.CanAccept(item);

        if (hoveredGrid is StorageGrid && canGridAccept)
        {
            HandlePlayerStorageStorage(data);
        }
        else if (hoveredGrid is ShopGrid && canGridAccept)
        {
            HandleShopStorage(data);
        }
    }

    private void HandleGridChange(CustomGrid grid)
    {
        hoveredGrid = grid;
    }

    #endregion

    #region Drag-and-Drop Logic

    private void HandlePlayerStorageStorage(DragDropUI data)
    {
        var overlappingItems = hoveredGrid.CountOverlaps(data);
        var dropCoordinates = hoveredGrid.GridPositionFromAnchorPosition(data.RectTransform);
        if (overlappingItems == null) return;

        if (overlappingItems.Count == 0)
        {
            // Place the item directly
            hoveredGrid.Place(data, dropCoordinates, currentCategoryIndex, currentStashIndex);
            ClearSelection();
            return;
        }

        if (overlappingItems.Count == 1)
        {
            var overlappedUI = overlappingItems.FirstOrDefault();

            if (hoveredGrid.CanSwap(data, overlappedUI))
            {
                SwapItems(data, overlappedUI, dropCoordinates,hoveredGrid);
                return;
            }

            // Place the dragged item and select the overlapped item
            hoveredGrid.Place(data, dropCoordinates, currentCategoryIndex, currentStashIndex);
            selectedUI = overlappedUI;
            hoveredGrid.MarkSlots(selectedUI.ItemData, null, selectedUI.ItemData.GridPosition);
        }
    }

    private void HandleShopStorage(DragDropUI data)
    {
        // Shop-specific logic can be implemented here
    }

    private void SwapItems(DragDropUI draggedItem, DragDropUI overlappedItem, Vector2Int dropCoordinates, CustomGrid grid)
    {
        var originalCategory = overlappedItem.ItemData.CategoryIndex;
        var originalStash = overlappedItem.ItemData.StashIndex;
        var itemOrigin = draggedItem.ItemData.GridPosition;

        // Move the overlapped item to the original position of the dragged item
        grid.MarkSlots(overlappedItem.ItemData, null, overlappedItem.ItemData.GridPosition);
        grid.Place(overlappedItem, itemOrigin, originalCategory, originalStash);
        UpdateUIVisibility(overlappedItem, hoveredGrid, itemOrigin);

        // Place the dragged item in the new position
        grid.Place(draggedItem, dropCoordinates, currentCategoryIndex, currentStashIndex);

        ClearSelection();
    }

    private void ReturnToOriginalPosition(DragDropUI dragDrop, CustomGrid oldGrid)
    {
        if (dragDrop == null) return;

        oldGrid.ReturnToOrginalPosition(dragDrop);
        oldGrid.MarkSlots(dragDrop.ItemData, dragDrop, dragDrop.ItemData.GridPosition);

        ClearSelection();
    }

    private void ClearSelection()
    {
        selectedUI = null;
        oldGrid = null;
    }

    private bool IsDropValid(DragDropUI drop, CustomGrid grid)
    {
        var dropCoordinates = hoveredGrid.GridPositionFromAnchorPosition(drop.RectTransform);
        return grid.InBounds(drop.ItemData, dropCoordinates);
    }

    #endregion

    #region UI Updates

    public void UpdateUIVisibility(DragDropUI dragDrop, CustomGrid grid, Vector2Int gridPos)
    {
        var item = dragDrop.ItemData;

        if (item.StashIndex != currentStashIndex)
        {
            dragDrop.gameObject.SetActive(false);
            grid.MarkSlots(item, null, gridPos);
        }
        else
        {
            dragDrop.gameObject.SetActive(true);
            grid.MarkSlots(item, dragDrop, gridPos);
        }
    }

    #endregion
}