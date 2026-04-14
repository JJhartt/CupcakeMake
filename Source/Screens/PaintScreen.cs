// PaintScreen.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

public class PaintScreen : IScreen
{
    private GraphicsDevice _graphicsDevice;
    private SpriteBatch _spriteBatch;
    private ScreenManager _screenManager;

    private RenderTarget2D _canvas = null!;
    private Texture2D _background = null!;
    private Brush _brush = null!;
    private Toolbar _toolbar = null!;
    private MouseState _mouse;
    private MouseState _prevMouse;
    private bool _isPainting;
    private Rectangle _canvasArea;
    private Rectangle _toolbarArea;
    private String _wrapper = null!;
    private SpriteFont _font = null!;

    public PaintScreen(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ScreenManager screenManager, String image, SpriteFont font)
    {
        _font = font;
        _wrapper = image;
        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;
        _screenManager = screenManager;

        int toolbarHeight = 50;
        _toolbarArea = new Rectangle(0, 600 - toolbarHeight, 800, toolbarHeight);
        _canvasArea  = new Rectangle(0, 0, 800, 600 - toolbarHeight);

        _canvas = new RenderTarget2D(
            _graphicsDevice,
            800, 550, false,
            SurfaceFormat.Color, DepthFormat.None, 0,
            RenderTargetUsage.PreserveContents
        );

        // clear canvas to transparent
        _graphicsDevice.SetRenderTarget(_canvas);
        _graphicsDevice.Clear(Color.Transparent);
        _graphicsDevice.SetRenderTarget(null);

        _brush = new Brush(_graphicsDevice);
        _toolbar = new Toolbar(_graphicsDevice,_font, _brush, _toolbarArea);

        using var stream = File.OpenRead(image);
        _background = Texture2D.FromStream(_graphicsDevice, stream);
    }

    public void Update(GameTime gameTime)
    {
        _prevMouse = _mouse;
        _mouse = Mouse.GetState();

        bool justClicked = _mouse.LeftButton == ButtonState.Pressed &&
                           _prevMouse.LeftButton == ButtonState.Released;

        _isPainting = _mouse.LeftButton == ButtonState.Pressed
                      && _canvasArea.Contains(_mouse.X, _mouse.Y);

        _toolbar.Update(_mouse.X, _mouse.Y, justClicked);

        if (_toolbar.ClearRequested)
            ClearCanvas();

        if (_toolbar.SaveRequested)
        {
            string savedFile = SaveCanvas();
            CupcakeRating rating = Rating.Analyze(_canvas, savedFile);
            _screenManager.SetScreen(new GalleryScreen(_graphicsDevice, _spriteBatch, _screenManager, savedFile, _wrapper, rating, _font));
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_isPainting)
        {
            _graphicsDevice.SetRenderTarget(_canvas);
            _spriteBatch.Begin();
            _brush.Paint(_spriteBatch, new Vector2(_mouse.X, _mouse.Y));
            _spriteBatch.End();
        }

        _graphicsDevice.SetRenderTarget(null);
        _graphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();
        _spriteBatch.Draw(_background, _canvasArea, Color.White);
        _spriteBatch.End();

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        _spriteBatch.Draw(_canvas, Vector2.Zero, Color.White);
        _spriteBatch.End();

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        _toolbar.Draw(_spriteBatch);
        _spriteBatch.End();
    }

    private void ClearCanvas()
    {
        _graphicsDevice.SetRenderTarget(_canvas);
        _graphicsDevice.Clear(Color.Transparent);
        _graphicsDevice.SetRenderTarget(null);
    }

    private string SaveCanvas()
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string filename = $"cupcake_{timestamp}.png";
        using var stream = File.OpenWrite(filename);
        _canvas.SaveAsPng(stream, _canvas.Width, _canvas.Height);
        return filename;
    }
}