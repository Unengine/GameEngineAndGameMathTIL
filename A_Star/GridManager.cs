using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    public static GridManager _instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<GridManager>();
                if (!instance) Debug.LogError("There should be ONE gameobject with GridManager in this scene!");
            }
            return instance;
        }
        private set { }
    }
    public bool showGrid = true;
    public bool showObstacles = true;
    public Vector3 originPos;
    public Vector3 _originPos { get; set; }

    public Node[,] nodes;

    private int rowLength;
    private int columnLength;
    private Vector3 cellSize;
    private Vector3 maxBound;
    

    // Start is called before the first frame update
    private void Start()
    {
        originPos = Vector3.zero;
        cellSize = new Vector3(1, 1, 1);
        rowLength = columnLength = 10;

        nodes = new Node[rowLength, columnLength];
        maxBound = new Vector3(originPos.x + cellSize.x * columnLength, originPos.y ,originPos.z + cellSize.z * rowLength);    //미리 범위 계산
    }

    public bool IsInBound(Vector3 pos)
    {
        return (pos.x >= originPos.x && pos.z >= originPos.z && pos.x <= maxBound.x && pos.z <= maxBound.z);
    }

    public int GetRow(int idx)
    {
        return idx / columnLength;
    }

    public int GetColumn(int idx)
    {
        return idx % columnLength;
    }

    public int GetGridIdx(Vector3 pos)
    {
        if (!IsInBound(pos)) return -1;

        //기준 원점(0,0,0) 으로
        pos -= originPos;
        int row = (int)(pos.z / cellSize.z);
        int column = (int)(pos.x / cellSize.x);

        return (row * columnLength + column);
    }

    public Vector3 GetGridCellCenterPos(int idx)
    {
        int row = GetRow(idx);
        int column = GetColumn(idx);

        float x = (originPos.x + cellSize.x * 0.5f) + cellSize.x * column;
        float z = (originPos.z + cellSize.z * 0.5f) + cellSize.z * row;

        return new Vector3(x, originPos.y, z);
    }

    private void OnDrawGizmos()
    {
        if(showGrid)
        {
            Debug.Log("Draw");
            DebugDrawGrid();
        }
    }

    private void DebugDrawGrid()
    {
        float width = (cellSize.x * columnLength);
        float height = (cellSize.y * rowLength);

        for (int i = 0; i <= columnLength; ++i)
        {
            Vector3 startPos = new Vector3(originPos.x, originPos.y, originPos.z + cellSize.z * i);
            Vector3 endPos = startPos + Vector3.right * width;
            Debug.DrawLine(startPos, endPos, Color.black);
        }

        for (int i = 0; i <= rowLength; ++i)
        {
            Vector3 startPos = new Vector3(originPos.x + cellSize.x * i, originPos.y, originPos.z);
            Vector3 endPos = startPos + Vector3.forward * height;
            Debug.DrawLine(startPos, endPos, Color.black);
        }
    }
}
