using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
namespace Sapper
{
    public partial class Form1 : Form
    {
        bool firstEntranceState = true;
        int gameState = -1;
        private Button[,] _buttons;
        private int[,] _mineField;
        private Timer timer;
        private Button restartBtn;
        private Label backGrLbl;
        private Label timeLbl;
        private Label scoreLbl;
        int openedSlotsAmount = 0;
        int secAmount = 0;
        int width = 0;
        int height = 0;
        public Form1()
        {
            InitializeComponent();
        }


        private void SetBtnColor(int row, int col, int val)
        {
            switch (val)
            {
                case 1:
                    _buttons[row, col].ForeColor = Color.Navy;
                    break;
                case 2:
                    _buttons[row, col].ForeColor = Color.Green;
                    break;
                case 3:
                    _buttons[row, col].ForeColor = Color.Red;
                    break;
                case 4:
                    _buttons[row, col].ForeColor = Color.DarkRed;
                    break;
                case 5:
                    _buttons[row, col].ForeColor = Color.Purple;
                    break;
                case 6:
                    _buttons[row, col].ForeColor = Color.DarkOrchid;
                    break;
                case 7:
                    _buttons[row, col].ForeColor = Color.DarkViolet;
                    break;
                case 8:
                    _buttons[row, col].ForeColor = Color.Pink;
                    break;
            }
        }


