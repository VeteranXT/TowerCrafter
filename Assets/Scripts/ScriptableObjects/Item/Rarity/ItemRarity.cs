using UnityEngine;

[CreateAssetMenu(menuName = "Customs/New Item Rarity")]
public class ItemRarity : ScriptableObject
{
    public string rarity;
    [Expose("Rarity Color")]
    public Color rarityColor;
}