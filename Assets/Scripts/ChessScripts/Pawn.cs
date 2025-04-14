using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override bool IsValidMove(Vector2Int targetPosition, GameObject[,] board)
    {
        int direction = color == "White" ? 1 : -1; // White moves up, Black moves down
        Vector2Int move = targetPosition - currentPosition;

        // Forward move
        if (move == new Vector2Int(0, direction))
        {
            return board[targetPosition.x, targetPosition.y] == null;
        }

        // First move (two squares)
        if (move == new Vector2Int(0, 2 * direction) && currentPosition.y == (color == "White" ? 1 : 6))
        {
            return board[targetPosition.x, targetPosition.y] == null && board[currentPosition.x, currentPosition.y + direction] == null;
        }

        // Diagonal capture
        if ((move == new Vector2Int(1, direction) || move == new Vector2Int(-1, direction)) &&
            board[targetPosition.x, targetPosition.y] != null &&
            board[targetPosition.x, targetPosition.y].GetComponent<Piece>().color != color)
        {
            return true;
        }

        return false;



    }

    public bool CanEnPassant(Vector2Int targetPosition, Vector2Int lastMoveStart, Vector2Int lastMoveEnd, GameObject[,] board)
    {
        Vector2Int move = targetPosition - currentPosition;
        int direction = color == "White" ? 1 : -1;

        // Check for diagonal move and correct direction
        if ((move == new Vector2Int(1, direction) || move == new Vector2Int(-1, direction)) &&
            lastMoveEnd == new Vector2Int(targetPosition.x, currentPosition.y) &&
            board[lastMoveEnd.x, lastMoveEnd.y] != null &&
            board[lastMoveEnd.x, lastMoveEnd.y].GetComponent<Pawn>() != null &&
            board[lastMoveEnd.x, lastMoveEnd.y].GetComponent<Pawn>().color != color)
        {
            return true;
        }

        return false;
    }


}
