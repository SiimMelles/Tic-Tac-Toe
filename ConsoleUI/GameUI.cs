using System;
using System.ComponentModel;
using System.Diagnostics;
using GameEngine;

namespace ConsoleUI
{
    public class GameUI
    
    {
        private static readonly string _verticalSeparator = "|";
        private static readonly string _horizontalSeparator = "-";
        private static readonly string _centerSeparator = "+";

        
        public static void PrintBoard(Game game)
        {
            var board = game.GetBoard();
            for (int yIndex = 0; yIndex < game.BoardWidth; yIndex++)
            {
                var line = "";
                for (int xIndex = 0; xIndex < game.BoardWidth; xIndex++)
                {
                    line += " " + GetSingleState(board[yIndex, xIndex]) + " ";
                    if (xIndex < game.BoardWidth - 1)
                    {
                        line += _verticalSeparator;
                    }
                }
                Console.WriteLine(line);
                if (yIndex < game.BoardWidth - 1)
                {
                    line = "";
                    for (int xIndex = 0; xIndex <game.BoardWidth; xIndex++)
                    {
                        line += _horizontalSeparator + _horizontalSeparator + _horizontalSeparator;
                        if (xIndex < game.BoardWidth - 1)
                        {
                            line += _centerSeparator;
                        }
                        
                    } 
                    Console.WriteLine(line);
                }
            }
        }

        public static string GetSingleState(CellState state)
        {
            switch (state)
            {
                case CellState.Empty:
                    return " ";
                
                case CellState.O:
                    return "O";
                case CellState.X:
                    return "X";
               default:
                    throw new InvalidEnumArgumentException("Unknown enum");
            }
        }
    }
}