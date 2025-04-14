using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override bool IsValidMove(Vector2Int targetPosition, GameObject[,] board)
    {
        Vector2Int move = targetPosition - currentPosition;

        // Diagonal move (absolute x equals absolute y)
        if (Mathf.Abs(move.x) == Mathf.Abs(move.y))
        {
            return IsPathClear(targetPosition, board);
        }

        return false;
    }

    private bool IsPathClear(Vector2Int targetPosition, GameObject[,] board)
    {
        Vector2Int direction = new Vector2Int(
            targetPosition.x > currentPosition.x ? 1 : -1,
            targetPosition.y > currentPosition.y ? 1 : -1
        );

        Vector2Int current = currentPosition + direction;

        while (current != targetPosition)
        {
            if (!IsWithinBounds(current) || board[current.x, current.y] != null)
            {
                return false;
            }
            current += direction;
        }

        return true;
    }

    private bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < 8 && position.y >= 0 && position.y < 8;
    }
}