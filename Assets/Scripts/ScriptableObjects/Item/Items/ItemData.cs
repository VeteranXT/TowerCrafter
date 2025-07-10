
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


[CreateAssetMenu(menuName ="Items/New Generic Item")]
public class ItemData : ScriptableObject
{
    [Header("General Stats")]
    [Expose("Name")]
    [SerializeField] private string itemName;
    //Has Exposed , Item Rarity
    [SerializeField] private ItemRarity rarity;
    [SerializeField] private ItemSize _gridsize;
    [SerializeField] private Sprite icon;
    [Header("Description")]
    [SerializeField] private Descriptor itemDescription = new Descriptor();
    [Header("Is Test Item")]
    [SerializeField] private bool isGeneric = true;

    private Vector2Int _gridPosition;
    public Vector2Int GridPosition {  get { return _gridPosition; }  set { _gridPosition = value; } }
    public Vector2Int GridSize { get { return _gridsize.GetItemSize; } }
    public string Item { get { return itemName; } set { itemName = value; } }

    public void SaveGridPositon(Vector2Int gridPos)
    {
        _gridPosition = gridPos;
    }
    public virtual string GetDiscription()
    {
       return itemDescription.FormatString(new object[] { this,rarity });
    }
    public static ItemData CreateInstance(Vector2Int gridPosition)
    {
        ItemData newItem = CreateInstance<ItemData>();

        newItem._gridPosition = gridPosition;
        return newItem;
    }
}
