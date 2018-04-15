using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace HitAndBlow
{

    public partial class Form1 : Form
    {
        Stopwatch sw = new Stopwatch();
        public int[] Answer;
        int count = 0;

        public Form1()
        {
            InitializeComponent();
            Answer = new int[4];
            Check_Button.Visible = false;
            label1.Focus();
            this.KeyPreview = true;
        }
        

        private void Number_Button_Click(object sender, EventArgs e)
        {
            //4桁までしか打てなくする
            if (label1.Text.Length < 4)
            {
                label1.Text = label1.Text + ((Button)sender).Text;
                //二回以上打てなくする
                ((Button)sender).Visible = false;
            }
            label1.Focus();
        }


        //Clearボタンを押すと打ち直せる
        private void Clear_Button_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            button5.Visible = true;
            button6.Visible = true;
            button7.Visible = true;
            button8.Visible = true;
            button9.Visible = true;
            label1.Focus();
        }

        //スタートボタンで乱数作成
        private void Start_Button_Click(object sender, EventArgs e)
        {
            sw.Restart();
            count = 0;
            Check_Button.Visible = true;
            //一回真っ白に戻す
            label3.Text = "";
            label2.Text = "H  B";
            label1.Text = "";
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            button5.Visible = true;
            button6.Visible = true;
            button7.Visible = true;
            button8.Visible = true;
            button9.Visible = true;

            Random rnd = new Random();

            for (int i = 0; i < 4; i++)
            {
                //1~9の数字を入れる
                Answer[i] = rnd.Next(9) + 1;
                for (int j = i - 1; 0 <= j; j--)
                {
                    //その前までの数値のどれかとかぶっていたら数値代入からやり直し
                    if (Answer[j] == Answer[i])
                    {
                        i--;
                        break;
                    }
                }
            }
            Start_Button.Text = "Restart";
            label1.Focus();
        }

        //Checkボタンを押すと答えと照らし合わせる
        private void Check_Button_Click(object sender, EventArgs e)
        {
            if (label1.Text.Length == 4)
            {
                int hit = 0;
                int blow = 0;
                int[] think = new int[4];
                int work = int.Parse(label1.Text);
                int c = 1000;
                for (int a = 0; a < 4; a++)
                {
                    think[a] = work / c;
                    work = work % c;
                    c = c / 10;
                }
                for (int a = 0; a < 4; a++)
                {
                    if (think[a] == Answer[a])
                    {
                        hit += 1;
                    }

                    else
                        for (int b = 0; b < 4; b++)
                        {
                            if (think[a] == Answer[b])
                            {
                                blow += 1;
                            }
                        }
                    label2.Text = hit + "H" + blow + "B";
                    if (a == 3)
                    {
                        label3.Text = label3.Text + label1.Text + "　" + label2.Text + "\n";
                        count += 1;
                    }



                    //4H0Bになった時にお祝いメッセージを出す
                    if (hit == 4)
                    {
                        TimeSpan StandardTime;
                        int StandardCount;
                        sw.Stop();
                        TimeSpan span = sw.Elapsed;
                        StandardCount = Properties.Settings.Default.boxCount;
                        StandardTime = Properties.Settings.Default.boxTime;

                        if (StandardCount > count)
                        {
                            Properties.Settings.Default.boxCount = count;
                            Properties.Settings.Default.Save();
                            Properties.Settings.Default.boxTime = span;
                            Properties.Settings.Default.Save();
                            MessageBox.Show("おめでとうございます！新記録です！\nあなたのタイムは" + span.ToString("hh':'mm':'ss") + "で、" + count + "回目で当てました");
                        }
                        else if (StandardCount == count)
                        {
                            Properties.Settings.Default.boxCount = count;
                            Properties.Settings.Default.Save();
                            if (StandardTime > span)
                            {
                                Properties.Settings.Default.boxTime = span;
                                Properties.Settings.Default.Save();
                                MessageBox.Show("おめでとうございます！新記録です！\nあなたのタイムは" + span.ToString("hh':'mm':'ss") + "で、" + count + "回目で当てました");
                            }
                            else
                            {
                                MessageBox.Show("おめでとうございます！\nあなたのタイムは" + span.ToString("hh':'mm':'ss") + "で、" + count + "回目で当てました");
                            }
                        }
                        
                        else
                        {
                            MessageBox.Show("おめでとうございます！\nあなたのタイムは" + span.ToString("hh':'mm':'ss") + "で、" + count + "回目で当てました");
                        }
 
                        Check_Button.Visible = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("4つの数字を入れてください", "エラー"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
            }
            label1.Focus();

        }

        private Button Get_Button_Object(int Number)
        {
            switch (Number)
            {
                case 1:
                    return button1;
                case 2:
                    return button2;
                case 3:
                    return button3;
                case 4:
                    return button4;
                case 5:
                    return button5;
                case 6:
                    return button6;
                case 7:
                    return button7;
                case 8:
                    return button8;
                case 9:
                    return button9;
                default:
                    return null;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.KeyCode);
            int keyValue;
            if ((e.KeyValue >= 0x31) && (e.KeyValue <= 0x39))
            {
                keyValue = e.KeyValue - 0x30;
                if (label1.Text.Length < 4)
                {
                    Button button = Get_Button_Object(keyValue);
                    if (button.Visible == true)
                    {
                        label1.Text = label1.Text + keyValue.ToString();

                        if (button != null)
                        {
                            button.Visible = false;
                        }
                        label1.Focus();
                    }
                }
            }
        }

        //メモボタンをクリックしたときの処理
        private void Color_Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == Color.White)
            {
                ((Button)sender).BackColor = Color.DarkGray;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
            }
            label1.Focus();
        }


    }
}

       

