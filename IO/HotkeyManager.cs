using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using H1Z1Bot.WinAPI;

namespace H1Z1Bot.IO
{
    public static class HotkeyManager
    {
        private class HiddenForm : Form
        {
            public HiddenForm()
            {
                base.SetVisibleCore(false);
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if (m.Msg == 0x0312)
                {
                    User32.VirtualKey key = (User32.VirtualKey)(((int)m.LParam >> 16) & 0xFFFF);
                    User32.ModifierKeys modifier = (User32.ModifierKeys)((int)m.LParam & 0xFFFF);
                    int id = m.WParam.ToInt32();
                    HandleKey(key, modifier, id);
                }
            }
        }

        private static readonly Lazy<Dictionary<int, HotkeyHandler>> handlers =
            new Lazy<Dictionary<int, HotkeyHandler>>(() => new Dictionary<int, HotkeyHandler>());

        private static readonly Lazy<HiddenForm> form = new Lazy<HiddenForm>(() => new HiddenForm());

        private static int iids = 0;

        public static void Add(HotkeyHandler h)
        {
            lock (handlers)
            {
                if (!handlers.Value.ContainsValue(h))
                {
                    int id = ++iids;
                    handlers.Value.Add(id, h);
                    User32.RegisterHotKey(form.Value.Handle, id, h.ModKey, h.Key);
                }
            }
        }

        public static void Remove(HotkeyHandler h)
        {
            lock (handlers)
            {
                if (handlers.Value.ContainsValue(h))
                {
                    int id = handlers.Value.First(x => x.Value == h).Key;
                    handlers.Value.Remove(id);
                    User32.UnregisterHotKey(form.Value.Handle, id);
                }
            }
        }

        private static void HandleKey(User32.VirtualKey key, User32.ModifierKeys modkey, int id)
        {
            if (handlers.Value.ContainsKey(id))
            {
                handlers.Value[id].Action(key, modkey);
            }
        }
    }
}
