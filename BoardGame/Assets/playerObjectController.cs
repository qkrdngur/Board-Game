using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class playerObjectController : MonoBehaviour
{
    //Player Data
    [SyncVar] public int ConnectionID;
    [SyncVar] public int PlayerIDNumber;
    [SyncVar] public ulong PlayerSteamID;
    [SyncVar(hook = nameof(PlayerNameUpdate))] public string PlayerName;

    private CustomNetworkManager manager;

    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null)
                return manager;

            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    //public override void OnStartAuthority()
    //{
    //    CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
    //    gameObject.name = "LocalGamePlayer";
    //    LobbyController.Instance.FindLocalPlayer();
    //    LobbyController.Instance.UpdateLobbyName();
    //} 
    
    //public override void OnStartClient()
    //{
    //    Manager.GamePlayers.Add(this);
    //    LobbyController.Instance.UpdateLobbyName();
    //    LobbyController.Instance.UpdatePlayerList();
    //}    
    
    //public override void OnStopClient()
    //{

    //}

    [Command]
    private void CmdSetPlayerName(string PlayerName)
    {

    }

    public void PlayerNameUpdate(string OldValue, string NewValue)
    {

    }
}
