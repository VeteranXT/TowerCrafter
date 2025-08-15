using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TowerCrafter.Grid.Utlis;
using System.Collections.Generic;
using System.Linq;

namespace TowerCrafter.Grid
{
    public class CustomGrid : BaseMonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
        protected DragDropUI[,] grid;
        [SerializeField] protected int slotWidth = 32;
        [SerializeField] protected int slotHeight = 32;
        [SerializeField] protected int rows = 12;
        [SerializeField] protected int collums = 18;
        [SerializeField] protected RectTransform rectTransform;

        public static event Action<CustomGrid> EventOnGridChanged;
        public static event Action<DragDropUI> EventDragedDroped;
        public int Width { get { return rows; } }
        public int Height { get { return collums; } }
        public Vector2Int CellSize { get { return new Vector2Int(slotWidth, slotHeight); } }
        public DragDropUI[,] GetGrid { get { return grid; } }
        public RectTransform GridRect { get { return rectTransform; } }


        protected override void FindReferences()
        {
            rectTransform = GetComponent<PlayerGridTag>().GetComponent<RectTransform>();
        }
        protected override void Awake()
        {
            base.Awake();
            // slotsImage.type = Image.Type.Tiled;;
            if (rectTransform == null)
            {
                Debug.LogError("Coudn't find parent");
                return;
            }


            grid = new DragDropUI[collums, rows];
            rectTransform.sizeDelta = new Vector2(collums * slotWidth, rows * slotHeight);
        }
        public void OnPointerExit(PointerEventData eventData)
        {

            EventOnGridChanged?.Invoke(null);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            EventOnGridChanged?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            var drag = eventData.pointerDrag.GetComponent<DragDropUI>();
            EventDragedDroped?.Invoke(drag);
        }

        public virtual bool CanAccept(ItemBase item)
        {
            return true;
        }
    }

}

