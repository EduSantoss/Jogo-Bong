using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // fonte e temporizador
    private SpriteFont _font;
    private string _timeText;

    // animaçao personagem, apenas teste
    private Texture2D _alienAnimation;
    private Rectangle[] _frames;
    private int _index;

    // pontuaçao
    private int _score;
    private int _score2;
    private bool _releasedScore;

    private Texture2D _ballTexture;
    private Texture2D _background;
    private Vector2 _ballPosition;
    private Vector2 _ballDirection;
    private float _ballSpeed;

    // cor da bola
    private Color _ballColor;
    private double _time;

    // barras 
    private Texture2D _bar;
    private Vector2 _bar1Position;
    private Vector2 _bar2Position;
    private Vector2 _bar2Direction;
    private float _barSpeed;

    private Random _random;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
      //  IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
        // TODO: Add your initialization logic here
        _random = new Random();

        _frames = new Rectangle[2]
        {
           new Rectangle(0, 0, 128, 128), new Rectangle(128, 0, 128, 128)
        };
        _index = 0;
       // _time = 0.0;
        _score = 0;
        _score2 = 0;
        _releasedScore = true;

        _ballPosition = new Vector2((_graphics.PreferredBackBufferWidth - _ballTexture.Width) / 2.0f, (_graphics.PreferredBackBufferHeight - _ballTexture.Height) / 2.0f);
        _ballSpeed = 5.0f;
        // para ir para horizontal ou vertical so trocando x e y
        // _ballDirection = Vector2.UnitX;
        _ballDirection = new Vector2(1.0f, GetRandomY());
        _ballDirection.Normalize();

        // cor da bola
        _ballColor = Color.White;
        _time = 0.0;

        _bar1Position = new Vector2(0.0f, (_graphics.PreferredBackBufferHeight - _bar.Height) / 2.0f );
        _bar2Position = new Vector2(_graphics.PreferredBackBufferWidth - _bar.Width, (_graphics.PreferredBackBufferHeight - _bar.Height) / 2.0f);
        // direcao da barra 2 apenas para deixar "äutomatico"
       // _bar2Direction = Vector2.UnitY;
        _barSpeed = 5.0f;

    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        _ballTexture = Content.Load<Texture2D>("ball");
        _background = Content.Load<Texture2D>("background");
        _bar = Content.Load<Texture2D>("bar");
        _font = Content.Load<SpriteFont>("arial24");
        _alienAnimation = Content.Load<Texture2D>("alien");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        // TODO: Add your update logic here

        // temporizador com tempo do game
         _timeText = string.Format("Tempo: {0}", gameTime.TotalGameTime.TotalSeconds);
        // tempo entre quadros
       // _timeText = string.Format("Tempo: {0}", gameTime.ElapsedGameTime.TotalSeconds);

        // mudança de cor da bola
        _time = _time + gameTime.ElapsedGameTime.TotalSeconds;
        if (_time > 0.5){
            _time = 0.0;
            if (_ballColor == Color.White){
                _ballColor = Color.Yellow;
            }
            else{
                _ballColor = Color.White;
            }
        }
        
        // logica da pontuaçao, score 1 e score 2
        // corrigir o update feito por frames, basta criar um booleano, mas ai a pontuçao fica travada em 1
        if(_ballPosition.X < 0.0f && _releasedScore == true){
            _score2++;
            _releasedScore = false;
        }
        if(_ballPosition.X > _graphics.PreferredBackBufferWidth - _ballTexture.Width && _releasedScore == true){
            _score++;
            _releasedScore = false;
        }
        // logica do vencedor
        // if(_score == 15){
            
        // }



        // para a pontuaçao voltar a subir, bastou eu botar para quando ela voltar ao meio, o booleano voltar a ser true
        if(_ballPosition == new Vector2((_graphics.PreferredBackBufferWidth - _ballTexture.Width) / 2.0f, (_graphics.PreferredBackBufferHeight - _ballTexture.Height) / 2.0f)){
            _releasedScore = true;
        }

       // animaçao alien
       // _time = _time + gameTime.ElapsedGameTime.TotalSeconds;
    //    if (_time > 0.1){
    //     _time = 0.0;
    //     _index++;
    //     if (_index > 1){
    //         _index = 0;
    //     }
    //    }

        // movimentaçao da bola
        _ballPosition = _ballPosition + (_ballDirection * _ballSpeed);

        // movimentaçao por tecla da barra 1
        KeyboardState keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.W)){
            _bar1Position.Y -=  _barSpeed; 
        }

        if (keyboardState.IsKeyDown(Keys.S)){
            _bar1Position.Y += _barSpeed;
        }

        // movimentaçao da barra 2
        _bar2Position.Y = Mouse.GetState().Y - (_bar.Height / 2.0f); 
       // descomentar parte de baixo e comentar parte de cima para barra 2 ficar automatica
       // _bar2Position = _bar2Position + (_bar2Direction * _barSpeed);

        // colisao das barras com os limites verticais
        if(_bar1Position.Y < 0){
          _bar1Position.Y = 0.0f;
        }
        else if(_bar1Position.Y > _graphics.PreferredBackBufferHeight - _bar.Height){
            _bar1Position.Y = _graphics.PreferredBackBufferHeight - _bar.Height;
        }
      // opçao de deixar barra 2 automatica
        if(_bar2Position.Y < 0){
            _bar2Position.Y = 0.0f;
         //   _bar2Direction.Y = -_bar2Direction.Y;
        }
        else if(_bar2Position.Y > _graphics.PreferredBackBufferHeight - _bar.Height){
            _bar2Position.Y = _graphics.PreferredBackBufferHeight - _bar.Height;
          //  _bar2Direction.Y = - _bar2Direction.Y;
        }

        // colisao da bola com as barras
        if (_ballPosition.X < _bar.Width){
           if ((_ballPosition.Y + _ballTexture.Height > _bar1Position.Y) && (_ballPosition.Y < _bar1Position.Y + _bar.Height))
            {
              _ballPosition.X = _bar.Width;
              _ballDirection = new Vector2(1.0f, GetRandomY());
              _ballDirection.Normalize();
            }
        }
        else if (_ballPosition.X + _ballTexture.Width > _graphics.PreferredBackBufferWidth - _bar.Width){
            if ((_ballPosition.Y + _ballTexture.Height > _bar2Position.Y) && (_ballPosition.Y < _bar2Position.Y + _bar.Height))
            {
              _ballPosition.X = _graphics.PreferredBackBufferWidth - _bar.Width - _ballTexture.Width;
              _ballDirection = new Vector2(-1.0f, GetRandomY());
              _ballDirection.Normalize();
            }
        }

        // colisao da bola com limite horizontal
        // if(_ballPosition.X < 0){
        //     _ballPosition.X = 0.0f;
        //     _ballDirection.X = -_ballDirection.X;
        // }
        // else if(_ballPosition.X >_graphics.PreferredBackBufferWidth -_ballTexture.Width){
        //     _ballPosition.X = _graphics.PreferredBackBufferWidth -_ballTexture.Width;
        //     _ballDirection.X = -_ballDirection.X;
        // }

        // alterado aqui para bola voltar ao meio quando passar da horizontal, ideia principal de pontuaçao do game
        if((_ballPosition.X + _ballTexture.Width < 0.0f) || (_ballPosition.X  > _graphics.PreferredBackBufferWidth)){
            _ballPosition = new Vector2((_graphics.PreferredBackBufferWidth - _ballTexture.Width) / 2.0f, (_graphics.PreferredBackBufferHeight - _ballTexture.Height) / 2.0f);
            // tentar corrigir randomX
            _ballDirection = new Vector2(1.0f, GetRandomY());
            _ballDirection.Normalize();
        }
        
        // colisao da bola com limite vertical
        if(_ballPosition.Y < 0){
            _ballPosition.Y = 0.0f;
            _ballDirection.Y = -_ballDirection.Y;
        }
        else if(_ballPosition.Y >_graphics.PreferredBackBufferHeight -_ballTexture.Height){
            _ballPosition.Y = _graphics.PreferredBackBufferHeight -_ballTexture.Height;
            _ballDirection.Y = -_ballDirection.Y;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {   // esse inicio é basicamente a cor de fundo
        GraphicsDevice.Clear(Color.CornflowerBlue);
 
        // TODO: Add your drawing code here
        // converter de interger para string, ToString()
        _spriteBatch.Begin();
        _spriteBatch.Draw(_background, new Vector2(0,0), Color.White);
      //   _spriteBatch.Draw(_alienAnimation, new Rectangle(400, 200, 128, 128), _frames[_index], Color.White);
        _spriteBatch.DrawString(_font, "Pontuacao: " + _score.ToString(), new Vector2(50,0), Color.Black);
        _spriteBatch.DrawString(_font, "Pontuacao: " + _score2.ToString(), new Vector2(500,0), Color.Black);
      //  _spriteBatch.DrawString(_font, _timeText, new Vector2(0,400), Color.Black);
        _spriteBatch.Draw(_bar, _bar1Position, Color.White);
        _spriteBatch.Draw(_bar, _bar2Position, Color.White);
        _spriteBatch.Draw(_ballTexture, _ballPosition, _ballColor);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private float GetRandomY()
    {
        return(_random.NextSingle() * 2.0f) - 1.0f;
    }
     private float GetRandomX()
    {
        return(_random.NextSingle() * 2.0f) - 1.0f;
    }
}
