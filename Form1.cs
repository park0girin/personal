using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 앙상부루마블.Properties;

namespace 앙상부루마블
{
    public partial class Form1 : Form
    {
        public int w;
        public int h;
        public Form1()
        {
            InitializeComponent();
            BackColor = Color.DarkSalmon;
            this.Size = new Size (1556,884);
            int w = this.Width;
            int h = this.Height;
            Console.WriteLine(w+"X"+h);
            //전체화면이 1556X884 ???
            this.Text = "앙상부루마블";
            PictureBox Game_Area = new PictureBox();
            // 픽쳐박스의 속성을 설정합니다.
            Game_Area.Name = "game_area"; // 게임판 픽쳐박스 생성
            
            Game_Area.Size = new Size(((w * 85 / 100)), ((h * 90/ 100))); // 게임판 전체화면 비율에 맞추기 (1322X795)
            //Console.WriteLine(((w*85 / 100)) + "X"+ ((h *90 / 100)));


            Game_Area.SizeMode = PictureBoxSizeMode.Zoom;
            Game_Area.Location = new Point(0, 0); // 위치 설정
            Game_Area.Image = Resources.게임판; // 로컬폴더의 이미지 로드
            Game_Area.BackColor = Color.BlanchedAlmond;
           // Game_Area.Dock = DockStyle.Top;

            // 폼에 픽쳐박스를 추가합니다.
            this.Controls.Add(Game_Area);
            
            Game_Area.Click += new EventHandler(Game_Area_Click);
            /*
            PictureBox nullbox = new PictureBox();
            nullbox.Name = "null";
            nullbox.Size = new Size(1920, 180);
            nullbox.Location = new Point(0, 851);
            nullbox.BackColor = Color.IndianRed;
            this.Controls.Add(nullbox);
            */
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Game_Area_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Game_Area_Click");
           /* this.Size = new Size(((w* 69 / 100)), ((h * 74 / 100)));
           Console.WriteLine(w + "X" + h);*/ // 게임판 전체화면 비율에 맞추기 (실패)
        }
    }
}
