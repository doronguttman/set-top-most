using System;
using System.Linq;
using System.Windows.Automation;
using SetTopMost.Extensions;

namespace SetTopMost
{
    class Program
    {
        static void Main(string[] args)
        {
            var windows = args.Select(a => int.TryParse(a, out var hWnd) ? hWnd : -1).Where(hWnd => hWnd > 0).ToArray();
            if (!windows.Any()) { windows = GetHandle(); }

            foreach (var window in windows.Select(w => AutomationElement.FromHandle(new IntPtr(w))))
            {
                var topMost = ((WindowPattern)window.GetCurrentPattern(WindowPattern.Pattern)).Current.IsTopmost;
                NativeMethods.SetWindowPos(
                    new IntPtr(window.Current.NativeWindowHandle),
                    topMost ? NativeMethods.HWND_NOTOPMOST : NativeMethods.HWND_TOPMOST,
                    0, 0, 0, 0,
                    NativeMethods.SetWindowPosFlags.IgnoreResize | NativeMethods.SetWindowPosFlags.IgnoreMove);
            }
        }

        private static int[] GetHandle()
        {
            var windows = AutomationElement.RootElement.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window))
                .Cast<AutomationElement>()
                .ToDictionary(e => e.Current.NativeWindowHandle, e => (e.Current.Name, ((WindowPattern)e.GetCurrentPattern(WindowPattern.Pattern)).Current.IsTopmost));

            foreach (var (hWnd, (name, topmost)) in windows.OrderBy(i => i.Value).ThenBy(e => e.Key))
            {
                if (topmost) Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(hWnd.ToString().PadRight(int.MinValue.ToString().Length));
                Console.ResetColor();
                Console.WriteLine($": {name}");
            }

            if (!int.TryParse(Console.ReadLine() ?? "-1", out var wnd)) { throw new InvalidOperationException("Invalid window handle"); }

            return new[] { wnd };
        }
    }
}
