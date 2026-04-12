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

    private GameState gameState;
    private Player player;
    private int score;
    private int lives;
    private List<Obstacle> obstacles;
    private float spawnTimer;
    private float spawnInterval;
    private List<Star> stars;
    private HUD hud;
    private ScreenManager screenManager;
    
    private Texture2D starSheet;
    private float animTimer;
    private float animDuration;
    
    private Texture2D obstacleSheet;
    private Texture2D playerSheet;
    private Texture2D backgroundTexture;
    
    private SpriteFont titleFont;
    private SpriteFont bodyFont;
    private SpriteFont hudFont;
    
    
    
    private Random _random;
    private int screenH;
    private int screenW;
    private KeyboardState prevKS;
    private KeyboardState currKS;

    private enum GameState
    {
        TITLE,
        PLAYING,
        PAUSED,
        WIN,
        LOSE
    }

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        gameState = GameState.TITLE;
        lives = 3;
        score = 0;
        spawnInterval = 1.5f;
        spawnTimer = 1.5f;
        animTimer = 0f;
        animDuration = 0.15f;
        obstacles = new List<Obstacle>();
        screenH = GraphicsDevice.Viewport.Height;
        screenW = GraphicsDevice.Viewport.Width;
        stars = new List<Star>();
        _random = new Random();
        prevKS = Keyboard.GetState();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        starSheet = Content.Load<Texture2D>("stars");
    
        // TODO: uncomment once Blake pushes his assets
        // obstacleSheet = Content.Load<Texture2D>("obstacle");
        // playerSheet = Content.Load<Texture2D>("player");
        // backgroundTexture = Content.Load<Texture2D>("background");

        // TODO: uncomment once Sydney pushes her assets
        // titleFont = Content.Load<SpriteFont>("TitleFont");
        // bodyFont = Content.Load<SpriteFont>("BodyFont");
        // hudFont = Content.Load<SpriteFont>("HudFont");
        // player = new Player(playerSheet, new Vector2(screenW / 2, screenH - 64));
        // hud = new HUD(hudFont, Content.Load<Texture2D>("heart"));
        // screenManager = new ScreenManager(titleFont, bodyFont);
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
            
            // pause
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

        // Title screen: press Enter to start
        if (gameState == GameState.TITLE)
        {
            if (currKS.IsKeyDown(Keys.Enter) && prevKS.IsKeyUp(Keys.Enter))
            {
                gameState = GameState.PLAYING;
            }
        }
        
        if (gameState == GameState.PLAYING)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // animation timer
            animTimer += dt;
            if (animTimer >= animDuration)
            {
                animTimer = 0f;
            }
            
            // difficulty scaling: decrease spawnInterval every 10 points
            spawnInterval = Math.Max(0.5f, 1.5f - (score / 10) * 0.1f);
            
            // decrease spawnTimer
            spawnTimer -= dt;

            // spawn new star when it expired
            if (spawnTimer <= 0f)
            {
                stars.Add(new Star(starSheet, _random.Next(0, screenW - starSheet.Width / 2), 30f, screenH));
                
                
                // obstacles.Add(new Obstacle(obstacleSheet, _random.Next(0, screenW - obstacleSheet.Width), 80f + (score / 10) * 10f));
                spawnTimer = spawnInterval;
            }
            
            // TODO: update player once Blake pushes Player.cs
            // player.Update(gameTime);

            // TODO: update obstacles once Blake pushes Obstacle.cs
            // foreach (Obstacle obstacle in obstacles) { obstacle.Update(gameTime); }
            
            // TODO: collision player vs obstacles once Blake pushes Obstacle.cs
            // foreach (Obstacle obstacle in obstacles)
            // {
            //     if (player.BoundingBox().Intersects(obstacle.BoundingBox()) && obstacle.Active())
            //     {
            //         lives -= 1;
            //         obstacle.Deactivate();
            //     }
            // }
        
            // remove all de-activated stars
            for (int i = stars.Count - 1; i >= 0; i--)
            {
                if (!stars[i].Active())
                {
                    stars.Remove(stars[i]);
                }
            }
            
            // TODO: remove inactive obstacles once Blake pushes Obstacle.cs
            // for (int i = obstacles.Count - 1; i >= 0; i--)
            // {
            //     if (!obstacles[i].Active()) { obstacles.RemoveAt(i); }
            // }
        
            // update all stars
            // TODO: uncomment once Blake pushes Player.cs
            // foreach (Star star in stars)
            // {
            //     if (player.BoundingBox().Intersects(star.BoundingBox()) && star.Active())
            //     {
            //         score += 1;
            //         star.Collected();
            //     }
            // }
            
            // check win/lose conditions
            if (score >= 50)
            {
                gameState = GameState.WIN;
            }
            else if (lives <= 0)
            {
                gameState = GameState.LOSE;
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
        
        // TODO: draw background once Sydney pushes asset
        // _spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
        
        foreach (Star star in stars)
        {
            star.Draw(_spriteBatch);
        }
        
        // foreach (Obstacle obstacle in obstacles) { obstacle.Draw(_spriteBatch); }
        
        // player.Draw(_spriteBatch);
        
        // hud.Draw(_spriteBatch, score, lives, score / 10);
        
        // if (gameState == GameState.TITLE) { screenManager.DrawTitle(_spriteBatch); }
        // else if (gameState == GameState.PAUSED) { screenManager.DrawPause(_spriteBatch); }
        // else if (gameState == GameState.WIN) { screenManager.DrawWin(_spriteBatch, score); }
        // else if (gameState == GameState.LOSE) { screenManager.DrawLose(_spriteBatch, score); }
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
