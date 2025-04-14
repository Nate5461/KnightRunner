using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public override bool IsValidMove(Vector2Int targetPosition, GameObject[,] board)
    {
        Vector2Int move = targetPosition - currentPosition;

        // Horizontal or vertical move
        if (move.x == 0 || move.y == 0)
        {
            if (!IsPathClear(targetPosition, board))
            {
                return false;
            }

            // Check if the target position is occupied by a friendly piece
            GameObject targetPiece = board[targetPosition.x, targetPosition.y];
            if (targetPiece != null)
            {
                Piece targetPieceComponent = targetPiece.GetComponent<Piece>();
                Piece currentPieceComponent = gameObject.GetComponent<Piece>();

                if (targetPieceComponent != null && currentPieceComponent != null)
                {
                    if (targetPieceComponent.color == currentPieceComponent.color)
                    {
                        return false; // Friendly piece is already there
                    }
                }
            }

            return true;
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
