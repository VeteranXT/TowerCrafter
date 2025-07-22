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
    [SerializeField] private DragDropUI prefabDragDropUI;

    public DragDropUI GetPrefabUI { get { return prefabDragDropUI; } }
    public void Start()
    {
        InventoryGrid.EventOnGridChanged += OnGridChanged;
        InventoryGrid.EventDragedDroped += OnDrop;
        DragDropUI.EventOnClick += HandleItemClickPick;
        DragDropUI.EventBeginDrag += HandleBeginDraging;
        DragDropUI.EventOnDraged += HandleDragging;
        DragDropUI.EventEndDrag += HandleEndDragging;

        
        canvas = FindFirstObjectByType<Canvas>();
    }


    private void HandleBeginDraging(DragDropUI uI)
    {
        uI.CanvasGroup.blocksRaycasts = true;
        uI.CanvasGroup.alpha = 0.7f; 
    }

    //While draging update Position
    private void HandleDragging(DragDropUI uI)
    {
        uI.RectTransform.position = Input.mousePosition / canvas.scaleFactor;
    }
    private void HandleEndDragging(DragDropUI uI)
    {
        uI.CanvasGroup.blocksRaycasts = false;
        uI.CanvasGroup.alpha = 1f;
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
                GridUtils.MarkSlots(Droped, oldGridPos, hoveredGrid, false);
                return;
            }
            if(!GridUtils.InBounds(hoveredGrid, Droped.ItemData, oldGridPos))
            {
                Droped.RectTransform.anchoredPosition = OrginalAnchorPositon;
                GridUtils.MarkSlots(Droped, oldGridPos, hoveredGrid, false);
            }
            if (hoveredGrid != null) 
            { 
            }
            var DropedCordinates = Droped.RectTransform.anchoredPosition;
        }

    }
    private void HandleItemClickPick(DragDropUI uI)
    {
        Debug.Log("Clicked:" + uI.name);
        //if (uI == null) return;
        //var itemSize = uI.ItemData.GridPosition;
        //// Unmark all slots occupied by the rectDropUI
        //GridUtils.MarkSlots(null, itemSize, hoveredGrid, false);

        //// Set the currently picked-up rectDropUI
        //clickedUp = uI;

        //// Mark it as clicked and start dragging
        //eventPickedUp = true;
    }

    private void OnGridChanged(InventoryGrid grid)
    {
        hoveredGrid = grid;
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