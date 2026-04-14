using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Button
{
    private Rectangle _bounds;
    private Texture2D _pixel = null!;
    private string _label;
    public Color Color { get; set; } = Color.Gray;
    public bool IsSelected { get; set; } = false;
    public string Label => _label; 

    public Button(GraphicsDevice graphicsDevice, Rectangle bounds, string label)
    {
        _bounds = bounds;
        _label = label;

        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
    }

    public bool IsClicked(int mouseX, int mouseY, bool justClicked)
    {
        return justClicked && _bounds.Contains(mouseX, mouseY);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // draw button background
        Color drawColor = IsSelected ? Color.White : Color;
        spriteBatch.Draw(_pixel, _bounds, drawColor);
    }
}