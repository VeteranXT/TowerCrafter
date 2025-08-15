using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragDropUI : BaseMonoBehaviour, IDraggable, IHoveredInfo, IPointerClickHandler
{
    #region Fields

    [SerializeField] private ItemBase itemData;
    [SerializeField] private RectTransform rectTransfom;
    [SerializeField] private Image background;
    [SerializeField] private Image itemPrerview;
    [SerializeField] private CanvasGroup canvasGroup;
    private bool isDragged = false;

    #endregion
    protected override void FindReferences()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void SetupItem(ItemBase item)
    {
        itemData = item;
        rectTransfom.sizeDelta = itemData.GridSize;
    }
    #region Properties
    public ItemBase ItemData { get { return itemData; } set { itemData = value; } }
    public RectTransform RectTransform { get { return rectTransfom; } set { rectTransfom = value; } }
    public CanvasGroup CanvasGroup { get { return canvasGroup; } set { canvasGroup = value; } }
    public bool IsDragged { get { return isDragged; } set { isDragged = value; } }

    #endregion

    #region Events

    public static event Action<DragDropUI> EventOnClick;
    public static event Action<DragDropUI> EventBeginDrag;
    public static event Action<PointerEventData> EventOnDraged;
    public static event Action<DragDropUI> EventEndDrag;
    public static event Action<IInformation> OnToolTipEvent;

    #endregion

    #region Drag Drop Events
    public void OnBeginDrag(PointerEventData eventData)
    {
        EventBeginDrag?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        EventOnDraged?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EventEndDrag?.Invoke(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventOnClick?.Invoke(this);
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