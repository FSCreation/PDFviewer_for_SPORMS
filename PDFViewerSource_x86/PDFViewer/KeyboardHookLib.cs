using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CSharpKeyboardHook
{


    /// <summary>
    /// keyboard hook management class
    /// </summary>
    public class KeyboardHookLib
    {
        private const int WH_KEYBOARD_LL = 13; //keyboard


        //Keyboard handling event delegate
        private delegate int HookHandle(int nCode, int wParam, IntPtr lParam);


        //Clients keyboard event handling
        public delegate void ProcessKeyHandle(HookStruct param, out bool handle);


        //Receive SetWindowsHookEx return value
        private static int _hHookValue = 0;


        //Hook handles events
        private HookHandle _KeyBoardHookProcedure;


        //Hook Struct
        [StructLayout(LayoutKind.Sequential)]
        public class HookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }


        //Set Hook
        [DllImport("user32.dll")]
        private static extern int SetWindowsHookEx(int idHook, HookHandle lpfn, IntPtr hInstance, int threadId);


        //Cancel Hook
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);


        //Call the next hook
        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);


        //Get the current thread ID
        [DllImport("kernel32.dll")]
        private static extern int GetCurrentThreadId();


        //Gets the main module for the associated process.
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string name);


        private IntPtr _hookWindowPtr = IntPtr.Zero;


        //Constructor
        public KeyboardHookLib() { }


        //Keyboard event handling external calls
        private static ProcessKeyHandle _clientMethod = null;


        /// <summary>
        /// Instal Hook
        /// </summary>
        /// <param name="hookProcess">Keyboard event handling external calls</param>
        public void InstallHook(ProcessKeyHandle clientMethod)
        {
            _clientMethod = clientMethod;


            // Install keyboard hook
            if (_hHookValue == 0)
            {
                _KeyBoardHookProcedure = new HookHandle(GetHookProc);


                _hookWindowPtr = GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);


                _hHookValue = SetWindowsHookEx(
                    WH_KEYBOARD_LL,
                    _KeyBoardHookProcedure,
                    _hookWindowPtr,
                    0);


                //Set hook fail
                if (_hHookValue == 0)
                    UninstallHook();
            }
        }


        //Cancel hook event
        public void UninstallHook()
        {
            if (_hHookValue != 0)
            {
                bool ret = UnhookWindowsHookEx(_hHookValue);
                if (ret) _hHookValue = 0;
            }
        }


        //Hook event internally calls,call _clientMethod menthod forwarded to the client application
        private static int GetHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                //Transfer Structure
                HookStruct hookStruct = (HookStruct)Marshal.PtrToStructure(lParam, typeof(HookStruct));


                if (_clientMethod != null)
                {
                    bool handle = false;
                    //Calls the event handler provided by the customer
                    _clientMethod(hookStruct, out handle);
                    if (handle)
                        return 1; //1:Interception
                }
            }
            return CallNextHookEx(_hHookValue, nCode, wParam, lParam);
        }


    }
}
