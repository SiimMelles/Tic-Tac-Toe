using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using GameEngine;
using Newtonsoft.Json;
using static System.IO.Path;

namespace DAL
{
    public class GameStateHandler
    {
        
        private const string FileName = "123.json";
        private static readonly string PathBase = Combine(AppDomain.CurrentDomain.BaseDirectory);
        private static string _saveFolder = PathBase;

        public static void SaveGame(Game game, string fileName = FileName)
        {
            
            fileName = fileName + ".json";
            CheckSaveGameFolder();
            string[] paths = {_saveFolder, fileName};
            using var writer = File.CreateText(Combine(paths));
            var jsonString = JsonConvert.SerializeObject(game);
            writer.Write(jsonString);
        }

        public static Game LoadGame(string fileName = FileName)
        {
            var paths = new[] {_saveFolder, fileName};
            if (!File.Exists(Combine(paths))) throw new Exception("Save file not found!");
            var jsonString = File.ReadAllText(Combine(paths));
            var res = JsonConvert.DeserializeObject<Game>(jsonString);
            return res;

        }
        
        public static void DeleteSave(string fileName)
        {
            fileName += ".json";
            var paths = new string[2] {_saveFolder, fileName};
            File.Delete(Combine(paths));
        }
        public static string[] AllSavedGames()
        {
            CheckSaveGameFolder();
            var d = new DirectoryInfo(_saveFolder);
            var files = d.GetFiles("*.json");
            var filesList = new List<string>();
            foreach (var file in files)
            {
                filesList.Add(file.ToString());            
            }
            return filesList.ToArray();
        }

        public static void CheckSaveGameFolder()
        {
            var paths = new string[2] {PathBase, "gamesaves"};
            if (!Directory.Exists(Combine(paths)))
            {
                Directory.CreateDirectory(Combine(paths));
            }

            if (_saveFolder != Combine(paths))
            {
             _saveFolder = Combine(paths);   
            }
        }

        public static async Task SaveGameToDb(Game game, string gameName)
        {
            using (var ctx = new AppDbContext())
            {
                if (ctx.GameStates.Find(game.DbId) != null)
                {
                    var entity = ctx.GameStates.Find(game.DbId);
                    entity.PlayerOneMove = game.PlayerOneMove;
                    entity.BoardString = JsonConvert.SerializeObject(game.Board);
                    entity.GameLost = game.GameLost;
                    entity.GameWon = game.GameWon;
                    entity.GameTied = game.GameTied;
                    entity.VsAI = game.VsAI;
                    entity.MoveCount = game.MoveCount;
                    ctx.GameStates.Update(entity);
                    await ctx.SaveChangesAsync();
                }
                else
                {
                    var gameState = new GameState()
                    {
                        Name = game.Name,
                        BoardWidth = game.BoardWidth,
                        BoardString = JsonConvert.SerializeObject(game.Board),
                        PlayerOneMove = game.PlayerOneMove,
                        GameWon = game.GameWon,
                        GameLost = game.GameLost,
                        GameTied = game.GameTied,
                        VsAI = game.VsAI,
                        MoveCount = game.MoveCount
                    };
                    
                    ctx.Add(gameState);
                    await ctx.SaveChangesAsync();
                    var id = ctx.GameStates.Max(x => x.GameStateId);
                    game.DbId = id;  
                }
                
            }
        }

        public static Game LoadGameFromDb(int gameId)
        {
            using (var ctx = new AppDbContext())
            {
                if (!ctx.GameStates.Any(x => x.GameStateId == gameId))
                {
                    return null;
                }
                var entity = ctx.GameStates.Find(gameId);
                
                var game = new Game()
                {
                    Name = entity.Name,
                    BoardWidth = entity.BoardWidth,
                    DbId = entity.GameStateId,
                    Board = JsonConvert.DeserializeObject<CellState[,]>(entity.BoardString),
                    PlayerOneMove = entity.PlayerOneMove,
                    GameLost = entity.GameLost,
                    GameWon = entity.GameWon,
                    GameTied = entity.GameTied,
                    VsAI = entity.VsAI,
                    MoveCount = entity.MoveCount
                };
                 
                return game;
            }
        }
        
        public static void DeleteFromDbUsingId(int id)
        {
            using (var ctx = new AppDbContext())
            {
                var entity = ctx.GameStates.Find(id);

                ctx.GameStates.Remove(entity);
                ctx.SaveChanges();
            }
        }
        
        public static void DeleteFromDb(Game game)
        {
            using (var ctx = new AppDbContext())
            {
                var entity = ctx.GameStates.Find(game.DbId);

                ctx.GameStates.Remove(entity);
                ctx.SaveChanges();
            }
        }

        public static List<GameState> ShowGameSavesInDb()
        {
            var gameSaves = new List<GameState>{};
            
            using (var ctx = new AppDbContext())
            {
                gameSaves.AddRange(ctx.GameStates.Select(gameState => gameState));
            }
            return gameSaves;
        }
    }
}
