using UnityEngine;
using Mirror;

public class GridGenerator : NetworkBehaviour
{
    public int width = 60;
    public int height = 60;
    public GameObject cellPrefab;

    [HideInInspector]
    public Cell[,] cells;

    public override void OnStartServer()
    {
        cells = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x, y, 0);
                GameObject cellObj = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
                NetworkServer.Spawn(cellObj);

                cells[x, y] = cellObj.GetComponent<Cell>();
            }
        }
    }
}
