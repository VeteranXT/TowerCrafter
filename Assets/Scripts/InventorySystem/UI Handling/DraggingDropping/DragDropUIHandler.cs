
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TowerCrafter.Grid;
using TowerCrafter.Grid.Utlis;
using UnityEditor;

public class DragDropUIHandler : MonoBehaviour
{
    [SerializeField] private InventoryGrid hoveredGrid;
    [SerializeField] private InventoryGrid oldGrid;
    [SerializeField] private DragDropUI selectedUI;
    [SerializeField] private Canvas canvas;

    public void Start()
    {
        InventoryGrid.EventOnGridChanged += HandleGridChange;
        InventoryGrid.EventDragedDroped += HandleDrop;
        DragDropUI.EventOnClick += HandleClicked;
        DragDropUI.EventBeginDrag += HandleBeginDragging;
        DragDropUI.EventOnDraged += HandleDragging;
        DragDropUI.EventEndDrag += HandleEndDragging;

        canvas = FindAnyObjectByType<Canvas>();
    }
    private void Update()
    {
        if (selectedUI == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            selectedUI.RectTransform.parent as RectTransform,
            Input.mousePosition,
            null,
            out Vector2 localPoint
        );

        // Get size of the RectTransform
        Vector2 size = selectedUI.RectTransform.rect.size;

        // Since pivot is (0,1), offset to center:
        Vector2 centerOffset = new Vector2(size.x * 0.5f, -size.y * 0.5f);

        selectedUI.RectTransform.anchoredPosition = localPoint - centerOffset;
    }

    #region Event handlers
    private void HandleBeginDragging(PointerEventData data)
    {
        var drop = data.pointerDrag.GetComponent<DragDropUI>();
        drop.CanvasGroup.blocksRaycasts = false;
        drop.CanvasGroup.alpha = 0.7f;
        
        
        GridUtils.MarkSlots(hoveredGrid, drop.ItemData, null, drop.ItemData.GridPosition);
        oldGrid = hoveredGrid;
        selectedUI = drop;

    }
    private void HandleDragging(PointerEventData data)
    {
        data.pointerDrag.GetComponent<RectTransform>().anchoredPosition += data.delta / canvas.scaleFactor; 
    }
    private void HandleEndDragging(PointerEventData data)
    {
        var drop = data.pointerDrag.GetComponent<DragDropUI>();
        drop.CanvasGroup.blocksRaycasts = true;
        drop.CanvasGroup.alpha = 1f;


        if (hoveredGrid == null)
        {
            oldGrid.ReturnToOrginalPosition(drop);
            oldGrid = null;
            selectedUI = null;
            return;
        }
    }
    private void HandleClicked(PointerEventData data)
    {
        if (selectedUI == null)
        {
            selectedUI = data.pointerClick.GetComponent<DragDropUI>();
            hoveredGrid.MarkSlots(selectedUI.ItemData, null, selectedUI.ItemData.GridPosition);
            return;
        }

        if (selectedUI != null)
        {
            if(hoveredGrid == null) return;
            HandleDrop(selectedUI);
        }

    }
    private void HandleDrop(DragDropUI data)
    {
        if (data == null || data.ItemData == null) return;

        if (hoveredGrid == null) return;

        var dragItemData = data.ItemData;
        Vector2Int hoveredGridPos = hoveredGrid.GridPositionFromAnchorPosition(data.RectTransform);

        if (!hoveredGrid.InBounds (dragItemData, hoveredGridPos)) return;

        var list = hoveredGrid.CountOverlaps(data);
        if(list == null) return;
        if (list.Count == 0)
        {
            hoveredGrid.Place(data, hoveredGridPos);
            if (selectedUI != null)
            {
                selectedUI = null;
            }
            return;
        }
        if (list.Count == 1)
        {
            var overlappedUI = list.FirstOrDefault();

            Debug.Log("Overlaping with: " + overlappedUI.name);

            if (hoveredGrid.CanSwap(data, overlappedUI))
            {
                if (hoveredGrid.GetGrid.GetType() == typeof(PlayerStorageGrid)) { }
                Debug.Log("Swaping with: " + overlappedUI.name);
                hoveredGrid.MarkSlots( overlappedUI.ItemData, null, overlappedUI.ItemData.GridPosition);
                hoveredGrid.Place(overlappedUI, data.ItemData.GridPosition);
                hoveredGrid.Place(data, hoveredGridPos);
                //We swaped items and no need to update 
                if(selectedUI != null) 
                { 
                    selectedUI = null; 
                }
                return;
            }
            else
            {
                hoveredGrid.Place(data, hoveredGridPos);
                hoveredGrid.MarkSlots(data.ItemData, data, data.ItemData.GridPosition);

                hoveredGrid.MarkSlots(overlappedUI.ItemData, null, overlappedUI.ItemData.GridPosition);
                selectedUI = overlappedUI;
            }
        }
    }
    private void HandleGridChange(InventoryGrid grid)
    {
        hoveredGrid = grid;
    }
    private void HandleCloseWindow()
    {
        oldGrid.ReturnToOrginalPosition(selectedUI);
    }
    #endregion

}