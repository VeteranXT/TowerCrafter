using UnityEngine;

[CreateAssetMenu(menuName = "Items/New Generic ItemName")]
public class ItemBase : ScriptableObject, IInformation
{
    [Header("General Stats")]
    [Expose("Name")]

    [SerializeField] private string itemName;
    [SerializeField] private int categoryIndex;
    [SerializeField] private int stashIndex;


    //Has Exposed , ItemName Rarity
    [SerializeField] private ItemRarity rarity;
    [SerializeField] private ItemSize _gridsize;
    [SerializeField] private Vector2 gridPosition;

    [SerializeField] private Sprite icon;
    [Header("Description")]
    [SerializeField] private Descriptor itemDescription = new Descriptor();
    [SerializeField] private bool hasRarity = true;
    [Header("Is Test ItemName")]
    [SerializeField] private bool isGeneric = true;


    public Vector2 GridPosition { get { return gridPosition; } set { gridPosition = value; } }
    public Vector2 GridSize { get { return _gridsize.GetItemSize; } }
    public string ItemName { get { return itemName; } set { itemName = value; } }
    public int CategoryIndex { get { return categoryIndex; } }
    public int StashIndex { get { return stashIndex; } }
    public Sprite ItemIcon { get { return icon; } }
    public void SaveGridPositon(Vector2 gridPos, int categoryIndex, int stashIndex)
    {
        this.categoryIndex = categoryIndex;
        this.stashIndex = stashIndex;
        this.gridPosition = gridPos;
    }
    public virtual string GetDiscription()
    {
        return itemDescription.FormatString(new object[] { this, rarity });
    }
    public static ItemBase CreateCopy(Vector2Int gridPosition, ItemBase orginal)
    {

        ItemBase newItem = Instantiate(orginal); ;

        newItem.gridPosition = gridPosition;
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
        return itemDescription.FormatString(new object[] { this, rarity });
    }
}
