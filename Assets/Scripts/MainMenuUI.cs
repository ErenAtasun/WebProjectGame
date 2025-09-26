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

        // Find Match butonunu gizle/deaktif et
        if (findMatchButton != null)
        {
            findMatchButton.gameObject.SetActive(false);
            // Ya da sadece deaktif etmek istersen:
            // findMatchButton.interactable = false;
        }

        // Character selection'ý göster
        CharacterSelectionUI charUI = FindObjectOfType<CharacterSelectionUI>();
        if (charUI != null)
        {
            Debug.Log("Found CharacterSelectionUI, calling ShowCharacterSelection");
            charUI.ShowCharacterSelection(msg.matchId);
        }
        else
        {
            Debug.LogError("CharacterSelectionUI not found!");
        }
    }
}