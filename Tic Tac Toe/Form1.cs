using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections; // ArrayList
using System.IO; // streams

namespace Tic_Tac_Toe
{
    public partial class Form1 : Form
    {
        class Play // Move used by AI
        {
            int m;
            string outcome;
            public Play(int Position, string Outcome)
            {
                m = Position;
                outcome = Outcome;
            }
            public int Position
            {
                get { return m; }
            }
            public string Outcome
            {
                get { return outcome; }
            }
            public override string ToString()
            {
                return m.ToString() + ", " + outcome;
            }
        }

        string Game;
        float X1, X2, X3, X4;
        float Y1, Y2, Y3, Y4;
        ArrayList Library;
        ArrayList Moves;

        public Form1()
        {
            InitializeComponent();
            Moves = new ArrayList();
            this.Text = "Tic Tac Toe";
            Game = "---------";
            label1.Text = Game;
            DrawGame(pictureBox1.Width);
            // Load library
            Library = new ArrayList();
            StreamReader sr = new StreamReader("library.txt");
            string Line = sr.ReadLine();
            while (Line != null)
            {
                Library.Add(Line);
                Line = sr.ReadLine();
            }
            sr.Close();
        }

        void DrawGame(int Side)
        {
            Bitmap Display = new Bitmap(Side, Side);
            Graphics g = Graphics.FromImage(Display);
            SolidBrush BackgroundBrush = new SolidBrush(Color.White);
            Pen GridPen = new Pen(Color.Black);
            g.FillRectangle(BackgroundBrush, 0, 0, Display.Width, Display.Height);
            X1 = 0.05f * Side;
            X2 = 0.35f * Side;
            X3 = 0.65f * Side;
            X4 = 0.95f * Side;
            Y1 = 0.05f * Side;
            Y2 = 0.35f * Side;
            Y3 = 0.65f * Side;
            Y4 = 0.95f * Side;
            g.DrawLine(GridPen, X2, Y1, X2, Y4);
            g.DrawLine(GridPen, X3, Y1, X3, Y4);
            g.DrawLine(GridPen, X1, Y2, X4, Y2);
            g.DrawLine(GridPen, X1, Y3, X4, Y3);

            for (int square = 0; square < 9; square++)
            {
                if (Game[square] != '-')
                {
                    // Compute row and column
                    int r = square / 3;
                    int c = square % 3;
                    // Create point for upper-left corner of drawing.
                    float x = X1 + c * (X3 - X2) + 10;
                    float y = Y1 + r * (Y3 - Y2) + 10;
                    String drawString = "";
                    SolidBrush CBrush;
                    if (Game[square] == 'X')
                    {
                        CBrush = new SolidBrush(Color.Purple);
                        drawString = "X";
                    }
                    else // must be 'O'
                    {
                        CBrush = new SolidBrush(Color.Green);
                        drawString = "O";
                    }
                    // Create font and brush
                    Font drawFont = new Font("Arial", 16);
                    // Set format of string
                    StringFormat drawFormat = new StringFormat();
                    // Draw string to screen
                    g.DrawString(drawString, drawFont, CBrush, x, y, drawFormat);
                } // end if
            } // end for
            pictureBox1.Image = Display;
            // Check and see if game is over
            if (WonGame(Game, 'X')) GameOver('X');
            if (WonGame(Game, 'O')) GameOver('O');
            if (CatGame(Game)) GameOver('c');
        }

        

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            label3.Text = "(" + e.X + " , " + e.Y + ")";
            label2.Text = e.Button.ToString(); // Left or Right
            // Determine which square click was in
            int s = 0;
            if (e.X < X2)
            {
                if (e.Y < Y2)
                    s = 0; // top left square
                else
                {
                    if (e.Y < Y3)
                        s = 3; // middle left square
                    else
                        s = 6; // bottom left square
                }
            }
            else
            {
                if (e.X < X3)
                {
                    if (e.Y < Y2)
                        s = 1; // top middle square
                    else
                    {
                        if (e.Y < Y3)
                            s = 4; // middle square
                        else
                            s = 7; // bottom middle square
                    }
                }
                else
                {
                    if (e.Y < Y2)
                        s = 2; // top right square
                    else
                    {
                        if (e.Y < Y3)
                            s = 5; // middle right square
                        else
                            s = 8; // bottom right square
                    }
                }
            }
            label2.Text = s.ToString();
            // see if square is open
            if (Game[s] == '-') // if square is open then move (put X) in that square
            {
                Game = Game.Substring(0, s) + "X" + Game.Substring(s + 1);
                DrawGame(pictureBox1.Width);
            }
        } // end method pictureBox1_MouseDown



