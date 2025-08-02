using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryGrid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private DragDropUI[,] grid;
    [SerializeField] private int slotWidth = 48;
    [SerializeField] private int slotHeight = 48;
    [SerializeField] private int rows = 12;
    [SerializeField] private int collums = 18;
    [SerializeField] private RectTransform parentHolder;
    public static event Action<InventoryGrid> EventOnGridChanged;
    public static event Action<PointerEventData> EventDragedDroped;
    public int Width { get { return slotWidth; } }
    public int Height { get { return slotHeight; } }
    public DragDropUI[,] GetGrid { get { return grid; } set { grid = value; } }
    public RectTransform GridRect { get { return parentHolder; } }
    private void OnValidate()
    {
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
        parentHolder.sizeDelta = new Vector2 (collums * slotWidth,  rows * slotHeight);

        bool debug = false;
        if (debug)
        {
            //grid[0,0] = 
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
        EventDragedDroped?.Invoke(eventData);
    }
}

