using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DungeonSaveData
{
    public bool IsLocked;
    public bool IsDefeated;
    public float CampaginExpEarned;
    public float EndlessExpEarned;
    public DungeonSaveData(Dungeon data)
    {
        IsDefeated = data.IsDefeated;
        IsLocked = data.IsLocked;
        CampaginExpEarned = data.CampaginExpEarned;
        EndlessExpEarned = data.EndlessExpEarned;
    }
}

