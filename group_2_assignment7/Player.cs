using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_2_assignment7;

public class Player
{
    private Vector2 position;
    private float speed;
    private Texture2D spriteSheet;
    private int currentFrame;
    private float frameTimer;
    private float frameDuration;
    private bool isMoving;
    private bool facingRight;
    private Rectangle boundingBox;

    // each frame is 1/4 of the sheet width: frames 0-1 idle, frames 2-3 walk
    private int frameW;
    private int frameH;

    // movement bounds
    private int minX;
    private int maxX;
    private int minY;
    private int maxY;

    // groundHeight: height of the ground strip in pixels
    public Player(Texture2D spriteSheet, Vector2 startPosition, int screenW, int screenH, int groundHeight)
    {
        this.spriteSheet = spriteSheet;
        position = startPosition;

        speed = 200f;
        frameDuration = 0.15f;
        frameTimer = 0f;
        currentFrame = 0;
        isMoving = false;
        facingRight = true;

        frameW = spriteSheet.Width / 4;
        frameH = spriteSheet.Height;

        minX = 0;
        maxX = screenW - frameW;
        minY = 0;
        maxY = screenH - groundHeight - frameH;

        boundingBox = new Rectangle((int)position.X, (int)position.Y, frameW, frameH);
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        KeyboardState ks = Keyboard.GetState();

        bool wasMoving = isMoving;
        isMoving = false;

        if (ks.IsKeyDown(Keys.A) || ks.IsKeyDown(Keys.Left))
        {
            position.X -= speed * dt;
            isMoving = true;
            facingRight = false;
        }
        if (ks.IsKeyDown(Keys.D) || ks.IsKeyDown(Keys.Right))
        {
            position.X += speed * dt;
            isMoving = true;
            facingRight = true;
        }
        // clamp to screen edges horizontally
        position.X = MathHelper.Clamp(position.X, minX, maxX);

        // reset to first frame of new animation when movement state changes
        if (isMoving != wasMoving)
        {
            currentFrame = isMoving ? 2 : 0;
            frameTimer = 0f;
        }

        // advance animation timer and cycle frames
        frameTimer += dt;
        if (frameTimer >= frameDuration)
        {
            frameTimer = 0f;
            if (isMoving)
                currentFrame = (currentFrame == 2) ? 3 : 2;   // walk: 2 <-> 3
            else
                currentFrame = (currentFrame == 0) ? 1 : 0;   // idle: 0 <-> 1
        }

        boundingBox = new Rectangle((int)position.X, (int)position.Y, frameW, frameH);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Rectangle sourceRect = new Rectangle(currentFrame * frameW, 0, frameW, frameH);
        SpriteEffects effect = facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        spriteBatch.Draw(spriteSheet, position, sourceRect, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
    }

    public Rectangle BoundingBox()
    {
        return boundingBox;
    }
}
