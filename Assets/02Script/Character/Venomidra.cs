using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Venomidra : MonoBehaviour
{
    private BossGage hp;
    private Creture creature;

    private void Start()
    {
        if (!GameObject.Find("BossGage").TryGetComponent<BossGage>(out hp))
        {
            Debug.Log("Venomidra - Awake - BossGage");
        }
        if (!TryGetComponent<Creture>(out creature))
        {
            Debug.Log("Venomidra - Awake - Creature");
        }
        hp.SetBossHP(1);
    }

    private void Update()
    {
        hp.SetBossHP(creature.HP);
        if(creature.HP <= 0)
        {
            hp.gameObject.SetActive(false);
            return;
        }
    }
}
