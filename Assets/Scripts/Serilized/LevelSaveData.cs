using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Serializable]
public class LevelSaveData
{
    public int level;
    public int maxLevel;
    public float exp;
    public float requiredXp;
    public float initialExpRequirments;
    public float powScale;
    public float maxExp;

    public LevelSaveData(ISavableLevel data)
    {
        level = data.level;
        maxLevel = data.maxLevel;
        requiredXp = data.expReq;
    }
}

 