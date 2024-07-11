using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StemToMain : MonoBehaviour
{
    private GameObject player;
    private FadeManager fadeManager;

    private void Awake()
    {
        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.Log("player is not found");
        }
        if (!GameObject.Find("Canvas").TryGetComponent<FadeManager>(out fadeManager))
        {
            Debug.Log("fadeManager is not found");
        }
    }
    private IEnumerator Move()
    {
        GameManager.Inst.fadeManager.FadeInOut(1f);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        player.transform.position = new Vector3(0, 0.5f, -5);
    }

    private void OnTriggerEnter(Collider other)
    {
        fadeManager.FadeInOut(0.5f);
        StartCoroutine(Move());
    }

}
