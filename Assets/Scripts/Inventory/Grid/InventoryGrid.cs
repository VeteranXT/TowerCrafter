using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryGrid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemData[,] grid;
    [SerializeField] private int slotWidth = 48;
    [SerializeField] private int slotHeight = 48;
    [SerializeField] private int rows = 12;
    [SerializeField] private int collums = 18;
    [SerializeField] private RectTransform parentHolder;
    [SerializeField] private Image slotsImage;
    [SerializeField] private GridLayoutGroup gridlayout;

    public static event Action<InventoryGrid> OnInventoryGrid;
    public int Width { get { return slotWidth; } }
    public int Height { get { return slotHeight; } }
    public ItemData[,] GetGrid { get { return grid; } set { grid = value; } }
    public RectTransform GridRect { get { return parentHolder; } }
    private void OnValidate()
    {
        parentHolder.sizeDelta = new Vector2(slotWidth * rows, slotHeight * collums);
    }

    private void Start()
    {
        slotsImage = GetComponent<Image>();
        slotsImage.type = Image.Type.Tiled;
        gridlayout = GetComponent<GridLayoutGroup>();
        parentHolder = GetComponent<RectTransform>();
        if (parentHolder == null)
        {
            Debug.LogError("Coudn't find parent");
            return;
        }
       
        gridlayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridlayout.constraintCount = collums;
        gridlayout.cellSize = new Vector2(slotWidth, slotHeight);

        grid = new ItemData[rows, collums];
        parentHolder.sizeDelta = new Vector2 (collums * slotWidth,  rows * slotHeight);


    }
    public void CreateInventoryGrid(int rows, int collums)
    {
      
        if (parentHolder == null)
        {
            Debug.LogError("Coudn't find parent");
            return;
        }
        grid = new ItemData[rows, collums];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < collums; j++)
            {
                GameObject gameObject = new GameObject();
                Image sprite = gameObject.AddComponent<Image>();
                sprite.transform.position = new Vector3(slotWidth * i, slotHeight * j);
                sprite.transform.SetParent(parentHolder, true);

                //GetGrid[i, j]
            }
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {

        OnInventoryGrid?.Invoke(null);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnInventoryGrid?.Invoke(this);
    }
}

