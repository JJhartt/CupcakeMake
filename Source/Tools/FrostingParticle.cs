using Microsoft.Xna.Framework;

public class FrostingParticle
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Color Color;
    public int Size;
    public bool Settled = false;

    public FrostingParticle(Vector2 position, Color color, int size)
    {
        Position = position;
        Color    = color;
        Size     = size;

        Velocity = new Vector2(
            Random.Shared.NextSingle() * 2f - 1f,  // -1 to 1 horizontal
            Random.Shared.NextSingle() * 2f + 1f   // 1 to 3 downward
        );
    }
}