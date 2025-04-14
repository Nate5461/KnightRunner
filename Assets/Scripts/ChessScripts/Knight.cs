using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public override bool IsValidMove(Vector2Int targetPosition, GameObject[,] board)
{
    Vector2Int move = targetPosition - currentPosition;
    int xAbs = Mathf.Abs(move.x);
    int yAbs = Mathf.Abs(move.y);

    // "L" shape movement
    bool isLShapeMove = (xAbs == 2 && yAbs == 1) || (xAbs == 1 && yAbs == 2);

    if (!isLShapeMove)
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
}
