using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override bool IsValidMove(Vector2Int targetPosition, GameObject[,] board)
    {
        Vector2Int move = targetPosition - currentPosition;

        if (Mathf.Abs(move.x) <= 1 && Mathf.Abs(move.y) <= 1 && targetPosition.x >= 0 && targetPosition.x < 8 && targetPosition.y >= 0 && targetPosition.y < 8)
        {
            GameObject targetPiece = board[targetPosition.x, targetPosition.y];
            if (targetPiece == null || targetPiece.GetComponent<Piece>().color != this.color)
            {
                if (!WouldBeInCheck(targetPosition, board))
                {
                    return true;
                }
            }
        }

        if (Mathf.Abs(move.x) == 2 && move.y == 0 && CanCastle(targetPosition, board))
        {
            return true;
        }

        return false;
    }

    
    private bool CanCastle(Vector2Int targetPosition, GameObject[,] board)
    {
        if (HasMoved || IsInCheck())
        {
            Debug.Log("Cannot castle: King has moved or is in check.");
            return false;
        }

        int direction = targetPosition.x > currentPosition.x ? 1 : -1;
        Vector2Int rookPosition = direction == 1 ? new Vector2Int(7, currentPosition.y) : new Vector2Int(0, currentPosition.y);

        GameObject rookObject = board[rookPosition.x, rookPosition.y];
        if (rookObject == null || rookObject.GetComponent<Rook>().HasMoved)
        {
            Debug.Log("Cannot castle: Rook has moved.");
            return false;
        }


        // Check that all squares between king and rook are empty
        for (int i = currentPosition.x + direction; i != rookPosition.x; i += direction)
        {
            if (board[i, currentPosition.y] != null)
            {
                Debug.Log("Cannot castle: Pieces between king and rook.");
                return false;
            }

            // Ensure the king does not pass through or land in check
            if (WouldBeInCheck(new Vector2Int(i, currentPosition.y), board))
            {
                Debug.Log("Cannot castle: King passes through or lands in check.");
                return false;
            }
                
        }

        return true;
    }

    private bool IsInCheck()
    {
        Debug.Log("IsInCheck ran");
        ChessController controller = FindObjectOfType<ChessController>();
        return controller.IsInCheck(color, currentPosition, controller.board);
    }

    private bool WouldBeInCheck(Vector2Int position, GameObject[,] board)
    {
        // Simulate moving the king to the new position and check for attacks
        GameObject originalPiece = board[position.x, position.y];
        board[currentPosition.x, currentPosition.y] = null;
        board[position.x, position.y] = gameObject;

        ChessController controller = FindObjectOfType<ChessController>();
        bool inCheck = controller.IsInCheck(color, position, board);

        board[position.x, position.y] = originalPiece;
        board[currentPosition.x, currentPosition.y] = gameObject;

        return inCheck;
    }
}
