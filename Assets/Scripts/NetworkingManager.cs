using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
    private const int MAX_PLAYERS = 20;
    private string _appId;

    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer && MultiAppManager.GetAppId() != null)
        {
            // using the Multi App Manager (available thanks to the pluto XRPK exporter package) 
            // to get the session-unique XRPK ID and use it as a room identifier
            _appId = MultiAppManager.GetAppId();
        }
        else
        {
            _appId = "test room";
        }
        Connect();
    }

    #region Public Methods
    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            print(string.Format("Joining Room {0}", _appId));
            PhotonNetwork.JoinOrCreateRoom(_appId, new RoomOptions { MaxPlayers = MAX_PLAYERS }, null);
        }
        else
        {
            print("Connecting...");
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    #endregion


    #region MonoBehaviourPunCallbacks CallBacks

    public override void OnConnectedToMaster()
    {
        print("Joining Room " + _appId);
        PhotonNetwork.JoinOrCreateRoom(_appId, new RoomOptions { MaxPlayers = 20 }, null);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room!");
        Debug.LogError(message);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room with " + PhotonNetwork.CurrentRoom.PlayerCount + " Player(s)");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject("Cubes", new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
    #endregion
}