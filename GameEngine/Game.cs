using System;
using System.Collections.Generic;

namespace GameEngine
{
    /// <summary>
    /// TIC-TAC-TOE Game
    /// </summary>

    ///   +-----+
    ///    X| | 
    ///    -+-+-
    ///     | | 
    ///    -+-+-
    ///     | | 
    ///   +-----+

    public class Game
    {
        public Game()
        {
        }

        public int DbId { get; set; }
        public string Name { get; set; }
        public CellState[,] Board { get; set; }
        public int BoardWidth { get; set; }

        public bool GameLost { get; set; }
        public bool GameWon { get; set; }
        public bool GameTied { get; set; }
        public int MoveCount { get; set; }
        
        public bool VsAI { get; set; }

        public Game(int boardWidth, bool vsAi)
        {
            BoardWidth = boardWidth;
            VsAI = vsAi;
            Board = new CellState[BoardWidth, BoardWidth];
            if (VsAI)
            {
                BestMove();
                PlayerOneMove = !PlayerOneMove;
                MoveCount++;
            }
        }
        public bool PlayerOneMove { get; set; }
        
        public Game(GameSettings settings)
        {
            if (settings.BoardHeight < 3 || settings.BoardWidth < 3)
            {
                throw new Exception("Board size has to be at least 3x3!");
            }
            BoardWidth = settings.BoardWidth;
            Board = new CellState[BoardWidth, BoardWidth];
        }
        
        public CellState[,] GetBoard()
        {
            var result = new CellState[BoardWidth, BoardWidth];
            Array.Copy(Board, result, Board.Length);
            return result;
        }

        public bool Move(int posY, int posX)
        {
            if (Board[posY, posX] != CellState.Empty)
            {
                return false;
            }
            Board[posY, posX] = PlayerOneMove ? CellState.O : CellState.X;
            MoveCount = MoveCount + 1;
            
            CheckWin(posY, posX);
            if (GameWon)
            {
                return true;
            }
            
            PlayerOneMove = !PlayerOneMove;
            
            if (VsAI)
            {
                BestMove();
                if (GameLost)
                {
                    return true;
                }
                MoveCount++;
                PlayerOneMove = !PlayerOneMove;
            }
            
            if (!GameLost && !GameWon)
            {
                if (MoveCount == BoardWidth * BoardWidth)
                {
                    GameTied = true;
                    return true;
                }
            }
            return false;
        }

        public void BestMove()
        {
            var bestScore = -9999;
            var bestX = 0;
            var bestY = 0;
            for (int i = 0; i < BoardWidth; i++)
            {
                for (int j = 0; j < BoardWidth; j++)
                {
                    if (Board[i, j] == CellState.Empty)
                    {
                        Board[i, j] = CellState.X;
                        var score = MiniMax(Board, 0, false);
                        Board[i, j] = CellState.Empty;
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestY = i;
                            bestX = j;
                        }
                    }
                }
            }
            Board[bestY, bestX] = CellState.X;
            CheckWin(bestY, bestX);
        }

         
        private int MiniMax(CellState[,] board, int depth, bool isMaximizing)
        {
            var result = CheckWinMiniMax();

            if (result != null) return (int) result; 
            
            if (isMaximizing)
            {
                var bestScore = -9999;
                for (int i = 0; i < BoardWidth; i++)
                {
                    for (int j = 0; j < BoardWidth; j++)
                    {
                        if (Board[i, j] == CellState.Empty)
                        {
                            Board[i, j] = CellState.X;
                            var score = MiniMax(Board, depth + 1, false);
                            Board[i, j] = CellState.Empty;
                            bestScore = score > bestScore ? score : bestScore;
                        }
                    }
                }
                return bestScore;
            }
            else
            {
                var bestScore = 9999;
                for (int i = 0; i < BoardWidth; i++)
                {
                    for (int j = 0; j < BoardWidth; j++)
                    {
                        if (Board[i, j] == CellState.Empty)
                        {
                            Board[i, j] = CellState.O;
                            var score = MiniMax(Board, depth + 1, true);
                            Board[i, j] = CellState.Empty;
                            bestScore = score < bestScore ? score : bestScore;
                        }
                    }
                }
                return bestScore;
            }
        }
        
