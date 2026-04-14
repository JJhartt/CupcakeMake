// GalleryScreen.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

public class StartScreen : IScreen
{
    private GraphicsDevice _graphicsDevice;
    private SpriteBatch _spriteBatch;
    private ScreenManager _screenManager;

    private Texture2D _wrapper1 = null!;
    private Texture2D _wrapper2 = null!;
    private Texture2D _pixel = null!;

    private Rectangle _wrapper1Rect;
    private Rectangle _wrapper2Rect;

    private MouseState _mouse;
    private MouseState _prevMouse;
    private SpriteFont _font = null!;

    public StartScreen(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ScreenManager screenManager,SpriteFont font)
    {
        _font = font;
        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;
        _screenManager = screenManager;

        // load both wrappers
        using (var stream = File.OpenRead("Content/Cupcake1.png"))
            _wrapper1 = Texture2D.FromStream(_graphicsDevice, stream);

        using (var stream = File.OpenRead("Content/Cupcake2.png"))
            _wrapper2 = Texture2D.FromStream(_graphicsDevice, stream);

        _pixel = new Texture2D(_graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        // position two wrappers side by side centered on screen
        _wrapper1Rect = new Rectangle(100, 150, 250, 300);
        _wrapper2Rect = new Rectangle(450, 150, 250, 300);
    }

    public void Update(GameTime gameTime)
    {
        _prevMouse = _mouse;
        _mouse = Mouse.GetState();

        bool justClicked = _mouse.LeftButton == ButtonState.Pressed &&
                           _prevMouse.LeftButton == ButtonState.Released;

        if (justClicked)
        {
            if (_wrapper1Rect.Contains(_mouse.X, _mouse.Y))
                _screenManager.SetScreen(new PaintScreen(_graphicsDevice, _spriteBatch, _screenManager, "Content/Cupcake1.png", _font));

            if (_wrapper2Rect.Contains(_mouse.X, _mouse.Y))
                _screenManager.SetScreen(new PaintScreen(_graphicsDevice, _spriteBatch, _screenManager, "Content/Cupcake2.png", _font));
        }
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _graphicsDevice.Clear(new Color(245, 220, 220));

        _spriteBatch.Begin();

        // title bar
        _spriteBatch.Draw(_pixel, new Rectangle(0, 50, 800, 60), new Color(200, 100, 120));

        // highlight on hover
        if (_wrapper1Rect.Contains(_mouse.X, _mouse.Y))
            _spriteBatch.Draw(_pixel, Inflate(_wrapper1Rect, 6), Color.Green);

        if (_wrapper2Rect.Contains(_mouse.X, _mouse.Y))
            _spriteBatch.Draw(_pixel, Inflate(_wrapper2Rect, 6), Color.Green);

        // draw wrappers
        _spriteBatch.Draw(_wrapper1, _wrapper1Rect, Color.White);
        _spriteBatch.Draw(_wrapper2, _wrapper2Rect, Color.White);

        _spriteBatch.End();
    }

    // helper — inflate a rectangle by n pixels on all sides
    private Rectangle Inflate(Rectangle rect, int amount)
    {
        return new Rectangle(
            rect.X - amount,
            rect.Y - amount,
            rect.Width + amount * 2,
            rect.Height + amount * 2
        );
    }
}