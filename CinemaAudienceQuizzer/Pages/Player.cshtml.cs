using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class PlayerModel : PageModel
{
    private readonly QuizState _quizState;

    [BindProperty]
    public Guess Guess { get; set; } = new();

    public QuizState QuizState => _quizState;
    public string CurrentSeatNumber { get; set; }

    public PlayerModel(QuizState quizState)
    {
        _quizState = quizState;
    }

    public void OnGet() {
        // Try to get seat number from cookie
        if (Request.Cookies.TryGetValue("seatNumber", out var seat))
        {
            CurrentSeatNumber = seat;
            Guess.SeatNumber = seat; // Pre-fill input
        }
    }

    public IActionResult OnPost()
    {
        if (_quizState.Guesses.Any(g => g.SeatNumber == Guess.SeatNumber && g.GameSessionId == _quizState.GameSessionId))
        {
            ModelState.AddModelError(string.Empty, "Du hast bereits eine Schätzung abgegeben");
            return Page();
        }

        // Add the guess with the correct GameSessionId
        _quizState.Guesses.Add(new Guess
        {
            SeatNumber = Guess.SeatNumber,
            RuntimeGuess = Guess.RuntimeGuess,
            GameSessionId = _quizState.GameSessionId
        });
        
        Response.Cookies.Append("seatNumber", Guess.SeatNumber, new CookieOptions { Path = "/" });

        return RedirectToPage();
    }
}