        public bool Equals3(CellState a,CellState b,CellState c) {
            return a == b && b == c && a != CellState.Empty;
        }

        public int? CheckWinMiniMax()
        {
            CellState? winner = null;

            // horizontal
            for (int i = 0; i < BoardWidth; i++)
            {
                if (Equals3(Board[i, 0], Board[i, 1],Board[i, 2])) {
                    winner = Board[i, 0];
                }
            }
            
            // Vertical
            for (var i = 0; i < BoardWidth; i++) {
                if (Equals3(Board[0, i], Board[1, i], Board[2, i])) {
                    winner = Board[0, i];
                }
            }
            
            // Diagonal
            if (Equals3(Board[0, 0], Board[1, 1], Board[2, 2])) {
                winner = Board[0, 0];
            }
            if (Equals3(Board[2, 0], Board[1, 1], Board[0, 2])) {
                winner = Board[2, 0];
            }
            
            var openSpots = 0;
            for (var i = 0; i < 3; i++) {
                for (var j = 0; j < 3; j++) {
                    if (Board[i, j] == CellState.Empty) {
                        openSpots++;
                    }
                }
            }
            
            if (winner == null && openSpots == 0) {
                return 0;
            }
            
            switch (winner)
            {
                case CellState.O:
                    return -1;
                case CellState.X:
                    return 1;
            }
            
            return null;
        }
        
        public void CheckWin(int posY, int posX)
        {
            if (MoveCount == BoardWidth * BoardWidth)
            {
                GameTied = true;
            }
            
            // check row
            for (int i = 0; i < BoardWidth; i++)
            {
                if (PlayerOneMove)
                {
                    if (Board[posY, i] != CellState.O)
                    {
                        break;
                    }
                    if (i == BoardWidth - 1)
                    {
                        GameLost = true;
                    }
                }
                else
                {
                    if (Board[posY, i] != CellState.X)
                    {
                        break;
                    } 
                    if (i == BoardWidth - 1)
                    {
                        GameWon = true;
                    }
                }
            }
            
            // check column
            for (int i = 0; i < BoardWidth; i++)
            {
                if (PlayerOneMove)
                {
                    if (Board[i, posX] != CellState.O)
                    {
                        break;
                    }
                    if (i == BoardWidth - 1)
                    {
                        GameLost = true;
                    }
                }
                else
                {
                    if (Board[i, posX] != CellState.X)
                    {
                        break;
                    } 
                    if (i == BoardWidth - 1)
                    {
                        GameWon = true;
                    }
                }
            }
            // diagonal
            if (posX == posY)
            {
                for (int i = 0; i < BoardWidth; i++)
                {
                    if (PlayerOneMove)
                    {
                        if (Board[i, i] != CellState.O)
                        {
                            break;
                        }
                        if (i == BoardWidth - 1)
                        {
                            GameLost = true;
                        }
                    }
                    else
                    {
                        if (Board[i, i] != CellState.X)
                        {
                            break;
                        } 
                        if (i == BoardWidth - 1)
                        {
                            GameWon = true;
                        }
                    }
                }
            }
            // reverse diag
            if (posX + posY == BoardWidth - 1)
            {
                for (var i = 0; i < BoardWidth; i++) {
                    
                    if (PlayerOneMove)
                    {
                        if(Board[i , (BoardWidth-1) - i] != CellState.O)
                            break;
                        if(i == BoardWidth - 1){
                            GameLost = true;
                        }   
                    }
                    else
                    {
                        if(Board[i , (BoardWidth-1) - i] != CellState.X)
                            break;
                        if(i == BoardWidth - 1){
                            GameWon = true;
                        }
                    }
                }
            }
        }
    }
}