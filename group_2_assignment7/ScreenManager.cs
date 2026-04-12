using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_2_assignment7;

// TODO: Sydney — implement title, pause, win, and lose screen overlays
public class ScreenManager
{
    private SpriteFont titleFont;
    private SpriteFont bodyFont;

    public ScreenManager(SpriteFont titleFont, SpriteFont bodyFont)
    {
        this.titleFont = titleFont;
        this.bodyFont = bodyFont;
    }

    public void DrawTitle(SpriteBatch spriteBatch)
    {
        // stub — draw title screen and "Press Enter to Start"
    }

    public void DrawPause(SpriteBatch spriteBatch)
    {
        // stub — draw semi-transparent overlay with "Paused" and resume instructions
    }

    public void DrawWin(SpriteBatch spriteBatch, int score)
    {
        // stub — draw victory message with final score and restart prompt
    }

    public void DrawLose(SpriteBatch spriteBatch, int score)
    {
        // stub — draw defeat message with final score and restart prompt
    }
}
