using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Button
{
    private Rectangle _bounds;
    private Texture2D _pixel = null!;
    private SpriteFont _font = null!;
    private string _label;
    public Color Color { get; set; } = Color.Gray;
    public bool IsSelected { get; set; } = false;
    public string Label => _label; 

    public Button(GraphicsDevice graphicsDevice, SpriteFont font, Rectangle bounds, string label)
    {
        _bounds = bounds;
        _label = label;
        _font = font;

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

        // draw label centered on button
        Vector2 textSize = _font.MeasureString(_label);
        Vector2 textPos  = new Vector2(
            _bounds.X + (_bounds.Width  - textSize.X) / 2,
            _bounds.Y + (_bounds.Height - textSize.Y) / 2
        );
        spriteBatch.DrawString(_font, _label, textPos, Color.Black);
    }
}