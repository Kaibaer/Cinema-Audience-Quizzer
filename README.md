# CinemaAudienceQuizzer

CinemaAudienceQuizzer is a Blazor-based web application that allows cinema audiences to participate in a fun movie runtime guessing game. Each participant submits their guess for the runtime of a featured movie, and after the actual runtime is revealed, the leaderboard displays all guesses, highlighting the closest guess(es). If multiple participants are equally close, all are recognized as winners.

## Features

- **Movie Poster Display:** Shows the current movie's poster to all participants.
- **Guess Submission:** Audience members enter their seat number and guess the movie's runtime in minutes.
- **Real-Time Updates:** Uses SignalR to update the UI in real time as guesses are submitted and the runtime is revealed.
- **Leaderboard:** Displays all guesses, their differences from the actual runtime, and highlights all closest guesses as winners.
- **Dark Theme:** The application uses a modern dark look for comfortable viewing in cinema environments.
- **No Navigation Bar:** The navigation bar is hidden for a distraction-free experience.

## How It Works

1. The quiz master sets up the movie and reveals the poster.
2. Audience members submit their seat number and runtime guess.
3. Once all guesses are in, the actual runtime is revealed.
4. The leaderboard displays all guesses, and the closest guess(es) are highlighted as winners.

## Open TODOs

- [x] Add authentication or unique user identification to prevent duplicate guesses from the same seat.
- [ ] Implement admin controls for starting/stopping the quiz and revealing the runtime.
- [ ] Add support for multiple rounds or movies in a single session.
- [ ] Enhance error handling and user feedback for invalid or duplicate submissions.
- [ ] Improve mobile responsiveness and accessibility.
- [ ] Add animations or visual effects for winner announcements.
- [ ] Add option to push emojis on the screen
- [ ] Refactor to use Blazor components instead of Razor Pages for a more modern architecture.

---

Feel free to contribute or suggest improvements!

