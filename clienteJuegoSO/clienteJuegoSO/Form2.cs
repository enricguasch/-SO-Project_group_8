using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clienteJuegoSO
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public class Card
        {
            string img;   //Image URL for the card
            string title; //Instructions of the card
            int id;
        }

        public class Position
        {
            int X;
            int Y;
            int id;
            bool used;    //Wether the position is used by a tile object

            public Position(int X, int Y, int id)
            {
                this.X = X;
                this.Y = Y;
                this.id = id;
            }
        }
        

        public class Tile
        {
            Position pos; //Every tile is located in a predefined position object
            Card card;    //Every tile has an assigned card object
            int status;   //Wether the card is Color, Sunk or Blank
            int played;   //If a player is on the card
            int id;       //Every tile has a unique id only for active positions
        }


        public class Board
        {
            Tile[] tiles;
            int id;

            public Board(int id)
            {
                this.id = id;
            }
        }

        Board board = new Board(1);

        public void GenerateBoard()
        {
            
        }

        int[] grid = new int[24];

        private void Form2_Load(object sender, EventArgs e)
        {
            /*
            PictureBox pic1 = new PictureBox
            {
                Image = Image.FromFile("a.jpg"),
                Size = MaximumSize,
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            */

            int numTiles = 24;


            //Generates an array of PictureBoxes for the board tiles
            PictureBox[] tiles = new PictureBox[numTiles];
            
            //Fills every PictureBox with an image of each tile
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new PictureBox(); //Pointer to new PictureBox instance
                tiles[i].Image = Image.FromFile($"app/tiles/a{i+1}.jpg"); //Path of file
                tiles[i].Size = MaximumSize; //Maximum size
                tiles[i].Dock = DockStyle.Fill; //fill max space
                tiles[i].SizeMode = PictureBoxSizeMode.StretchImage;
            }

            //Randomize board tile order
            Random random = new Random();
            tiles = tiles.OrderBy(x => random.Next()).ToArray();

            //Define TableLayoutPanel usable tile indexes
            int[] columns = { 2, 3, 1, 2, 3, 4, 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5, 1, 2, 3, 4, 2, 3 };
            int[] rows =    { 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5 };

            

            for (int i = 0; i < numTiles; i++)
            {
                grid[i] = Convert.ToInt32(string.Format("{0}{1}", rows[i],columns[i]));
            }

            //Fill TableLayoutPanel with tiles
            for (int i = 0; i < numTiles; i++)
            {
                tableLayoutPanel1.Controls.Add(tiles[i], columns[i], rows[i]);
            }
            
            
            
            //tableLayoutPanel1.Controls.Add(pic1, columns[0], rows[0]);
            
            
            
            Image bg = Image.FromFile($"app/bg/sand.png");
            tableLayoutPanel1.BackgroundImage = bg;
            tableLayoutPanel1.BackgroundImageLayout = ImageLayout.Tile;

            
            foreach (PictureBox space in this.tableLayoutPanel1.Controls)
            {
                space.MouseClick += new MouseEventHandler(clickOnSpace);
                //space.MouseHover += new EventHandler(hoverOnSpace);
            }
            

        }

        
        public void hoverOnSpace(object sender, EventArgs e)
        {
            MessageBox.Show("Cell chosen: (" +
                             tableLayoutPanel1.GetRow((PictureBox)sender) + ", " +
                             tableLayoutPanel1.GetColumn((PictureBox)sender) + ")");
        }
        public void clickOnSpace(object sender, MouseEventArgs e)
        {
            int row = tableLayoutPanel1.GetRow((PictureBox)sender);
            int col = tableLayoutPanel1.GetColumn((PictureBox)sender);
            int id = Convert.ToInt32(string.Format("{0}{1}", row, col));
            MessageBox.Show("ID: " + Array.IndexOf(grid, id));
            //MessageBox.Show("Cell chosen: (" + row + ", " + col + ")");
            
        }
        
        }
    }
