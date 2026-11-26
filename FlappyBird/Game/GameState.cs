namespace FlappyBird.Game;

/// <summary>Oyunun içinde bulunabileceği durumlar.</summary>
public enum GameState
{
    /// <summary>Oyun başlamayı bekliyor (ilk tuşa basılınca başlar).</summary>
    Ready,
    /// <summary>Oyun oynanıyor.</summary>
    Running,
    /// <summary>Kuş çarptı, oyun bitti.</summary>
    GameOver
}
