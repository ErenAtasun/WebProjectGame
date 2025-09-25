using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Button findMatchButton;
    public GameObject queuePanel;
    public Text statusText; // Bu satýrý ekle

    void Start()
    {
        // Button listener ekle
        if (findMatchButton != null)
        {
            findMatchButton.onClick.AddListener(OnFindMatchClicked);
        }
    }

    void OnFindMatchClicked()
    {
        Debug.Log("Find Match button clicked");

        // Tüm NetworkManager türlerini kontrol et
        NetworkManager[] allNetworkManagers = FindObjectsOfType<NetworkManager>();
        GameNetworkManager[] gameNetworkManagers = FindObjectsOfType<GameNetworkManager>();

        Debug.Log($"Found {allNetworkManagers.Length} NetworkManager(s)");
        Debug.Log($"Found {gameNetworkManagers.Length} GameNetworkManager(s)");

        if (gameNetworkManagers.Length > 0)
        {
            Debug.Log("Using GameNetworkManager");
            gameNetworkManagers[0].JoinQueue();

            // UI güncelle
            UpdateUIForSearching();
        }
        else if (allNetworkManagers.Length > 0)
        {
            Debug.Log("Found NetworkManager but not GameNetworkManager - you need to replace it!");
        }
        else
        {
            Debug.LogError("No NetworkManager found at all!");
        }
    }

    void UpdateUIForSearching()
    {
        if (findMatchButton != null)
        {
            findMatchButton.interactable = false;
            Text buttonText = findMatchButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "Searching...";
            }
        }

        if (statusText != null)
        {
            statusText.text = "Searching for match...";
        }
    }

    public void OnMatchFoundReceived(MatchFoundMessage msg)
    {
        Debug.Log($"UI: Match found! Match ID: {msg.matchId}");

        if (statusText != null)
        {
            statusText.text = $"Match Found! ID: {msg.matchId}";
        }

        if (findMatchButton != null)
        {
            Text buttonText = findMatchButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "Match Found!";
            }
        }

        // Sonraki adým: Karakter seçme ekranýna geç
        // StartCharacterSelection(msg.matchId);
    }
}