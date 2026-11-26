namespace FlappyBird.Models;

/// <summary>
/// Oyuncunun kontrol ettiği kuş. Konumunu ve dikey hızını kendisi yönetir.
/// </summary>
public class Bird
{
    public const int Size = 34;

    private const float Gravity = 0.55f;      // Her karede hıza eklenen yer çekimi
    private const float FlapStrength = -8.5f; // Zıplamada verilen yukarı hız
    private const float MaxFallSpeed = 12f;   // Aşağı düşüş hız sınırı

    private readonly float _startX;
    private readonly float _startY;

    public float X { get; private set; }
    public float Y { get; private set; }
    public float VelocityY { get; private set; }

    /// <summary>Çarpışma kontrolünde kullanılan dikdörtgen (biraz içeriden).</summary>
    public RectangleF Bounds => new(X + 3, Y + 3, Size - 6, Size - 6);

    public Bird(float startX, float startY)
    {
        _startX = startX;
        _startY = startY;
        Reset();
    }

    /// <summary>Kuşa yukarı doğru ani hız verir.</summary>
    public void Flap() => VelocityY = FlapStrength;

    /// <summary>Bir kare ilerletir: yer çekimini uygular ve konumu günceller.</summary>
    public void Update()
    {
        VelocityY = Math.Min(VelocityY + Gravity, MaxFallSpeed);
        Y += VelocityY;
        if (Y < 0)
        {
            Y = 0;
            VelocityY = 0;
        }
    }

    public void Reset()
    {
        X = _startX;
        Y = _startY;
        VelocityY = 0;
    }

    public void Draw(Graphics g)
    {
        // Gövde
        using var body = new SolidBrush(Color.Gold);
        g.FillEllipse(body, X, Y, Size, Size);
        g.DrawEllipse(Pens.DarkOrange, X, Y, Size, Size);

        // Göz
        g.FillEllipse(Brushes.White, X + Size * 0.55f, Y + Size * 0.18f, Size * 0.3f, Size * 0.3f);
        g.FillEllipse(Brushes.Black, X + Size * 0.68f, Y + Size * 0.26f, Size * 0.12f, Size * 0.12f);

        // Gaga
        PointF[] beak =
        {
            new(X + Size * 0.85f, Y + Size * 0.5f),
            new(X + Size * 1.2f, Y + Size * 0.6f),
            new(X + Size * 0.85f, Y + Size * 0.72f)
        };
        g.FillPolygon(Brushes.OrangeRed, beak);
    }
}
