using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TowerCrafter.Grid.Utlis;

namespace TowerCrafter.Grid
{
    public class InventoryGrid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
        private DragDropUI[,] grid;
        [SerializeField] private int slotWidth = 32;
        [SerializeField] private int slotHeight = 32;
        [SerializeField] private int rows = 12;
        [SerializeField] private int collums = 18;
        [SerializeField] private RectTransform parentHolder;

        public static event Action<InventoryGrid> EventOnGridChanged;
        public static event Action<DragDropUI> EventDragedDroped;
        public int Width { get { return slotWidth; } }
        public int Height { get { return slotHeight; } }
        public DragDropUI[,] GetGrid { get { return grid; } set { grid = value; } }
        public RectTransform GridRect { get { return parentHolder; } }

        [SerializeField] bool debug = false;
        [SerializeField] private ItemBase[] items;
        [SerializeField] private InventoryGrid tempGrid;
        [SerializeField] private DragDropUI prefab;
        private void DebugCreate(ItemBase[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                this.TryFindFirstAvailablePosition(item, out Vector2Int pos);
                GeneralFactory.CreateDragDropUI(tempGrid.transform.parent, prefab, item, pos, tempGrid);
            }
        }
        public virtual void OnValidate()
        {
            parentHolder = GetComponentInChildren<PlayerGridTag>().GetComponent<RectTransform>();

            parentHolder.sizeDelta = new Vector2(slotWidth * collums, slotHeight * rows);
        }


        private void Awake()
        {
            // slotsImage.type = Image.Type.Tiled;;
            parentHolder = GetComponent<RectTransform>();
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
            if (debug)
            {
                DebugCreate(items);
            }
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

