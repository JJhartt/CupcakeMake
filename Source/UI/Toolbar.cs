using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class Toolbar
{
    private List<Button> _shapeButtons = new();
    private List<Button> _colorButtons = new();
    private Button _sizeUp = null!;
    private Button _sizeDown = null!;
    private Button _clearButton = null!;
    private Button _saveButton = null!;
    private Brush _brush;
    private Texture2D _background = null!;
    private GraphicsDevice _graphicsDevice = null!;
    private Rectangle _backgroundRect;
    public bool ClearRequested { get; private set; } = false;
    public bool SaveRequested { get; private set; } = false;


    public Toolbar(GraphicsDevice graphicsDevice,SpriteFont font, Brush brush, Rectangle _toolbarArea)
    {
        _graphicsDevice = graphicsDevice;
        _brush = brush;
        _backgroundRect = _toolbarArea;
        _background = new Texture2D(_graphicsDevice,1,1);
        _background.SetData(new[] {new Color(40,40,40,220)});

        int toolbarY = _toolbarArea.Y + 10;      // vertical position of toolbar
        int buttonH  = 30;       // button height
        int buttonW  = 80;       // wide button width
        int colorW   = 30;       // color swatch width
        int padding  = 5;        // gap between buttons
        

        // shape buttons — start at x=10
        _shapeButtons.Add(new Button(_graphicsDevice, font, GetButtonRect(0, 10, toolbarY, buttonW, buttonH, padding), "Circle"));
        _shapeButtons.Add(new Button(_graphicsDevice, font, GetButtonRect(1, 10, toolbarY, buttonW, buttonH, padding), "Square"));

        // color buttons — start at x=200
        _colorButtons.Add(new Button(_graphicsDevice, font, GetButtonRect(0, 200, toolbarY, colorW, buttonH, padding), "R") { Color = Color.Red });
        _colorButtons.Add(new Button(_graphicsDevice, font, GetButtonRect(1, 200, toolbarY, colorW, buttonH, padding), "G") { Color = Color.Green });
        _colorButtons.Add(new Button(_graphicsDevice, font, GetButtonRect(2, 200, toolbarY, colorW, buttonH, padding), "B") { Color = Color.Blue });
        _colorButtons.Add(new Button(_graphicsDevice, font, GetButtonRect(3, 200, toolbarY, colorW, buttonH, padding), "Y") { Color = Color.Yellow });
        _colorButtons.Add(new Button(_graphicsDevice, font, GetButtonRect(4, 200, toolbarY, colorW, buttonH, padding), "W") { Color = Color.White });
        _colorButtons.Add(new Button(_graphicsDevice, font, GetButtonRect(5, 200, toolbarY, colorW, buttonH, padding), "BK") { Color = Color.Black });

        // size buttons — start at x=400
        _sizeUp   = new Button(_graphicsDevice, font, GetButtonRect(6, 200, toolbarY, colorW, buttonH, padding), "+");
        _sizeDown = new Button(_graphicsDevice, font, GetButtonRect(7, 200, toolbarY, colorW, buttonH, padding), "-");

        _clearButton = new Button(_graphicsDevice, font, GetButtonRect(8, 200, toolbarY, colorW, buttonH, padding), "Clear");
        _saveButton =  new Button(_graphicsDevice, font, GetButtonRect(9, 200, toolbarY, colorW, buttonH, padding), "Save");
    }
    private Rectangle GetButtonRect(int index, int startX, int y, int width, int height, int padding)
    {
        return new Rectangle(startX + index * (width + padding), y, width, height);
    }

    public void Update(int mouseX, int mouseY, bool justClicked)
    {
        // shape buttons
        foreach (var btn in _shapeButtons)
        {
            if (btn.IsClicked(mouseX, mouseY, justClicked))
            {
                _brush.Shape = btn.Label == "Circle" 
                    ? Brush.BrushShape.Circle 
                    : Brush.BrushShape.Square;
                _brush.GenerateTexture();
            }
        }

        // color buttons
        foreach (var btn in _colorButtons)
        {
            if (btn.IsClicked(mouseX, mouseY, justClicked))
                _brush.Color = btn.Color;
        }

        // size buttons
        if (_sizeUp.IsClicked(mouseX, mouseY, justClicked))
        {
            _brush.Size += 5;
            _brush.GenerateTexture();
        }
        if (_sizeDown.IsClicked(mouseX, mouseY, justClicked))
        {
            _brush.Size = Math.Max(5, _brush.Size - 5); // minimum size 5
            _brush.GenerateTexture();
        }
        ClearRequested = false; 
        if(_clearButton.IsClicked(mouseX,mouseY, justClicked))
        {
            ClearRequested = true;
        }
        SaveRequested = false;
        if(_saveButton.IsClicked(mouseX,mouseY, justClicked))
        {
            SaveRequested = true;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_background, _backgroundRect, Color.White);

        foreach (var btn in _shapeButtons)
            btn.Draw(spriteBatch);
        foreach (var btn in _colorButtons)
            btn.Draw(spriteBatch);

        _sizeUp.Draw(spriteBatch);
        _sizeDown.Draw(spriteBatch);
        _clearButton.Draw(spriteBatch);
        _saveButton.Draw(spriteBatch);
    }
}