using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class Stockfish : MonoBehaviour
{
    
    private Process stockfishProcess;
    private StreamWriter stockfiishInput;
    private StreamReader stockfishOutput;

    private string stockfishPath;

    private void Start()
    {
        // Path to Stockfish binary
        stockfishPath = Path.Combine(Application.streamingAssetsPath, "stockfish"); 

        StartStockfish();
        SetSkillLevel(0);          // Weakest skill level
        SetSearchDepth(1); 
        LimitStrength(400);
    }

    private void StartStockfish()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = stockfishPath,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        stockfishProcess = new Process
        {
            StartInfo = startInfo
        };

        stockfishProcess.Start();

        stockfiishInput = stockfishProcess.StandardInput;
        stockfishOutput = stockfishProcess.StandardOutput;

        SendCommand("uci");
    }

    private void LimitStrength(int elo)
    {
        SendCommand("setoption name UCI_LimitStrength value true");
        SendCommand($"setoption name UCI_Elo value {elo}");
    }

    private void SetSkillLevel(int level)
    {
        SendCommand($"setoption name Skill Level value {level}");
    }

    private void SetSearchDepth(int depth)
    {
        if (depth < 1) depth = 1; // Minimum depth is 1
        SendCommand($"setoption name Depth value {depth}");
    }

    public async Task<string> GetBestMove(string fen)
    {
        SendCommand($"position fen {fen}");
        SendCommand("go movetime 10");

        return await ParseBestMove();
    }

    private async Task<string> ParseBestMove()
    {
        string bestMove = null;

        while (true)
        {
            string output = await stockfishOutput.ReadLineAsync();

            if (output.StartsWith("bestmove"))
            {
                bestMove = output.Split(' ')[1];
                break;
            }
        }

        return bestMove;
    }

    private void SendCommand(string command)
    {
        stockfiishInput.WriteLine(command);
        stockfiishInput.Flush();
    }

    private void OnDestroy()
    {
        SendCommand("quit");
        stockfishProcess?.Kill();
        stockfishProcess?.Dispose();
    }
}
