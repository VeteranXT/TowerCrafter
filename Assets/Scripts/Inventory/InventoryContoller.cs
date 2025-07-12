using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;


public class InventoryContoller : MonoBehaviour, IDropHandler
{
    [SerializeField] private InventoryGrid hoveredGrid;
    [SerializeField] private bool isClickedPickUp = false;
    [SerializeField] DragDropUI clickedUp;
    [SerializeField] Canvas canvas;


    public void Start()
    {
        InventoryGrid.OnInventoryGrid += GetCurrentGrid;
        DragDropUI.EventOnClick += HandleClickPickUp;

        canvas = FindFirstObjectByType<Canvas>();

    }
    private void Update()
    {
        if (!isClickedPickUp) return;
        clickedUp.transform.position += Input.mousePosition / canvas.scaleFactor;
    }

    private void HandleClickPickUp(DragDropUI uI)
    {
        isClickedPickUp = true;
        clickedUp = uI;
    }

    private void GetCurrentGrid(InventoryGrid grid)
    {
        hoveredGrid = grid;
    }
    public void OnDrop(PointerEventData eventData)
    {
        var item = eventData.pointerDrag.GetComponent<DragDropUI>().ItemData;
        var dropedCanvasPosition = eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition;
        var gridDropedPos = GetGridPositionFromRectAnchor(hoveredGrid, dropedCanvasPosition);
        if (hoveredGrid == null || !InBounds(hoveredGrid, item, gridDropedPos))
        {
            //TO DO
            //Return to orignal position
        }
        else
        {
            if (InBounds(hoveredGrid, item, gridDropedPos))
            {
            }

        }
    }

    #region Helper Methods
    private Vector2 GetGridPositionFromRectAnchor(InventoryGrid grid, Vector2 rect)
    {
        return new Vector2Int(Mathf.FloorToInt(rect.x / grid.Width), Mathf.FloorToInt(-rect.y / grid.Height));
    }
    private  Vector2 GetGridPositionFromMouse(InventoryGrid grid)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(grid.GridRect, Input.mousePosition, null, out Vector2 local);
        return new Vector2Int(Mathf.FloorToInt(local.x / grid.Width), Mathf.FloorToInt(-local.y / grid.Height));
    }
    private  Vector2 GetCanvasPositionFromGridPosition(InventoryGrid grid, Vector2 gridPosition)
    {
        return new Vector2Int((int)gridPosition.x * grid.Width, (int)-gridPosition.y * grid.Height);
    }
    private bool InBounds(InventoryGrid grid, ItemData item, Vector2 gridPosition)
    {
        for (int i = 0; i < item.GridSize.x; i++)
        {
            for (int j = 0; j < item.GridSize.y; j++)
            {
                int posX = i + (int)gridPosition.x;
                int posY = j + (int)gridPosition.y;

                if (posX < 0 && posX >= grid.GetGrid.GetLength(0) && posY < 0 && posY >= grid.GetGrid.GetLength(1))
                {
                    return false;
                }
            }
        }
        return true;
    }
 
    #endregion
}
