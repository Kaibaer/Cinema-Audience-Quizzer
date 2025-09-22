using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.IO;
using System.Threading.Tasks;

public class AdminModel : PageModel
{
    private readonly QuizState _quizState;
    private readonly IWebHostEnvironment _env;
    private readonly IHubContext<QuizHub> _hubContext;

    [BindProperty]
    public IFormFile PosterFile { get; set; }

    [BindProperty]
    public int? ActualRuntime { get; set; }

    public QuizState QuizState => _quizState;

    public AdminModel(QuizState quizState, IWebHostEnvironment env, IHubContext<QuizHub> hubContext)
    {
        _quizState = quizState;
        _env = env;
        _hubContext = hubContext;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync(string action)
    {
        if (action == "setup")
        {
            // Handle poster upload if a file is provided
            if (PosterFile != null)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);

                var fileName = Path.GetRandomFileName() + Path.GetExtension(PosterFile.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await PosterFile.CopyToAsync(stream);
                }

                _quizState.PosterUrl = "/uploads/" + fileName;
                await _hubContext.Clients.All.SendAsync("ReceivePoster", _quizState.PosterUrl);
            }

            // Always reset game state for a new game, regardless of poster upload
            _quizState.RuntimeRevealed = false;
            _quizState.GameSessionId = Guid.NewGuid();
            _quizState.ActualRuntime = null;
            _quizState.PendingRuntime = ActualRuntime; // Store the value entered in the setup form
            _quizState.Guesses.Clear();
        }
        else if (action == "reveal")
        {
            if (_quizState.PendingRuntime == null)
            {
                ModelState.AddModelError("", "Bitte geben Sie die tatsächliche Laufzeit ein.");
                return Page();
            }

            _quizState.ActualRuntime = _quizState.PendingRuntime.Value;
            _quizState.RuntimeRevealed = true;

            foreach (var g in _quizState.Guesses)
                g.Difference = Math.Abs(g.RuntimeGuess - _quizState.ActualRuntime.Value);

            await _hubContext.Clients.All.SendAsync("RevealRuntime", _quizState.ActualRuntime);
            await _hubContext.Clients.All.SendAsync("UpdateGuesses", _quizState.Guesses);
            _quizState.PendingRuntime = null;
        }

        return RedirectToPage();
    }
}