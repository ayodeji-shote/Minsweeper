using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinesweeperGame
{
    public partial class Form1 : Form
    {

        Button[,] btn;// Create 2D array of buttons
        Random rnd = new Random(); //Random Number Generator
        int numberOfBombs;
        String playersName;
        Stopwatch timer = new Stopwatch();
        Button rules = new Button();



        public Form1()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            
            //Panel imagePanel = new Panel();
            //TextBox textBox1 = new TextBox();
            //imagePanel.Dock = DockStyle.Top;
            //imagePanel.BackgroundImage = Properties.Resources.menuImage;
            //imagePanel.BackgroundImageLayout = ImageLayout.Center;
            //imagePanel.Height = 400;

            //Button[] difficulties = new Button[3];
            //for (int i=0;i<3;i++)
            //{
            //    difficulties[i] = new Button();
            //    //difficulties[i].Dock = DockStyle.Bottom;

            //    difficulties[i].Width = 100;
            //    difficulties[i].Height = 50;
            //    difficulties[i].SetBounds(150 * i, 0, 100 , 50);

            //    this.Controls.Add(difficulties[i]);
            //    ;

            //}

            //this.CenterToScreen();
            //this.FormBorderStyle = FormBorderStyle.FixedDialog;

            //this.Controls.Add(imagePanel);
            //this.Controls.Add(groupBox1);

            //createGrid(32, 32);
            // this.FormBorderStyle = FormBorderStyle.FixedDialog;


        }

        void createGrid(int collumn, int row)
        {

            // Starts the timer for the game
            timer.Start();

            //sets how big the buttons of the grid are going to be
            this.Width = (collumn * 20) + 40;
            this.Height = (row * 20) + 120;

           
            rules.BackColor = Color.LightPink;
            rules.Text = "RULES";
            rules.Height = 50;
            rules.Dock = DockStyle.Bottom;
            rules.MouseClick += new MouseEventHandler(this.btnEvent_ruleClick);
            Controls.Add(rules);

            btn = new Button[collumn + 1, row + 1];// Create 2D array of buttons

            for (int x = 0; x < btn.GetLength(0); x++)         // Loop for x
            {
                for (int y = 0; y < btn.GetLength(1); y++)     // Loop for y
                {
                    btn[x, y] = new Button();
                    btn[x, y].SetBounds(20 * x , 20 * y, 20, 20);
                    btn[x, y].BackColor = Color.PowderBlue; // sets the back color of the buttons to powderBlue
                    btn[x, y].Tag = 0; //sets Tags to 0
                    btn[x, y].Name = x + "," + y; // sets the name to the x and y indexes of the button
                    btn[x, y].MouseDown += new MouseEventHandler(this.btnEvent_Click);
                    int randomMine = rnd.Next(0, btn.GetLength(1));

                    //setting up the border of the grid
                    if (x == 0 || x == btn.GetLength(0) - 1 || y == 0 || y == btn.GetLength(1) - 1)
                    {
                        btn[x, y].BackColor = Color.Black; 
                        btn[x, y].Tag = 10; // Tag of 10 for the border
                    }
                    else if (randomMine <= btn.GetLength(1) * 4 / 16)
                    {
                        //btn[x, y].BackColor = Color.Red;
                        btn[x, y].Tag = 9; // mines have a Tag of 9 
                        numberOfBombs++;
                    }
                   
                    Controls.Add(btn[x, y]);
                }
            }

            //sets the number of the buttons depending on how many mines there are around that specific button
            for (int x = 1; x < btn.GetLength(0) - 1; x++)         // Loop for x
            {
                for (int y = 1; y < btn.GetLength(1) - 1; y++)     // Loop for y
                {
                    if (Convert.ToInt32(btn[x, y].Tag) != 9)
                    {
                        int counter = 0;

                        if (Convert.ToInt32(btn[x - 1, y - 1].Tag) == 9) counter++;
                        if (Convert.ToInt32(btn[x - 1, y].Tag) == 9) counter++;
                        if (Convert.ToInt32(btn[x - 1, y + 1].Tag) == 9) counter++;
                        if (Convert.ToInt32(btn[x, y + 1].Tag) == 9) counter++;
                        if (Convert.ToInt32(btn[x + 1, y + 1].Tag) == 9) counter++;
                        if (Convert.ToInt32(btn[x + 1, y].Tag) == 9) counter++;
                        if (Convert.ToInt32(btn[x + 1, y - 1].Tag) == 9) counter++;
                        if (Convert.ToInt32(btn[x, y - 1].Tag) == 9) counter++;


                        btn[x, y].Tag = counter; // sets the number of mines around the buttonas a Tag
                        //if (counter != 0 ) btn[x, y].Text = Convert.ToString(btn[x, y].Tag);

                    }
                }
            }
            
        }


        void btnEvent_Click(object sender, MouseEventArgs e)
        {
           
            // if the user clicks the Left button on the Mouse
            if (e.Button == MouseButtons.Left)
            {
                //if the user clicks on a bomb 
                if (Convert.ToInt32(((Button)sender).Tag) == 9)
                {
                    
                    //looping through the grid
                    for (int x1 = 1; x1 < btn.GetLength(0)-1; x1++)         // Loop for x
                    {
                        for (int y1 = 1; y1 < btn.GetLength(1)-1; y1++)     // Loop for y
                        {

                            // correctly flagged mines by the user
                            if (btn[x1, y1].BackColor == Color.Green && Convert.ToInt32(btn[x1, y1].Tag) == 9)
                            {
                                btn[x1, y1].BackgroundImage = null;
                                btn[x1, y1].BackgroundImage = Properties.Resources.flag;
                            }

                            //missed mines
                            if (Convert.ToInt32(btn[x1, y1].Tag) == 9 && btn[x1, y1].BackColor != Color.Green)
                            {
                                btn[x1, y1].BackgroundImage = Properties.Resources.mine;
                            }

                            // incorrectly flaggged buttons
                            if (btn[x1, y1].BackColor == Color.Green && Convert.ToInt32(btn[x1, y1].Tag) != 9) {
                                btn[x1, y1].BackgroundImage = null;
                                btn[x1, y1].BackgroundImage = Properties.Resources.wrongFlag;
                            }

                        }
  
                    }

                    ((Button)sender).BackgroundImage = Properties.Resources.clickedMine;
                    MessageBox.Show("Bomb");
                    //brings the user back to the menu
                    backToMenu();
                    return;

                }

                else if (Convert.ToInt32(((Button)sender).Tag) != 11 && Convert.ToInt32(((Button)sender).Tag) != 10)
                {
                    ((Button)sender).BackColor = Color.HotPink;
                    ((Button)sender).BackgroundImage = null;
                    if (Convert.ToInt32(((Button)sender).Tag) != 0)
                    {
                        ((Button)sender).Text = Convert.ToString(((Button)sender).Tag);
                    }
                }

                

                //MessageBox.Show(Convert.ToString(((Button)sender).Name));
                string indexes = ((Button)sender).Name; //These lines of code are to get the array position of the button
                string[] splitIndex = indexes.Split(',');
                int x = Convert.ToInt32(splitIndex[0]);
                int y = Convert.ToInt32(splitIndex[1]);
                if (x == 0 || x == btn.GetLength(0) - 1 || y == 0 || y == btn.GetLength(1) - 1)
                {
                    return;
                }
                // if the button is from 1-8 display the number -> doesn't go to revealing
                if (btn[x, y].Text.Equals("1") || btn[x, y].Text.Equals("2") || btn[x, y].Text.Equals("3") || btn[x, y].Text.Equals("4") || btn[x, y].Text.Equals("5") || btn[x, y].Text.Equals("6") || btn[x, y].Text.Equals("7") || btn[x, y].Text.Equals("8")) return;
                reveal(x, y);
            }
                
            if (e.Button == MouseButtons.Right)
            {

                string indexes = ((Button)sender).Name; //These lines of code are to get the array position of the button
                string[] splitIndex = indexes.Split(',');
                int x = Convert.ToInt32(splitIndex[0]);
                int y = Convert.ToInt32(splitIndex[1]);
                if (x == 0 || x == btn.GetLength(0) - 1 || y == 0 || y == btn.GetLength(1) - 1)
                {
                    return;
                }

                if (((Button)sender).BackColor == Color.PowderBlue || ((Button)sender).BackColor == Color.Red)
                {
                    ((Button)sender).BackgroundImage = Properties.Resources.flag;
                    ((Button)sender).BackColor = Color.Green;
                }             
                else if (((Button)sender).BackColor == Color.Green)
                {
                    ((Button)sender).BackgroundImage = null;
                    ((Button)sender).BackColor = Color.PowderBlue;
                    if (Convert.ToInt32(((Button)sender).Tag) == 9 ) 
                    {
                        ((Button)sender).BackColor = Color.Red;
                    }
                }

                endCondition();

            }
        }

        bool endCondition()
        {
            Console.WriteLine("Number of mines: " + numberOfBombs);
            int NumberOfFlags = 0;
            int correctlyPlased = 0;
            bool won = false;
            for (int x = 1; x < btn.GetLength(0) - 1; x++)
                for (int y = 1; y < btn.GetLength(0) - 1; y++)
                {
                    if (btn[x, y].BackColor == Color.Green && Convert.ToInt32((btn[x, y]).Tag) == 9)
                    {
                        correctlyPlased++;
                    }
                    if (btn[x, y].BackColor == Color.Green) NumberOfFlags++;
                }

            Console.WriteLine("Number of correctly plased flags: " + correctlyPlased);
            Console.WriteLine("NumberOfFlags: " + NumberOfFlags);

            if (correctlyPlased == numberOfBombs && NumberOfFlags == numberOfBombs)
            {


                timer.Stop();

                TimeSpan timeTaken = timer.Elapsed;

                string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");

                MessageBox.Show("Congrats " + playersName + "! You won! \n " + foo);
                won = true;

                backToMenu();
                


            }

            return won;
        }

        void backToMenu()
        {
            InitializeComponent();

            for (int x1 = 0; x1 < btn.GetLength(0); x1++)         // Loop for x
            {
                for (int y1 = 0; y1 < btn.GetLength(1); y1++)     // Loop for y
                {
                    this.Controls.Remove(btn[x1, y1]);
                }
            }

            this.Controls.Remove(rules);

        }


            void reveal(int x, int y)
            {

                if (Convert.ToInt32(btn[x, y].Tag) == 1 || Convert.ToInt32(btn[x, y].Tag) == 2 || Convert.ToInt32(btn[x, y].Tag) == 3 || Convert.ToInt32(btn[x, y].Tag) == 4 || Convert.ToInt32(btn[x, y].Tag) == 5 || Convert.ToInt32(btn[x, y].Tag) == 6 || Convert.ToInt32(btn[x, y].Tag) == 7 || Convert.ToInt32(btn[x, y].Tag) == 8)
                {
                    btn[x, y].Text = Convert.ToString(btn[x, y].Tag);
                    btn[x, y].BackColor = Color.HotPink;
                    return;
                }

                Console.WriteLine(x + " " + y);
                if (Convert.ToInt32(btn[x, y].Tag) != 9 && Convert.ToInt32(btn[x, y].Tag) != 10 && Convert.ToInt32(btn[x, y].Tag) != 11)
                {
                    if (Convert.ToInt32(btn[x, y].Tag) == 0)
                    {
                        btn[x, y].BackColor = Color.HotPink;
                        btn[x, y].Tag = 11;
                    }
                    else
                    {
                        btn[x, y].Text = Convert.ToString(btn[x, y].Tag);
                        btn[x, y].BackColor = Color.HotPink;
                    }
                }

                // ------------------------------------------ START -------------------------------------------

                if (Convert.ToInt32(btn[x - 1, y - 1].Tag) == 0)
                {
                    btn[x - 1, y - 1].BackColor = Color.HotPink;
                    btn[x - 1, y - 1].Tag = 11;
                    reveal(x - 1, y - 1);
                }
                else if (Convert.ToInt32(btn[x - 1, y - 1].Tag) != 11 && Convert.ToInt32(btn[x - 1, y - 1].Tag) != 9 && Convert.ToInt32(btn[x - 1, y - 1].Tag) != 10)
                {
                    btn[x - 1, y - 1].Text = Convert.ToString(btn[x - 1, y - 1].Tag);
                    btn[x - 1, y - 1].BackColor = Color.HotPink;
                }
                //-----------------------------------------------------------------

                if (Convert.ToInt32(btn[x - 1, y].Tag) == 0)
                {
                    btn[x - 1, y].BackColor = Color.HotPink;
                    btn[x - 1, y].Tag = 11;
                    reveal(x - 1, y);
                }
                else if (Convert.ToInt32(btn[x - 1, y].Tag) != 11 && Convert.ToInt32(btn[x - 1, y].Tag) != 9 && Convert.ToInt32(btn[x - 1, y].Tag) != 10)
                {
                    btn[x - 1, y].Text = Convert.ToString(btn[x - 1, y].Tag);
                    btn[x - 1, y].BackColor = Color.HotPink;
                }

                //------------------------------------------------------------------

                if (Convert.ToInt32(btn[x - 1, y + 1].Tag) == 0)
                {
                    btn[x - 1, y + 1].BackColor = Color.HotPink;
                    reveal(x - 1, y + 1);
                    btn[x - 1, y + 1].Tag = 11;
                }
                else if (Convert.ToInt32(btn[x - 1, y + 1].Tag) != 11 && Convert.ToInt32(btn[x - 1, y + 1].Tag) != 9 && Convert.ToInt32(btn[x - 1, y + 1].Tag) != 10)
                {
                    btn[x - 1, y + 1].Text = Convert.ToString(btn[x - 1, y + 1].Tag);
                    btn[x - 1, y + 1].BackColor = Color.HotPink;
                }

                //-----------------------------------------------------------------

                if (Convert.ToInt32(btn[x, y + 1].Tag) == 0)
                {
                    btn[x, y + 1].BackColor = Color.HotPink;
                    reveal(x, y + 1);
                    btn[x, y + 1].Tag = 11;
                }
                else if (Convert.ToInt32(btn[x, y + 1].Tag) != 11 && Convert.ToInt32(btn[x, y + 1].Tag) != 9 && Convert.ToInt32(btn[x, y + 1].Tag) != 10)
                {
                    btn[x, y + 1].Text = Convert.ToString(btn[x, y + 1].Tag);
                    btn[x, y + 1].BackColor = Color.HotPink;
                }

                //-----------------------------------------------------------------

                if (Convert.ToInt32(btn[x + 1, y + 1].Tag) == 0)
                {
                    btn[x + 1, y + 1].BackColor = Color.HotPink;
                    reveal(x + 1, y + 1);
                    btn[x + 1, y + 1].Tag = 11;
                }
                else if (Convert.ToInt32(btn[x + 1, y + 1].Tag) != 11 && Convert.ToInt32(btn[x + 1, y + 1].Tag) != 9 && Convert.ToInt32(btn[x + 1, y + 1].Tag) != 10)
                {
                    btn[x + 1, y + 1].Text = Convert.ToString(btn[x + 1, y + 1].Tag);
                    btn[x + 1, y + 1].BackColor = Color.HotPink;
                }

                //-----------------------------------------------------------------

                if (Convert.ToInt32(btn[x + 1, y].Tag) == 0)
                {
                    btn[x + 1, y].BackColor = Color.HotPink;
                    reveal(x + 1, y);
                    btn[x + 1, y].Tag = 11;
                }
                else if (Convert.ToInt32(btn[x + 1, y].Tag) != 11 && Convert.ToInt32(btn[x + 1, y].Tag) != 9 && Convert.ToInt32(btn[x + 1, y].Tag) != 10)
                {
                    btn[x + 1, y].Text = Convert.ToString(btn[x + 1, y].Tag);
                    btn[x + 1, y].BackColor = Color.HotPink;
                }

                //-----------------------------------------------------------------

                if (Convert.ToInt32(btn[x + 1, y - 1].Tag) == 0)
                {
                    btn[x + 1, y - 1].BackColor = Color.HotPink;
                    reveal(x + 1, y - 1);
                    btn[x + 1, y - 1].Tag = 11;
                }
                else if (Convert.ToInt32(btn[x + 1, y - 1].Tag) != 11 && Convert.ToInt32(btn[x + 1, y - 1].Tag) != 9 && Convert.ToInt32(btn[x + 1, y - 1].Tag) != 10)
                {
                    btn[x + 1, y - 1].Text = Convert.ToString(btn[x + 1, y - 1].Tag);
                    btn[x + 1, y - 1].BackColor = Color.HotPink;
                }

                //-----------------------------------------------------------------

                if (Convert.ToInt32(btn[x, y - 1].Tag) == 0)
                {
                    btn[x, y - 1].BackColor = Color.HotPink;
                    reveal(x, y - 1);
                    btn[x, y - 1].Tag = 11;
                }
                else if (Convert.ToInt32(btn[x, y - 1].Tag) != 11 && Convert.ToInt32(btn[x, y - 1].Tag) != 9 && Convert.ToInt32(btn[x, y - 1].Tag) != 10)
                {
                    btn[x, y - 1].Text = Convert.ToString(btn[x, y - 1].Tag);
                    btn[x, y - 1].BackColor = Color.HotPink;
                }

                //-----------------------------------------------------------------

            }



        private void btnEvent_ruleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Right Click any (non-black) button to start playing \nLeft Click any (non-black) button to place a flag \nFlags should be placed where you think mines are \nTo win simply place a flag on every mine, and the game will end. \nLeft clicking the a mine will cause the game to end, and reveal all other mines, as well as incorrectly placed flags.");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            playersName = textBox1.Text;
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            resetForm();
            createGrid(9, 9);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            resetForm();
            createGrid(16, 16);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            resetForm();
            createGrid(32, 32);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
           
        }

        public void resetForm ()
        {
            
            this.Controls.Remove(button1);
            this.Controls.Remove(button2);
            this.Controls.Remove(button3);
            this.Controls.Remove(pictureBox1);
            this.Controls.Remove(label1);
            this.Controls.Remove(textBox1);

        }

       
    }
}