using QRCoder;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Net.Sockets;
using System.Net;

public class QuizState
{
    public string PosterUrl { get; set; } = "/images/default-poster.png";
    public int? ActualRuntime { get; set; }
    public int? PendingRuntime { get; set; }
    public bool RuntimeRevealed { get; set; } = false;
    public List<Guess> Guesses { get; set; } = new();
    public Guid GameSessionId { get; set; } = Guid.NewGuid(); // Unique per poster/game


   // [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    private Bitmap GenerateQrCode(string appUrl, int size)
    {
        using var qrGenerator = new QRCodeGenerator();
        using QRCodeData qrCodeData = qrGenerator.CreateQrCode(appUrl, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(size);

        using var ms = new MemoryStream(qrCodeAsPngByteArr);
        return new Bitmap(ms);
    }

    public void SaveQrCodeImage(string appUrl, string filePath, int size = 250)
    {
        using var bitmap = GenerateQrCode(appUrl, size);
        bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
    }

    public void SetPoster(string posterUrl)
    {
        PosterUrl = posterUrl;
    }

    public string GetLocalIPv4()
    {
        foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "127.0.0.1";
    }

}