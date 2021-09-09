using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int maxHP = 3;

    [HideInInspector]
    public int hp;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
    }
}
