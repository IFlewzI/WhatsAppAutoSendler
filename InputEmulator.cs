using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;

namespace WhatsApp_Auto_Newslatter
{
    class InputEmulator
    {
        private const string RussianAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        private const string EnglishAlphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const ushort LeftAltVirtualKeyCode = 164;
        private const ushort LeftShiftVirtualKeyCode = 160;
        private const ushort EnterVirtualKeyCode = 13;
        private const ushort DeleteVirtualKeyCode = 46;
        private const ushort EscapeVirtualKeyCode = 27;

        private bool _isRussianLayout;

        private List<char> _anyLayoutUpperSymbols = new List<char>()
        {
            '!',
            '?',
            '(',
            ')',
            ':',
            '.',
            '_',
            '+',
            '\n',
        };

        private List<char> _russianLayoutUpperSymbols = new List<char>()
        {
            '№',
            ',',
        };

        private List<char> _englishLayoutUpperSymbols = new List<char>()
        {
            ';',
            '#',
            '"',
        };

        private struct Input
        {
            public InputType Type;
            public InputUnion Union;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)] public MouseInput MouseInput;
            [FieldOffset(0)] public KeyboardInput KeyboardInput;
            [FieldOffset(0)] public HardwareInput HardwareInput;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardInput
        {
            public ushort VirtualKeyCode;
            public ushort KeyCodeScan;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MouseInput
        {
            public int xPosition;
            public int yPosition;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HardwareInput
        {
            public uint Message;
            public ushort LowOrderParameter;
            public ushort HighOrderParameter;
        }

        [Flags]
        private enum InputType
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }

        [Flags]
        private enum KeyEventFlags
        {
            KeyDown = 0x0000,
            ExtendedKey = 0x0001,
            KeyUp = 0x0002,
            Unicode = 0x0004,
            Scancode = 0x0008
        }

        [Flags]
        private enum MouseEventFlags
        {
            Absolute = 0x8000,
            HWheel = 0x01000,
            Move = 0x0001,
            MoveNoCoalesce = 0x2000,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            VirtualDesk = 0x4000,
            Wheel = 0x0800,
            XDown = 0x0080,
            XUp = 0x0100
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern short VkKeyScan(char ch);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr windowHandle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr windowHandle, int showWindowCommand);

        public InputEmulator()
        {
            _isRussianLayout = VkKeyScan('ъ') != -1;
        }

        public void SetCursorPosition(int x, int y)
        {
            Thread.Sleep(5);
            SetCursorPos(x, y);
        }

        public void Click()
        {
            Thread.Sleep(5);

            Input[] inputs = new Input[]
            {
                new Input
                {
                    Type = InputType.Mouse,
                    Union = new InputUnion
                    {
                        MouseInput = new MouseInput
                        {
                            Flags = (uint)MouseEventFlags.LeftDown
                        }
                    }
                },
                new Input
                {
                    Type = InputType.Mouse,
                    Union = new InputUnion
                    {
                        MouseInput = new MouseInput
                        {
                            Flags = (uint)MouseEventFlags.LeftUp
                        }
                    }
                }
            };

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
        }

        public void PressEnter()
        {
            Thread.Sleep(5);

            Input[] inputs = new Input[]
            {
                new Input
                {
                    Type = InputType.Keyboard,
                    Union = new InputUnion
                    {
                        KeyboardInput = new KeyboardInput
                        {
                            VirtualKeyCode = EnterVirtualKeyCode,
                            Flags = (uint)KeyEventFlags.KeyDown,
                            ExtraInfo = GetMessageExtraInfo()
                        }
                    }
                },
                new Input
                {
                    Type = InputType.Keyboard,
                    Union = new InputUnion
                    {
                        KeyboardInput = new KeyboardInput
                        {
                            VirtualKeyCode = EnterVirtualKeyCode,
                            Flags = (uint)KeyEventFlags.KeyUp,
                            ExtraInfo = GetMessageExtraInfo()
                        }
                    }
                }
            };

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
        }

        public void PressDelete()
        {
            Thread.Sleep(5);

            Input[] inputs = new Input[]
            {
                new Input
                {
                    Type = InputType.Keyboard,
                    Union = new InputUnion
                    {
                        KeyboardInput = new KeyboardInput
                        {
                            VirtualKeyCode = DeleteVirtualKeyCode,
                            Flags = (uint)KeyEventFlags.KeyDown,
                            ExtraInfo = GetMessageExtraInfo()
                        }
                    }
                },
                new Input
                {
                    Type = InputType.Keyboard,
                    Union = new InputUnion
                    {
                        KeyboardInput = new KeyboardInput
                        {
                            VirtualKeyCode = DeleteVirtualKeyCode,
                            Flags = (uint)KeyEventFlags.KeyUp,
                            ExtraInfo = GetMessageExtraInfo()
                        }
                    }
                }
            };

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
        }

        public void PressEscape()
        {
            Thread.Sleep(5);

            Input[] inputs = new Input[]
            {
                new Input
                {
                    Type = InputType.Keyboard,
                    Union = new InputUnion
                    {
                        KeyboardInput = new KeyboardInput
                        {
                            VirtualKeyCode = EscapeVirtualKeyCode,
                            Flags = (uint)KeyEventFlags.KeyDown,
                            ExtraInfo = GetMessageExtraInfo()
                        }
                    }
                },
                new Input
                {
                    Type = InputType.Keyboard,
                    Union = new InputUnion
                    {
                        KeyboardInput = new KeyboardInput
                        {
                            VirtualKeyCode = EscapeVirtualKeyCode,
                            Flags = (uint)KeyEventFlags.KeyUp,
                            ExtraInfo = GetMessageExtraInfo()
                        }
                    }
                }
            };

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
        }

        public void SetActiveSpecificProcess(string processName)
        {
            Thread.Sleep(5);

            foreach (var process in Process.GetProcessesByName(processName))
            {
                ShowWindow(process.MainWindowHandle, 5);
                SetForegroundWindow(process.MainWindowHandle);
            }
        }

        public void SwapKeyboardLayoutAndLanguage()
        {
            Input[] inputs = new Input[]
            {
                new Input
                {
                    Type = InputType.Keyboard,
                    Union = new InputUnion
                    {
                        KeyboardInput = new KeyboardInput
                        {
                            VirtualKeyCode = LeftAltVirtualKeyCode,
                            Flags = (uint)KeyEventFlags.KeyDown,
                            ExtraInfo = GetMessageExtraInfo()
                        }
                    }
                },
                new Input
                {
                    Type = InputType.Keyboard,
                    Union = new InputUnion
                    {
                        KeyboardInput = new KeyboardInput
                        {
                            VirtualKeyCode = LeftShiftVirtualKeyCode,
                            Flags = (uint)KeyEventFlags.KeyDown,
                            ExtraInfo = GetMessageExtraInfo()
                        }
                    }
                },
                new Input
                {
                    Type = InputType.Keyboard,
                    Union = new InputUnion
                    {
                        KeyboardInput = new KeyboardInput
                        {
                            VirtualKeyCode = LeftAltVirtualKeyCode,
                            Flags = (uint)KeyEventFlags.KeyUp,
                            ExtraInfo = GetMessageExtraInfo()
                        }
                    }
                },
                new Input
                {
                    Type = InputType.Keyboard,
                    Union = new InputUnion
                    {
                        KeyboardInput = new KeyboardInput
                        {
                            VirtualKeyCode = LeftShiftVirtualKeyCode,
                            Flags = (uint)KeyEventFlags.KeyUp,
                            ExtraInfo = GetMessageExtraInfo()
                        }
                    }
                }
            };

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
            SwapKeyboardLanguage();
            Thread.Sleep(10);
        }

        private void SwapKeyboardLanguage()
        {
            if (InputLanguage.CurrentInputLanguage.LayoutName == "США")
            {
                for (int i = 0; i < InputLanguage.InstalledInputLanguages.Count; i++)
                {
                    if (InputLanguage.InstalledInputLanguages[i].LayoutName == "Русская")
                    {
                        InputLanguage.CurrentInputLanguage = InputLanguage.InstalledInputLanguages[i];
                        _isRussianLayout = true;
                    }
                }
            }
            else if (InputLanguage.CurrentInputLanguage.LayoutName == "Русская")
            {
                for (int i = 0; i < InputLanguage.InstalledInputLanguages.Count; i++)
                {
                    if (InputLanguage.InstalledInputLanguages[i].LayoutName == "США")
                    {
                        InputLanguage.CurrentInputLanguage = InputLanguage.InstalledInputLanguages[i];
                        _isRussianLayout = false;
                    }
                }
            }
        }

        public void WriteText(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                InputSymbol(text[i]);
                Thread.Sleep(10);
            }
        }

