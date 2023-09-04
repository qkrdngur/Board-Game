using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using Steamworks;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField]private playerObjectController GamePlayerPrefab;
    public List<playerObjectController> GamePlayers { get; } = new List<playerObjectController>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if(SceneManager.GetActiveScene().name == "Lobby")
        {
            playerObjectController GamePlayerInstance = Instantiate(GamePlayerPrefab);
          
            GamePlayerInstance.ConnectionID = conn.connectionId;
            GamePlayerInstance.PlayerIDNumber = GamePlayers.Count + 1;
            GamePlayerInstance.PlayerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.Instance.CurrentLobbyID, GamePlayers.Count);

            NetworkServer.AddPlayerForConnection(conn, GamePlayerInstance.gameObject);
        }
    }
}