        bool WonGame(string GameState, char C)
        {
            if ((GameState[0] == C) && (GameState[1] == C) && (GameState[2] == C)) return true; // top row
            if ((GameState[3] == C) && (GameState[4] == C) && (GameState[5] == C)) return true; // middle row
            if ((GameState[6] == C) && (GameState[7] == C) && (GameState[8] == C)) return true; // bottom row
            if ((GameState[0] == C) && (GameState[3] == C) && (GameState[6] == C)) return true; // left column
            if ((GameState[1] == C) && (GameState[4] == C) && (GameState[7] == C)) return true; // middle column
            if ((GameState[2] == C) && (GameState[5] == C) && (GameState[8] == C)) return true; // right column
            if ((GameState[0] == C) && (GameState[4] == C) && (GameState[8] == C)) return true; // main diagonal
            if ((GameState[2] == C) && (GameState[4] == C) && (GameState[6] == C)) return true; // off diagonal
            return false;
        }

        bool CatGame(string GameState)
        {
            int n = 0;
            for (int i = 0; i < 9; i++)
                if (GameState[i] == '-') n++;
            if (n > 0) return false;
            if (WonGame(GameState, 'X')) return false;
            if (WonGame(GameState, 'O')) return false;
            return true;
        }

        void GameOver(char Winner)
        {
            string Caption = "Cat game";
            if (Winner == 'X')
            {
                Caption = "Player wins";
                for (int i = 0; i < 9; i++)
                {
                    if (Game[i] == 'X')
                    {
                        Library.Add(Game.Substring(0, i) + '-' + Game.Substring(i + 1) + " L");
                    }
                }
            }
            if (Winner == 'O') Caption = "Computer wins";
            MessageBox.Show("Game over", Caption);
            Game = "---------"; // reset game state
        }



        bool Better(string New, string Old) // make strict inequality (true iff New > Old)
        {
            if ((Old == "L") && (New != "L")) return true;
            if ((Old == "C") && (New != "C") && (New != "L")) return true;
            if ((Old == "U") && (New != "L") && (New != "C") && (New != "U")) return true;
            if (Old == "W") return false;
            return false;
        }



        // Computer move
        private void button2_Click(object sender, EventArgs e)
        {
            Moves = new ArrayList();
            for (int i=0; i<9; i++)
            {
                if (Game[i] == '-') // possible move for '0'
                {
                    string Temp = Game.Substring(0, i) + 'O' + Game.Substring(i + 1);
                    bool Found = false;
                    foreach (string State in Library)
                    {
                        if (Temp == State.Substring(0, 9)) // match found in knowledge database
                        {
                            Moves.Add(new Play(i, State.Substring(10, 1)));
                            Found = true;
                        }
                    } // end foreach
                    if (!Found)
                    {
                        Moves.Add(new Play(i, "U"));
                    }
                }
            } // end for

            // Show moves
            listBox1.Items.Clear();
            foreach (Play move in Moves)
                listBox1.Items.Add(move);

            // Check if all plays are not unknown - if so, then add prior moves to library
            bool AllOutcomesKnown = true;
            foreach (Play p in Moves)
            {
                if (p.Outcome == "U") AllOutcomesKnown = false;
            }
            if (AllOutcomesKnown)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (Game[i] == 'X')
                    {
                        Library.Add(Game.Substring(0, i) + '-' + Game.Substring(i + 1) + " L");
                    }
                }
            }

            // Choose next "best" move in order
            Play m = (Play)Moves[0];
            foreach (Play p in Moves)
            {
                if (Better(p.Outcome, m.Outcome)) m = p; // p > m
            }
            Game = Game.Substring(0, m.Position) + 'O' + Game.Substring(m.Position + 1);
            DrawGame(pictureBox1.Width);
        } // end method button2_Click



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StreamWriter sw = new StreamWriter("library.txt");
            foreach (string s in Library)
                sw.WriteLine(s);
            sw.Close();
        } 

    }
}
