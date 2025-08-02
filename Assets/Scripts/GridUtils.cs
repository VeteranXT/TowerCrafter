using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public static class GridUtils
{
    public static void MarkSlots(ItemBase itemData, DragDropUI UI, Vector2 gridPos, InventoryGrid grid, bool savesToItem = false, int cat = -1, int stash = -1)
    {
        if (grid == null || grid.GetGrid == null)
        {
            Debug.LogError("Grid or Grid.GetGrid is null.");
            return;
        }

        for (int yPos = 0; yPos < itemData.GridSize.y; yPos++) // Iterate over height (y)
        {
            for (int xPos = 0; xPos < itemData.GridSize.x; xPos++) // Iterate over width (x)
            {
                var PosX = gridPos.x + xPos;
                var PosY = gridPos.y + yPos;

                // Check bounds
                if (PosX >= 0 && PosX < grid.Width && PosY >= 0 && PosY < grid.Height)
                {
                    grid.GetGrid[(int)PosX, (int)PosY] = UI;
                }
                else
                {
                    grid.GetGrid[(int)PosX, (int)PosY] = null;

                }
            }
        }
    }
    public static HashSet<DragDropUI> OverlapedCount(DragDropUI itemToPlace, InventoryGrid grid)
    {
        HashSet<DragDropUI> overlapedData = new HashSet<DragDropUI>();
        var gridPosition = GetGridPositionFromAnchorPosition(grid, itemToPlace.RectTransform);

        for (int xSize = 0; xSize < itemToPlace.ItemData.GridSize.x; xSize++)
        {
            for (int ySize = 0; ySize < itemToPlace.ItemData.GridSize.y; ySize++)
            {
                int PosX = (int)gridPosition.x + xSize;
                int PosY = (int)gridPosition.y + ySize;

                // Check if the position is within bounds
                if (!InBounds(grid, itemToPlace.ItemData, itemToPlace.RectTransform))
                {
                    return null; // Exit if out of bounds
                }

                // Check if the cell is occupied
                DragDropUI cellItem = grid.GetGrid[PosX, PosY];
                if (cellItem != null && cellItem != itemToPlace && !overlapedData.Contains(cellItem))
                {
                    overlapedData.Add(cellItem);
                }
            }
        }

        return overlapedData;
    }
    public static bool InBounds(InventoryGrid grid, ItemBase item, RectTransform rectTransform)
    {
        int gridWidth = grid.Width;
        int gridHeight = grid.Height;
        var gridPosition = GetGridPositionFromAnchorPosition(grid, rectTransform);
        // Check if the rectDropUI fits within the inventoryGrid bounds
        if(grid == null)
            return false;
        return 
            gridPosition.x >= 0 && 
            gridPosition.y >= 0 &&
            gridPosition.x + item.GridSize.x <= gridWidth &&
            gridPosition.y + item.GridSize.y <= gridHeight;
    }
    public static bool SameOverlapping(InventoryGrid grid, Vector2 gridPos, ItemBase currentDragedItem, ItemBase item)
    {
        var OldPosition = currentDragedItem.GridPosition;

        for (int x = 0; x < currentDragedItem.GridSize.x; x++)
        {
            for (int y = 0; y < currentDragedItem.GridSize.y; y++)
            {
                var cellX = OldPosition.x + x;
                var cellY = OldPosition.y + y;

                for (int x2 = 0; x2 < item.GridSize.x; x2++)
                {
                    for (int y2 = 0; y2 < item.GridSize.y; y2++)
                    {

                        var cellX2 = gridPos.x + x2;
                        var cellY2 = gridPos.y + y2;

                        if (cellX >= cellX2 && cellX < cellX2 + item.GridSize.x &&
                            cellY >= cellY2 && cellY < cellY2 + item.GridSize.y)
                        {
                            return true;
                        }
                        if (cellX == cellX2 && cellY == cellY2)
                            return true;

                    }
                }
            }
        }
        return false;
    }
    public static Vector2Int GetGridPositionFromAnchorPosition(InventoryGrid grid, RectTransform rect)
    {
        return new Vector2Int(Mathf.FloorToInt(rect.anchoredPosition.x / grid.Width), Mathf.FloorToInt(-rect.anchoredPosition.y / grid.Height));
    }

    //private Vector2 GetGridPositionFromMouse(InventoryGrid inventoryGrid)
    //{
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryGrid.GridRect, Input.mousePosition, null, out Vector2 local);
    //    return new Vector2Int(Mathf.FloorToInt(local.x / inventoryGrid.Width), Mathf.FloorToInt(-local.y / inventoryGrid.Height));
    //}
    public static  Vector2 GetAnchorPositionFromItem(InventoryGrid grid, ItemBase item)
    {
        return new Vector2(Mathf.FloorToInt(item.GridPosition.x * grid.Width), Mathf.FloorToInt(item.GridPosition.y * grid.Height));
    }
    public static Vector2 GetAnchorPositionFromGridPosition(InventoryGrid grid, Vector2 gridPosition)
    {
        return new Vector2(Mathf.FloorToInt(gridPosition.x * grid.Width), Mathf.FloorToInt(-gridPosition.y * grid.Width));
    }
  

}

