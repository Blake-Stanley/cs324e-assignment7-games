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

    private GameState gameState = GameState.TITLE;
    private Player player;
    private int score;
    private int lives;
    private List<Obstacle> obstacles;
    private List<Star> stars;
    private HUD hud;
    private ScreenManager screenManager;

    private Texture2D starSheet;
    private float animTimer;
    private float animDuration;

    // separate spawn timers for stars and swords
    private float starSpawnTimer;
    private float starSpawnInterval;
    private float swordSpawnTimer;
    private float swordSpawnInterval;

    // separate recent spawn X lists so stars and swords don't push each other to opposite sides
    private List<float> recentStarX;
    private List<float> recentSwordX;
    private const int MinSpawnDist = 100;

    private Texture2D swordTexture;
    private Texture2D playerSheet;
    private Texture2D backgroundTexture;
    private Texture2D groundTexture;
    private const int GroundHeight = 48;

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
        _graphics.PreferredBackBufferWidth = 480;
        _graphics.PreferredBackBufferHeight = 800;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        gameState = GameState.TITLE;
        lives = 3;
        score = 0;
        animTimer = 0f;
        animDuration = 0.15f;
        obstacles = new List<Obstacle>();
        stars = new List<Star>();
        recentStarX = new List<float>();
        recentSwordX = new List<float>();
        starSpawnTimer = 1.5f; // first star after 1.5s
        starSpawnInterval = 2.5f; // stars spawn less frequently
        swordSpawnTimer = 2.0f; // first sword after 2.0s (staggered)
        swordSpawnInterval = 2.0f;
        screenH = GraphicsDevice.Viewport.Height;
        screenW = GraphicsDevice.Viewport.Width;
        _random = new Random();
        prevKS = Keyboard.GetState();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        starSheet = Content.Load<Texture2D>("stars");

        // knight sprite sheet: 4 frames (64x64 each) — idle(0,1), walk(2,3)
        playerSheet = Content.Load<Texture2D>("player");

        backgroundTexture = Content.Load<Texture2D>("background");
        groundTexture = Content.Load<Texture2D>("ground");
        swordTexture = Content.Load<Texture2D>("sword");

        player = new Player(playerSheet,
            new Vector2(screenW / 2 - 32, screenH - GroundHeight - 64),
            screenW, screenH, GroundHeight);

        // TODO: uncomment once Sydney pushes her assets
        titleFont = Content.Load<SpriteFont>("TitleFont");
        bodyFont = Content.Load<SpriteFont>("BodyFont");
        hudFont = Content.Load<SpriteFont>("HudFont");
        
        Texture2D heartTexture = Content.Load<Texture2D>("heart");

        hud = new HUD(hudFont, heartTexture, screenW);
        screenManager = new ScreenManager(titleFont, bodyFont, screenW, screenH);
        
        Console.WriteLine("Heart texture loaded: " + (heartTexture != null));
    }

    protected override void Update(GameTime gameTime)
    {
        currKS = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
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

        // Title screen: press Enter to start
        if (gameState == GameState.TITLE)
        {
            if (currKS.IsKeyDown(Keys.Enter) && prevKS.IsKeyUp(Keys.Enter))
            {
                gameState = GameState.PLAYING;
            }
        }

        // R key to restart from win/lose screens
        if ((gameState == GameState.WIN || gameState == GameState.LOSE) &&
            currKS.IsKeyDown(Keys.R) && prevKS.IsKeyUp(Keys.R))
        {
            score = 0;
            lives = 3;
            gameState = GameState.PLAYING;
            stars = new List<Star>();
            obstacles = new List<Obstacle>();
            recentStarX = new List<float>();
            recentSwordX = new List<float>();
            starSpawnTimer = 1.5f;
            swordSpawnTimer = 2.0f;
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

            // difficulty scaling: intervals shrink every 10 points
            starSpawnInterval = Math.Max(1.2f, 2.5f - (score / 10));
            swordSpawnInterval = Math.Max(0.8f, 2.0f - (score / 10));

            float swordFallSpeed = 120f + score * 3;

            starSpawnTimer -= dt;
            swordSpawnTimer -= dt;

            if (starSpawnTimer <= 0f)
            {
                int starRenderW = (int)(starSheet.Width / 2 * Star.Scale);
                stars.Add(new Star(starSheet, PickSpawnX(starRenderW, recentStarX), 30f, screenH));
                starSpawnTimer = starSpawnInterval;
            }

            if (swordSpawnTimer <= 0f)
            {
                int swordRenderW = (int)(swordTexture.Width * Obstacle.Scale);
                obstacles.Add(new Obstacle(swordTexture, PickSpawnX(swordRenderW, recentSwordX), swordFallSpeed,
                    screenH));
                swordSpawnTimer = swordSpawnInterval;
            }

            player.Update(gameTime);

            // update swords and check collision
            foreach (Obstacle obstacle in obstacles)
            {
                obstacle.Update(gameTime);
                if (player.BoundingBox().Intersects(obstacle.BoundingBox()) && obstacle.Active())
                {
                    lives -= 1;
                    obstacle.Deactivate();
                }
            }

            // update stars and check collision
            foreach (Star star in stars)
            {
                star.Update(gameTime);
                if (player.BoundingBox().Intersects(star.BoundingBox()) && star.Active())
                {
                    score += 1;
                    star.Collected();
                }
            }

            // remove inactive stars
            for (int i = stars.Count - 1; i >= 0; i--)
            {
                if (!stars[i].Active())
                {
                    stars.RemoveAt(i);
                }
            }

            // remove inactive obstacles
            for (int i = obstacles.Count - 1; i >= 0; i--)
            {
                if (!obstacles[i].Active())
                {
                    obstacles.RemoveAt(i);
                }
            }

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

    // Pick a spawn X that is at least MinSpawnDist from recent spawns.
    // Tries 8 candidates and takes the best; falls back to a random pick.
    private float PickSpawnX(int objectWidth, List<float> recentList)
    {
        int maxX = screenW - objectWidth;
        float bestX = _random.Next(0, maxX);
        float bestDist = 0f;

        for (int i = 0; i < 8; i++)
        {
            float candidate = _random.Next(0, maxX);
            float minDist = float.MaxValue;
            for (int j = 0; j < recentList.Count; j++)
                minDist = Math.Min(minDist, Math.Abs(candidate - recentList[j]));

            if (recentList.Count == 0 || minDist > bestDist)
            {
                bestDist = minDist;
                bestX = candidate;
            }

            if (bestDist >= MinSpawnDist) break;
        }

        recentList.Add(bestX);
        if (recentList.Count > 6) recentList.RemoveAt(0);
        return bestX;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        _spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
        _spriteBatch.Draw(groundTexture, new Vector2(0, screenH - GroundHeight), Color.White);

        if (gameState == GameState.PLAYING)
        {
            foreach (Star star in stars)
                star.Draw(_spriteBatch);

            foreach (Obstacle obstacle in obstacles)
                obstacle.Draw(_spriteBatch);

            player.Draw(_spriteBatch);

            hud.Draw(_spriteBatch, score, lives, score / 10);
        }
        else if (gameState == GameState.TITLE)
        {
            screenManager.DrawTitle(_spriteBatch);
        }
        else if (gameState == GameState.PAUSED)
        {
            screenManager.DrawPause(_spriteBatch);
        }
        else if (gameState == GameState.WIN)
        {
            screenManager.DrawWin(_spriteBatch, score);
        }
        else if (gameState == GameState.LOSE)
        {
            screenManager.DrawLose(_spriteBatch, score);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}


