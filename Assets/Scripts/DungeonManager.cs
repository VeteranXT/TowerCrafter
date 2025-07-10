using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] private List<Dungeon> originalSOs;
    [SerializeField] private List<Dungeon> runtimeList = new();
    private Dictionary<string, Dungeon> dungeonDict = new();
    public bool isFirstTime = true;
    public List<Dungeon> GetDungeon {  get { return runtimeList; }  set { runtimeList = value; } }

    private void Start()
    {
        if (isFirstTime)
        {
            CreateCopyAndLinks();
            isFirstTime = false;
        }
    }
    public void SaveDungeonData()
    {
       //SaveManager.SaveData(new PlayerData(runtimeList));
    }
    public  List<Dungeon> LoadSaveDungeonData(List<IDungeonSaveData> dungeonList)
    {
        if (dungeonList == null) 
            return null;

        List<Dungeon> runtimeLis = new List<Dungeon>();
        for (int i = 0; i < dungeonList.Count; i++)
        {
            runtimeLis[i] = (Dungeon)dungeonList[i];
        }

        runtimeList = runtimeLis;
        return runtimeList;
    }
    public void CreateCopyAndLinks()
    {
        runtimeList.Clear();
        dungeonDict.Clear();

        // Load all original dungeon ScriptableObjects
        originalSOs = Resources.LoadAll<Dungeon>("").ToList(); // Use your actual folder name

        // Step 1: Clone and store
        foreach (var original in originalSOs)
        {
            var clone = original.CreateCopy();
            runtimeList.Add(clone);
            dungeonDict[clone.DungeonName] = clone;
        }

        // Step 2: Relink references
        for (int i = 0; i < originalSOs.Count; i++)
        {
            var original = originalSOs[i];
            var clone = runtimeList[i];

            // Relink prerequisites
            foreach (var prereq in original.Prerequsites)
            {
                if (dungeonDict.TryGetValue(prereq.DungeonName, out var linked))
                {
                    clone.Prerequsites.Add(linked);
                }
            }

            // Relink unlocks
            foreach (var unlock in original.DungeonsToUnlock)
            {
                if (dungeonDict.TryGetValue(unlock.DungeonName, out var linked))
                {
                    clone.DungeonsToUnlock.Add(linked);
                }
            }
        }
    }
}
