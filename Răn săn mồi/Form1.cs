using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Răn_săn_mồi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }
        private const int GridSize = 20;
        private const int InitialSnakeLength = 3;
        private const int SnakeSpeed = 100; // milliseconds
        private Point snakeHead;
        private List<Point> snakeBody;
        private Point food;
        private Direction snakeDirection;
        private Direction nextSnakeDirection;
        private bool isGameOver;
        private Random random = new Random();


        private void InitializeGame()
        {
            ClientSize = new Size(800, 600);
            DoubleBuffered = true;

            snakeHead = new Point(ClientSize.Width / 2, ClientSize.Height / 2);
            snakeBody = new List<Point>();
            snakeDirection = Direction.Right;
            nextSnakeDirection = Direction.Right;
            isGameOver = false;

            for (int i = 0; i < InitialSnakeLength; i++)
            {
                snakeBody.Add(new Point(snakeHead.X - i * GridSize, snakeHead.Y));
            }

            GenerateFood();

            KeyDown += MainForm_KeyDown;
            Paint += MainForm_Paint;

            Timer gameTimer = new Timer();
            gameTimer.Interval = SnakeSpeed;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (snakeDirection != Direction.Right)
                        nextSnakeDirection = Direction.Left;
                    break;
                case Keys.Right:
                    if (snakeDirection != Direction.Left)
                        nextSnakeDirection = Direction.Right;
                    break;
                case Keys.Up:
                    if (snakeDirection != Direction.Down)
                        nextSnakeDirection = Direction.Up;
                    break;
                case Keys.Down:
                    if (snakeDirection != Direction.Up)
                        nextSnakeDirection = Direction.Down;
                    break;
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (isGameOver)
                return;

            snakeDirection = nextSnakeDirection;

            Point newHead = new Point(snakeHead.X, snakeHead.Y);

            switch (snakeDirection)
            {
                case Direction.Left:
                    newHead.X -= GridSize;
                    break;
                case Direction.Right:
                    newHead.X += GridSize;
                    break;
                case Direction.Up:
                    newHead.Y -= GridSize;
                    break;
                case Direction.Down:
                    newHead.Y += GridSize;
                    break;
            }

            if (IsCollisionWithBody(newHead) || IsCollisionWithWall(newHead))
            {
                isGameOver = true;
                Invalidate();
                return;
            }

            snakeBody.Insert(0, snakeHead);
            snakeHead = newHead;

            if (snakeHead == food)
            {
                GenerateFood();
            }
            else
            {
                snakeBody.RemoveAt(snakeBody.Count - 1);
            }

            Invalidate();
        }

        private void GenerateFood()
        {
            int maxX = ClientSize.Width / GridSize;
            int maxY = ClientSize.Height / GridSize;

            int foodX = random.Next(0, maxX) * GridSize;
            int foodY = random.Next(0, maxY) * GridSize;

            food = new Point(foodX, foodY);
        }

        private bool IsCollisionWithBody(Point head)
        {
            return snakeBody.Any(segment => segment == head);
        }

        private bool IsCollisionWithWall(Point head)
        {
            return head.X < 0 || head.X >= ClientSize.Width || head.Y < 0 || head.Y >= ClientSize.Height;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (isGameOver)
            {
                g.DrawString("Game Over", new Font("Arial", 24), Brushes.Black, ClientSize.Width / 2 - 100, ClientSize.Height / 2 - 20);
                return;
            }

            g.FillRectangle(Brushes.Black, 0, 0, ClientSize.Width, ClientSize.Height);

            foreach (var segment in snakeBody)
            {
                g.FillRectangle(Brushes.Green, segment.X, segment.Y, GridSize, GridSize);
            }

            g.FillRectangle(Brushes.Red, food.X, food.Y, GridSize, GridSize);
        }

        private enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }
    }
}
