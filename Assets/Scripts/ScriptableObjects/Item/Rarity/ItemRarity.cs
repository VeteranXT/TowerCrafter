using UnityEngine;

[CreateAssetMenu(menuName = "Customs/New ItemName Rarity")]
public class ItemRarity : ScriptableObject
{
    [Expose("Rarity Color")]
    public string rarityHex =>  "#" + ColorUtility.ToHtmlStringRGBA(rarityColor);
  
    [SerializeField] private Color rarityColor;
}