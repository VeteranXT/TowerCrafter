
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TowerCrafter.Grid;
using TowerCrafter.Grid.Utlis;
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
        var curretDropCordintates = hoveredGrid.GridPositionFromAnchorPosition( drop.RectTransform);

        if (hoveredGrid == null && !hoveredGrid.InBounds( drop.ItemData, curretDropCordintates))
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
            hoveredGrid.MarkSlots( selectedUI.ItemData, null, selectedUI.ItemData.GridPosition);
            return;
        }

        if (selectedUI != null)
        {
            HandleDrop(selectedUI);
        }

    }
    private void HandleDrop(DragDropUI data)
    {
        if(data == null || data.ItemData == null) return;

        if(hoveredGrid == null) return;

        var item = data.ItemData;
        var originPosition = item.GridPosition;
        var dropCordinates = hoveredGrid.GridPositionFromAnchorPosition(data.RectTransform);

        if (!hoveredGrid.InBounds(item, dropCordinates)) return;

            var list = hoveredGrid.CountOverlaps( data);
            var count = list.Count;
        if (list.Count == 0) return;

            var ovrlapedUI = list.FirstOrDefault();
        if (count > 1) return;

        if (count == 1)
        {
            if (hoveredGrid.CanSwap(data, ovrlapedUI))
            {
                hoveredGrid.MarkSlots(ovrlapedUI.ItemData, null, ovrlapedUI.ItemData.GridPosition);
                hoveredGrid.Place(ovrlapedUI, originPosition);
                hoveredGrid.Place(data, dropCordinates);
                if(selectedUI != null)
                {
                    selectedUI = null;
                }
                
                
            }
            else
            {
                hoveredGrid.Place(data, dropCordinates);
                selectedUI = ovrlapedUI;
                hoveredGrid.MarkSlots(selectedUI.ItemData, null, selectedUI.ItemData.GridPosition);
            }
          
          
        }
        else if (count == 0)
        {
            hoveredGrid.Place(data, dropCordinates);
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