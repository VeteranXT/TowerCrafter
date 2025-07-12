using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
public enum TowerSlot
{
    WeaponMount, //MinMax damage, +%Damage, +Range, +%range, Crit , +%criti, +%CritDmaga, +%stun, +Stun, +SlowAmount, AOE
    Fundation,//maxHP, +%MaxHP, HPRegen +%HPRegen, PhyResist,LightResist,UnholyResist 
    LoadingMechanism, //Attack Speed, Increased Criti Chance, +%AoE,
    Projectile, 
    PowerCore, 
    UtilitySlot
    // +% or % is increased by ...aka if damage is 100 and increased damage is 10% damage is 110.
}
public class TowerEquipment : ItemData
{
    [SerializeField] private TowerSlot slot;
    public TowerSlot GetSlot {  get { return slot; } }
    public int GetSlotIndex { get { return (int)GetSlot; } }
   
}

