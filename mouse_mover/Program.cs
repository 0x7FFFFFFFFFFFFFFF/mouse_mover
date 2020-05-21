using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace mouse_mover
{
    class Program
    {
        private enum Win32Consts
        {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2,
        }

        private struct INPUT
        {
            public int type;
            public MOUSEINPUT mi;
        }

        [Flags]
        private enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public MouseEventFlags dwFlags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        private void MoveMousePointer(Point point)
        {
            var mi = new MOUSEINPUT
            {
                dx = point.X,
                dy = point.Y,
                mouseData = 0,
                time = 0,
                dwFlags = MouseEventFlags.MOVE,
                dwExtraInfo = UIntPtr.Zero
            };
            var input = new INPUT
            {
                mi = mi,
                type = Convert.ToInt32(Win32Consts.INPUT_MOUSE)
            };
            SendInput(1, ref input, Marshal.SizeOf(input));
        }


        static void Main(string[] args)
        {
            var p = new Program();
            const int mouseMoveLoopSleep = 1;
            const int mouseSpeed = 1;
            const int moveSquareSize = 3;
            int interval = 60000;

            if (args.Length == 1)
            {
                int.TryParse(args[0], out interval);
            }

            while (true)
            {
                var cursorStartPosition = Cursor.Position;
                for (int i = 0; i < moveSquareSize; i += mouseSpeed)
                {
                    p.MoveMousePointer(new Point(1, 0));
                    Thread.Sleep(mouseMoveLoopSleep);
                }

                for (int i = 0; i < moveSquareSize; i += mouseSpeed)
                {
                    p.MoveMousePointer(new Point(0, 1));
                    Thread.Sleep(mouseMoveLoopSleep);
                }

                for (int i = 0; i < moveSquareSize; i += mouseSpeed)
                {
                    p.MoveMousePointer(new Point(-1, 0));
                    Thread.Sleep(mouseMoveLoopSleep);
                }

                for (int i = 0; i < moveSquareSize; i += mouseSpeed)
                {
                    p.MoveMousePointer(new Point(0, -1));
                    Thread.Sleep(mouseMoveLoopSleep);
                }

                Cursor.Position = cursorStartPosition;
                Thread.Sleep(interval);
            }
        }
    }
}