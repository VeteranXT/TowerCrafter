using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class GridUtils
{
    /// <summary>
    /// Marks the slots in the grid as occupied by the provided item.
    /// This updates the grid to track which slots are used by the given UI element.
    /// </summary>
    /// <param name="itemData">The item data containing grid size and position information.</param>
    /// <param name="UI">The UI element associated with the item.</param>
    /// <param name="gridPos">The position in the grid where the item starts.</param>
    /// <param name="grid">The inventory grid to update.</param>
    /// <param name="savesToItem">If true, saves the item's position to its data and categry and stash indxes.</param>
    /// <param name="cat">Optional category index.</param>
    /// <param name="stash">Optional stash index.</param>
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

    /// <summary>
    /// Checks for overlapping items in the grid where an item is being placed.
    /// Returns a set of unique overlapping DragDropUI elements.
    /// </summary>
    /// <param name="itemToPlace">The item being placed in the grid.</param>
    /// <param name="grid">The inventory grid being checked.</param>
    /// <returns>A HashSet containing overlapping DragDropUI items.</returns>
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

    /// <summary>
    /// Checks if an item is within the bounds of the grid.
    /// Ensures the item fits entirely within the grid dimensions.
    /// </summary>
    /// <param name="grid">The inventory grid to check.</param>
    /// <param name="item">The item being checked.</param>
    /// <param name="rectTransform">The RectTransform of the item.</param>
    /// <returns>True if the item is within bounds, otherwise false.</returns>
    public static bool InBounds(InventoryGrid grid, ItemBase item, RectTransform rectTransform)
    {
        int gridWidth = grid.Width;
        int gridHeight = grid.Height;
        var gridPosition = GetGridPositionFromAnchorPosition(grid, rectTransform);

        // Check if the item fits within the inventory grid bounds
        if (grid == null)
            return false;

        return
            gridPosition.x >= 0 &&
            gridPosition.y >= 0 &&
            gridPosition.x + item.GridSize.x <= gridWidth &&
            gridPosition.y + item.GridSize.y <= gridHeight;
    }

    /// <summary>
    /// Checks if two items overlap in the grid.
    /// Compares the positions and sizes of both items to determine if they occupy the same slots.
    /// </summary>
    /// <param name="grid">The inventory grid being checked.</param>
    /// <param name="gridPos">The grid position of the dragged item.</param>
    /// <param name="currentDragedItem">The currently dragged item.</param>
    /// <param name="item">The item being overlapped.</param>
    /// <returns>True if the items overlap, otherwise false.</returns>
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

    /// <summary>
    /// Converts a RectTransform's anchored position to a grid position.
    /// Calculates the grid cell based on the item's anchored position and grid dimensions.
    /// </summary>
    /// <param name="grid">The inventory grid being referenced.</param>
    /// <param name="rect">The RectTransform of the item.</param>
    /// <returns>The grid position as a Vector2Int.</returns>
    public static Vector2Int GetGridPositionFromAnchorPosition(InventoryGrid grid, RectTransform rect)
    {
        return new Vector2Int(
            Mathf.FloorToInt(rect.anchoredPosition.x / grid.Width),
            Mathf.FloorToInt(-rect.anchoredPosition.y / grid.Height)
        );
    }

    /// <summary>
    /// Converts an item's grid position to an anchored position in the grid.
    /// Aligns the item's anchor point based on its position in the grid.
    /// </summary>
    /// <param name="grid">The inventory grid being referenced.</param>
    /// <param name="item">The item whose position is being converted.</param>
    /// <returns>The anchored position as a Vector2.</returns>
    public static Vector2 GetAnchorPositionFromItem(InventoryGrid grid, ItemBase item)
    {
        return new Vector2(
            Mathf.FloorToInt(item.GridPosition.x * grid.Width),
            Mathf.FloorToInt(item.GridPosition.y * grid.Height)
        );
    }

    /// <summary>
    /// Converts a grid position to an anchored position in the grid.
    /// Aligns the item's anchor point based on its grid cell position.
    /// </summary>
    /// <param name="grid">The inventory grid being referenced.</param>
    /// <param name="gridPosition">The grid position of the item.</param>
    /// <returns>The anchored position as a Vector2.</returns>
    public static Vector2 GetAnchorPositionFromGridPosition(InventoryGrid grid, Vector2 gridPosition)
    {
        return new Vector2(
            Mathf.FloorToInt(gridPosition.x * grid.Width),
            Mathf.FloorToInt(-gridPosition.y * grid.Width)
        );
    }
}