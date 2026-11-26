namespace FlappyBird.Models;

/// <summary>
/// Üst ve alt borudan oluşan bir engel çifti. Sağdan sola hareket eder,
/// ekrandan çıkınca sağ tarafa yeniden konumlandırılır.
/// </summary>
public class Pipe
{
    public const int Width = 70;
    public const int GapSize = 170;     // Kuşun geçeceği boşluk
    private const int MinGapTop = 60;   // Boşluğun ekranın üstüne en fazla yaklaşabileceği mesafe

    private static readonly Random Random = new();

    private readonly int _areaHeight;

    public float X { get; private set; }
    public int GapTop { get; private set; }

    /// <summary>Kuş bu borunun yanından geçtiyse ve skor alındıysa true olur.</summary>
    public bool Passed { get; set; }

    public RectangleF TopRect => new(X, 0, Width, GapTop);
    public RectangleF BottomRect => new(X, GapTop + GapSize, Width, _areaHeight - GapTop - GapSize);

    public Pipe(float startX, int areaHeight)
    {
        _areaHeight = areaHeight;
        Reposition(startX);
    }

    /// <summary>Boruyu verilen hız kadar sola kaydırır.</summary>
    public void Move(float speed) => X -= speed;

    public bool IsOffScreen => X + Width < 0;

    /// <summary>Boruyu yeni X konumuna taşır ve boşluğu rastgele bir yüksekliğe ayarlar.</summary>
    public void Reposition(float newX)
    {
        X = newX;
        GapTop = Random.Next(MinGapTop, _areaHeight - GapSize - MinGapTop);
        Passed = false;
    }

    public bool CollidesWith(RectangleF rect) =>
        rect.IntersectsWith(TopRect) || rect.IntersectsWith(BottomRect);

    public void Draw(Graphics g)
    {
        using var fill = new SolidBrush(Color.ForestGreen);
        using var border = new Pen(Color.DarkGreen, 3);

        g.FillRectangle(fill, TopRect);
        g.DrawRectangle(border, TopRect.X, TopRect.Y, TopRect.Width, TopRect.Height);
        g.FillRectangle(fill, BottomRect);
        g.DrawRectangle(border, BottomRect.X, BottomRect.Y, BottomRect.Width, BottomRect.Height);

        // Boru ağızları
        g.FillRectangle(fill, X - 5, GapTop - 20, Width + 10, 20);
        g.DrawRectangle(border, X - 5, GapTop - 20, Width + 10, 20);
        g.FillRectangle(fill, X - 5, GapTop + GapSize, Width + 10, 20);
        g.DrawRectangle(border, X - 5, GapTop + GapSize, Width + 10, 20);
    }
}
