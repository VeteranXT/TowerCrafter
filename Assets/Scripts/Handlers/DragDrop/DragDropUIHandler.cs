using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropUIHandler : MonoBehaviour
{
    [SerializeField] private InventoryGrid hoveredGrid;
    [SerializeField] private InventoryGrid oldGrid;

    [SerializeField] protected int currentCategoryIndex = 0;
    [SerializeField] protected int currentStashIndex = 0;
    private bool isUIOpen = false;

    public bool IsUIOpen { get { return isUIOpen; } set { isUIOpen = value; } }

    public void Start()
    {
        InventoryGrid.EventOnGridChanged += HandleGridChange;
        InventoryGrid.EventDragedDroped += HandleDrop;
        DragDropUI.EventOnClick += HandleClicked;
        DragDropUI.EventBeginDrag += HandleBeginDragging;
        DragDropUI.EventOnDraged += HandleDragging;
        DragDropUI.EventEndDrag += HandleEndDragging;
        StorageFilterHandler.EventCategoryChanged += UpdateCategory;
        StorageFilterHandler.EventStashChanged += UpdateStash;
    }

    #region Event Handlers Methods

    private void HandleGridChange(InventoryGrid grid)
    {
        hoveredGrid = grid;
    }

    public virtual void HandleClicked(DragDropUI uI)
    {
        // Custom behavior for clicked items
    }

    private void HandleBeginDragging(PointerEventData draggable)
    {
        var dragUI = draggable.pointerDrag.GetComponent<DragDropUI>();

        dragUI.CanvasGroup.blocksRaycasts = false;
        dragUI.CanvasGroup.alpha = 0.7f;
        oldGrid = draggable.pointerDrag.GetComponentInParent<InventoryGrid>();
        GridUtils.MarkSlots(dragUI.ItemData, null, dragUI.ItemData.GridPosition, oldGrid); // Clear old slots
    }

    private void HandleDragging(PointerEventData draggable)
    {
        var dragUI = draggable.pointerDrag.GetComponent<DragDropUI>();
        dragUI.RectTransform.anchoredPosition += draggable.delta;
    }

    public void HandleDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag.GetComponent<DragDropUI>();
        var gridPos = GridUtils.GetGridPositionFromAnchorPosition(hoveredGrid, dragged.RectTransform);

        // Check if out of bounds
        if (!GridUtils.InBounds(hoveredGrid, dragged.ItemData, dragged.RectTransform))
        {
            ReturnToOriginalPosition(dragged);
            return;
        }

        var overlappedItems = GridUtils.OverlapedCount(dragged, hoveredGrid);
        if (overlappedItems == null || overlappedItems.Count > 1)
        {
            ReturnToOriginalPosition(dragged);
            return;
        }

        if (overlappedItems.Count == 1)
        {
            var overlapped = overlappedItems.First();
            if (overlapped != null && !GridUtils.SameOverlapping(hoveredGrid, gridPos, dragged.ItemData, overlapped.ItemData))
            {
                // Swap positions
                var overlappedPosition = overlapped.ItemData.GridPosition;
                overlapped.RectTransform.anchoredPosition = GridUtils.GetAnchorPositionFromGridPosition(hoveredGrid, dragged.ItemData.GridPosition);
                dragged.RectTransform.anchoredPosition = GridUtils.GetAnchorPositionFromGridPosition(hoveredGrid, gridPos);

                // Update slots
                GridUtils.MarkSlots(overlapped.ItemData, overlapped, overlappedPosition, hoveredGrid, true);
                GridUtils.MarkSlots(dragged.ItemData, dragged, gridPos, hoveredGrid, true);
                return;
            }
        }

        // No overlap, place the item
        dragged.RectTransform.anchoredPosition = GridUtils.GetAnchorPositionFromGridPosition(hoveredGrid, gridPos);
        GridUtils.MarkSlots(dragged.ItemData, dragged, gridPos, hoveredGrid, true);
    }

    private void HandleEndDragging(PointerEventData draggable)
    {
        var dragUI = draggable.pointerDrag.GetComponent<DragDropUI>();
        dragUI.CanvasGroup.blocksRaycasts = true;
        dragUI.CanvasGroup.alpha = 1f;

        if (hoveredGrid == null)
        {
            ReturnToOriginalPosition(dragUI);
        }
    }

    private void ReturnToOriginalPosition(DragDropUI dragUI)
    {
        dragUI.RectTransform.anchoredPosition = GridUtils.GetAnchorPositionFromItem(oldGrid, dragUI.ItemData);
        GridUtils.MarkSlots(dragUI.ItemData, dragUI, dragUI.ItemData.GridPosition, oldGrid, true);
    }

    private void UpdateStash(int StashChange)
    {
        currentStashIndex = StashChange;
    }

    private void UpdateCategory(int CategoryChange)
    {
        currentCategoryIndex = CategoryChange;
    }

    #endregion
}