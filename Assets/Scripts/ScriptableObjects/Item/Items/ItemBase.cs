
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


[CreateAssetMenu(menuName ="Items/New Generic Item")]
public class ItemBase : ScriptableObject, IInformation
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
    [SerializeField] private bool hasRarity = true;
    private Vector2 _gridPosition;
    public Vector2 GridPosition {  get { return _gridPosition; }  set { _gridPosition = value; } }
    public Vector2 GridSize { get { return _gridsize.GetItemSize; } }
    public string Item { get { return itemName; } set { itemName = value; } }

    public void SaveGridPositon(Vector2 gridPos)
    {
        _gridPosition = gridPos;
    }
    public virtual string GetDiscription()
    {
       return itemDescription.FormatString(new object[] { this,rarity });
    }
    public static ItemBase CreateCopy(Vector2Int gridPosition)
    {
      
        ItemBase newItem = ScriptableObject.CreateInstance<ItemBase>()  ;

        newItem._gridPosition = gridPosition;
        return newItem;
    }

    public string InformationName()
    {
        if (hasRarity)
            return string.Format("<color={0}> {1} </color>", rarity, itemName);
        else
            return itemName;
    }
    public string Description()
    {
        return itemDescription.FormatString(new object[] {this, rarity });
    }
}
