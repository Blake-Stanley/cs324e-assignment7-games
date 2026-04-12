using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_2_assignment7;

public class Star
{
    private Vector2 position;
    private float fallSpeed;
    private Texture2D spriteSheet;
    private int currentFrame;
    private float frameTimer;
    private float frameDuration;
    private Rectangle boundingBox;
    private bool isActive;
    private int frameW;
    private int frameH;
    private int screenH;
    

    public Star(Texture2D spriteSheet, float xPosition, float fallSpeed, int screenH)
    {
        this.spriteSheet = spriteSheet;
        this.fallSpeed = fallSpeed;
        position = new Vector2(xPosition, 0);

        currentFrame = 0;
        frameTimer = 0f;
        frameDuration = 1.0f;
        frameW = spriteSheet.Width / 2;
        frameH = spriteSheet.Height;
        boundingBox = new Rectangle((int)xPosition, 0, frameW, frameH);
        isActive = true;
        this.screenH = screenH;
    }

    public void Update(GameTime gameTime)
    {
        if (isActive)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Y += fallSpeed * dt;
            frameTimer += dt;

            if (frameTimer >= frameDuration)
            {
                frameTimer = 0f;
                if (currentFrame == 0)
                {
                    currentFrame = 1;
                }
                else
                {
                    currentFrame = 0;
                }
            }

            if (position.Y >= screenH)
            {
                isActive = false;
            }

            boundingBox = new Rectangle((int)position.X, (int)position.Y, 
                frameW, frameH);
        }
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (isActive)
        {
            Rectangle currentRect = new Rectangle((int)frameW * currentFrame,
            0, frameW, frameH);

            spriteBatch.Draw(spriteSheet, position, currentRect, Color.White);
        }
    }

    public Rectangle BoundingBox()
    {
        return boundingBox;
    }

    public void Collected()
    {
        isActive = false;
    }

    public bool Active()
    {
        return isActive;
    }
}