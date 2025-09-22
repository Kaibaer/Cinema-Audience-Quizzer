using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class PlayerModel : PageModel
{
    private readonly QuizState _quizState;

    [BindProperty]
    public Guess Guess { get; set; } = new();

    public QuizState QuizState => _quizState;

    public PlayerModel(QuizState quizState)
    {
        _quizState = quizState;
    }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        _quizState.Guesses.Add(new Guess
        {
            SeatNumber = Guess.SeatNumber,
            RuntimeGuess = Guess.RuntimeGuess
        });

        return RedirectToPage();
    }
}