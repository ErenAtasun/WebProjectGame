using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnCharacterIdChanged))] // HOOK EKLE
    public int characterId;
    private Vector3 lastMoveDirection = Vector3.down;
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

    void HandleInput()
    {
        if (Time.time - lastInputTime < INPUT_RATE_LIMIT)
            return;
            
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 inputDir = new Vector3(horizontal, vertical, 0);
        
        if (inputDir.magnitude > 0.01f)
        {
            lastMoveDirection = inputDir.normalized;
            CmdMove(inputDir, lastMoveDirection); // Yönü de gönder
            lastInputTime = Time.time;
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            HandleInput();
        }
    }

    [Command]
    void CmdMove(Vector3 inputDirection, Vector3 moveDirection)
    {
        if (inputDirection.magnitude > maxInputMagnitude)
        {
            Debug.LogWarning($"[ANTI-CHEAT] Player sent invalid input");
            return;
        }
        
        Vector3 normalizedInput = inputDirection.normalized;
        Vector3 movement = normalizedInput * moveSpeed * Time.deltaTime;
        
        syncedPosition = transform.position + movement;
        transform.position = syncedPosition;
        
        // Tüm clientlara yönü bildir
        RpcUpdateDirection(moveDirection);
    }

    void OnPositionChanged(Vector3 oldPos, Vector3 newPos)
    {
        if (!isServer)
        {
            transform.position = Vector3.Lerp(transform.position, newPos, 0.5f);
        }
    }

    // CHARACTER ID DE����NCE �A�RILIR
    void OnCharacterIdChanged(int oldId, int newId)
    {
        Debug.Log($"Character ID changed from {oldId} to {newId}");
        ApplyCharacterVisuals();
    }

    void ApplyCharacterVisuals()
    {
        PlayerVisuals visuals = GetComponent<PlayerVisuals>();
        
        if (CharacterDatabase.Instance != null && visuals != null)
        {
            CharacterData charData = CharacterDatabase.Instance.GetCharacterData(characterId);
            if (charData != null)
            {
                visuals.ApplyCharacterSprites(charData);
                Debug.Log($"Applied {charData.characterName} visuals");
            }
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        syncedPosition = transform.position;
    }
    [ClientRpc]
    void RpcUpdateDirection(Vector3 direction)
    {
        PlayerVisuals visuals = GetComponent<PlayerVisuals>();
        if (visuals != null)
        {
            visuals.SetDirection(direction);
        }
    }
}