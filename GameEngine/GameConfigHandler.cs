using System.Text.Json;
using GameEngine;

namespace GameEngine
{
    public class GameConfigHandler
    {
        private const string FileName = "gamesettings.json";
        public static void SaveConfig(GameSettings settings, string fileName = FileName)
        {
            using (var writer = System.IO.File.CreateText(fileName))
            {
                var jsonString = JsonSerializer.Serialize(settings);
                writer.Write(jsonString);
            }
        }

        public static GameSettings LoadConfig(string fileName = FileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                var jsonString = System.IO.File.ReadAllText(fileName); 
                var res = JsonSerializer.Deserialize<GameSettings>(jsonString);
                return res;
            }
            
            return new GameSettings();
        }
    }
}