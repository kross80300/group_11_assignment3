using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    private List<(string word, Vector2 pos, Color color)> _wordCloud = new();
    private List<(int freq, int count)> _frequencyData = new List<(int, int)>();
    private bool _showWordCloud = true;
    private readonly Color[] _palette = { Color.ForestGreen, Color.MediumPurple, Color.Black };
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

        for (int i = 0; i < 30 && i < _words.Length; i++)
        {
            string word = _words[_rng.Next(_words.Length)];
            Vector2 pos = new Vector2(_rng.Next(700), _rng.Next(600));
            Color color = _palette[_rng.Next(_palette.Length)];
            _wordCloud.Add((word, pos, color));
        }

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

        if (keyboardState.IsKeyDown(Keys.Enter) && _prevKeyboardState.IsKeyUp(Keys.Enter))
            _showWordCloud = !_showWordCloud;

        _prevKeyboardState = keyboardState;

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();

        if (_showWordCloud)
            DisplayUniqueWords();
        else
            DisplayWordFrequency();

        _spriteBatch.End();
        base.Draw(gameTime);
    }

    // Add these two functions inside Game1:
    private void DisplayUniqueWords()
    {
        int x = 50;
        int y = 50;
        int maxWidth = 600;

        foreach (var (word, pos, color) in _wordCloud)
        {
            int freq = 1;
            foreach (var (f, count) in _frequencyData)
            {
                if (f > 0)
                {
                    freq = f;
                    break;
                }
            }

            float scale = Math.Clamp(freq / 10f, 0.5f, 3f);

            Vector2 size = _font.MeasureString(word) * scale;

            if (x + size.X > maxWidth)
            {
                x = 50;
                y += (int)(size.Y + 20);
            }

            _spriteBatch.DrawString(_font, word, new Vector2(x, y), color,
                0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            x += (int)(size.X + 20);
        }
    }

    private void DisplayWordFrequency()
    {
        _spriteBatch.DrawString(_font, "WORD FREQUENCIES", new Vector2(20, 20), Color.Black);

        int startX = 50;
        int startY = GraphicsDevice.Viewport.Height - 50;
        int barWidth = 15;
        int spacing = 2;

        if (_frequencyData.Count == 0) return;

        int maxFreq = _frequencyData.Max(fd => fd.count);
        float scale = 300f / maxFreq;

        for (int i = 0; i < _frequencyData.Count && i < 50; i++)
        {
            int freq = _frequencyData[i].count;
            int barHeight = (int)(freq * scale);

            Rectangle bar = new Rectangle(
                startX + i * (barWidth + spacing),
                startY - barHeight,
                barWidth,
                barHeight);

            _spriteBatch.Draw(_graph, bar, Color.White);

            // outline
            _spriteBatch.Draw(_graph, new Rectangle(bar.X, bar.Y, bar.Width, 1), Color.Black);
            _spriteBatch.Draw(_graph, new Rectangle(bar.X, bar.Bottom - 1, bar.Width, 1), Color.Black);
            _spriteBatch.Draw(_graph, new Rectangle(bar.X, bar.Y, 1, bar.Height), Color.Black);
            _spriteBatch.Draw(_graph, new Rectangle(bar.Right - 1, bar.Y, 1, bar.Height), Color.Black);
        }
    }
}