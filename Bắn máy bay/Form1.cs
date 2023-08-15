using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
namespace Bắn_máy_bay
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }
        private PlayerShip playerShip;
        private List<Invader> invaders;
        private List<Projectile> projectiles;
        private int score = 0;


        private void InitializeGame()
        {
            ClientSize = new Size(800, 600);
            DoubleBuffered = true;

            playerShip = new PlayerShip(ClientSize.Width / 2, ClientSize.Height - 50);
            invaders = new List<Invader>();
            projectiles = new List<Projectile>();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    invaders.Add(new Invader(j * 50 + 50, i * 50 + 50));
                }
            }

            KeyDown += MainForm_KeyDown;
            Paint += MainForm_Paint;

            Timer gameTimer = new Timer();
            gameTimer.Interval = 20;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                playerShip.MoveLeft();
            }
            else if (e.KeyCode == Keys.Right)
            {
                playerShip.MoveRight(ClientSize.Width);
            }
            else if (e.KeyCode == Keys.Space)
            {
                projectiles.Add(new Projectile(playerShip.X + playerShip.Width / 2, playerShip.Y));
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            playerShip.Update();

            foreach (Projectile projectile in projectiles)
            {
                projectile.Update();
            }

            for (int i = invaders.Count - 1; i >= 0; i--)
            {
                Invader invader = invaders[i];
                invader.Update();

                for (int j = projectiles.Count - 1; j >= 0; j--)
                {
                    Projectile projectile = projectiles[j];
                    if (projectile.CollidesWith(invader))
                    {
                        invaders.RemoveAt(i);
                        projectiles.RemoveAt(j);
                        score += 10;
                        break;
                    }
                }
            }

            Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            playerShip.Draw(g);

            foreach (Projectile projectile in projectiles)
            {
                projectile.Draw(g);
            }

            foreach (Invader invader in invaders)
            {
                invader.Draw(g);
            }

            g.DrawString("Điển: " + score, new Font("Arial", 16), Brushes.Green, 10, 10);
        }
    }

    public class PlayerShip
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; } = 50;
        public int Height { get; } = 30;
        public int Speed { get; } = 5;

        public PlayerShip(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void MoveLeft()
        {
            if (X > 0)
            {
                X -= Speed;
            }
        }

        public void MoveRight(int maxX)
        {
            if (X + Width < maxX)
            {
                X += Speed;
            }
        }

        public void Update()
        {
            // No additional update logic for player ship
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Blue, X, Y, Width, Height);
        }
    }

    public class Projectile
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; } = 5;
        public int Height { get; } = 10;
        public int Speed { get; } = 10;

        public Projectile(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Update()
        {
            Y -= Speed;
        }

        public bool CollidesWith(Invader invader)
        {
            Rectangle projectileRect = new Rectangle(X, Y, Width, Height);
            Rectangle invaderRect = new Rectangle(invader.X, invader.Y, invader.Width, invader.Height);
            return projectileRect.IntersectsWith(invaderRect);
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.White, X, Y, Width, Height);
        }
    }

    public class Invader
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; } = 40;
        public int Height { get; } = 30;
        public int Speed { get; private set; } = 2; // Fix bug: Speed should be private set

        public Invader(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Update()
        {
            X += Speed;
            if (X + Width > 800 || X < 0)
            {
                X -= Speed;
                Y += Height;
                Speed = -Speed;
            }
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Red, X, Y, Width, Height);
        }
    }
}
