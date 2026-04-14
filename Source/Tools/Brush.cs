using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Brush {

    public enum BrushShape { Circle, Square }

    private Texture2D _texture = null!;
    private GraphicsDevice _graphicsDevice;

    public int Size { get; set; } = 10;
    public Color Color { get; set; } = Color.Red;
    public BrushShape Shape { get; set; } = BrushShape.Circle;

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

        for(int i = 0; i<Size;i++){
            data[i] = Color.White;
        } 
        square.SetData(data);
        return square;
    }

    public void Paint(SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.Draw(
            _texture,
            new Vector2(position.X - Size, position.Y - Size), // center on cursor
            Color
        );
    }
}