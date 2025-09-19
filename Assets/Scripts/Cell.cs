using Mirror;
using UnityEngine;

public class Cell : NetworkBehaviour
{
    private SpriteRenderer sr;

    void Awake()
    {
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.color = Color.white;
    }

    [ClientRpc]
    public void RpcPaint(Color color)
    {
        sr.color = color;
    }

    // Server çaðýracak
    public void Paint(Color color)
    {
        if (!isServer) return;
        RpcPaint(color);
    }
}
