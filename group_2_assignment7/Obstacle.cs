using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_2_assignment7;

public class Obstacle
{
    private Vector2 position;
    private float fallSpeed;
    private Texture2D texture;
    private Rectangle boundingBox;
    private bool isActive;
    private int screenH;

    public const float Scale = 1f / 2f;

    public Obstacle(Texture2D texture, float xPosition, float fallSpeed, int screenH)
    {
        this.texture = texture;
        this.fallSpeed = fallSpeed;
        this.screenH = screenH;

        position = new Vector2(xPosition, 0);
        boundingBox = new Rectangle((int)xPosition, 0,
            (int)(texture.Width * Scale), (int)(texture.Height * Scale));
        isActive = true;
    }

    public void Update(GameTime gameTime)
    {
        if (!isActive) return;

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        position.Y += fallSpeed * dt;

        if (position.Y >= screenH)
            isActive = false;

        boundingBox = new Rectangle((int)position.X, (int)position.Y,
            (int)(texture.Width * Scale), (int)(texture.Height * Scale));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (isActive)
            spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
    }

    public Rectangle BoundingBox() => boundingBox;
    public bool Active() => isActive;
    public void Deactivate() => isActive = false;
}
