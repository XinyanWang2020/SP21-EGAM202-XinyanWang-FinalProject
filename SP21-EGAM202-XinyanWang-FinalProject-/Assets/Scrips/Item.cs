using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public virtual void Use()
    {
        Debug.Log("You wave your Generic Item in the air. Nothing happens.");
    }

    public virtual void Wear()
    {
        Debug.Log("You try to wear your Generic Object, but it won't fit.");
    }

    public virtual void Eat()
    {
        Debug.Log("You bite the Generic Object. Clank, it hurts your teeth");
    }
}
