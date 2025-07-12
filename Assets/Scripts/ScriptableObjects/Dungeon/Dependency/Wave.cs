using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Wave
{ 
    [SerializeField] private EntityData enemy;
    [SerializeField] private CharacterStat amount;
    [SerializeField] private int spawnLocation = 0;
    public int MobAmont {get { return (int)Mathf.Round(amount.Value); }  }
    public EntityData GetData {  get { return enemy; } }
    public int GetSpawnLocation { get { return spawnLocation; } }

}

