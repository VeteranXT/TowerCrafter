using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryContoller : MonoBehaviour
{
    [SerializeField] private InventoryGrid hoveredGrid;
    [SerializeField] private bool eventPickedUp = false;
    [SerializeField] private DragDropUI clickedUp;
    [SerializeField] Canvas canvas;
    public void Start()
    {
        InventoryGrid.EventOnGridChanged += OnGridChanged;
        InventoryGrid.EventDragedDroped += OnDrop;
        DragDropUI.EventOnClick += HandleItemClickPick;
        DragDropUI.EventOnDraged += HandleDragging;

        canvas = FindFirstObjectByType<Canvas>();
    }
    //While draging update Position
    private void HandleDragging(DragDropUI uI)
    {
        uI.RectTransform.position = Input.mousePosition / canvas.scaleFactor;
    }

    private void Update()
    {
        if (clickedUp == null)
            eventPickedUp = false;
        if (eventPickedUp)
        { 
            var Rect = clickedUp.RectTransform;
            Rect.anchoredPosition = Input.mousePosition;
        }
        if(Input.GetMouseButtonDown(0))
        {

        }


    }
    public void OnDrop(PointerEventData eventData)
    {
        var Droped = eventData.pointerDrag.GetComponent<DragDropUI>();
        var OrginalAnchorPositon = GetAnchorPositionFromItem(hoveredGrid, Droped.ItemData);
        var oldGridPos = Droped.ItemData.GridPosition;
        var currentGridPos = GetGridPositionFromTransform(hoveredGrid, Droped.RectTransform);
        if(Droped.ItemData == null)
        {
            //We created itemUI but didn't assign actual item  to its UI
            Debug.LogError("Item is not assigned on creation");
            //Destroy(Droped);

            if(hoveredGrid == null)
            {
                Droped.RectTransform.anchoredPosition = OrginalAnchorPositon;
                MarkSlots(Droped, oldGridPos, hoveredGrid, false);
                return;
            }
            if(!InBounds(hoveredGrid, Droped.ItemData, oldGridPos))
            {
                Droped.RectTransform.anchoredPosition = OrginalAnchorPositon;
                MarkSlots(Droped, oldGridPos, hoveredGrid, false);
            }
            if (hoveredGrid != null) 
            { 
            }
            var DropedCordinates = Droped.RectTransform.anchoredPosition;
        }

    }
    private void HandleItemClickPick(DragDropUI uI)
    {
        if (uI == null) return;
        var itemSize = uI.ItemData.GridPosition;
        // Unmark all slots occupied by the rectDropUI
        MarkSlots(null, itemSize, hoveredGrid, false);

        // Set the currently picked-up rectDropUI
        clickedUp = uI;

        // Mark it as clicked and start dragging
        eventPickedUp = true;
    }

    private void OnGridChanged(InventoryGrid grid)
    {
        hoveredGrid = grid;
    }
    public static void MarkSlots(DragDropUI UI, Vector2 GridPos, InventoryGrid grid, bool  saves)
    {
        for (int xPos = (int)GridPos.x; xPos < GridPos.x + UI.ItemData.GridSize.x; xPos++)
        {
            for (int yPos = (int)GridPos.y; yPos < GridPos.y + UI.ItemData.GridSize.y; yPos++)
            {
                if (UI != null)
                    grid.GetGrid[xPos, yPos] = UI;
                else
                    grid.GetGrid[xPos, yPos] = null;
                if(saves)   
                    UI.ItemData.SaveGridPositon(GridPos);
            }
        }
    }


    #region Helper Methods
    public static Vector2Int GetGridPositionFromTransform(InventoryGrid grid, RectTransform rect)
    {
        return new Vector2Int(Mathf.FloorToInt(rect.anchoredPosition.x / grid.Width), Mathf.FloorToInt(-rect.anchoredPosition.y / grid.Height));
    }

    //private Vector2 GetGridPositionFromMouse(InventoryGrid grid)
    //{
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(grid.GridRect, Input.mousePosition, null, out Vector2 local);
    //    return new Vector2Int(Mathf.FloorToInt(local.x / grid.Width), Mathf.FloorToInt(-local.y / grid.Height));
    //}
    public Vector2 GetAnchorPositionFromItem(InventoryGrid grid, ItemBase item)
    {
        return new Vector2(Mathf.FloorToInt(item.GridPosition.x * grid.Width), Mathf.FloorToInt(item.GridPosition.y * grid.Height));
    }
    private Vector2 GetAnchorPositionFromGridPosition(InventoryGrid grid, Vector2 gridPosition)
    {
        return new Vector2(Mathf.FloorToInt(gridPosition.x * grid.Width), Mathf.FloorToInt(-gridPosition.y * grid.Width));
    }
    #endregion
    private HashSet<DragDropUI> OverlapedCount(Vector2 gridPosition, ItemBase itemToPlace)
    {
        HashSet<DragDropUI> overlapedData = new HashSet<DragDropUI>();

        for (int xSize = 0; xSize < itemToPlace.GridSize.x; xSize++)
        {
            for (int ySize = 0; ySize < itemToPlace.GridSize.y; ySize++)
            {
                int PosX = (int)gridPosition.x + xSize;
                int PosY = (int)gridPosition.y + ySize;

                // Check if the position is within bounds
                if (!InBounds(hoveredGrid, itemToPlace, gridPosition))
                {
                    return null; // Exit if out of bounds
                }

                // Check if the cell is occupied
                DragDropUI cellItem = hoveredGrid.GetGrid[PosX, PosY];
                if (cellItem != null && cellItem !=itemToPlace && !overlapedData.Contains(cellItem))
                {
                    overlapedData.Add(cellItem);
                }
            }
        }

        return overlapedData;
    }
    private bool InBounds(InventoryGrid grid, ItemBase item, Vector2 gridPosition)
    {
        int gridWidth = grid.Width;
        int gridHeight = grid.Height;

        // Check if the rectDropUI fits within the grid bounds
        return gridPosition.x >= 0 && gridPosition.y >= 0 &&
               gridPosition.x + item.GridSize.x <= gridWidth &&
               gridPosition.y + item.GridSize.y <= gridHeight;
    }
    //
    private bool IsOccupied(InventoryGrid grid, Vector2 GridPos, ItemBase item)
    {

        for (int i = 0; i < item.GridSize.x; i++)
        {
            for (int y = 0; y < item.GridSize.y; y++)
            {
                var PositionX = GridPos.x + i;
                var PositionY = GridPos.y + y;

                if (grid.GetGrid[(int)PositionX, (int)PositionY] != null)
                    return true;
            }
        }

        return false;
    }
    private bool SameOverlapping(InventoryGrid grid, Vector2 gridPos, ItemBase newItem, ItemBase item)
    {
        var OldPosition = newItem.GridPosition;

        for (int x = 0; x < newItem.GridSize.x; x++)
        {
            for (int y = 0; y < newItem.GridSize.y; y++)
            {
                var cellX = OldPosition.x + x;
                var cellY = OldPosition.y + y;

                for (int x2 = 0; x2 < item.GridSize.x; x2++)
                {
                    for (int y2 = 0; y2 < item.GridSize.y; y2++)
                    {

                        var oldPosX = gridPos.x + x2;
                        var oldPosY = gridPos.y + y2;

                        if (cellX == oldPosX && cellY == oldPosY)
                            return true;

                    }
                }
            }
        }
        return false;
    }

    
}

