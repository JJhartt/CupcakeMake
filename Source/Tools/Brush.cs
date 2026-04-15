using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class Brush {

    public enum BrushShape { Circle, Square }

    private Texture2D _texture = null!;
    private GraphicsDevice _graphicsDevice;
    private List<FrostingParticle> _particles = new();
    private bool[,] _settledGrid;
    private int _gridWidth;
    private int _gridHeight;

    public int Size { get; set; } = 10;
    public Color Color { get; set; } = Color.Red;
    public BrushShape Shape { get; set; } = BrushShape.Circle;
    public bool HasUnsettledParticles => _particles.Exists(p => !p.Settled);

    private int _canvasHeight = 200;

    public Brush(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
        GenerateTexture();
    }

    public void GenerateTexture()
    {
        if(Shape == BrushShape.Circle){
            _texture = CreateCircle();
        } else {
            _texture = CreateSquare();
        }
    }

    public void Update(float deltaTime, int canvasHeight)
    {
        List<FrostingParticle> toRemove = new();
        foreach(var p in _particles)
        {
            if(p.Settled)
            {
                toRemove.Add(p);
                continue;
            }

            p.Velocity.Y += 80f * deltaTime;
            p.Position += p.Velocity * deltaTime * 60f;
            p.Velocity *= 0.95f;
            int px = (int)p.Position.X;
            int py = (int)p.Position.Y;

            if(py + p.Size >= canvasHeight)
            {
                p.Position.Y = canvasHeight - p.Size;
                SettleParticle(p);
                continue;
            }

            if(IsGridFilled(px,py+p.Size))
            {
                SettleParticle(p);
                continue;
            }
            if (py > canvasHeight + 50 || px < -50 || px > _gridWidth + 50)
            {
                toRemove.Add(p);
                continue;
            }
            System.Diagnostics.Debug.WriteLine($"particle Y={py}, canvasH={canvasHeight}, gridH={_gridHeight}");
        }

        foreach(var p in toRemove)
            _particles.Remove(p);
    }
    private void SettleParticle(FrostingParticle p)
    {
        p.Settled = true;
        p.Velocity = Vector2.Zero;

        // mark this area as filled in the grid
        int px = (int)p.Position.X;
        int py = (int)p.Position.Y;

        for (int y = py; y < py + p.Size * 2 && y < _gridHeight; y++)
            for (int x = px - p.Size; x < px + p.Size && x >= 0 && x < _gridWidth; x++)
                _settledGrid[x, y] = true;
    }

    private bool IsGridFilled(int x, int y)
    {
        if (x < 0 || x >= _gridWidth || y < 0 || y >= _gridHeight)
            return false;
        return _settledGrid[x, y];
    }

    public void LoadCollisionFromTexture(Texture2D texture)
    {
        Color[] pixels = new Color[texture.Width * texture.Height];
        texture.GetData(pixels);

        float scaleX = (float)texture.Width / _gridWidth;
        float scaleY = (float)texture.Height / _gridHeight;

        for (int y = 0; y < _gridHeight; y++)
        {
            for (int x = 0; x < _gridWidth; x++)
            {
                int srcX = (int)(x * scaleX);
                int srcY = (int)(y * scaleY);

                if (srcX >= texture.Width) srcX = texture.Width - 1;
                if (srcY >= texture.Height) srcY = texture.Height - 1;

                if (pixels[srcY * texture.Width + srcX].A > 128)
                    _settledGrid[x, y] = true;
            }
        }
    }

    public void SpawnParticles(Vector2 position)
    {
        for(int i = 0; i<3; i++)
        {
            Vector2 offset = new Vector2(
                Random.Shared.NextSingle() * Size - Size / 2f,
                Random.Shared.NextSingle() * Size - Size / 2f
            );
            _particles.Add(new FrostingParticle(position + offset, Color, Size / 2));
        }
    }

    public void DrawSettledParticles(SpriteBatch spriteBatch)
    {
        foreach (var p in _particles)
        {
            if (p.Settled)
                spriteBatch.Draw(_texture,
                    new Rectangle((int)p.Position.X - p.Size, (int)p.Position.Y - p.Size, p.Size * 2, p.Size * 2),
                    p.Color);
        }
    }

    public void DrawFallingParticles(SpriteBatch spriteBatch)
    {
        foreach (var p in _particles)
        {
            if (!p.Settled)
                spriteBatch.Draw(_texture,
                    new Rectangle((int)p.Position.X - p.Size, (int)p.Position.Y - p.Size, p.Size * 2, p.Size * 2),
                    p.Color);
        }
    }

    private Texture2D CreateCircle()
    {
        int diameter = Size * 2;
        Texture2D circle = new Texture2D(_graphicsDevice, diameter, diameter);
        Color[] data = new Color[diameter * diameter];

        for (int y = 0; y < diameter; y++)
        {
            for (int x = 0; x < diameter; x++)
            {
                float dx = x - Size;
                float dy = y - Size;
                if (dx * dx + dy * dy <= Size * Size)
                    data[y * diameter + x] = Color.White;
                else
                    data[y * diameter + x] = Color.Transparent;
            }
        }
        circle.SetData(data);
        return circle;
    }

    private Texture2D CreateSquare(){
        Texture2D square = new Texture2D(_graphicsDevice,Size*2,Size*2);
        Color[] data = new Color[(Size*2) * (Size *2 )];

        for(int i = 0; i<((Size*2)*(Size*2));i++){
            data[i] = Color.White;
        } 
        square.SetData(data);
        return square;
    }

    public void InitGrid(int width, int height)
    {
        _gridWidth  = width;
        _gridHeight = height;
        _settledGrid = new bool[width, height];
    }

    /*public void Paint(SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.Draw(
            _texture,
            new Vector2(position.X - Size, position.Y - Size), // center on cursor
            Color
        );
    }*/


}