using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropUI
    : MonoBehaviour, IBeginDragHandler, IDragHandler ,IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ItemBase itemData;
    [SerializeField] private RectTransform rectTransfom;
    [SerializeField] private Image background;
    [SerializeField] private Image itemPrerview;
    [SerializeField] private CanvasGroup canvasGroup;
    public void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public ItemBase ItemData { get { return itemData; } set { itemData = value; } }
    public RectTransform RectTransform { get { return rectTransfom; } set { rectTransfom = value; } } 
    public CanvasGroup CanvasGroup { get { return canvasGroup; } set { canvasGroup = value; } }

    public static event Action<DragDropUI> EventOnClick;
    public static event Action<DragDropUI> EventBeginDrag;
    public static event Action<DragDropUI> EventOnDraged;
    public static event Action<DragDropUI> EventEndDrag;
    public static event Action<IInformation> OnToolTipEvent;

    #region Drag Drop Events
    public void OnBeginDrag(PointerEventData eventData)
    {

        //canvasGroup.blocksRaycasts = false;
        //canvasGroup.alpha = 0.7f;
        EventBeginDrag?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        EventOnDraged?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //canvasGroup.blocksRaycasts = true;
        //canvasGroup.alpha = 1f;
        EventEndDrag?.Invoke(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventOnClick?.Invoke(this);
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