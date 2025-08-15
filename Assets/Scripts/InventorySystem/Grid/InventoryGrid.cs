using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TowerCrafter.Grid.Utlis;
using System.Collections.Generic;
using System.Linq;

namespace TowerCrafter.Grid
{
    public class InventoryGrid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
        protected DragDropUI[,] grid;
        [SerializeField] protected int slotWidth = 32;
        [SerializeField] protected int slotHeight = 32;
        [SerializeField] protected int rows = 12;
        [SerializeField] protected int collums = 18;
        [SerializeField] protected RectTransform rectTransform;

        public static event Action<InventoryGrid> EventOnGridChanged;
        public static event Action<DragDropUI> EventDragedDroped;
        public int Width { get { return slotWidth; } }
        public int Height { get { return slotHeight; } }
        public DragDropUI[,] GetGrid { get { return grid; } }
        public RectTransform GridRect { get { return rectTransform; } }


     


        private void Awake()
        {
            // slotsImage.type = Image.Type.Tiled;;
            rectTransform = GetComponent<PlayerGridTag>().GetComponent<RectTransform>();
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
    }

}

