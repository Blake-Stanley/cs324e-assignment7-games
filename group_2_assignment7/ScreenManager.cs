using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_2_assignment7;

public class ScreenManager
{
   private SpriteFont titleFont;
    private SpriteFont bodyFont;

    private int screenW;
    private int screenH;

    public ScreenManager(SpriteFont titleFont, SpriteFont bodyFont, int screenW, int screenH)
    {
        this.titleFont = titleFont;
        this.bodyFont = bodyFont;
        this.screenW = screenW;
        this.screenH = screenH;
    }

    public void DrawTitle(SpriteBatch spriteBatch)
    {
        string title = "SKY DODGE";
        string prompt = "Press Enter to Start";

        Vector2 titleSize = titleFont.MeasureString(title);
        Vector2 promptSize = bodyFont.MeasureString(prompt);

        spriteBatch.DrawString(
            titleFont,
            title,
            new Vector2((screenW - titleSize.X) / 2, screenH / 3),
            Color.White
        );

        spriteBatch.DrawString(
            bodyFont,
            prompt,
            new Vector2((screenW - promptSize.X) / 2, screenH / 2),
            Color.White
        );
    }

    public void DrawPause(SpriteBatch spriteBatch)
    {
        string text = "PAUSED";
        string prompt = "Press P to Resume";

        Vector2 textSize = titleFont.MeasureString(text);
        Vector2 promptSize = bodyFont.MeasureString(prompt);

        spriteBatch.DrawString(
            titleFont,
            text,
            new Vector2((screenW - textSize.X) / 2, screenH / 3),
            Color.Yellow
        );

        spriteBatch.DrawString(
            bodyFont,
            prompt,
            new Vector2((screenW - promptSize.X) / 2, screenH / 2),
            Color.White
        );
    }

    public void DrawWin(SpriteBatch spriteBatch, int score)
    {
        string winText = "YOU WIN!";
        string scoreText = "Final Score: " + score;
        string restartText = "Press R to Restart";

        Vector2 winSize = titleFont.MeasureString(winText);
        Vector2 scoreSize = bodyFont.MeasureString(scoreText);
        Vector2 restartSize = bodyFont.MeasureString(restartText);

        spriteBatch.DrawString(titleFont, winText,
            new Vector2((screenW - winSize.X) / 2, screenH / 3),
            Color.Green);

        spriteBatch.DrawString(bodyFont, scoreText,
            new Vector2((screenW - scoreSize.X) / 2, screenH / 2),
            Color.White);

        spriteBatch.DrawString(bodyFont, restartText,
            new Vector2((screenW - restartSize.X) / 2, screenH / 2 + 40),
            Color.White);
    }

    public void DrawLose(SpriteBatch spriteBatch, int score)
    {
        string loseText = "GAME OVER";
        string scoreText = "Final Score: " + score;
        string restartText = "Press R to Restart";

        Vector2 loseSize = titleFont.MeasureString(loseText);
        Vector2 scoreSize = bodyFont.MeasureString(scoreText);
        Vector2 restartSize = bodyFont.MeasureString(restartText);

        spriteBatch.DrawString(titleFont, loseText,
            new Vector2((screenW - loseSize.X) / 2, screenH / 3),
            Color.Red);

        spriteBatch.DrawString(bodyFont, scoreText,
            new Vector2((screenW - scoreSize.X) / 2, screenH / 2),
            Color.White);

        spriteBatch.DrawString(bodyFont, restartText,
            new Vector2((screenW - restartSize.X) / 2, screenH / 2 + 40),
            Color.White);
    }
}
