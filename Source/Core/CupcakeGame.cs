using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class CupcakeGame : Game
{
    GraphicsDeviceManager _graphics;
    SpriteBatch _spriteBatch = null!;
    ScreenManager _screenManager = null!;

    public CupcakeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 600;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        SpriteFont _font = Content.Load<SpriteFont>("UI");
        _screenManager = new ScreenManager();
        _screenManager.SetScreen(new StartScreen(GraphicsDevice, _spriteBatch, _screenManager, _font));
    }

    protected override void Update(GameTime gameTime)
    {
        _screenManager.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _screenManager.Draw(_spriteBatch);
        base.Draw(gameTime);
    }
}