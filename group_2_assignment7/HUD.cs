using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_2_assignment7;

// TODO: Sydney — implement full HUD drawing (score, heart icons, difficulty level)
public class HUD
{
    private SpriteFont font;
    private Texture2D heartTexture;

    public HUD(SpriteFont font, Texture2D heartTexture)
    {
        this.font = font;
        this.heartTexture = heartTexture;
    }

    public void Draw(SpriteBatch spriteBatch, int score, int lives, int difficulty)
    {
        // SCORE (top-left)
        spriteBatch.DrawString(font, "Score: " + score, new Vector2(10, 10), Color.White);

        // DIFFICULTY (below score)
        spriteBatch.DrawString(font, "Difficulty: " + difficulty, new Vector2(10, 40), Color.White);

        // LIVES (top-right)
        int heartSize = 30;
        int spacing = 5;

        for (int i = 0; i < lives; i++)
        {
            Vector2 position = new Vector2(
                800 - (i + 1) * (heartSize + spacing), // adjust 800 if your screen width differs
                10
            );

            spriteBatch.Draw(heartTexture, new Rectangle(
                (int)position.X,
                (int)position.Y,
                heartSize,
                heartSize
            ), Color.White);
        }
    }
}
