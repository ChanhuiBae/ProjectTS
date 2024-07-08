using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private LobbySceneManager lobby;

    private void Awake()
    {
        if (!GameObject.Find("LobbySceneManager").TryGetComponent<LobbySceneManager>(out lobby))
        {
            Debug.Log("Armory - Awake - LobbySceneManager");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            lobby.SetInteracte(Interaction_Type.Mission, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        lobby.SetInteracte(Interaction_Type.None, false);
    }
}
