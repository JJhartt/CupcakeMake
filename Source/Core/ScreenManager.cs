// ScreenManager.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class ScreenManager
{
    private IScreen _currentScreen = null!;

    public void SetScreen(IScreen screen)
    {
        _currentScreen = screen;
    }

    public void Update(GameTime gameTime)
    {
        _currentScreen.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _currentScreen.Draw(spriteBatch);
    }
}