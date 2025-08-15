using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerCrafter.Grid;


public class ShopGrid : CustomGrid
{
    public virtual bool CanSell(ItemBase item)
    {
        return true;
    }
    public virtual int SellItem(ItemBase item)
    {
        //Calculate item value?
        return 0;
    }

    public bool CanBuy(ItemBase item) 
    { 
        return true;
    }
    public ItemBase BuyItem(ItemBase item)
    {
        return item;
    }

}
