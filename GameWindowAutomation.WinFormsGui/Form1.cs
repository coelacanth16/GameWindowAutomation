using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameWindowAutomation.Core;

namespace GameWindowAutomation.WinFormsGui
{
    public partial class Form1 : Form
    {
        //public static KeyboardListener.KeyboardListener keyListener = new KeyboardListener.KeyboardListener();
        IProgress<string> textBoxProgress;    

        public Form1()
        {
            InitializeComponent();
            //keyListener.AllKeys = false;
            //keyListener.AddKeycode(Keycode.VK_CONTROL);
            //keyListener.AddKeycode(Keycode.VK_SHIFT);
            //keyListener.AddKeycode(Keycode.VK_ALT);
            //keyListener.AddKeycode(Keycode.VK_C);
            //keyListener.AddKeyCombo(new[] { Keycode.VK_CONTROL, Keycode.VK_SHIFT, Keycode.VK_C }).KeyComboPressed += CaptureWindowComboPressed;
            //keyListener.AddKeyCombo(new[] { Keycode.VK_CONTROL, Keycode.VK_ALT, Keycode.VK_C }).KeyComboPressed += CaptureStateComboPressed;
            //keyListener.KeyPressed += KeyListener_KeyPressed;
            //keyListener.KeyComboPressed += KeyListener_KeyComboPressed;
            //keyListener.Listen();
            //KeyPressedStateGrid.SelectedObject = CurrentKeyState;
            textBoxProgress = new Progress<string>((str) => keyPressedLogTB.AppendText(str + Environment.NewLine));
            //keyListener.KeyPressed += keycode => textBoxProgress?.Report(keycode.ToString());
        }

        private void CaptureWindowComboPressed(int[] keycodes)
        {
            //KeyComboPressed(keycodes);
            Point point = new Point();
            if (Utilities.GetCursorPos(ref point))
            {
                var inst = GameInstanceWindow.GetWindowAtMouseCursor(point);
                pictureBox1.Image = inst?.DoScan();
            }
                //Task.Run(() =>
                //{
                //    Point point = new Point();
                //    if (Utilities.GetCursorPos(ref point))
                //    {
                //        var inst = GameInstanceWindow.GetWindowAtMouseCursor(point);
                //        var newForm = new Form();
                //        var picBox = new PictureBox();
                //        picBox.Dock = DockStyle.Fill;
                //        picBox.Image = inst.DoScan();
                //        picBox.SizeMode = PictureBoxSizeMode.AutoSize;
                //        newForm.Controls.Add(picBox);
                //        newForm.Show();
                //    }
                //});
        }

        //private void CaptureStateComboPressed(Keycode[] keycodes)
        //{
        //    KeyComboPressed(keycodes);
        //}


        //private void KeyComboPressed(Keycode[] keyCombo)
        //{
        //    textBoxProgress?.Report(string.Join("+", keyCombo.Select(keycode => keycode.ToString().Replace("VK_", string.Empty))));
        //}


        //private void KeyListener_KeyPressed(Keycode keycode)
        //{
        //    textBoxProgress?.Report(keycode.ToString().Replace("VK_", string.Empty));
        //}

        //private static async Task CaptureWindow()
        //{

        //}

        //private static async Task CaptureState()
        //{

        //}

        private void timer1_Tick(object sender, EventArgs e)
        {
            Point cursor = new Point();
            Utilities.GetCursorPos(ref cursor);
            xLbl.Text = $"X: {cursor.X}";
            yLbl.Text = $"Y: {cursor.Y}";
            //var c = GameInstanceWindow.GetColorAt(cursor);
            //pictureBox1.BackColor = c;
            var img = Utilities.GetImageAt(cursor, new Point(cursor.X + 30, cursor.Y + 30));
            pictureBox1.Image = img;
            pictureBox1.Refresh();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void endBtn_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }
}
