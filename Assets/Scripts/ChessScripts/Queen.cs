using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override bool IsValidMove(Vector2Int targetPosition, GameObject[,] board)
    {
        Vector2Int move = targetPosition - currentPosition;

        // Horizontal, vertical, or diagonal move
        if (move.x == 0 || move.y == 0 || Mathf.Abs(move.x) == Mathf.Abs(move.y))
        {
            if (IsPathClear(targetPosition, board))
            {
                // Ensure the target position is not occupied by a piece of the same color
                GameObject targetPiece = board[targetPosition.x, targetPosition.y];
                if (targetPiece == null || targetPiece.GetComponent<Piece>().color != this.color)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsPathClear(Vector2Int targetPosition, GameObject[,] board)
    {
        Vector2Int direction = new Vector2Int(
            targetPosition.x > currentPosition.x ? 1 : targetPosition.x < currentPosition.x ? -1 : 0,
            targetPosition.y > currentPosition.y ? 1 : targetPosition.y < currentPosition.y ? -1 : 0);

        Vector2Int checkPosition = currentPosition + direction;

        while (checkPosition != targetPosition)
        {
            if (board[checkPosition.x, checkPosition.y] != null)
            {
                return false;
            }
            checkPosition += direction;
        }

        return true;
    }
}
