using Assets.Scripts.Interfaces;
using Pathfinding.Util;
using System;
using TowerCrafter.Grid;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragDropUI : MonoBehaviour, IDraggable, IHoveredInfo, IPointerClickHandler
{
    #region Fields

    [SerializeField] private ItemBase itemData;
    [SerializeField] private RectTransform rectTransfom;
    [SerializeField] private Image background;
    [SerializeField] private Image itemPrerview;
    [SerializeField] private CanvasGroup canvasGroup;
    private bool isDragged = false;


    public Image GetItemIcon { get { return itemPrerview; } }
    #endregion
    public void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetupItem(ItemBase item, InventoryGrid grid )
    {
        if (grid is null)
        {
            throw new ArgumentNullException(nameof(grid));
        }

        itemData = item ?? throw new ArgumentNullException(nameof(item));
        rectTransfom.sizeDelta = itemData.GridSize * grid.CellSize;
        if (itemData.ItemIcon != null) 
        {
            itemPrerview.sprite = itemData.ItemIcon;
        }
        else
        {
            var r = UnityEngine.Random.Range(15f, 255f);
            var b = UnityEngine.Random.Range(15f, 255f);
            var g = UnityEngine.Random.Range(15f, 255f);
            itemPrerview.color = new Color(r, b, g, 255f);
        }
    }
    #region Properties
    public ItemBase ItemData { get { return itemData; } set { itemData = value; } }
    public RectTransform RectTransform { get { return rectTransfom; } set { rectTransfom = value; } }
    public CanvasGroup CanvasGroup { get { return canvasGroup; } set { canvasGroup = value; } }
    public bool IsDragged { get { return isDragged; } set { isDragged = value; } }
    #endregion

    #region Events

    public static event Action<PointerEventData> EventOnClick;
    public static event Action<PointerEventData> EventBeginDrag;
    public static event Action<PointerEventData> EventOnDraged;
    public static event Action<PointerEventData> EventEndDrag;
    public static event Action<IInformation> OnToolTipEvent;

    #endregion

    #region Drag Drop Events
    public void OnBeginDrag(PointerEventData eventData)
    {
        EventBeginDrag?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        EventOnDraged?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EventEndDrag?.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventOnClick?.Invoke(eventData);
        isDragged = true;
    }
    #endregion

    #region ToolTip
    public void OnPointerExit(PointerEventData eventData)
    {
        OnToolTipEvent?.Invoke(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnToolTipEvent?.Invoke(itemData);
    }
    #endregion

    

}