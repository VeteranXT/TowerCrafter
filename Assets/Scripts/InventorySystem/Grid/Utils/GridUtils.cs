using System.Collections.Generic;
using UnityEngine;

namespace TowerCrafter.Grid.Utlis
{
    public static class GridUtils
    {
        public static void MarkSlots(this InventoryGrid grid, ItemBase itemData, DragDropUI UI, Vector2 gridPos)
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
                    if (UI != null)
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
        public static bool InBounds(this InventoryGrid grid, ItemBase item, Vector2Int gridPosition)
        {
            for (int x = 0; x < item.GridSize.x; x++)
            {
                for (int y = 0; y < item.GridSize.y; y++)
                {
                    int checkX = gridPosition.x + x;
                    int checkY = gridPosition.y + y;

                    if (checkX < 0 || checkX >= grid.Width || checkY < 0 || checkY >= grid.Height)
                    {
                        return false; // Out of bounds
                    }
                }
            }
            return true;

        }
        public static Vector2Int GridPositionFromAnchorPosition(this InventoryGrid grid, RectTransform rect)
        {
            return new Vector2Int(
                Mathf.RoundToInt(rect.anchoredPosition.x / grid.Width),
                Mathf.RoundToInt(-rect.anchoredPosition.y / grid.Height)
            );
        }
        public static Vector2 AnchorPositionFromItem(this InventoryGrid grid, ItemBase item)
        {
            return new Vector2(
                Mathf.RoundToInt(item.GridPosition.x * grid.Width),
                Mathf.RoundToInt(item.GridPosition.y * grid.Height)
            );
        }
        public static Vector2 AnchorPositionFromGridPosition(this InventoryGrid grid, Vector2 gridPosition)
        {
            return new Vector2(
                Mathf.FloorToInt(gridPosition.x * grid.Width),
                Mathf.FloorToInt(-gridPosition.y * grid.Width)
            );
        }
        public static void ReturnToOrginalPosition(this InventoryGrid grid, DragDropUI dragDrop)
        {
            dragDrop.RectTransform.anchoredPosition = AnchorPositionFromItem(grid, dragDrop.ItemData);
        }
        public static void Place(this InventoryGrid grid, DragDropUI dragged, Vector2Int gridPos)
        {
            if (InBounds(grid, dragged.ItemData, gridPos))
            {
                dragged.RectTransform.anchoredPosition = AnchorPositionFromGridPosition(grid, gridPos);
                MarkSlots(grid, dragged.ItemData, dragged, gridPos);
                dragged.ItemData.GridPosition = gridPos;
                return;
            }
            Debug.Log("Trying to place: " + dragged.name + "at Cord:" + gridPos + " Is out of bounds");
            ;
        }
        public static HashSet<DragDropUI> CountOverlaps(this InventoryGrid grid, DragDropUI currentlyDragging)
        {
            HashSet<DragDropUI> overlappingItems = new HashSet<DragDropUI>();
            overlappingItems.Clear();
            var item = currentlyDragging.ItemData;
            var gridPosition = GridPositionFromAnchorPosition(grid, currentlyDragging.RectTransform);

            if (grid == null) return null;
            if (!InBounds(grid, item, gridPosition)) return null;

            for (int x = 0; x < item.GridSize.x; x++)
            {
                for (int y = 0; y < item.GridSize.y; y++)
                {
                    int xPos = (int)gridPosition.x + x;
                    int yPos = (int)gridPosition.y + y;

                    var Drag = grid.GetGrid[xPos, yPos];

                    if (Drag != null)
                        overlappingItems.Add(Drag);
                }
            }
            return overlappingItems;
        }
        public static void ReturnToOrginalPosition(this InventoryGrid grid, DragDropUI dragDrop)
        {
            dragDrop.RectTransform.anchoredPosition = AnchorPositionFromItem(grid, dragDrop.ItemData);
        }

