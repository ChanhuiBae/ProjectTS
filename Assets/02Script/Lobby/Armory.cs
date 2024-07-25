using UnityEngine;

public class Armory : MonoBehaviour
{
    private LobbySceneManager lobby;

    private void Awake()
    {
        if(!GameObject.Find("LobbySceneManager").TryGetComponent<LobbySceneManager>(out lobby))
        {
            Debug.Log("Armory - Awake - LobbySceneManager");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            lobby.SetInteracte(Interaction_Type.Armory, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        lobby.SetInteracte(Interaction_Type.None, false);
    }
}