//var rect = eventData.pointerDrag.GetComponent<RectTransform>();
//var rectAnchor = rect.anchoredPosition;
//var rectDropUI = rect.GetComponent<DragDropUI>();
//var rectItem = rectDropUI.ItemData;
//Vector2 gridPos = GetGridPositionFromRectAnchor(hoveredGrid, rect);
////Check if we are draging out of grid
//if (hoveredGrid == null)
//{
//    //if we are then return it to orginal position
//    rect.anchoredPosition = GetAnchorPositionFromItem(hoveredGrid, rectItem);
//    return;
//}

////Out of bounds return it to its old position
//if (!InBounds(hoveredGrid, rectItem, gridPos))
//    //Again Return it back to orginal position
//    rect.anchoredPosition = GetAnchorPositionFromItem(hoveredGrid, rectItem);

////Check if we are overlaping more than 1 DragDRop
//if (OverlapedCount(gridPos, rectItem).Count > 1)
//{
//    return;
//}
//if (OverlapedCount(gridPos, rectItem).Count == 1)
//{
//    var uiOver = OverlapedCount(gridPos, rectItem).First();
//    //Check if current UI we dragged when swaped will not share same cells  wkaka overlap each other
//    if (!SameOverlapping(hoveredGrid, gridPos, rectItem, uiOver.ItemData))
//    {
//        //Place rectItem over other rectItem
//        rect.anchoredPosition = GetAnchorPositionFromGridPosition(hoveredGrid, rectAnchor);
//        //Move layered rectItem to old moved rectItem position
//        uiOver.GetComponent<RectTransform>().anchoredPosition = GetAnchorPositionFromItem(hoveredGrid, rectItem);
//        //un mark slots of old position
//        MarkSlots(null, uiOver.ItemData.GridPosition, hoveredGrid, false);
//        //Mark new to new position and save item Position
//        MarkSlots(uiOver, rectItem.GridPosition, hoveredGrid, true);
//        //mark slots and save position
//        MarkSlots(rectDropUI, gridPos, hoveredGrid, true);
//    }

//}
//else
//{
//    //Set this UI to new position
//    rect.anchoredPosition = GetAnchorPositionFromGridPosition(hoveredGrid, gridPos);
//    MarkSlots(rectDropUI, gridPos, hoveredGrid, true);
//}