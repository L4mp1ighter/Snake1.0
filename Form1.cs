using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Snake1._0
{
    public partial class Form1 : Form
    {
        int ls = 3;
        int lsp = 21;
        int step = 21;
        int nx = 21;
        int ny = 210;
        int appx;
        int appy;
        char direction = 'W';
        Body[] snake = new Body[107];
        PictureBox target = new PictureBox();
        Label Score = new Label();
        bool Looser = false;
        bool Start = true;
        bool Reload = false;
        bool ShowSn = true;
        bool Am = true;
        int count = 0;
        Random rnd = new Random();
        int score = 0;
        //
        int col1 = 26;
        int col2 = 35;
        int col3 = 19;
        PictureBox[] left = new PictureBox[26];
        PictureBox[] right = new PictureBox[26];
        PictureBox[] top = new PictureBox[35];
        PictureBox[] bottom = new PictureBox[35];
        PictureBox[] obst1 = new PictureBox[19];
        PictureBox[] obst2 = new PictureBox[19];

        public Form1()
        {
            InitializeComponent();
            // Let's have some fun!
            Draw_Field();
            Snake();
            timer1.Start();
            
        }

        public void Set_And_Draw_Target(PictureBox p,int x, int y)
        {
            p.Image = new Bitmap(Properties.Resources.apple);
            p.BackColor = Color.Transparent;
            Draw_Brick(p, x, y);
        }

        public void Rand_appxy()
        {
            appx = rnd.Next(22, 713);
            if (appx <= 100) appy = rnd.Next(50, 545);
            if ((appx>=168) && (appx>=546))
            {
                if ((appx % 2)==0) appy = rnd.Next(22, 147);
                else if ((appx % 3) == 0) appy = rnd.Next(420, 545);
            }
            else appy = rnd.Next(189, 357);
        }

        public void Snake()
        {
            PictureBox p = new PictureBox();
            p.Image = new Bitmap(Properties.Resources.headW);
            p.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            snake[0] = new Body();
            snake[0].x = nx;
            snake[0].y = ny;
            snake[0].s = p;
            int i;
            for (i = 1; i < 101; i++)
            {
                PictureBox pic = new PictureBox();
                pic.Image = new Bitmap(Properties.Resources.snake);
                pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                pic.Visible = false;
                snake[i] = new Body();
                snake[i].x = nx;
                snake[i].y = ny + lsp * i;
                snake[i].s = pic;
            }
            Draw_Snake(snake, ls, lsp, nx, ny);
        }

        public bool check (int x, int y)
        {
            if ((x >= appx) && (y >= appy) && (x <= appx + 25) && (y <= appy + 25)) return true;
            else return false;
        }

        public void Hungry()
        {
            int x, y;
            x = snake[0].x;
            y = snake[0].y;
            //
            if (check(x, y))
            {
                Am = true;
                ls++;
                score++;
            }
            if (check(x + 21, y))
            {
                Am = true;
                ls++;
                score++;
            }
            if (check(x, y + 21))
            {
                Am = true;
                ls++;
                score++;
            }
            if (check(x + 21, y + 21))
            {
                Am = true;
                ls++;
                score++;
            }
        }

        public void NoPast()
        {
            int i;
            for (i = 0; i < 101; i++)
            {
                snake[i].x = 0;
                snake[i].y = 0;
            }
        }

        public void Set_Snake(Body[] snake, int ls,int lsp)
        {
            int i;
            for (i = 0; i < ls; i++)
            {
                snake[i].s.Visible = true;
                snake[i].x = nx;
                snake[i].y = ny + lsp * i;
                Draw_Brick(snake[i].s, snake[i].x, snake[i].y);
            }
        }

        public void Draw_Snake(Body[] snake, int ls, int lsp, int x, int y)
        {
            int i;
            for (i = ls-1; i > -1; --i)
            {
                snake[i].s.Visible = true;
                Draw_Brick(snake[i].s, snake[i].x, snake[i].y);
            }
        }

        public void Hide_Snake(Body[] snake, int ls, int lsp, int x, int y)
        {
            int i;
            for (i = ls - 1; i > -1; --i)
            {
                snake[i].s.Visible = false;
                Draw_Brick(snake[i].s, snake[i].x, snake[i].y);
            }
        }

        public void Redraw_Snake(Body[] snake, int ls, int step)
        {
            int i;
            for (i = ls-1; i > 0; i--)
            {
                snake[i].s.Visible = true;
                snake[i].x = snake[i - 1].x;
                snake[i].y = snake[i - 1].y;
                Draw_Brick(snake[i].s, snake[i].x, snake[i].y);
            }
            if (direction == 'W') snake[0].y = snake[0].y - step;
            if (direction == 'A') snake[0].x = snake[0].x - step;
            if (direction == 'S') snake[0].y = snake[0].y + step;
            if (direction == 'D') snake[0].x = snake[0].x + step;
            Draw_Brick(snake[0].s, snake[0].x, snake[0].y);
            // Auch!
            Auch(left, col1, 0, 0, 'A');
            Auch(right, col1, 755, 0, 'D');
            Auch(top, col2, 0, 0, 'W');
            Auch(bottom, col2, 0, 567, 'S');
            Auch(obst1, col3, 168, 168, 'W');
            Auch(obst2, col3, 168, 399, 'W');
            // If Looser == true
            if (Looser)
            {
                for (i = 0; i < ls - 1; i++)
                {
                    snake[i].x = snake[i + 1].x;
                    snake[i].y = snake[i + 1].y;
                    Draw_Brick(snake[i].s, snake[i].x, snake[i].y);
                }
            }
        }

        public void Draw_Brick(PictureBox pic, int x, int y)
        {
            pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pic.Location = new Point(x, y);
            this.Controls.Add(pic);
        }

        public void Draw_Wall(PictureBox[] pic1, int col, int x, int y, char index)
        {
            int i;
            for (i = 0; i < col; i++)
            {
                PictureBox pic = new PictureBox();
                if ((index == 'A') || (index == 'D')) pic.Location = new Point(x, y + ((i + 1) * 21));
                if ((index == 'W') || (index == 'S')) pic.Location = new Point(x + ((i + 1) * 21), y);
                if (index == 'W') pic.Image = new Bitmap(Properties.Resources.top);
                if (index == 'A') pic.Image = new Bitmap(Properties.Resources.left);
                if (index == 'S') pic.Image = new Bitmap(Properties.Resources.bottom);
                if (index == 'D') pic.Image = new Bitmap(Properties.Resources.right);
                pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                pic1[i] = pic;
                this.Controls.Add(pic1[i]);
            }
        }

        public void Auch(PictureBox[] pic1, int col, int x, int y, char index)
        {
            int i;
            for (i = 0; i < col; i++)
            { 
                if (((index == 'A') || (index == 'D')) && (snake[0].x == x) && (snake[0].y == y + ((i + 1) * 21))) Looser = true;
                if (((index == 'W') || (index == 'S')) && (snake[0].x == x + ((i + 1) * 21)) && (snake[0].y == y)) Looser = true;
            }
            // It's me!
            for(i = 1; i < col; i++)
            {
                if ((snake[0].x == snake[i].x) && (snake[0].y == snake[i].y)) Looser = true;
            }
        }

        public void Draw_Field()
        {
            // Draw left wall
            Draw_Wall(left, col1, 0, 0, 'A');
            // Draw right wall
            Draw_Wall(right, col1, 755, 0, 'D');
            // Draw top wall
            Draw_Wall(top, col2, 0, 0, 'W');
            // Draw bottom wall
            Draw_Wall(bottom, col2, 0, 567, 'S');
            // Draw obstacles
            Draw_Wall(obst1, col3, 168, 168, 'W');
            Draw_Wall(obst2, col3, 168, 399, 'W');
            // Draw angles
            PictureBox angle_t_r = new PictureBox();
            PictureBox angle_b_r = new PictureBox();
            PictureBox angle_t_l = new PictureBox();
            PictureBox angle_b_l = new PictureBox();
            angle_t_r.Image = new Bitmap(Properties.Resources.angle_t_r);
            angle_b_r.Image = new Bitmap(Properties.Resources.angle_b_r);
            angle_t_l.Image = new Bitmap(Properties.Resources.angle_t_l);
            angle_b_l.Image = new Bitmap(Properties.Resources.angle_b_l);
            Draw_Brick(angle_t_l, 0, 0);
            Draw_Brick(angle_t_r, 755, 0);
            Draw_Brick(angle_b_l, 0, 567);
            Draw_Brick(angle_b_r, 755, 567);
            // Draw ssscore
            Score.ForeColor = Color.Maroon;
            Score.Font = new Font(FontFamily.GenericSansSerif, 12.0F, FontStyle.Bold);
            Score.Top = 22;
            Score.Left = 22;
            Score.Text = "Score: " + Convert.ToString(score);
            this.Controls.Add(Score);
        }

        private void Timer1_Tick_1(object sender, EventArgs e)
        {

            if (((Start) || (Looser) || (Reload)) && (count < 14) && (!ShowSn))
            {
                Draw_Snake(snake, ls, lsp, nx, ny);
                ShowSn = true;
                count++;
            }
            else if (((Start) || (Looser) || (Reload)) && (count < 14) && (ShowSn))
            {
                Hide_Snake(snake, ls, lsp, nx, ny);
                ShowSn = false;
                count++;
            }
            else if ((count == 14) && (Looser))
            {
                Looser = false;
                Start = false;
                Reload = true;
                Hide_Snake(snake, ls, lsp, nx, ny);
                nx = 21;
                ny = 210;
                ls = 3;
                count = 0;
                snake[0].x = nx;
                snake[0].y = ny;
                direction = 'W';
                snake[0].s.Image = new Bitmap(Properties.Resources.headW);
                NoPast();
                Set_Snake(snake, ls, lsp);
                score = 0;
                
            }
            else if ((count == 14) && (Start))
            {
                Start = false;
                count = 0;
            }
            else if ((count == 14) && (Reload))
            {
                Reload = false;
                count = 0;
            }
            else if ((!Start) && (!Looser) && (!Reload)) Redraw_Snake(snake, ls, step);
            //
            if(Am)
            {
                Rand_appxy();
                Set_And_Draw_Target(target, appx, appy);
                Am = false;
            }
            Hungry();
            Score.Text = "Score: " + Convert.ToString(score);

            // WIN!!!
            if (score == 100)
            {
                Winner.Visible = true;
                timer1.Stop();
            }

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (((e.KeyData == Keys.W) || (e.KeyData == Keys.Up) && (!Looser) && (!Start)))
            {
                direction = 'W';
                snake[0].s.Image = new Bitmap(Properties.Resources.headW);
            }   
            if (((e.KeyData == Keys.A) || (e.KeyData == Keys.Left) && (!Looser) && (!Start))) 
            {
                direction = 'A';
                snake[0].s.Image = new Bitmap(Properties.Resources.headA);
            }
            if (((e.KeyData == Keys.D) || (e.KeyData == Keys.Right) && (!Looser) && (!Start)))
            {
                direction = 'D';
                snake[0].s.Image = new Bitmap(Properties.Resources.headD);
            }
            if (((e.KeyData == Keys.S) || (e.KeyData == Keys.Down) && (!Looser) && (!Start)))
            {
                direction = 'S';
                snake[0].s.Image = new Bitmap(Properties.Resources.headS);
            }
        }
    }

    public class Body
    {
        public PictureBox s = new PictureBox();
        public int x;
        public int y;
    }

}