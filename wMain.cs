using H1Z1Bot.IO;
using H1Z1Bot.WinAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsInput;

namespace H1Z1Bot
{
    public partial class wMain : Form
    {
        private static readonly int COORD_X = 1444;
        private static readonly int COORD_Y = 719;
        private bool[] state = new bool[] { false, false, false, false, false };
        private bool stop = false;
        private InputSimulator iosim = new InputSimulator();
        private Random rand = new Random();

        public wMain()
        {
            InitializeComponent();
            this.Icon = H1Z1Bot.Properties.Resources.icon;

            this.lConnection.Text = "Desconectado";

            HotkeyManager.Add(new HotkeyHandler(User32.VirtualKey.VK_F10, User32.ModifierKeys.None, OnF10Pressed));
            HotkeyManager.Add(new HotkeyHandler(User32.VirtualKey.VK_F11, User32.ModifierKeys.None, OnF11Pressed));
        }

        private void OnF10Pressed(User32.VirtualKey key, User32.ModifierKeys modkey)
        {
            //Esto en su lugar es una petición al servidor
            uint[] codes = new uint[] {0000,1112,1113,1114,1115};

            for(int j = 0; j < codes.Length; j++ ){
                SendCode(codes[j]);
                state[j] = true;
                if (stop) break;
            }

            if (!stop)
            {
                //Confirma al server que ha indroducido el set de códigos
                SendCode(9999);
            }
            else
            {
                //Confirma al server hasta donde haya llegado
                SendCode(0000);
            }
            
        }

        private void OnF11Pressed(User32.VirtualKey key, User32.ModifierKeys modkey)
        {
            MessageBox.Show("F11 pulsado");
            stop = true;
        }

        private void SendCode(uint code)
        {
            iosim
                .Keyboard
                .KeyPress(WindowsInput.Native.VirtualKeyCode.VK_E)
                .Sleep(rand.Next(1000, 2000))
                .TextEntry(code.ToString("D4"))
                .Sleep(rand.Next(1000, 2000))
                .Mouse
                .MoveMouseTo(COORD_X, COORD_Y)
                .LeftButtonClick()
                .Sleep(rand.Next(1000, 2000));
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
