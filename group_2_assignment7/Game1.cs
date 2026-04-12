using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace group_2_assignment7;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Player player;
    private int score;
    private float spawnTimer;
    private float spawnInterval;
    private List<Star> stars;
    private Texture2D starSheet;
    private Random _random;
    private int screenH;
    private int screenW;
    private KeyboardState prevKS;
    private KeyboardState currKS;

    private enum GameState
    {
        PAUSED,
        PLAYING
    }
    private GameState gameState;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        score = 0;
        spawnInterval = 1.5f;
        spawnTimer = 1.5f;
        screenH = GraphicsDevice.Viewport.Height;
        screenW = GraphicsDevice.Viewport.Width;
        stars = new List<Star>();
        _random = new Random();
        prevKS = Keyboard.GetState();
        gameState = GameState.PLAYING;
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        starSheet = Content.Load<Texture2D>("stars");
    }

    protected override void Update(GameTime gameTime)
    {
        currKS = Keyboard.GetState();
        
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        // P key for resume/pause
        if (currKS.IsKeyDown(Keys.P) && prevKS.IsKeyUp(Keys.P))
        {
            // resume
            if (gameState == GameState.PAUSED)
            {
                gameState = GameState.PLAYING;
            }
            
            // pasue
            else
            {
                gameState = GameState.PAUSED;
            }
        }
        
        // Enter key for restart
        if (currKS.IsKeyDown(Keys.Enter) && prevKS.IsKeyUp(Keys.Enter))
        {
            spawnTimer = 1.5f;
            score = 0;
            gameState = GameState.PLAYING;
            stars = new List<Star>();
        }


        if (gameState == GameState.PLAYING)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // decrease spawnTimer
            spawnTimer -= dt;

            // spawn new star when it expired
            if (spawnTimer <= 0f)
            {
                stars.Add(new Star(starSheet, _random.Next(0, screenW - starSheet.Width / 2), 30f, screenH));
                spawnTimer = 1.5f;
            }

            // check each star whether it is collected by player
            foreach (Star star in stars)
            {
                if (player.BoundingBox().Intersects(star.BoundingBox()) && star.Active())
                {
                    score += 1;
                    star.Collected();
                }
            }
        
            // remove all de-activated stars
            for (int i = stars.Count - 1; i >= 0; i--)
            {
                if (!stars[i].Active())
                {
                    stars.Remove(stars[i]);
                }
            }
        
            // update all stars
            foreach (Star star in stars)
            {
                star.Update(gameTime);
            }
        }

        prevKS = currKS;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        foreach (Star star in stars)
        {
            star.Draw(_spriteBatch);
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