        private void InputSymbol(char symbol)
        {
            bool isUpper = false;
            ushort symbolVirtualKeyCode = 65535;

            List<Input> inputs = new List<Input>();
            Input symbolKeyDownInput;
            Input symbolKeyUpInput;

            if (RussianAlphabet.Contains(symbol))
            {
                isUpper = Char.IsUpper(symbol);

                if (_isRussianLayout)
                {
                    symbolVirtualKeyCode = (ushort)VkKeyScan(symbol);
                }
                else
                {
                    SwapKeyboardLayoutAndLanguage();
                    Thread.Sleep(10);
                    symbolVirtualKeyCode = (ushort)VkKeyScan(symbol);
                }
            }
            else if (EnglishAlphabet.Contains(symbol))
            {
                isUpper = Char.IsUpper(symbol);

                if (_isRussianLayout)
                {
                    SwapKeyboardLayoutAndLanguage();
                    Thread.Sleep(10);
                    symbolVirtualKeyCode = (ushort)VkKeyScan(symbol);
                }
                else
                {
                    symbolVirtualKeyCode = (ushort)VkKeyScan(symbol);
                }
            }
            else if (_anyLayoutUpperSymbols.Contains(symbol))
            {
                isUpper = true;
                symbolVirtualKeyCode = (ushort)VkKeyScan(symbol);
            }
            else if (_russianLayoutUpperSymbols.Contains(symbol))
            {
                isUpper = true;

                if (_isRussianLayout)
                {
                    symbolVirtualKeyCode = (ushort)VkKeyScan(symbol);
                }
                else
                {
                    SwapKeyboardLayoutAndLanguage();
                    Thread.Sleep(10);
                    symbolVirtualKeyCode = (ushort)VkKeyScan(symbol);
                }
            }
            else if (_englishLayoutUpperSymbols.Contains(symbol))
            {
                isUpper = true;

                if (_isRussianLayout)
                {
                    SwapKeyboardLayoutAndLanguage();
                    Thread.Sleep(10);
                    symbolVirtualKeyCode = (ushort)VkKeyScan(symbol);
                }
                else
                {
                    symbolVirtualKeyCode = (ushort)VkKeyScan(symbol);
                }
            }
            else
            {
                symbolVirtualKeyCode = (ushort)VkKeyScan(symbol);
            }

            symbolKeyDownInput = new Input
            {
                Type = InputType.Keyboard,
                Union = new InputUnion
                {
                    KeyboardInput = new KeyboardInput
                    {
                        VirtualKeyCode = symbolVirtualKeyCode,
                        Flags = (uint)KeyEventFlags.KeyDown,
                        ExtraInfo = GetMessageExtraInfo()
                    }
                }
            };
            symbolKeyUpInput = new Input
            {
                Type = InputType.Keyboard,
                Union = new InputUnion
                {
                    KeyboardInput = new KeyboardInput
                    {
                        VirtualKeyCode = symbolVirtualKeyCode,
                        Flags = (uint)KeyEventFlags.KeyUp,
                        ExtraInfo = GetMessageExtraInfo()
                    }
                }
            };

            if (isUpper)
            {
                Input shiftKeyDownInput = new Input
                {
                    Type = InputType.Keyboard,
                    Union = new InputUnion
                    {
                        KeyboardInput = new KeyboardInput
                        {
                            VirtualKeyCode = LeftShiftVirtualKeyCode,
                            Flags = (uint)KeyEventFlags.KeyDown,
                            ExtraInfo = GetMessageExtraInfo()
                        }
                    }
                };
                Input shiftKeyUpInput = new Input
                {
                    Type = InputType.Keyboard,
                    Union = new InputUnion
                    {
                        KeyboardInput = new KeyboardInput
                        {
                            VirtualKeyCode = LeftShiftVirtualKeyCode,
                            Flags = (uint)KeyEventFlags.KeyUp,
                            ExtraInfo = GetMessageExtraInfo()
                        }
                    }
                };

                inputs.Add(shiftKeyDownInput);
                inputs.Add(symbolKeyDownInput);
                inputs.Add(symbolKeyUpInput);
                inputs.Add(shiftKeyUpInput);
            }
            else
            {
                inputs.Add(symbolKeyDownInput);
                inputs.Add(symbolKeyUpInput);
            }

            var inputsArray = inputs.ToArray();
            SendInput((uint)inputsArray.Length, inputsArray, Marshal.SizeOf(typeof(Input)));
        }
    }
}
