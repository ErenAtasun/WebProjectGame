using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 5f;
    public float minX = 0f;
    public float maxX = 59f;
    public float minY = 0f;
    public float maxY = 59f;

    private GridGenerator grid; // cache için

    void Start()
    {
        grid = GridGenerator.FindFirstObjectByType<GridGenerator>(); // sadece 1 kez çalışır
    }

    [Server]
    public void Move(float h, float v)
    {
        Vector3 move = new Vector3(h, v, 0);
        transform.position += move * speed * Time.deltaTime;
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(moveX, moveY, 0).normalized * speed * Time.deltaTime;
        transform.position += move;

        // Grid sınırına clamp
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);

        // Hücreyi boyama
        CmdPaintCell(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
    }

    [Command]

    void CmdPaintCell(int x, int y)
    {
        if (grid != null && grid.cells[x, y] != null)
        {
            grid.cells[x, y].Paint(Color.blue); // bu server çağırıyor → RPC client’da render ediyor
        }
    }

}