        public static void Place(this InventoryGrid grid, DragDropUI dragged, Vector2Int gridPos)
        {
            if (InBounds(grid, dragged.ItemData, gridPos))
            {
                dragged.RectTransform.anchoredPosition = AnchorPositionFromGridPosition(grid, gridPos);
                MarkSlots(grid, dragged.ItemData, dragged, gridPos);
                dragged.ItemData.GridPosition = gridPos;
                return;
            }
            Debug.Log("Trying to place: " + dragged.name + "at Cord:" + gridPos + " Is out of bounds");
            ;
        }
        public static bool CanSwap(this InventoryGrid grid, DragDropUI dragged, DragDropUI overlapped)
        {
            // Get the current grid position of the dragged item
            var dropGridPos = GridPositionFromAnchorPosition(grid, dragged.RectTransform);
            var orginalOriginPosition = dragged.ItemData.GridPosition;

            // Check if the dragged item's new position overlaps with the overlapped item's current position
            for (int x = 0; x < dragged.ItemData.GridSize.x; x++)
            {
                for (int y = 0; y < dragged.ItemData.GridSize.y; y++)
                {
                    int draggedNewPosX = dropGridPos.x + x;
                    int draggedNewPosY = dropGridPos.y + y;

                    var overlappedCell = grid.GetGrid[draggedNewPosX, draggedNewPosY];
                    if (overlappedCell != null && overlappedCell != overlapped)
                    {
                        return false; // A third item is occupying this cell
                    }

                    for (int x2 = 0; x2 < overlapped.ItemData.GridSize.x; x2++)
                    {
                        for (int y2 = 0; y2 < overlapped.ItemData.GridSize.y; y2++)
                        {
                            int overlappedPosX = orginalOriginPosition.x + x2;
                            int overlappedPosY = orginalOriginPosition.y + y2;

                            // If any cell of the dragged item's new position overlaps with the overlapped item's current position and not 3th item, return false
                            if (draggedNewPosX == overlappedPosX && draggedNewPosY == overlappedPosY)
                            {
                                var overlappedCell2 = grid.GetGrid[overlappedPosX, overlappedPosY];
                                if (overlappedCell2 != null && overlappedCell2 != overlapped)
                                {
                                    return false; // A third item is occupying this cell
                                }
                                return false;
                            }
                        }
                    }
                }
            }


            // Ensure the dragged item's new position is within bounds
            if (!InBounds(grid, dragged.ItemData, dropGridPos))
            {
                return false;
            }

            // Ensure the overlapped item's new position is within bounds
            if (!InBounds(grid, overlapped.ItemData, orginalOriginPosition))
            {
                return false;
            }

            return true; // Swap is valid
        }
        public static bool TryFindFirstAvailablePosition(this InventoryGrid grid, ItemBase item, out Vector2Int gridPosition)
        {
            gridPosition = new Vector2Int(-1, -1);

            // Loop over every possible top-left position in the grid
            for (int x = 0; x < grid.GetGrid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetGrid.GetLength(1); y++)
                {
                    // Check if the item would be fully inside the grid from this position
                    if (!InBounds(grid, item, new Vector2Int(x, y)))
                        continue;

                    bool canPlaceHere = true;

                    // Check all cells that the item would occupy
                    for (int offsetX = 0; offsetX < item.GridSize.x; offsetX++)
                    {
                        for (int offsetY = 0; offsetY < item.GridSize.y; offsetY++)
                        {
                            int checkX = x + offsetX;
                            int checkY = y + offsetY;

                            if (grid.GetGrid[checkX, checkY] != null)
                            {
                                canPlaceHere = false;
                                break; // stop checking this position
                            }
                        }

                        if (!canPlaceHere) break; // break outer footprint loop
                    }

                    // If we found a fit, return it immediately
                    if (canPlaceHere)
                    {
                        gridPosition = new Vector2Int(x, y);
                        return true;
                    }
                }
            }

            // No valid position found
            return false;
        }
    }
}
