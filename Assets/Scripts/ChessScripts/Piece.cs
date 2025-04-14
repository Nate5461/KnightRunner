using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector2Int currentPosition;
    public string color;
    public bool HasMoved { get; set; }
    public string type;

    public virtual bool IsValidMove(Vector2Int targetPosition, GameObject[,] board)
    {
        return false;
    }

}
