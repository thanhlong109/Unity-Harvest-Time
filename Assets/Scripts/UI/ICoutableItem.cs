using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICountableItem : IInventoryItem
{
    public int Quantity { get; set; }

}
