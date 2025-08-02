using UnityEngine;


[CreateAssetMenu(menuName = "Customs/New ItemName Size")]
public class ItemSize : ScriptableObject
{
    [SerializeField] private Vector2Int itemSize = Vector2Int.one;

    public Vector2Int GetItemSize { get { return itemSize; } }
}