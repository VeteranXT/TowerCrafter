using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DungeonSaveData
{
    private bool isLocked;
    public bool isDefeated;
    public float campaginExpEarned;
    public float endlessExpEarned;
  

    public DungeonSaveData(Dungeon data)
    {
        isDefeated = data.IsDefeated;
        isLocked = data.IsLocked;
        campaginExpEarned = data.CampaginExpEarned;
        endlessExpEarned = data.EndlessExpEarned;
    }
}

