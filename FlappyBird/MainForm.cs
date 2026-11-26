namespace FlappyBird;

/// <summary>
/// Oyunun ana penceresi. Şimdilik yalnızca boş oyun alanını çizer.
/// </summary>
public class MainForm : Form
{
    private const int GameWidth = 480;
    private const int GameHeight = 640;

    public MainForm()
    {
        Text = "YMH219 - Flappy Bird";
        ClientSize = new Size(GameWidth, GameHeight);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        DoubleBuffered = true;
        BackColor = Color.SkyBlue;
    }
}
