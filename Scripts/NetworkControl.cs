using FishNet.Managing;
using TMPro;
using UnityEngine;

public class NetworkControl : MonoBehaviour
{

    public enum PlayerType { None ,Host , Client}
    public PlayerType myPlayer = PlayerType.None;
    public NetworkManager networkManager;
    public GameObject PreGame;

    public void StartHost()
    {
        networkManager.ServerManager.StartConnection();
        networkManager.ClientManager.StartConnection();
        myPlayer = PlayerType.Host;
        Debug.Log("Host");
        PreGame.SetActive(false);
    }

    public void StartClient()
    {
        networkManager.ClientManager.StartConnection();
        myPlayer = PlayerType.Client;
        Debug.Log("Client");
        PreGame.SetActive(false);
    }

}
