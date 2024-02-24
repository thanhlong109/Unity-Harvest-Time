using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUncountableItem : IInventoryItem
{
    public int Amounts { get; set; }

}
