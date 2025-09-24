using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class QuizHub : Hub
{
    public async Task BroadcastPoster(string imageUrl)
        => await Clients.All.SendAsync("ReceivePoster", imageUrl);

    public async Task BroadcastReveal(int actualRuntime)
        => await Clients.All.SendAsync("RevealRuntime", actualRuntime);

    public async Task BroadcastGuesses(List<Guess> guesses)
        => await Clients.All.SendAsync("UpdateGuesses", guesses);

    public async Task SwitchPage(string page)
    {
        await Clients.All.SendAsync("SwitchPage", page);
    }
}
