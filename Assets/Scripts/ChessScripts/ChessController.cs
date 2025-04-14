using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ChessController : MonoBehaviour
{


    public GameObject[,] board = new GameObject[8, 8];

    public GameObject whitePawnPrefab;
    public GameObject blackPawnPrefab;
    public GameObject whiteRookPrefab;
    public GameObject blackRookPrefab;
    public GameObject whiteKnightPrefab;
    public GameObject blackKnightPrefab;
    public GameObject whiteBishopPrefab;
    public GameObject blackBishopPrefab;
    public GameObject whiteQueenPrefab;
    public GameObject blackQueenPrefab;
    public GameObject whiteKingPrefab;
    public GameObject blackKingPrefab;


    public Grid grid;

    public Stockfish stockfish;

    public TextMeshProUGUI GameEnd;
    public GameObject contBtn;
    private GameObject selectedPiece;
    private bool isPlayerTurn = true;
    private bool isAIMoving = false;
    private bool result = false;

    // Start is called before the first frame update
    void Start()
    {
        contBtn.SetActive(false);
        GameEnd.text = "";
        SetupBoard();
    }

    void SetupBoard()
    {


    // Instantiate pieces in starting positions
    for (int i = 0; i < 8; i++)
    {
        board[i, 1] = Instantiate(whitePawnPrefab, GetWorldPosition(i, 1), Quaternion.identity);
        board[i, 1].GetComponent<Piece>().currentPosition = new Vector2Int(i, 1);
        board[i, 6] = Instantiate(blackPawnPrefab, GetWorldPosition(i, 6), Quaternion.identity);
        board[i, 6].GetComponent<Piece>().currentPosition = new Vector2Int(i, 6);
    }

    // Add other pieces (rooks, knights, etc.)
    board[0, 0] = Instantiate(whiteRookPrefab, GetWorldPosition(0, 0), Quaternion.identity);
    board[7, 0] = Instantiate(whiteRookPrefab, GetWorldPosition(7, 0), Quaternion.identity);
    board[0, 0].GetComponent<Piece>().currentPosition = new Vector2Int(0, 0);
    board[7, 0].GetComponent<Piece>().currentPosition = new Vector2Int(7, 0);


    board[0, 7] = Instantiate(blackRookPrefab, GetWorldPosition(0, 7), Quaternion.identity);
    board[7, 7] = Instantiate(blackRookPrefab, GetWorldPosition(7, 7), Quaternion.identity);
    board[0, 7].GetComponent<Piece>().currentPosition = new Vector2Int(0, 7);
    board[7, 7].GetComponent<Piece>().currentPosition = new Vector2Int(7, 7);


    board[1, 0] = Instantiate(whiteKnightPrefab, GetWorldPosition(1, 0), Quaternion.identity);
    board[6, 0] = Instantiate(whiteKnightPrefab, GetWorldPosition(6, 0), Quaternion.identity);
    board[1, 0].GetComponent<Piece>().currentPosition = new Vector2Int(1, 0);
    board[6, 0].GetComponent<Piece>().currentPosition = new Vector2Int(6, 0);

    board[1, 7] = Instantiate(blackKnightPrefab, GetWorldPosition(1, 7), Quaternion.identity);
    board[6, 7] = Instantiate(blackKnightPrefab, GetWorldPosition(6, 7), Quaternion.identity);
    board[1, 7].GetComponent<Piece>().currentPosition = new Vector2Int(1, 7);
    board[6, 7].GetComponent<Piece>().currentPosition = new Vector2Int(6, 7);

    board[2, 0] = Instantiate(whiteBishopPrefab, GetWorldPosition(2, 0), Quaternion.identity);
    board[5, 0] = Instantiate(whiteBishopPrefab, GetWorldPosition(5, 0), Quaternion.identity);
    board[2, 0].GetComponent<Piece>().currentPosition = new Vector2Int(2, 0);
    board[5, 0].GetComponent<Piece>().currentPosition = new Vector2Int(5, 0);

    board[2, 7] = Instantiate(blackBishopPrefab, GetWorldPosition(2, 7), Quaternion.identity);
    board[5, 7] = Instantiate(blackBishopPrefab, GetWorldPosition(5, 7), Quaternion.identity);
    board[2, 7].GetComponent<Piece>().currentPosition = new Vector2Int(2, 7);
    board[5, 7].GetComponent<Piece>().currentPosition = new Vector2Int(5, 7);


    board[3, 0] = Instantiate(whiteQueenPrefab, GetWorldPosition(3, 0), Quaternion.identity);
    board[4, 0] = Instantiate(whiteKingPrefab, GetWorldPosition(4, 0), Quaternion.identity);
    board[3, 0].GetComponent<Piece>().currentPosition = new Vector2Int(3, 0);
    board[4, 0].GetComponent<Piece>().currentPosition = new Vector2Int(4, 0);

    board[3, 7] = Instantiate(blackQueenPrefab, GetWorldPosition(3, 7), Quaternion.identity);
    board[4, 7] = Instantiate(blackKingPrefab, GetWorldPosition(4, 7), Quaternion.identity);
    board[3, 7].GetComponent<Piece>().currentPosition = new Vector2Int(3, 7);
    board[4, 7].GetComponent<Piece>().currentPosition = new Vector2Int(4, 7);

    }

    Vector3 GetWorldPosition(int x, int y)
{
    Vector3Int cellPosition = new Vector3Int(x, y, 0);
    Vector3 cellWorldPosition = grid.CellToWorld(cellPosition);
    Vector3 cellCenterOffset = new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2, 0);

    
    Vector3 manualOffset = new Vector3(-4 * grid.cellSize.x, -4 * grid.cellSize.y, 0); // Adjust -4 by the observed offset
    return cellWorldPosition + cellCenterOffset + manualOffset;
}
    
    async void Update()
    {
        if (isPlayerTurn && Input.GetMouseButtonDown(0))
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int square = GetSquareFromPosition(worldPosition);
            if (IsValidSquare(square))
            {
                Debug.Log("Clicked square: " + square);
                // Handle the square interaction (select/move pieces)
                PlayerMove(square);
            }
        }
        else if (!isPlayerTurn && !isAIMoving)
        {
            isAIMoving = true;
            await AIMove();
            isAIMoving = false;
        }
    }

    void PlayerMove(Vector2Int clickedSquare)
    {
        if (selectedPiece == null)
        {
            // Select a piece
            selectedPiece = board[clickedSquare.x, clickedSquare.y];
            if (selectedPiece != null && selectedPiece.GetComponent<Piece>().color == "White")
            {
                Debug.Log("Selected: " + selectedPiece.name);
                Debug.Log("Square: " + clickedSquare);
            }
            else
            {
                selectedPiece = null; // Deselect invalid piece
            }
        }
        else
        {
            // Attempt to move the piece
            Piece pieceScript = selectedPiece.GetComponent<Piece>();
            if (pieceScript.IsValidMove(clickedSquare, board))
            {
                MovePiece(selectedPiece, clickedSquare);
                CheckForCheckmate();

            }
            else
            {
                Debug.Log("Invalid move");
            }
            selectedPiece = null; // Deselect after move attempt
        }
    }

    async Task AIMove()
    {
        Debug.Log("AI's turn");
        string fen = GetFEN();

        string bestMove = await stockfish.GetBestMove(fen);
        
        
        
        Debug.Log("Best move: " + bestMove);

        Vector2Int from = ParsePosition(bestMove.Substring(0, 2));
        Vector2Int to = ParsePosition(bestMove.Substring(2, 2));

        GameObject piece = board[from.x, from.y];

        if (piece != null)
        {
            MovePiece(piece, to);
            CheckForCheckmate();
        }

        isPlayerTurn = true; // End AI's turn
    }

    void MovePiece(GameObject piece, Vector2Int targetPosition)
    {
        Piece pieceScript = piece.GetComponent<Piece>();

        if (WouldBeInCheckAfterMove(pieceScript, targetPosition, board))
        {
            Debug.Log("Invalid move: King would be in check");
            return;
        }

        // Check if the target position is occupied by an opponent's piece
        GameObject targetPiece = board[targetPosition.x, targetPosition.y];
        if (targetPiece != null)
        {
            Piece targetPieceScript = targetPiece.GetComponent<Piece>();
            // Ensure the target piece belongs to the opponent
            if (targetPieceScript.color != pieceScript.color)
            {
                // Remove the opponent's piece from the board
                Destroy(targetPiece);
            }
            else
            {
                // If the target piece belongs to the same player, do not move
                Debug.Log("Cannot capture your own piece");
                return;
            }
        }


        if (pieceScript is King && Mathf.Abs(targetPosition.x - pieceScript.currentPosition.x) == 2)
        {
            int direction = targetPosition.x > pieceScript.currentPosition.x ? 1 : -1;
            Vector2Int rookPosition = direction == 1 ? new Vector2Int(7, pieceScript.currentPosition.y) : new Vector2Int(0, pieceScript.currentPosition.y);
            Vector2Int newRookPosition = pieceScript.currentPosition + new Vector2Int(direction, 0);

            GameObject rook = board[rookPosition.x, rookPosition.y];
            board[rookPosition.x, rookPosition.y] = null;
            board[newRookPosition.x, newRookPosition.y] = rook;
            rook.GetComponent<Piece>().currentPosition = newRookPosition;
            rook.transform.position = GetWorldPosition(newRookPosition.x, newRookPosition.y);
        }

        // Update the board
        board[pieceScript.currentPosition.x, pieceScript.currentPosition.y] = null;
        pieceScript.currentPosition = targetPosition;
        board[targetPosition.x, targetPosition.y] = piece;

        // Move the piece to the new position
        piece.transform.position = GetWorldPosition(targetPosition.x, targetPosition.y);
        
        if (pieceScript is Pawn && (targetPosition.y == 0 || targetPosition.y == 7))
        {
            PromotePawn(piece, targetPosition);
        }

        isPlayerTurn = false; 
    }

    void PromotePawn(GameObject pawn, Vector2Int position)
    {
        Piece pawnScript = pawn.GetComponent<Piece>();
        GameObject newQueen = Instantiate(pawnScript.color == "White" ? whiteQueenPrefab : blackQueenPrefab, GetWorldPosition(position.x, position.y), Quaternion.identity);
        newQueen.GetComponent<Piece>().currentPosition = position;

        // Replace the pawn with the new queen on the board
        board[position.x, position.y] = newQueen;
        Destroy(pawn);
    }

    void CheckForCheckmate()
    {
        //string opponentColor = isPlayerTurn ? "Black" : "White";
        string opponentColor = isAIMoving ? "White" : "Black";
        Vector2Int kingPosition = FindKingPosition(opponentColor, board);

        if (IsCheckmate(opponentColor, kingPosition, board))
        {
            Debug.Log("Checkmate!!!");
            EndGame(opponentColor == "White" ? "Black" : "White");
            isPlayerTurn = false;
        }
    }

    void EndGame(string winner)
    {
        Debug.Log("Checkmate! " + winner + " wins!");
        
        contBtn.SetActive(true);
        if (winner == "White")
        {
            GameResult.PlayerWon = true;
            result = true;
            GameEnd.text = "You have won!, I grant you my powers!";
        }
        else
        {
            GameResult.PlayerWon = false;
            result = false;
            GameEnd.text = "You have lost, come try again!";
        }

    }

    public void continueGame()
    {  

        if (result)
        {
            SceneManager.LoadScene("mountain_level_won");
        }
        else
        {
            SceneManager.LoadScene("cave_level");
        }

    }

    string GetFEN()
    {
        string fen = "";
        for (int y = 7; y >= 0; y--)
        {
            int emptyCount = 0;
            for (int x = 0; x < 8; x++)
            {
                GameObject currentPiece = board[x, y];
                if (currentPiece == null)
                {
                    emptyCount++;
                }
                else
                {
                    if (emptyCount > 0)
                    {
                        fen += emptyCount;
                        emptyCount = 0;
                    }


                    fen += GetPieceSymbol(currentPiece.GetComponent<Piece>());
                }
            }

            if (emptyCount > 0)
            {
                fen += emptyCount;
            }
            if (y > 0)
            {
                fen += "/";
            }
        }

        fen += isPlayerTurn ? " w " : " b ";

        fen += " - - 0 1";

        return fen;
    }

    char GetPieceSymbol(Piece piece){
        char symbol = piece.type switch
        {
            "Pawn" => 'p',
            "Rook" => 'r',
            "Knight" => 'n',
            "Bishop" => 'b',
            "Queen" => 'q',
            "King" => 'k',
            _ => throw new System.Exception("Invalid piece type")
        };

        return piece.color == "White" ? char.ToUpper(symbol) : char.ToLower(symbol);
    }

    Vector2Int ParsePosition(string position)
    {
        int x = position[0] - 'a';
        int y = int.Parse(position[1].ToString()) - 1;
        return new Vector2Int(x, y);
    }


    Vector2Int GetSquareFromPosition(Vector2 position)
    {
        // Convert the Vector2 position to a Vector3
        Vector3 position3D = new Vector3(position.x, position.y, 0);

        // Apply the inverse of the manual offset
        Vector3 manualOffset = new Vector3(-4 * grid.cellSize.x, -4 * grid.cellSize.y, 0); // Match the offset applied in GetWorldPosition
        Vector3 adjustedPosition = position3D - manualOffset;

        Vector3Int cellPosition = grid.WorldToCell(adjustedPosition);
        return new Vector2Int(cellPosition.x, cellPosition.y);
    }

    bool IsValidSquare(Vector2Int square)
    {
        return square.x >= 0 && square.x < 8 && square.y >= 0 && square.y < 8;
    }

    public bool IsInCheck(string kingColor, Vector2Int kingPosition, GameObject[,] board)
    {
        foreach (GameObject pieceObject in board)
        {
            if (pieceObject == null) continue;

            Piece piece = pieceObject.GetComponent<Piece>();
            if (piece.color != kingColor && piece.IsValidMove(kingPosition, board))
            {
                //Debug.Log("King is in check");
                return true;
            }
        }

        Debug.Log("King is not in check" + kingColor + " " + kingPosition);
        return false;
    }

    public bool IsCheckmate(string kingColor, Vector2Int kingPosition, GameObject[,] board)
    {
        // First, verify that the king is actually in check
        if (!IsInCheck(kingColor, kingPosition, board)){
            Debug.Log("King is not in check, no checkmate");
            return false;
        }

        // Check if the king can move to a safe square
        foreach (GameObject pieceObject in board)
        {
            if (pieceObject == null) continue;

            Piece piece = pieceObject.GetComponent<Piece>();
            if (piece.color == kingColor)
            {
                List<Vector2Int> validMoves = GetValidMoves(piece, board);
                foreach (Vector2Int move in validMoves)
                {
                    // Simulate move and check if it resolves the check
                    if (!WouldBeInCheckAfterMove(piece, move, board))
                        return false;
                }
            }
        }
        return true;
    }


    private List<Vector2Int> GetValidMoves(Piece piece, GameObject[,] board)
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Vector2Int targetPosition = new Vector2Int(x, y);
                if (piece.IsValidMove(targetPosition, board) && !WouldBeInCheckAfterMove(piece, targetPosition, board))
                {
                    validMoves.Add(targetPosition);
                }
            }
        }

        return validMoves;
    }



    private bool WouldBeInCheckAfterMove(Piece piece, Vector2Int targetPosition, GameObject[,] board)
    {
        Debug.Log("Checking if move would result in check");
        GameObject[,] simulatedBoard = new GameObject[8, 8];
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                simulatedBoard[x, y] = board[x, y];
            }
        }
        simulatedBoard[targetPosition.x, targetPosition.y] = simulatedBoard[piece.currentPosition.x, piece.currentPosition.y];
        simulatedBoard[piece.currentPosition.x, piece.currentPosition.y] = null;


        Debug.Log("Simulated move with piece " + piece.name + " " + piece.currentPosition + " to " + targetPosition);
        Vector2Int kingPosition = piece is King ? targetPosition : FindKingPosition(piece.color, simulatedBoard);

        Debug.Log("King piece " + kingPosition);
        return IsInCheck(piece.color, kingPosition, simulatedBoard);
    }

    

    private Vector2Int FindKingPosition(string color, GameObject[,] board)
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                GameObject pieceObject = board[x, y];
                if (pieceObject != null)
                {
                    Piece piece = pieceObject.GetComponent<Piece>();
                    Debug.Log("Piece color " + piece.color + " " + color);
                    if (piece.color == color && piece is King)
                    {
                        Debug.Log(color + " King found at " + x + ", " + y);
                        return new Vector2Int(x, y);
                    }
                }
            }
        }

        Debug.Log("King not found");
        return new Vector2Int(-1, -1); // King not found
    }

    public void forfeit()
    {
        Debug.Log("Forfeit");
        EndGame("Black");
    }
}
