using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public static class GridUtils
{
    public static void MarkSlots(DragDropUI UI, Vector2 GridPos, InventoryGrid grid, bool saves)
    {
        for (int xPos = (int)GridPos.x; xPos < GridPos.x + UI.ItemData.GridSize.x; xPos++)
        {
            for (int yPos = (int)GridPos.y; yPos < GridPos.y + UI.ItemData.GridSize.y; yPos++)
            {
                if (UI != null)
                    grid.GetGrid[xPos, yPos] = UI;
                else
                    grid.GetGrid[xPos, yPos] = null;
                if (saves)
                    UI.ItemData.SaveGridPositon(GridPos);
            }
        }
    }
    public static HashSet<DragDropUI> OverlapedCount(Vector2 gridPosition, ItemBase itemToPlace, InventoryGrid grid)
    {
        HashSet<DragDropUI> overlapedData = new HashSet<DragDropUI>();

        for (int xSize = 0; xSize < itemToPlace.GridSize.x; xSize++)
        {
            for (int ySize = 0; ySize < itemToPlace.GridSize.y; ySize++)
            {
                int PosX = (int)gridPosition.x + xSize;
                int PosY = (int)gridPosition.y + ySize;

                // Check if the position is within bounds
                if (!InBounds(grid, itemToPlace, gridPosition))
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
    public static bool InBounds(InventoryGrid grid, ItemBase item, Vector2 gridPosition)
    {
        int gridWidth = grid.Width;
        int gridHeight = grid.Height;

        // Check if the rectDropUI fits within the grid bounds
        return gridPosition.x >= 0 && gridPosition.y >= 0 &&
               gridPosition.x + item.GridSize.x <= gridWidth &&
               gridPosition.y + item.GridSize.y <= gridHeight;
    }
    public static bool IsOccupied(InventoryGrid grid, Vector2 GridPos, ItemBase item)
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
    public static bool SameOverlapping(InventoryGrid grid, Vector2 gridPos, ItemBase newItem, ItemBase item)
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

