// GalleryScreen.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

public class GalleryScreen : IScreen
{
    private GraphicsDevice _graphicsDevice;
    private SpriteBatch _spriteBatch;
    private ScreenManager _screenManager;
    private Texture2D _savedImage = null!;
    private Texture2D _pixel = null!;
    private MouseState _mouse;
    private MouseState _prevMouse;
    private string _wrapper;
    private Texture2D _wrapperImage = null!;
    private Texture2D think = null!;
    private Texture2D yes = null!;
    private Texture2D no = null!;
    private CupcakeRating _rating = null!;
    private float _timer = 0f;
    private bool _showReaction = false;
    private Button _restartButton = null!;
    private SpriteFont _font = null!;

    public GalleryScreen(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ScreenManager screenManager, string imagePath, string wrapper, CupcakeRating rating, SpriteFont font)
    {
        _font = font;
        _rating = rating;
        _wrapper = wrapper;
        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;
        _screenManager = screenManager;

        // load the saved image
        using (var stream = File.OpenRead(imagePath))
        _savedImage = Texture2D.FromStream(_graphicsDevice, stream);

        using (var stream = File.OpenRead(wrapper))
        _wrapperImage = Texture2D.FromStream(_graphicsDevice, stream);

        think = Texture2D.FromFile(_graphicsDevice,"Content/Think.png");
        yes = Texture2D.FromFile(_graphicsDevice,"Content/Yes.png");
        no = Texture2D.FromFile(_graphicsDevice,"Content/No.png");

        _restartButton = new Button(_graphicsDevice, _font, new Rectangle(300, 540, 200, 40), "Restart");

        _pixel = new Texture2D(_graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
    }

    public void Update(GameTime gameTime)
    {
        _prevMouse = _mouse;
        _mouse = Mouse.GetState();

        bool justClicked = _mouse.LeftButton == ButtonState.Pressed &&
                           _prevMouse.LeftButton == ButtonState.Released;

        if (!_showReaction)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= 3f)
                _showReaction = true;
        }
        if (_restartButton.IsClicked(_mouse.X, _mouse.Y, justClicked))
        {
            _screenManager.SetScreen(new StartScreen(_graphicsDevice, _spriteBatch, _screenManager, _font));
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _graphicsDevice.Clear(new Color(245, 220, 220));

        Rectangle cupcakeRect = new Rectangle(50, 100, 300, 350);
        Rectangle thinkRect   = new Rectangle(420, 100, 300, 350);

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

        // left side — wrapper + painting composited
        _spriteBatch.Draw(_wrapperImage, cupcakeRect, Color.White);
        _spriteBatch.Draw(_savedImage,   cupcakeRect, Color.White);
        DrawBorder(cupcakeRect, 4, 6, new Color(180, 100, 120));
        DrawBorder(thinkRect,   4, 6, new Color(180, 100, 120));

        if (_showReaction)
        {
            // think is NOT drawn here — it's gone
            if (_rating.Stars >= 3)
                _spriteBatch.Draw(yes, thinkRect, Color.White);
            else
                _spriteBatch.Draw(no, thinkRect, Color.White);
        }
        else
        {
            // yes/no are NOT drawn here
            _spriteBatch.Draw(think, thinkRect, Color.White);
        }
        // new cupcake button
        _spriteBatch.Draw(_pixel, new Rectangle(300, 540, 200, 40), new Color(200, 100, 120));

        _spriteBatch.End();
    }

    private void DrawBorder(Rectangle rect, int thickness, int buffer, Color color)
    {
        Rectangle bordered = new Rectangle(
            rect.X - buffer,
            rect.Y - buffer,
            rect.Width + buffer * 2,
            rect.Height + buffer * 2
        );

        // top
        _spriteBatch.Draw(_pixel, new Rectangle(bordered.X, bordered.Y, bordered.Width, thickness), color);
        // bottom
        _spriteBatch.Draw(_pixel, new Rectangle(bordered.X, bordered.Bottom - thickness, bordered.Width, thickness), color);
        // left
        _spriteBatch.Draw(_pixel, new Rectangle(bordered.X, bordered.Y, thickness, bordered.Height), color);
        // right
        _spriteBatch.Draw(_pixel, new Rectangle(bordered.Right - thickness, bordered.Y, thickness, bordered.Height), color);
    }
}