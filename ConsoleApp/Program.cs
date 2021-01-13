using System;
using System.Collections.Generic;
using ConsoleUI;
using GameEngine;
using MenuSystem;

namespace ConsoleApp
{
    class Program
    {
        private static GameSettings _settings; 
        static void Main(string[] args)
        {
            Console.Clear();

            _settings = GameConfigHandler.LoadConfig();
            
            Console.WriteLine($"Hello {_settings.GameName}!");
            
            var menuLevel2 = new Menu(2)
            {
                Title = "Level2 menu",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
                {
                    {
                        "1", new MenuItem()
                        {
                            Title = "Nothing here",
                            CommandToExecute = null
                        }
                    },
                }
            };
            
            var gameMenu = new Menu(1)
            {
                Title = $"Start a new game of {_settings.GameName}",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
                {
                    {
                        "1", new MenuItem()
                        {
                            Title = "Human against AI",
                            CommandToExecute = RunGameAI
                        }
                    },
                    {
                        "3", new MenuItem()
                        {
                            Title = "Human against Human",
                            CommandToExecute = RunGamePvP
                        }
                    },
                    {
                        "4", new MenuItem()
                        {
                            Title = "One more menu",
                            CommandToExecute = menuLevel2.Run
                        }
                    },
                }
            };

 
            
            var menu0 = new Menu(0)
            {
                Title = $"{_settings.GameName} Main Menu",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
                {
                    {
                        "S", new MenuItem()
                        {
                            Title = "Start game",
                            CommandToExecute = gameMenu.Run
                        }
                    },
                    {
                        "J", new MenuItem()
                        {
                            Title = "Set defaults for game (save to JSON)",
                            CommandToExecute = SaveSettings
                        }
                    }
                }
            };
            
            menu0.Run();
        }

        static string SaveSettings() 
        {
                Console.Clear();
                var boardWidth = 0;
                var boardHeight = 0;
                var userCanceled = false;
                
                (boardWidth, userCanceled) = GetUserIntInput("Enter board width (3-20)", 3, 20, 0);
            
                if (userCanceled)
                {
                    return "";
                }
                (boardHeight, userCanceled) = GetUserIntInput("Enter board height (3-20)", 3, 20, 0);
            
                if (userCanceled)
                {
                    return "";
                }
                
                _settings.BoardHeight = boardHeight;
                _settings.BoardWidth = boardWidth;
                GameConfigHandler.SaveConfig(_settings);
                
                return "";
        }
        private static string RunGamePvP()
        {
            var game = new Game(3, false);
            GameRunner(game);
            return "";
        }

        private static string RunGameAI()
        {
            var game = new Game(3, true);
            GameRunner(game);
            return "";
        }
        
        static string GameRunner(Game game)
        {
            
            

            var done = false;
            do
            {
                Console.Clear();
                GameUI.PrintBoard(game);
                
                var userXint = 0;
                var userYint = 0;
                var userCanceled = false;
                
                (userXint, userCanceled) = GetUserIntInput("Enter X coordinate", 1, game.BoardWidth, 0);
                if (!userCanceled)
                {
                    (userYint, userCanceled) = GetUserIntInput("Enter Y coordinate", 1, game.BoardWidth, 0);
                }

                if (userCanceled)
                {
                    done = true;
                }
                else
                {
                    game.Move(userYint-1, userXint-1);
                }

                if (!done)
                {
                    done = game.GameWon || game.GameLost || game.GameTied;
   
                }
                
            } while (!done);
            
            GameUI.PrintBoard(game);

            if (game.GameWon)
            {
                Console.WriteLine("Player 2 won!");
            }

            if (game.GameLost)
            {
                Console.WriteLine("Player 1 won!");
            }

            if (game.GameTied)
            {
                Console.WriteLine("Game tied!");

            }

            return "";
        }

        static (int result, bool wasCanceled) GetUserIntInput(string prompt, int min, int max, int? cancelIntValue = null, string cancelStrValue = "" )
        { 
            do
            {
                Console.WriteLine(prompt);
                if (cancelIntValue.HasValue || !string.IsNullOrWhiteSpace(cancelStrValue))
                {
                    /*Console.WriteLine($"To cancel input enter: {cancelIntValue}" +
                                      $"{ (cancelIntValue.HasValue && !string.IsNullOrWhiteSpace(cancelStrValue) ? " or " : "") }" +
                                      $"{cancelStrValue}");*/
                    Console.WriteLine($"To cancel game press Enter");
                }

                Console.Write(">");
                var consoleLine = Console.ReadLine();

                if (consoleLine == cancelStrValue) return (0, true);
                
                if (int.TryParse(consoleLine, out var userInt))
                {
                    if (!(userInt > max || userInt < min))
                    {
                        return userInt == cancelIntValue ? (userInt, true) : (userInt, false);
                    }
                    else
                    {
                        Console.WriteLine($"'{consoleLine}' is out of bounds!");
                    }
                }
                else
                {
                    Console.WriteLine($"'{consoleLine}' cant be converted to int value!");
                    
                }

            } while (true);  

        }
        
    }
}
