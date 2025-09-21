using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_11_assignment3;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    private Texture2D _graph;
    private string[] _words;
    private Random _rng = new Random();
    
    private List<(int freq, int count)> _frequencyData = new List<(int, int)>();
    private bool _showWordCloud = true;
    private KeyboardState _prevKeyboardState;
    

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        _graphics.PreferredBackBufferWidth = 700;
        _graphics.PreferredBackBufferHeight = 600;
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        _font = Content.Load<SpriteFont>("font/DripOctober");

        _graph = new Texture2D(GraphicsDevice, 1, 1);
        _graph.SetData(new[] { Color.White });

        _words = File.ReadAllLines("uniquewords.txt");

        if (File.Exists("wordfrequency.txt"))
        {
            var counts = new Dictionary<string, int>();

            foreach (var line in File.ReadAllLines("wordfrequency.txt"))
            {
                var parts = line.Split(' ');
                if (parts.Length == 2)
                {
                    string freq = parts[1];
                    if (!counts.ContainsKey(freq)) counts[freq] = 0;
                    counts[freq]++;
                }
            }

            foreach (var kv in counts)
                _frequencyData.Add((0, kv.Value));
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        var keyboardState = Keyboard.GetState();

        // Toggle on Enter press (not hold)
        if (keyboardState.IsKeyDown(Keys.Enter) && _prevKeyboardState.IsKeyUp(Keys.Enter))
            _showWordCloud = !_showWordCloud;

        _prevKeyboardState = keyboardState;

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();

        for (int i = 0; i < 30 && i < _words.Length; i++)
        {
            string word = _words[_rng.Next(_words.Length)];
            Vector2 pos = new Vector2(_rng.Next(700), _rng.Next(600));
            Color color = new Color(_rng.Next(255), _rng.Next(255), _rng.Next(255));

            _spriteBatch.DrawString(_font, word, pos, color);
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    
}