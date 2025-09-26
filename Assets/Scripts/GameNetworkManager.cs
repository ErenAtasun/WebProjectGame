using Mirror;
using UnityEngine;
using Mirror.SimpleWeb;
public class GameNetworkManager : NetworkManager
{
    void Start()
    {
        // SimpleWebTransport kullan
        GetComponent<SimpleWebTransport>().port = 7778; // Web için farklý port
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        // Client mesaj handler'larýný kaydet
        NetworkClient.RegisterHandler<MatchFoundMessage>(OnMatchFoundMessage);

        Debug.Log("Client message handlers registered");
    }

    public override void OnStopClient()
    {
        // Temizlik yap
        NetworkClient.UnregisterHandler<MatchFoundMessage>();
        base.OnStopClient();
    }

    void OnMatchFoundMessage(MatchFoundMessage msg)
    {
        Debug.Log($"Received MatchFoundMessage: Match ID {msg.matchId}");

        // MainMenuUI'a bildir
        MainMenuUI menuUI = FindObjectOfType<MainMenuUI>();
        if (menuUI != null)
        {
            menuUI.OnMatchFoundReceived(msg);
        }
    }

    public void JoinQueue()
    {
        Debug.Log($"JoinQueue called. NetworkClient.isConnected: {NetworkClient.isConnected}");

        if (NetworkClient.isConnected)
        {
            Debug.Log("Sending QueueJoinMessage to server");
            NetworkClient.Send(new QueueJoinMessage());
        }
        else
        {
            Debug.Log("Not connected, starting client...");
            StartClient();
        }
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("Client connected to server");

        // OTOMATIK QUEUE JOIN'I KALDIRDIK
        // Artýk sadece butona basýnca queue'ya katýlacak
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("GameNetworkManager server started");
    }
}