using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class CupcakeGame : Game
{
    GraphicsDeviceManager _graphics;
    SpriteBatch _spriteBatch = null!;
    RenderTarget2D _canvas = null!;
    Texture2D _pixel = null!;
    Texture2D _background = null!;
    MouseState _mouse;
    MouseState _prevMouse;
    Brush _brush = null!;
    Toolbar _toolbar = null!;
    bool _isPainting = false;
    Rectangle _canvasArea;
    Rectangle _toolbarArea;

    public CupcakeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 600;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {   
        int toolbarHeight = 50;
        _toolbarArea = new Rectangle(0, 600 - toolbarHeight, 800, toolbarHeight);
        _canvasArea  = new Rectangle(0, 0, 800, 600 - toolbarHeight);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
        _canvas = new RenderTarget2D(
            GraphicsDevice,
            800, 600,
            false,
            SurfaceFormat.Color,
            DepthFormat.None,
            0,
            RenderTargetUsage.PreserveContents
        );
        _background = Content.Load<Texture2D>("cupcake");
        _brush = new Brush(GraphicsDevice);
        _brush.Size = 10;
        _brush.Color = Color.Red;
        _brush.Shape = Brush.BrushShape.Circle; 
        _toolbar = new Toolbar(GraphicsDevice, _brush, _toolbarArea);
    }

    protected override void Update(GameTime gameTime)
    {
        _prevMouse = _mouse;
        _mouse = Mouse.GetState();
        _isPainting = _mouse.LeftButton == ButtonState.Pressed && _canvasArea.Contains(_mouse.X,_mouse.Y);
        bool justClicked = _mouse.LeftButton == ButtonState.Pressed && _prevMouse.LeftButton == ButtonState.Released;
        _toolbar.Update(_mouse.X, _mouse.Y, justClicked);
        if(_toolbar.ClearRequested){
            clearCanvas();
        }
        base.Update(gameTime);

    }

    protected override void Draw(GameTime gameTime)
    {
        if (_isPainting)
        {
            GraphicsDevice.SetRenderTarget(_canvas);
            _spriteBatch.Begin();
            _brush.Paint(_spriteBatch, new Vector2(_mouse.X, _mouse.Y));
            _spriteBatch.End();
        }
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin();
        _spriteBatch.Draw(_background, new Rectangle(0, 0, 800, 600), Color.White);
        _spriteBatch.End();

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        _spriteBatch.Draw(_canvas, Vector2.Zero, Color.White);
        _spriteBatch.End();

        _spriteBatch.Begin();
        _toolbar.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
    private void clearCanvas(){
        GraphicsDevice.SetRenderTarget(_canvas);
        GraphicsDevice.Clear(Color.Transparent);
        GraphicsDevice.SetRenderTarget(null);
    }
}