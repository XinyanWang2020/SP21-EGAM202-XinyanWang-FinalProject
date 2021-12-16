using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloom : Item
{
    public int currentItemIndex;

    public override void Use()
    {
        GetComponent<Animator>().SetTrigger("Sweep");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Spikes")
            Destroy(other.gameObject);
    }
}
