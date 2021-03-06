using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image HpImage;
    public float Hp;

    public float MaxHp;

    private void Start()
    {
        Hp = MaxHp;
    }

    private void Update()
    {
        HpImage.fillAmount = Hp / MaxHp;
    }
}
