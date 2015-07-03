using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using H1Z1Bot.WinAPI;

namespace H1Z1Bot.IO
{
    public class HotkeyHandler
    {
        public delegate void HotkeyAction(User32.VirtualKey key, User32.ModifierKeys modkey);

        public readonly User32.VirtualKey Key;
        public readonly User32.ModifierKeys ModKey;
        public readonly HotkeyAction Action;

        public HotkeyHandler(User32.VirtualKey key, HotkeyAction action) : this(key, User32.ModifierKeys.None, action) { }

        public HotkeyHandler(User32.VirtualKey key, User32.ModifierKeys modkey, HotkeyAction action)
        {
            this.Key = key;
            this.ModKey = modkey;
            this.Action = action;
        }
    }
}