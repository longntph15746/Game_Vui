using System;
using System.Drawing;
using System.Windows.Forms;

namespace Caro
{
    public partial class Form1 : Form
    {

        private int currentPlayer = 1;
        private bool gameEnded = false;
        private int boardSize = 15; 
        private Button[,] boardButtons;
        private const int buttonSize = 30;
        
        private Timer fireworksTimer;
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            InitializeBoard();
            InitializeFireworksTimer();
        }

        private void InitializeFireworksTimer()
        {
            fireworksTimer = new Timer();
            fireworksTimer.Interval = 100;
            fireworksTimer.Tick += FireworksTimer_Tick;
        }

        private void FireworksTimer_Tick(object sender, EventArgs e)
        {
            int x = random.Next(0, ClientSize.Width - 50);
            int y = random.Next(0, ClientSize.Height - 50);
            Color color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));

            using (Graphics g = CreateGraphics())
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.FillEllipse(brush, x, y, 50, 50);
            }
        }
        private void InitializeBoard()
        {
            int boardWidth = boardSize * buttonSize;
            int boardHeight = boardSize * buttonSize;

            ClientSize = new Size(boardWidth, boardHeight);

            boardButtons = new Button[boardSize, boardSize];

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    var button = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(j * buttonSize, i * buttonSize),
                        Tag = new Tuple<int, int>(i, j),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Arial", 14, FontStyle.Bold)
                    };
                    button.Click += BoardButtonClick;
                    Controls.Add(button);
                    boardButtons[i, j] = button;
                }
            }
        }

        private void BoardButtonClick(object sender, EventArgs e)
        {
            if (!gameEnded)
            {
                var button = (Button)sender;
                var coords = (Tuple<int, int>)button.Tag;
                int row = coords.Item1;
                int col = coords.Item2;

                if (button.Text == "")
                {
                    button.Text = (currentPlayer == 1) ? "X" : "O";

                    if (CheckWin(row, col))
                    {
                        gameEnded = true;
                        fireworksTimer.Start();
                        var result = MessageBox.Show("Player " + currentPlayer + " chiến thắng! Bạn có muốn chơi lại không?", "Dừng cuộc chơi", MessageBoxButtons.YesNo);
                        fireworksTimer.Stop();

                        if (result == DialogResult.Yes)
                        {
                            ResetGame();
                        }
                        else
                        {
                            Application.Exit();
                        }
                    }
                    else
                    {
                        currentPlayer = 3 - currentPlayer;
                    }
                }
            }
        }

        private bool CheckWin(int row, int col)
        {
            string playerSymbol = (currentPlayer == 1) ? "X" : "O";

            int count = 1;
            for (int i = col - 1; i >= 0 && boardButtons[row, i].Text == playerSymbol; i--)
                count++;
            for (int i = col + 1; i < boardButtons.GetLength(1) && boardButtons[row, i].Text == playerSymbol; i++)
                count++;
            if (count >= 5) return true;

            count = 1;
            for (int i = row - 1; i >= 0 && boardButtons[i, col].Text == playerSymbol; i--)
                count++;
            for (int i = row + 1; i < boardButtons.GetLength(0) && boardButtons[i, col].Text == playerSymbol; i++)
                count++;
            if (count >= 5) return true;

            count = 1;
            for (int i = row - 1, j = col - 1; i >= 0 && j >= 0 && boardButtons[i, j].Text == playerSymbol; i--, j--)
                count++;
            for (int i = row + 1, j = col + 1; i < boardButtons.GetLength(0) && j < boardButtons.GetLength(1) && boardButtons[i, j].Text == playerSymbol; i++, j++)
                count++;
            if (count >= 5) return true;

            count = 1;
            for (int i = row - 1, j = col + 1; i >= 0 && j < boardButtons.GetLength(1) && boardButtons[i, j].Text == playerSymbol; i--, j++)
                count++;
            for (int i = row + 1, j = col - 1; i < boardButtons.GetLength(0) && j >= 0 && boardButtons[i, j].Text == playerSymbol; i++, j--)
                count++;
            if (count >= 5) return true;

            return false;
        }
        private void ResetGame()
        {
            foreach (var button in boardButtons)
            {
                button.Text = "";
            }

            currentPlayer = 1;
            gameEnded = false;
        }
    }
}
