using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnCharacterIdChanged))] // HOOK EKLE
    public int characterId;

    [SyncVar(hook = nameof(OnPositionChanged))]
    private Vector3 syncedPosition;

    [SyncVar]
    public int matchId;

    [SyncVar]
    public string playerName;

    [Header("Movement Settings - SERVER ONLY")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxInputMagnitude = 1.5f;

    private float lastInputTime;
    private const float INPUT_RATE_LIMIT = 0.016f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (isServer)
        {
            syncedPosition = transform.position;
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        if (Time.time - lastInputTime < INPUT_RATE_LIMIT)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(horizontal, vertical, 0);

        if (inputDir.magnitude > 0.01f)
        {
            CmdMove(inputDir);
            lastInputTime = Time.time;
        }
    }

    [Command]
    void CmdMove(Vector3 inputDirection)
    {
        if (inputDirection.magnitude > maxInputMagnitude)
        {
            Debug.LogWarning($"[ANTI-CHEAT] Player {connectionToClient.connectionId} sent invalid input");
            return;
        }

        Vector3 normalizedInput = inputDirection.normalized;
        Vector3 movement = normalizedInput * moveSpeed * Time.deltaTime;

        syncedPosition = transform.position + movement;
        transform.position = syncedPosition;
    }

    void OnPositionChanged(Vector3 oldPos, Vector3 newPos)
    {
        if (!isServer)
        {
            transform.position = Vector3.Lerp(transform.position, newPos, 0.5f);
        }
    }

    // CHARACTER ID DEÐÝÞÝNCE ÇAÐRILIR
    void OnCharacterIdChanged(int oldId, int newId)
    {
        Debug.Log($"Character ID changed from {oldId} to {newId}");
        ApplyCharacterVisuals();
    }

    void ApplyCharacterVisuals()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (CharacterDatabase.Instance != null && spriteRenderer != null)
        {
            CharacterData charData = CharacterDatabase.Instance.GetCharacterData(characterId);
            if (charData != null)
            {
                // Sprite varsa uygula
                if (charData.characterSprite != null)
                {
                    spriteRenderer.sprite = charData.characterSprite;
                }

                // Rengi uygula
                spriteRenderer.color = charData.characterTint;

                // Local player daha parlak
                if (isLocalPlayer)
                {
                    spriteRenderer.color = Color.Lerp(charData.characterTint, Color.white, 0.3f);
                }

                Debug.Log($"Applied {charData.characterName} visuals - Color: {charData.characterTint}");
            }
            else
            {
                Debug.LogError($"Character ID {characterId} not found in database!");
            }
        }
        else
        {
            Debug.LogWarning("CharacterDatabase or SpriteRenderer is null!");
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        syncedPosition = transform.position;
    }
}