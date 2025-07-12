using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu]
public class Dungeon : ScriptableObject, IDungeonSaveData
{
    #region Fields
    [SerializeField,Expose("Dungeon Name")] private string dungeonName;
    [SerializeField] private List<Wave> waves = new List<Wave>();
    //List of dungeons  that must be defeated in order to unlock this dungeon
    //Flag to set if this dungeon is unlocked
    //flag to set status of defeated dungeon
    //List of dungeons we can unlock if this dungeon is defeated
    [SerializeField] private GameObject dungeonMapPrefab;
    [SerializeField] private Descriptor dungeonDescription = new Descriptor();
    [SerializeField] private Sprite lockedDungeonIcon;
    [SerializeField] private Sprite campaignOpen;
    [SerializeField] private Sprite campaignWon;
    [SerializeField] private Sprite endless;

    [Header("Base Stats")]
    [SerializeField] private bool isDungeonDefeated = false;
    [SerializeField] private List<Dungeon> preRequsite = new List<Dungeon>();
    [SerializeField] private bool isLocked = false;
    [SerializeField] private List<Dungeon> unlocksDungeons = new List<Dungeon>();
    [SerializeField] private float expEarned = 0;
    [SerializeField] private float expEndlessEarned = 0;
    [Header("Reset stats match above!")]
    [SerializeField] private float resetExpEndlessEarned = 0;
    [SerializeField] private float resetEarned = 0;
    [SerializeField] private bool resetIsLocked = false;
    [SerializeField] private bool resetIsDungeonDefeated = false;
    [SerializeField] private List<Dungeon> resetPreRequsite = new List<Dungeon>();
    [SerializeField] private List<Dungeon> resetUnlocksDungeons = new List<Dungeon>();
    #endregion

    #region Properties
    public Sprite IconLocked { get { return lockedDungeonIcon; } }
    public Sprite IconCampaginOpen { get { return campaignOpen; } }
    public Sprite IconCampaginWon { get { return campaignWon; } }
    public Sprite IconEndless { get { return endless; } }
    public GameObject GetBattlePrefab { get { return dungeonMapPrefab; } }
    public List<Wave> Waves { get { return waves; } }
    public List<Dungeon> DungeonsToUnlock { get { return unlocksDungeons; } }
    public List<Dungeon> Prerequsites { get { return preRequsite; }    }
    public string DungeonName { get { return dungeonName; } set { dungeonName = value; } }
    public bool IsLocked { get { return isLocked; } set { isLocked = value; } }
    public bool IsDefeated { get { return isDungeonDefeated; } set { isDungeonDefeated = value; } }
    public float CampaginExpEarned { get { return expEarned; } set { expEarned = value; } }
    public float EndlessExpEarned { get { return expEndlessEarned; }  set { expEndlessEarned = value; } }

    #endregion

    public bool EarnedMoreCampaign(float amount)
    {
        return amount > CampaginExpEarned;
    }
    public bool EarnedMoreEndless(float amount)
    {
        return amount > EndlessExpEarned;
    }
    public float SaveExpEndlessGiveDiffrence(float amount)
    {
        //Check if we earned more that current
        if (amount > EndlessExpEarned)
        {
            //Substract form last time and gives us diffrence
            var earned = amount - EndlessExpEarned;

            //Save currently earned
            expEarned = amount;
            //Return diffrence
            return earned;
        }
        //Return 0 since we earned less than last time
        return 0f;
    }
    public float GetAndUpdateEndlessExpDelta(float amount)
    {
        //Check if we earned more that current
        if(amount > expEndlessEarned)
        {
            //Substract form last time and gives us diffrence
            var earned = amount - expEndlessEarned;

            //Save currently earned
            expEndlessEarned = amount;
            //Return diffrence
            return earned;
        }
        //Return 0 since we earned less than last time
        return 0f;
    }
    public bool MeetsRequimentsToUnlock()
    {
        if (Prerequsites == null || preRequsite.Count == 0)
            return true;

        foreach (var pre in Prerequsites)
        {
            if (!pre.IsDefeated)
            {
                return false;
            }
        }
        return true;
    }
    public void UnlockDungeon()
    {
        IsLocked = false;
    }
    public void LockDungeon()
    {
        isLocked = true;
    }
    public void DefeatDungeon()
    {
        isDungeonDefeated = true;
    }
    public void UndefeatDungeon()
    {
        isDungeonDefeated = false;
    }

    [Expose("Unique Enemies Types")]
    public List<string> UniqueWaveEnemyNames
    {
        get
        {
            HashSet<string> names = new();
            foreach (var wave in waves)
            {
                if (wave.GetData != null)
                    names.Add(wave.GetData.name);
            }
            return names.ToList();
        }
    }
    public string GetDungeonDescription()
    {
        HashSet<string> list = new HashSet<string>();
        foreach (var wave in waves)
        {
            var ModName = wave.GetData.name;
            if (!list.Contains(ModName))
            {
                list.Add(ModName);
            }
        }
        return dungeonDescription.FormatString(new object[] {this, waves });
    }
    public Dungeon CreateCopy()
    {
        return Instantiate(this); 
    }
    
}

