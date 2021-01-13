using System.Threading.Tasks;
using DAL;
using Domain;
using GameEngine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.GamePages
{
    public class NewGame : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public NewGame(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public GameState GameState { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? difficulty)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (difficulty == null)
            {
                return RedirectToPage("./GameIndex");
            }

            if (difficulty == 1)
            {
                var game = new Game(3, true);
                await GameStateHandler.SaveGameToDb(game, GameState.Name);
                return RedirectToPage("./GameRunner", new {gameId = game.DbId});
            }
            if (difficulty == 2) 
            {
                var game = new Game(3, false);
                await GameStateHandler.SaveGameToDb(game, GameState.Name);
                return RedirectToPage("./GameRunner", new {gameId = game.DbId});
            }

            return RedirectToPage("./GameIndex");
        }
    }
}