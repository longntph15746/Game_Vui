using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chọn_ảnh_giống_nhau
{
    public partial class Form1 : Form
    {
        private List<Button> buttons = new List<Button>();
        private Button firstButton = null;
        private bool isAnimating = false;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            const int rows = 4;
            const int columns = 4;

            ClientSize = new Size(columns * 100, rows * 100);

            List<string> icons = new List<string>
            {
                "🐶", "🐱", "🐭", "🐹",
                "🐰", "🦊", "🐻", "🐼",
                "🐶", "🐱", "🐭", "🐹",
                "🐰", "🦊", "🐻", "🐼"
            };

            Random random = new Random();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Button button = new Button
                    {
                        Size = new Size(80, 80),
                        Location = new Point(j * 100, i * 100),
                        Tag = icons[0],
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Arial", 20, FontStyle.Bold),
                        Visible = true
                    };

                    button.Click += CardButtonClick;

                    buttons.Add(button);
                    icons.RemoveAt(0);

                    Controls.Add(button);
                }
            }
        }

        private async void CardButtonClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (!isAnimating && button.Text == "")
            {
                button.Text = button.Tag.ToString();

                if (firstButton == null)
                {
                    firstButton = button;
                }
                else
                {
                    isAnimating = true;

                    if (firstButton.Tag.ToString() == button.Tag.ToString())
                    {
                        firstButton.Enabled = false;
                        button.Enabled = false;
                        firstButton = null;
                        isAnimating = false;
                    }
                    else
                    {
                        await System.Threading.Tasks.Task.Delay(1000);
                        firstButton.Text = "";
                        button.Text = "";
                        firstButton = null;
                        isAnimating = false;
                    }
                }
            }
        }
    }
}
