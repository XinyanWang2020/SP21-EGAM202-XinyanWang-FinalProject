using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image HpImage;
    public float Hp;

    [SerializeField] private float MaxHp;

    private void Start()
    {
        Hp = MaxHp;
    }


}
