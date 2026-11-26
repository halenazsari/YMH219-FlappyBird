namespace FlappyBird.Services;

/// <summary>
/// En yüksek skoru uygulama klasöründeki bir metin dosyasında saklar.
/// Dosya okunamaz veya yazılamazsa oyun çalışmaya devam eder.
/// </summary>
public class HighScoreStore
{
    private readonly string _filePath;

    public HighScoreStore(string? filePath = null)
    {
        _filePath = filePath ?? Path.Combine(AppContext.BaseDirectory, "highscore.txt");
    }

    public int Load()
    {
        try
        {
            if (!File.Exists(_filePath))
                return 0;

            string text = File.ReadAllText(_filePath).Trim();
            return int.TryParse(text, out int value) && value >= 0 ? value : 0;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return 0;
        }
    }

    /// <summary>Skoru kaydeder. Başarılıysa true döner.</summary>
    public bool Save(int score)
    {
        try
        {
            File.WriteAllText(_filePath, score.ToString());
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return false;
        }
    }
}
