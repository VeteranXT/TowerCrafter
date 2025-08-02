using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropUI : MonoBehaviour, IBeginDragHandler, IDragHandler ,IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    #region Fields
    [SerializeField] private ItemBase itemData;
    [SerializeField] private RectTransform rectTransfom;
    [SerializeField] private Image background;
    [SerializeField] private Image itemPrerview;
    [SerializeField] private CanvasGroup canvasGroup;
    private bool isDragged = false;
    #endregion
    public void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void OnValidate()
    {
        if (itemData != null) 
        {
            var Grid = GetComponentInParent<InventoryGrid>();
            rectTransfom.sizeDelta = new Vector2(itemData.GridSize.x * Grid.Width, itemData.GridSize.y * Grid.Height);
            itemPrerview.sprite = itemData.ItemIcon;
        }
    }
    #region Properties
    public ItemBase ItemData { get { return itemData; } set { itemData = value; } }
    public RectTransform RectTransform { get { return rectTransfom; } set { rectTransfom = value; } } 
    public CanvasGroup CanvasGroup { get { return canvasGroup; } set { canvasGroup = value; } }
    public bool IsDragged { get { return isDragged; } set { isDragged = value; } }
    #endregion

    #region Events

    public static event Action<DragDropUI> EventOnClick;
    public static event Action<PointerEventData> EventBeginDrag;
    public static event Action<PointerEventData> EventOnDraged;
    public static event Action<PointerEventData> EventEndDrag;
    public static event Action<IInformation> OnToolTipEvent;

    #endregion

    #region Drag Drop Events
    public void OnBeginDrag(PointerEventData eventData)
    {
        //canvasGroup.blocksRaycasts = false;
        //canvasGroup.alpha = 0.7f;
        EventBeginDrag?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        EventOnDraged?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //canvasGroup.blocksRaycasts = true;
        //canvasGroup.alpha = 1f;
        EventEndDrag?.Invoke(eventData);
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