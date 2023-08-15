using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Khủng_long_vượt_khó
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }
        private int dinoY = 0;
        private int dinoYSpeed = 0;
        private int gravity = 2;
        private int jumpStrength = -30;
        private bool isJumping = false;

        private int obstacleX = 800;
        private int obstacleSpeed = 10;
        private int obstacleWidth = 30;
        private int obstacleHeight = 60;

        private int score = 0;
        private int maxObstacleSpeed = 20;
        private int obstacleSpeedIncrement = 1;

        private bool isGameOver = false;

        private void InitializeGame()
        {
            ClientSize = new Size(800, 400);
            DoubleBuffered = true;

            dinoY = ClientSize.Height - 50;
            obstacleX = ClientSize.Width;

            KeyDown += MainForm_KeyDown;
            Paint += MainForm_Paint;

            Timer gameTimer = new Timer();
            gameTimer.Interval = 20;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && !isJumping && !isGameOver)
            {
                isJumping = true;
                dinoYSpeed = jumpStrength;
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (!isGameOver)
            {
                dinoY += dinoYSpeed;
                dinoYSpeed += gravity;

                if (dinoY >= ClientSize.Height - 50)
                {
                    dinoY = ClientSize.Height - 50;
                    isJumping = false;
                }

                obstacleX -= obstacleSpeed;

                if (obstacleX + obstacleWidth <= 0)
                {
                    obstacleX = ClientSize.Width;
                    score++;
                    if (score % 5 == 0 && obstacleSpeed < maxObstacleSpeed)
                    {
                        obstacleSpeed += obstacleSpeedIncrement;
                    }
                    if (score % 10 == 0)
                    {
                        obstacleHeight += 10;
                    }
                }

                if (obstacleX <= 50 && obstacleX + obstacleWidth >= 50 && dinoY >= ClientSize.Height - obstacleHeight)
                {
                    isGameOver = true;
                }

                Invalidate();
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.FillRectangle(Brushes.Gray, 0, ClientSize.Height - 50, ClientSize.Width, 50);

            int dinoWidth = 40 + (score / 10) * 10;
            g.FillRectangle(Brushes.Green, 50, dinoY, dinoWidth, 40);

            g.FillRectangle(Brushes.Brown, obstacleX, ClientSize.Height - obstacleHeight, obstacleWidth, obstacleHeight);

            g.DrawString("Điểm: " + score, new Font("Arial", 16), Brushes.Black, 10, 10);

            if (isGameOver)
            {
                g.DrawString("Game Over", new Font("Arial", 24), Brushes.Black, ClientSize.Width / 2 - 100, ClientSize.Height / 2 - 20);
            }
        }


    }
}
