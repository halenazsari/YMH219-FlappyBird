using FlappyBird.Models;

namespace FlappyBird.Game;

/// <summary>
/// Oyunun kurallarını ve durumunu yöneten sınıf. Arayüzden bağımsızdır:
/// form yalnızca Update() çağırır, Flap() ile girdi iletir ve Draw() ile çizdirir.
/// </summary>
public class GameEngine
{
    private const int PipeCount = 2;
    private const int PipeSpacing = 260;
    private const int GroundHeight = 60;
    private const float BaseSpeed = 3f;
    private const float SpeedPerLevel = 0.5f;
    private const float MaxSpeed = 7f;
    private const int PointsPerLevel = 5;   // Her 5 puanda bir seviye atlanır

    private readonly int _width;
    private readonly int _height;
    private readonly List<Pipe> _pipes = new();

    public Bird Bird { get; }
    public IReadOnlyList<Pipe> Pipes => _pipes;

    public GameState State { get; private set; } = GameState.Ready;
    public int Score { get; private set; }
    public float Speed { get; private set; } = BaseSpeed;
    public int Level => Score / PointsPerLevel + 1;
    public int HighScore { get; private set; }

    /// <summary>Son oyunda yeni rekor kırıldıysa true.</summary>
    public bool IsNewHighScore { get; private set; }

    /// <summary>Oyun alanında zeminin başladığı Y koordinatı.</summary>
    public int GroundY => _height - GroundHeight;

    /// <summary>Skor değiştiğinde tetiklenir.</summary>
    public event EventHandler? ScoreChanged;

    /// <summary>Kuş çarpıp oyun bittiğinde tetiklenir.</summary>
    public event EventHandler? GameEnded;

    public GameEngine(int width, int height, int highScore = 0)
    {
        _width = width;
        _height = height;
        HighScore = highScore;
        Bird = new Bird(100, (height - GroundHeight) / 2f);

        for (int i = 0; i < PipeCount; i++)
            _pipes.Add(new Pipe(0, GroundY));

        Reset();
    }

    /// <summary>Oyunu başlangıç durumuna getirir (Yeni Oyun).</summary>
    public void Reset()
    {
        Bird.Reset();
        Score = 0;
        Speed = BaseSpeed;
        IsNewHighScore = false;
        for (int i = 0; i < _pipes.Count; i++)
            _pipes[i].Reposition(_width + 100 + i * PipeSpacing);

        State = GameState.Ready;
        ScoreChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Oyuncu zıplama tuşuna bastığında çağrılır.</summary>
    public void Flap()
    {
        if (State == GameState.GameOver)
            return;

        if (State == GameState.Ready)
            State = GameState.Running;

        Bird.Flap();
    }

    /// <summary>Oyunu bir kare ilerletir. Timer her tetiklendiğinde çağrılır.</summary>
    public void Update()
    {
        if (State != GameState.Running)
            return;

        Bird.Update();
        MovePipes();
        CheckScore();
        CheckCollisions();
    }

    private void MovePipes()
    {
        foreach (var pipe in _pipes)
        {
            pipe.Move(Speed);
            if (pipe.IsOffScreen)
            {
                float rightMost = _pipes.Max(p => p.X);
                pipe.Reposition(rightMost + PipeSpacing);
            }
        }
    }

    private void CheckScore()
    {
        foreach (var pipe in _pipes)
        {
            // Borunun sağ kenarı kuşun soluna geçtiyse bu engel aşılmış demektir
            if (!pipe.Passed && pipe.X + Pipe.Width < Bird.X)
            {
                pipe.Passed = true;
                Score++;
                Speed = Math.Min(BaseSpeed + (Level - 1) * SpeedPerLevel, MaxSpeed);
                ScoreChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void CheckCollisions()
    {
        var birdRect = Bird.Bounds;
        bool hitGround = birdRect.Bottom >= GroundY;
        bool hitPipe = _pipes.Any(p => p.CollidesWith(birdRect));

        if (hitGround || hitPipe)
        {
            State = GameState.GameOver;
            if (Score > HighScore)
            {
                HighScore = Score;
                IsNewHighScore = true;
            }
            GameEnded?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Draw(Graphics g)
    {
        foreach (var pipe in _pipes)
            pipe.Draw(g);

        // Zemin
        using var ground = new SolidBrush(Color.SandyBrown);
        g.FillRectangle(ground, 0, GroundY, _width, _height - GroundY);
        using var grass = new SolidBrush(Color.YellowGreen);
        g.FillRectangle(grass, 0, GroundY, _width, 12);

        Bird.Draw(g);
    }
}
