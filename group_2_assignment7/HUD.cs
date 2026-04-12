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
        // stub — replace with real HUD drawing
    }
}
