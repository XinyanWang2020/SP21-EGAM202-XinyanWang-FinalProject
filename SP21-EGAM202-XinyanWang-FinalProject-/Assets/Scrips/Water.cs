using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Item
{
    public int currentItemIndex;
    public override void Use()
    {
        GameObject babyfish = GameObject.Find("BabyFish");
        babyfish.GetComponentInChildren<BabyFishBrain>().Water += 10;
        //remove from avatar
        transform.parent.GetComponent<AvatarController>().Inventory[currentItemIndex] = null;

        //destroy itself
        Destroy(this.gameObject);
    }
}
