using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu]
public class EntityData : ScriptableObject
{

    [SerializeField, Expose("Mob Name")] private string EntityName;
    [SerializeField] private GameObject prefab;

    public GameObject GetPrefab { get { return prefab; } }  
}