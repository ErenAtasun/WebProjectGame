using Mirror;
using UnityEngine;

public class PlayerInput : NetworkBehaviour
{
    void Update()
    {
        if (!isLocalPlayer) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        CmdSendInput(h, v);
    }

    [Command]
    void Start()
    {
        if (isLocalPlayer)
        {
            Camera.main.GetComponent<CameraFollow>().target = this.transform;
        }
    }
    void CmdSendInput(float h, float v)
    {
        GetComponent<PlayerMovement>().Move(h, v);
    }
}
