using FlappyBird.Game;

namespace FlappyBird;

/// <summary>
/// Oyunun ana penceresi. Timer ile oyun döngüsünü çalıştırır, klavye olaylarını
/// GameEngine'e iletir ve oyun durumunu ekranda gösterir.
/// </summary>
public class MainForm : Form
{
    private const int GameWidth = 480;
    private const int GameHeight = 640;

    private readonly System.Windows.Forms.Timer _gameTimer = new() { Interval = 16 }; // ~60 FPS
    private readonly GameEngine _engine = new(GameWidth, GameHeight);

    private readonly Label _lblScore = new();
    private readonly Button _btnNewGame = new();

    public MainForm()
    {
        Text = "YMH219 - Flappy Bird";
        ClientSize = new Size(GameWidth, GameHeight);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        DoubleBuffered = true;
        BackColor = Color.SkyBlue;
        KeyPreview = true;

        InitializeControls();

        _engine.ScoreChanged += (_, _) => UpdateScoreLabel();
        _engine.GameEnded += Engine_GameEnded;

        _gameTimer.Tick += GameTimer_Tick;
        KeyDown += MainForm_KeyDown;

        UpdateScoreLabel();
        _gameTimer.Start();
    }

    private void InitializeControls()
    {
        _lblScore.AutoSize = true;
        _lblScore.Location = new Point(12, 10);
        _lblScore.Font = new Font("Segoe UI", 16, FontStyle.Bold);
        _lblScore.ForeColor = Color.White;
        _lblScore.BackColor = Color.Transparent;

        _btnNewGame.Text = "Yeni Oyun";
        _btnNewGame.Size = new Size(140, 44);
        _btnNewGame.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        _btnNewGame.Location = new Point((GameWidth - 140) / 2, GameHeight / 2 + 50);
        _btnNewGame.Visible = false;
        _btnNewGame.TabStop = false; // Space tuşu butona değil oyuna gitsin
        _btnNewGame.Click += (_, _) => StartNewGame();

        Controls.Add(_lblScore);
        Controls.Add(_btnNewGame);
    }

    private void StartNewGame()
    {
        _engine.Reset();
        _btnNewGame.Visible = false;
        Focus();
        Invalidate();
    }

    private void UpdateScoreLabel() => _lblScore.Text = $"Skor: {_engine.Score}";

    private void Engine_GameEnded(object? sender, EventArgs e)
    {
        _btnNewGame.Visible = true;
    }

    private void GameTimer_Tick(object? sender, EventArgs e)
    {
        _engine.Update();
        Invalidate(); // Formun yeniden çizilmesini ister (OnPaint tetiklenir)
    }

    private void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Space:
            case Keys.Up:
                _engine.Flap();
                e.SuppressKeyPress = true;
                break;
            case Keys.R when _engine.State == GameState.GameOver:
                StartNewGame();
                break;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        _engine.Draw(g);

        if (_engine.State == GameState.Ready)
            DrawCenteredText(g, "Başlamak için SPACE", "Zıplamak: SPACE / ↑");
        else if (_engine.State == GameState.GameOver)
            DrawCenteredText(g, "GAME OVER", $"Skor: {_engine.Score}   (R: yeniden başlat)");
    }

    private void DrawCenteredText(Graphics g, string title, string subtitle)
    {
        using var overlay = new SolidBrush(Color.FromArgb(120, Color.Black));
        g.FillRectangle(overlay, 40, GameHeight / 2 - 110, GameWidth - 80, 140);

        using var titleFont = new Font("Segoe UI", 26, FontStyle.Bold);
        using var subFont = new Font("Segoe UI", 12);
        var format = new StringFormat { Alignment = StringAlignment.Center };

        g.DrawString(title, titleFont, Brushes.White, GameWidth / 2f, GameHeight / 2f - 95, format);
        g.DrawString(subtitle, subFont, Brushes.White, GameWidth / 2f, GameHeight / 2f - 35, format);
    }
}
