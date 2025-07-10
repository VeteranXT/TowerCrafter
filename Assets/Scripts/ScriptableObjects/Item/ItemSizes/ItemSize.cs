using UnityEngine;


[CreateAssetMenu(menuName = "Customs/New Item Size")]
public class ItemSize : ScriptableObject
{
    [SerializeField] private Vector2Int itemSize = Vector2Int.one;

    public Vector2Int GetItemSize { get { return itemSize; } }
}