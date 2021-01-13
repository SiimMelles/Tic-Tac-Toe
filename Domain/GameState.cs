using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameState
    {
        
        public int GameStateId { get; set; }

        [MinLength(2)]
        [MaxLength(128)]
        [Display(Name = "Gamesave name:")]
        public string Name { get; set; } = default!;
        public string BoardString { get; set; } = default!;
        
        [Display(Name = "Width of the board:")]
        [Range(3, 10)]
        public int BoardWidth { get; set; } = default!;
        public bool PlayerOneMove { get; set; }
        public int MoveCount { get; set; } = default!;

        public bool GameLost { get; set; } = false;
        public bool GameWon { get; set; } = false;
        public bool GameTied { get; set; }
        public bool VsAI { get; set; }
    
    }
}