        private void ShowBombs(int row, int col)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (_mineField[i, j] == -1)
                        _buttons[i, j].Image = Image.FromFile(@"Images\mine.png");
                }
            }
            timer.Stop();
            _buttons[row, col].Image = Image.FromFile(@"Images\mine.png");
            _buttons[row, col].BackColor = Color.Red;
            restartBtn.Image = Image.FromFile(@"Images\dead.png");
        }

        private void OpenOrExplode(int row, int col)
        {
            if (_mineField[row, col] == -1)
            {
                ShowBombs(row, col);
                gameState = 0;
                return;
            }
            else if (_mineField[row, col] == 1)
                return;


            int val = 0;
            _mineField[row, col] = 1;
            openedSlotsAmount++;
            scoreLbl.Text = $"{openedSlotsAmount}";
            if (row - 1 >= 0 && col - 1 >= 0)
            {
                if (_mineField[row - 1, col - 1] == -1)
                    val++;
            }
            if (row - 1 >= 0)
            {
                if (_mineField[row - 1, col] == -1)
                    val++;
            }
            if (row - 1 >= 0 && col + 1 < width)
            {
                if (_mineField[row - 1, col + 1] == -1)
                    val++;
            }
            if (col + 1 < width)
            {
                if (_mineField[row, col + 1] == -1)
                    val++;
            }
            if (row + 1 < height && col + 1 < width)
            {
                if (_mineField[row + 1, col + 1] == -1)
                    val++;
            }
            if (row + 1 < height)
            {
                if (_mineField[row + 1, col] == -1)
                    val++;
            }
            if (row + 1 < height && col - 1 >= 0)
            {
                if (_mineField[row + 1, col - 1] == -1)
                    val++;
            }
            if (col - 1 >= 0)
            {
                if (_mineField[row, col - 1] == -1)
                    val++;
            }

            if (val > 0)
            {

                _buttons[row, col].Text = val.ToString();
                SetBtnColor(row, col, val);
                return;
            }

            _buttons[row, col].Enabled = false;

            if (row - 1 >= 0 && col - 1 >= 0)
            {
                if (_mineField[row - 1, col - 1] != 1)
                    OpenOrExplode(row - 1, col - 1);
            }
            if (row - 1 >= 0)
            {
                if (_mineField[row - 1, col] != 1)
                    OpenOrExplode(row - 1, col);
            }
            if (row - 1 >= 0 && col + 1 < width)
            {
                if (_mineField[row - 1, col + 1] != 1)
                    OpenOrExplode(row - 1, col + 1);
            }
            if (col + 1 < width)
            {
                if (_mineField[row, col + 1] != 1)
                    OpenOrExplode(row, col + 1);
            }
            if (row + 1 < height && col + 1 < width)
            {
                if (_mineField[row + 1, col + 1] != 1)
                    OpenOrExplode(row + 1, col + 1);
            }
            if (row + 1 < height)
            {
                if (_mineField[row + 1, col] != 1)
                    OpenOrExplode(row + 1, col);
            }
            if (row + 1 < height && col - 1 >= 0)
            {
                if (_mineField[row + 1, col - 1] != 1)
                    OpenOrExplode(row + 1, col - 1);
            }
            if (col - 1 >= 0)
            {
                if (_mineField[row, col - 1] != 1)
                    OpenOrExplode(row, col - 1);
            }
        }

        private bool ChekForWin()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (_mineField[i, j] == 0)
                        return false;
                }
            }
            return true;
        }

        void ButtonWasPressed(object sender, EventArgs e)
        {
            if (gameState == 0)
                return;

            int row = (((Button)sender).Location.Y - 70) / 35;
            int col = ((Button)sender).Location.X / 35;


            if (gameState == -1)
            {
                timer.Start();
                gameState = 1;
                _mineField = new int[height, width];
                _mineField[row, col] = 1;
                EstablishMines();
                _mineField[row, col] = 0;
                OpenOrExplode(row, col);
            }
            else if (gameState == 1)
            {
                OpenOrExplode(row, col);
            }
            if (ChekForWin())
            {
                timer.Stop();
                DialogResult dr = MessageBox.Show("You Won\nTry again press Yes", "Sapper", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                    Restart();
                else
                    Close();
            }

        }
        private void Restart()
        {

            if (gameState == -1)
                return;
            timer.Stop();
            InitializeField();
            gameState = -1;

        }
        private void PutFlag(object sender, MouseEventArgs e)
        {
            if (gameState == 1)
            {
                int row = (((Button)sender).Location.Y - 70) / 35;
                int col = ((Button)sender).Location.X / 35;

                if (MouseButtons.Right == e.Button)
                {
                    if (_mineField[row, col] == 1)
                        return;
                    _buttons[row, col].Image = Image.FromFile(@"Images\flag.png");
                    return;
                }
            }
        }
        private void EstablishMines()
        {
            Random rand = new Random();
            int minesAmount = (height * width - (int)(height * width * 0.5)) / 3;
            int curMineAmount = 0;
            while (curMineAmount != minesAmount)
            {
                int w = rand.Next(0, width);
                int h = rand.Next(0, height);
                if (_mineField[h, w] == -1 || _mineField[h, w] == 1)
                    continue;
                _mineField[h, w] = -1;
                curMineAmount++;
            }
        }

        private void InitializeField()
        {

            Controls.Clear();
            openedSlotsAmount = 0;
            CenterToScreen();
            secAmount = 0;
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += (s, e) =>
            {
                secAmount += 1;
                timeLbl.Text = $"{secAmount:00}";
            };
            ClientSize = new Size(35 * width, 35 * (height + 2));
            MaximumSize = new Size(35 * width + 16, 35 * (height + 2) + 39);
            MinimumSize = MaximumSize;
            //Header
            {
                backGrLbl = new Label();
                backGrLbl.BorderStyle = BorderStyle.Fixed3D;
                backGrLbl.Bounds = new Rectangle(0, 0, width * 35, 35 * 2);

                timeLbl = new Label();
                timeLbl.Font = new Font(FontFamily.GenericSerif, 25);
                timeLbl.Text = "000";
                timeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                timeLbl.Bounds = new Rectangle(width * 35 - 72, 3, 70, 65);
                timeLbl.ForeColor = Color.Red;
                timeLbl.BackColor = Color.Black;

                scoreLbl = new Label();
                scoreLbl.Font = new Font(FontFamily.GenericSerif, 25);
                scoreLbl.Text = $"{openedSlotsAmount}";
                scoreLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                scoreLbl.Bounds = new Rectangle(3, 3, 70, 65);
                scoreLbl.ForeColor = Color.Red;
                scoreLbl.BackColor = Color.Black;

                restartBtn = new Button();
                restartBtn.Bounds = new Rectangle(width * 35 / 2 - 34, 1, 68, 68);
                restartBtn.MouseClick += (s, e) => { Restart(); };
                restartBtn.MouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        Controls.Clear();
                        MaximumSize = new Size(600, 400);
                        MinimumSize = MaximumSize;
                        InitializeComponent();
                    }
                };
                Bitmap restBtnPict = (Bitmap)Bitmap.FromFile(@"Images\alive.png");
                restartBtn.Image = restBtnPict;

                Controls.Add(restartBtn);
                Controls.Add(scoreLbl);
                Controls.Add(timeLbl);
                Controls.Add(backGrLbl);
            }
            //Buttons
            {
                _buttons = new Button[height, width];
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        _buttons[i, j] = new Button();
                        _buttons[i, j].Font = new Font(FontFamily.GenericSerif, 14, FontStyle.Bold);
                        _buttons[i, j].Size = new Size(35, 35);
                        _buttons[i, j].Location = new Point(j * 35, (i + 2) * 35);
                        _buttons[i, j].MouseClick += ButtonWasPressed;
                        _buttons[i, j].MouseDown += PutFlag;

                        Controls.Add(_buttons[i, j]);
                    }
                }
            }
            if(firstEntranceState)
            {
                MessageBox.Show("RightClick smile button to resize field");
                firstEntranceState = false;
            }
       
        }

        private void button1_Click(object sender, EventArgs e)
        {

            width = (int)numericUpDown2.Value;
            height = (int)numericUpDown1.Value;
            if (width < 7 || height < 7)
            {
                MessageBox.Show("Size should be equal to 7x7 or bigger");
                return;
            }

            InitializeField();
        }

    }
}
