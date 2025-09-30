using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [SyncVar]
    public int characterId;

    [SyncVar]
    public int matchId;

    [SyncVar]
    public string playerName;

    [Header("Movement Settings - SERVER ONLY")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxInputMagnitude = 1.5f; // Anti-cheat limit

    private Vector3 serverPosition;
    private float lastInputTime;
    private const float INPUT_RATE_LIMIT = 0.016f; // ~60 inputs/saniye max

    void Start()
    {
        if (isServer)
        {
            serverPosition = transform.position;
        }
    }

    void Update()
    {
        // CLIENT: Sadece input topla ve gönder
        if (isLocalPlayer)
        {
            HandleInput();
        }

        // CLIENT: Smooth interpolation (görsel için)
        if (!isServer && serverPosition != Vector3.zero)
        {
            transform.position = Vector3.Lerp(transform.position, serverPosition, Time.deltaTime * 10f);
        }
    }

    void HandleInput()
    {
        // Rate limiting - spam prevention
        if (Time.time - lastInputTime < INPUT_RATE_LIMIT)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(horizontal, 0, vertical);

        // Input gönder
        if (inputDir.magnitude > 0.01f)
        {
            CmdMove(inputDir);
            lastInputTime = Time.time;
        }
    }

    [Command]
    void CmdMove(Vector3 inputDirection)
    {
        // SERVER: Input doðrulama
        if (inputDirection.magnitude > maxInputMagnitude)
        {
            Debug.LogWarning($"[ANTI-CHEAT] Player {connectionToClient.connectionId} sent invalid input magnitude");
            return;
        }

        // SERVER: Hareket hesapla
        Vector3 normalizedInput = inputDirection.normalized;
        Vector3 movement = normalizedInput * moveSpeed * Time.deltaTime;

        // SERVER: Pozisyonu güncelle
        serverPosition = transform.position + movement;
        transform.position = serverPosition;

        // Tüm clientlara pozisyon senkronize edilir (NetworkTransform otomatik yapar)
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        GetComponent<Renderer>().material.color = Color.green;
        Debug.Log($"Local player spawned - Character: {characterId}, Match: {matchId}");
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        serverPosition = transform.position;
    }
}