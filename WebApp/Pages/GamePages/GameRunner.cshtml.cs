using System.Threading.Tasks;
using DAL;
using GameEngine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.GamePages
{
    public class GameRunner : PageModel
    {
        public Game Game { get; set; }
        public int GameId { get; set; }
        public bool Flag { get; set; }

        public async Task<ActionResult> OnGet(int? gameId, int? col, int? row, bool flag)
        {
            if (gameId == null)
            {
                return RedirectToPage("./GameIndex");
            }

            GameId = gameId.Value;

            Flag = flag;

            Game = GameStateHandler.LoadGameFromDb(GameId);

            if (Game == null)
            {
                return RedirectToPage("./GameIndex");
            }

            if (col != null && row != null)
            {
                var lost = Game.Move(row.Value, col.Value);
                if (!lost)
                {
                    Game.CheckWin(row.Value, col.Value);
                }

                await GameStateHandler.SaveGameToDb(Game, Game.Name);
            }

            return Page();
        }
    }
}