using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;


public class BattleHandler : MonoBehaviour
{
    public static event Action EventBattleVictory;
    private float waveTime;
    private CharacterStat waveTimer = new CharacterStat(3f);
    private CharacterStat mobTimer = new CharacterStat(0.2f);
    private Dungeon currentDungeon;
    private GameObject curretBattleMap;
    private List<EnemyEntity> living =  new List<EnemyEntity>();

    public int totalCountToSpawn = 0;
    private bool isBattlefieldReady = false;
    private bool isBattleStarted = false;
    //TO do change to objects
    public Transform[] spawnPositions;
    public GameObject playerCore;
    private int currentWave = 0;
    public BattleHandler CreateBattlefield(Dungeon dungon)
    {
        BattleHandler handler = Instantiate(this);
        SetupBattlefeld(dungon);
        return handler;
    }
    public void Update()
    {
        if (!isBattlefieldReady)
            return;

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isBattleStarted = true;
        }

        if (isBattleStarted)
            return;


        waveTime -= Time.deltaTime;

        if (waveTime <= 0)
        {
            //Start Spawning Current wave
            StartCoroutine(SpawnMobs(currentDungeon));
            //Reset Timer
            waveTime = waveTimer.Value;
            //Increment next wave counter
            if (currentWave < currentDungeon.Waves.Count)
                currentWave++;
        }
        //Check if we are on last wave, we summoned all and killed all
        if (currentWave == currentDungeon.Waves.Count && totalCountToSpawn == 0 && living.Count == 0)
        {
            //Call Victory Screen
            EventBattleVictory?.Invoke();
        }

    }
    private void Start()
    {
        spawnPositions = GameObject.Find("BatMobSpawns").GetComponentsInChildren<Transform>();
        playerCore = GameObject.Find("Cores");
    }
    private IEnumerator SpawnMobs(Dungeon dungeon)
    {
        var remaining = dungeon.Waves[currentWave].MobAmont;
        int spawnLocation = dungeon.Waves[currentWave].GetSpawnLocation;
        var enemyMobData = dungeon.Waves[currentWave].GetData;
        if (remaining > 1)
        {
            // EnemyEntity en = EnemyEntity.CreateInstance(enemyMobData, spawnPositions[spawnLocation], playerCore);

            //living.Add(en);
            remaining--;
            totalCountToSpawn--;
        }
        yield return new WaitForSeconds(mobTimer.Value);
    }
    private void SetupBattlefeld(Dungeon dungeon)
    {
        currentDungeon = dungeon;
        totalCountToSpawn = 0;
        curretBattleMap = Instantiate(dungeon.GetBattlePrefab);
        ResetBattlefield();
        isBattlefieldReady = true;
    }   
    public void ResetBattlefield()
    {
        isBattlefieldReady = false;
        //Check if we have an living mob to destry
        if (living.Count > 0)
        {
            foreach (EnemyEntity enemy in living)
            {
                //Check if not notinh
                if (enemy != null)
                    //Destroy it
                    Destroy(enemy);
            }
            living.Clear();
        }

        //Destry Current map if map already exists 
        if (currentDungeon != null)
            Destroy(curretBattleMap);


        //Remake whole map
        curretBattleMap = Instantiate(currentDungeon.GetBattlePrefab);
        //Reset how many are left to spawn
        totalCountToSpawn = 0;
        //Get total amount to spawn
        foreach (var mob in currentDungeon.Waves)
        {
            //TODO: Handle increased or changed amount
            totalCountToSpawn += mob.MobAmont;
        }
        currentWave = 0;
        isBattleStarted = false;
        isBattlefieldReady = true;
    }

}

