using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private RectTransform rectTransfom;
    [SerializeField] private Image background;
    [SerializeField] private Image itemPrerview;
    [SerializeField] private CanvasGroup canvasGroup;

    public void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public ItemData ItemData { get { return itemData; } set { itemData = value; } }
    public RectTransform GetRect { get { return rectTransfom; } set { rectTransfom = value; } } 

    public static event Action<DragDropUI> EventOnClick;
    public static event Action<DragDropUI> EventBeginDraged;
    public static event Action<DragDropUI> EventOnDraged;
    public static event Action<DragDropUI> EventEndDraged;
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 0.7f;
        EventBeginDraged?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        EventOnDraged?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 1f;
        EventEndDraged?.Invoke(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventOnClick?.Invoke(this);
    }

}