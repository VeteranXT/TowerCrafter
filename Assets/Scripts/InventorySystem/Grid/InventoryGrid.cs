using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TowerCrafter.Grid.Utlis;
using NUnit.Framework.Constraints;

namespace TowerCrafter.Grid
{
    public class InventoryGrid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
        protected DragDropUI[,] grid;
        [SerializeField] protected int slotWidth = 32;
        [SerializeField] protected int slotHeight = 32;
        [SerializeField] protected int rows = 12;
        [SerializeField] protected int collums = 18;
        [SerializeField] protected RectTransform parentHolder;

        public static event Action<InventoryGrid> EventOnGridChanged;
        public static event Action<DragDropUI> EventDragedDroped;
        public int Width { get { return slotWidth; } }
        public int Height { get { return slotHeight; } }
        public DragDropUI[,] GetGrid { get { return grid; } set { grid = value; } }
        protected RectTransform GridRect { get { return parentHolder; } }

        public Vector2Int GetGridSize { get { return new Vector2Int(Width, Height); } }

        public Vector2Int CellSize { get { return new Vector2Int(slotWidth, slotHeight); } }
        public virtual void OnValidate()
        {
            parentHolder = GetComponent<PlayerGridTag>().GetComponent<RectTransform>();
            if (parentHolder == null)
            {
                Debug.LogError("Coudn't find parent");
                return;
            }
            grid = new DragDropUI[collums, rows];
            parentHolder.sizeDelta = new Vector2(collums * slotWidth, rows * slotHeight);
        }
        private void Awake()
        {
            parentHolder = GetComponent<PlayerGridTag>().GetComponent<RectTransform>(); 
            if (parentHolder == null)
            {
                Debug.LogError("Coudn't find parent");
                return;
            }
            grid = new DragDropUI[collums, rows];
            parentHolder.sizeDelta = new Vector2(collums * slotWidth, rows * slotHeight);
        }
        private void Start()
        {
            parentHolder = GetComponentInChildren<PlayerGridTag>().GetComponent<RectTransform>();
            if (parentHolder == null)
            {
                Debug.LogError("Coudn't find parent");
                return;
            }

            grid = new DragDropUI[collums, rows];
            parentHolder.sizeDelta = new Vector2(collums * slotWidth, rows * slotHeight);
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

