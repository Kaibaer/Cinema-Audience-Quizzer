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
    public int ActualRuntime { get; set; }

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
                _quizState.RuntimeRevealed = false; // Hide runtime until revealed
                _quizState.Guesses.Clear();
                await _hubContext.Clients.All.SendAsync("ReceivePoster", _quizState.PosterUrl);
            }



            // Set the actual runtime
            _quizState.ActualRuntime = ActualRuntime;

            // Reset guesses for a new game
            _quizState.Guesses.Clear();
        }
        else if (action == "reveal")
        {
            _quizState.RuntimeRevealed = true; // <-- This is crucial!

            // Only reveal, do not change poster or runtime
            foreach (var g in _quizState.Guesses)
                g.Difference = Math.Abs(g.RuntimeGuess - _quizState.ActualRuntime.GetValueOrDefault());

            await _hubContext.Clients.All.SendAsync("RevealRuntime", _quizState.ActualRuntime);
            await _hubContext.Clients.All.SendAsync("UpdateGuesses", _quizState.Guesses);
        }

        return RedirectToPage();
    }
}