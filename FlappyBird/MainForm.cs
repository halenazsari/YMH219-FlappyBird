using FlappyBird.Models;

namespace FlappyBird;

/// <summary>
/// Oyunun ana penceresi. Timer ile oyun döngüsünü çalıştırır, klavye olaylarını yakalar.
/// </summary>
public class MainForm : Form
{
    private const int GameWidth = 480;
    private const int GameHeight = 640;

    private readonly System.Windows.Forms.Timer _gameTimer = new() { Interval = 16 }; // ~60 FPS
    private readonly Bird _bird = new(100, GameHeight / 2f);
    private readonly List<Pipe> _pipes = new();

    private const int PipeCount = 2;
    private const int PipeSpacing = 260;
    private const float PipeSpeed = 3f;

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

        _gameTimer.Tick += GameTimer_Tick;
        KeyDown += MainForm_KeyDown;

        for (int i = 0; i < PipeCount; i++)
            _pipes.Add(new Pipe(GameWidth + i * PipeSpacing, GameHeight));

        _gameTimer.Start();
    }

    private void GameTimer_Tick(object? sender, EventArgs e)
    {
        _bird.Update();

        foreach (var pipe in _pipes)
        {
            pipe.Move(PipeSpeed);
            if (pipe.IsOffScreen)
            {
                // En sağdaki borunun arkasına yerleştir
                float rightMost = _pipes.Max(p => p.X);
                pipe.Reposition(rightMost + PipeSpacing);
            }
        }

        Invalidate(); // Formun yeniden çizilmesini ister (OnPaint tetiklenir)
    }

    private void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode is Keys.Space or Keys.Up)
        {
            _bird.Flap();
            e.SuppressKeyPress = true;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        foreach (var pipe in _pipes)
            pipe.Draw(e.Graphics);
        _bird.Draw(e.Graphics);
    }
}
