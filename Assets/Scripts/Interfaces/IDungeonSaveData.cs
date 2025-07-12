using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IDungeonSaveData
{
    string DungeonName { get; }
    bool IsLocked { get; set; }
    bool IsDefeated { get; set; }
    float CampaginExpEarned { get; set; }
    float EndlessExpEarned { get; set; }
}

