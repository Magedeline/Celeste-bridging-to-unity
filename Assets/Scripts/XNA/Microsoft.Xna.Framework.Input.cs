// XNA Framework Input Compatibility Layer for Unity
// This provides XNA Input types that map to Unity Input

using System;
using UnityEngine;

namespace Microsoft.Xna.Framework.Input
{
    /// <summary>
    /// Keyboard keys enum
    /// </summary>
    public enum Keys
    {
        None = 0,
        Back = 8,
        Tab = 9,
        Enter = 13,
        Pause = 19,
        CapsLock = 20,
        Kana = 21,
        Kanji = 25,
        Escape = 27,
        Space = 32,
        PageUp = 33,
        PageDown = 34,
        End = 35,
        Home = 36,
        Left = 37,
        Up = 38,
        Right = 39,
        Down = 40,
        Select = 41,
        Print = 42,
        Execute = 43,
        PrintScreen = 44,
        Insert = 45,
        Delete = 46,
        Help = 47,
        D0 = 48,
        D1 = 49,
        D2 = 50,
        D3 = 51,
        D4 = 52,
        D5 = 53,
        D6 = 54,
        D7 = 55,
        D8 = 56,
        D9 = 57,
        A = 65,
        B = 66,
        C = 67,
        D = 68,
        E = 69,
        F = 70,
        G = 71,
        H = 72,
        I = 73,
        J = 74,
        K = 75,
        L = 76,
        M = 77,
        N = 78,
        O = 79,
        P = 80,
        Q = 81,
        R = 82,
        S = 83,
        T = 84,
        U = 85,
        V = 86,
        W = 87,
        X = 88,
        Y = 89,
        Z = 90,
        LeftWindows = 91,
        RightWindows = 92,
        Apps = 93,
        Sleep = 95,
        NumPad0 = 96,
        NumPad1 = 97,
        NumPad2 = 98,
        NumPad3 = 99,
        NumPad4 = 100,
        NumPad5 = 101,
        NumPad6 = 102,
        NumPad7 = 103,
        NumPad8 = 104,
        NumPad9 = 105,
        Multiply = 106,
        Add = 107,
        Separator = 108,
        Subtract = 109,
        Decimal = 110,
        Divide = 111,
        F1 = 112,
        F2 = 113,
        F3 = 114,
        F4 = 115,
        F5 = 116,
        F6 = 117,
        F7 = 118,
        F8 = 119,
        F9 = 120,
        F10 = 121,
        F11 = 122,
        F12 = 123,
        F13 = 124,
        F14 = 125,
        F15 = 126,
        F16 = 127,
        F17 = 128,
        F18 = 129,
        F19 = 130,
        F20 = 131,
        F21 = 132,
        F22 = 133,
        F23 = 134,
        F24 = 135,
        NumLock = 144,
        Scroll = 145,
        LeftShift = 160,
        RightShift = 161,
        LeftControl = 162,
        RightControl = 163,
        LeftAlt = 164,
        RightAlt = 165,
        BrowserBack = 166,
        BrowserForward = 167,
        BrowserRefresh = 168,
        BrowserStop = 169,
        BrowserSearch = 170,
        BrowserFavorites = 171,
        BrowserHome = 172,
        VolumeMute = 173,
        VolumeDown = 174,
        VolumeUp = 175,
        MediaNextTrack = 176,
        MediaPreviousTrack = 177,
        MediaStop = 178,
        MediaPlayPause = 179,
        LaunchMail = 180,
        SelectMedia = 181,
        LaunchApplication1 = 182,
        LaunchApplication2 = 183,
        OemSemicolon = 186,
        OemPlus = 187,
        OemComma = 188,
        OemMinus = 189,
        OemPeriod = 190,
        OemQuestion = 191,
        OemTilde = 192,
        OemOpenBrackets = 219,
        OemPipe = 220,
        OemCloseBrackets = 221,
        OemQuotes = 222,
        Oem8 = 223,
        OemBackslash = 226,
        ProcessKey = 229,
        Attn = 246,
        Crsel = 247,
        Exsel = 248,
        EraseEof = 249,
        Play = 250,
        Zoom = 251,
        Pa1 = 253,
        OemClear = 254
    }

    /// <summary>
    /// Game pad buttons
    /// </summary>
    [Flags]
    public enum Buttons
    {
        DPadUp = 1,
        DPadDown = 2,
        DPadLeft = 4,
        DPadRight = 8,
        Start = 16,
        Back = 32,
        LeftStick = 64,
        RightStick = 128,
        LeftShoulder = 256,
        RightShoulder = 512,
        BigButton = 2048,
        A = 4096,
        B = 8192,
        X = 16384,
        Y = 32768,
        LeftThumbstickLeft = 2097152,
        RightTrigger = 4194304,
        LeftTrigger = 8388608,
        RightThumbstickUp = 16777216,
        RightThumbstickDown = 33554432,
        RightThumbstickRight = 67108864,
        RightThumbstickLeft = 134217728,
        LeftThumbstickUp = 268435456,
        LeftThumbstickDown = 536870912,
        LeftThumbstickRight = 1073741824
    }

    /// <summary>
    /// Keyboard state
    /// </summary>
    public struct KeyboardState
    {
        private Keys[] _pressedKeys;

        public KeyboardState(params Keys[] keys)
        {
            _pressedKeys = keys;
        }

        public bool IsKeyDown(Keys key)
        {
            if (_pressedKeys == null) return false;
            foreach (var k in _pressedKeys)
            {
                if (k == key) return true;
            }
            return false;
        }

        public bool IsKeyUp(Keys key) => !IsKeyDown(key);

        public KeyState this[Keys key] => IsKeyDown(key) ? KeyState.Down : KeyState.Up;

        public Keys[] GetPressedKeys() => _pressedKeys ?? Array.Empty<Keys>();
    }

    /// <summary>
    /// Gamepad state (stub)
    /// </summary>
    public struct GamePadState
    {
        public bool IsConnected { get; set; }
        public GamePadButtons Buttons { get; set; }
        public GamePadDPad DPad { get; set; }
        public GamePadThumbSticks ThumbSticks { get; set; }
        public GamePadTriggers Triggers { get; set; }

        public bool IsButtonDown(Buttons button)
        {
            if (!IsConnected) return false;
            
            return button switch
            {
                Microsoft.Xna.Framework.Input.Buttons.A => this.Buttons.A == ButtonState.Pressed,
                Microsoft.Xna.Framework.Input.Buttons.B => this.Buttons.B == ButtonState.Pressed,
                Microsoft.Xna.Framework.Input.Buttons.X => this.Buttons.X == ButtonState.Pressed,
                Microsoft.Xna.Framework.Input.Buttons.Y => this.Buttons.Y == ButtonState.Pressed,
                Microsoft.Xna.Framework.Input.Buttons.DPadUp => DPad.Up == ButtonState.Pressed,
                Microsoft.Xna.Framework.Input.Buttons.DPadDown => DPad.Down == ButtonState.Pressed,
                Microsoft.Xna.Framework.Input.Buttons.DPadLeft => DPad.Left == ButtonState.Pressed,
                Microsoft.Xna.Framework.Input.Buttons.DPadRight => DPad.Right == ButtonState.Pressed,
                _ => false
            };
        }
        
        public bool IsButtonUp(Buttons button) => !IsButtonDown(button);
    }

    public struct GamePadButtons
    {
        public ButtonState A { get; set; }
        public ButtonState B { get; set; }
        public ButtonState X { get; set; }
        public ButtonState Y { get; set; }
        public ButtonState Start { get; set; }
        public ButtonState Back { get; set; }
        public ButtonState LeftShoulder { get; set; }
        public ButtonState RightShoulder { get; set; }
        public ButtonState LeftStick { get; set; }
        public ButtonState RightStick { get; set; }
    }

    public struct GamePadDPad
    {
        public ButtonState Up { get; set; }
        public ButtonState Down { get; set; }
        public ButtonState Left { get; set; }
        public ButtonState Right { get; set; }
    }

    public struct GamePadThumbSticks
    {
        public Microsoft.Xna.Framework.Vector2 Left { get; set; }
        public Microsoft.Xna.Framework.Vector2 Right { get; set; }
    }

    public struct GamePadTriggers
    {
        public float Left { get; set; }
        public float Right { get; set; }
    }

    public enum ButtonState
    {
        Released,
        Pressed
    }

    public enum KeyState
    {
        Up,
        Down
    }

    /// <summary>
    /// Player index enum
    /// </summary>
    public enum PlayerIndex
    {
        One = 0,
        Two = 1,
        Three = 2,
        Four = 3
    }

    /// <summary>
    /// Keyboard input - legacy Unity input
    /// </summary>
    public static class Keyboard
    {
        public static KeyboardState GetState()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            var pressedKeys = new System.Collections.Generic.List<Keys>();
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (key == Keys.None)
                    continue;

                var keyCode = KeysToKeyCode(key);
                if (keyCode != KeyCode.None && UnityEngine.Input.GetKey(keyCode))
                    pressedKeys.Add(key);
            }

            return new KeyboardState(pressedKeys.ToArray());
#else
            return new KeyboardState();
#endif
        }

        private static KeyCode KeysToKeyCode(Keys key)
        {
            return key switch
            {
                Keys.A => KeyCode.A,
                Keys.B => KeyCode.B,
                Keys.C => KeyCode.C,
                Keys.D => KeyCode.D,
                Keys.E => KeyCode.E,
                Keys.F => KeyCode.F,
                Keys.G => KeyCode.G,
                Keys.H => KeyCode.H,
                Keys.I => KeyCode.I,
                Keys.J => KeyCode.J,
                Keys.K => KeyCode.K,
                Keys.L => KeyCode.L,
                Keys.M => KeyCode.M,
                Keys.N => KeyCode.N,
                Keys.O => KeyCode.O,
                Keys.P => KeyCode.P,
                Keys.Q => KeyCode.Q,
                Keys.R => KeyCode.R,
                Keys.S => KeyCode.S,
                Keys.T => KeyCode.T,
                Keys.U => KeyCode.U,
                Keys.V => KeyCode.V,
                Keys.W => KeyCode.W,
                Keys.X => KeyCode.X,
                Keys.Y => KeyCode.Y,
                Keys.Z => KeyCode.Z,
                Keys.D0 => KeyCode.Alpha0,
                Keys.D1 => KeyCode.Alpha1,
                Keys.D2 => KeyCode.Alpha2,
                Keys.D3 => KeyCode.Alpha3,
                Keys.D4 => KeyCode.Alpha4,
                Keys.D5 => KeyCode.Alpha5,
                Keys.D6 => KeyCode.Alpha6,
                Keys.D7 => KeyCode.Alpha7,
                Keys.D8 => KeyCode.Alpha8,
                Keys.D9 => KeyCode.Alpha9,
                Keys.Up => KeyCode.UpArrow,
                Keys.Down => KeyCode.DownArrow,
                Keys.Left => KeyCode.LeftArrow,
                Keys.Right => KeyCode.RightArrow,
                Keys.Space => KeyCode.Space,
                Keys.Enter => KeyCode.Return,
                Keys.Escape => KeyCode.Escape,
                Keys.Tab => KeyCode.Tab,
                Keys.Back => KeyCode.Backspace,
                Keys.Delete => KeyCode.Delete,
                Keys.LeftShift => KeyCode.LeftShift,
                Keys.RightShift => KeyCode.RightShift,
                Keys.LeftControl => KeyCode.LeftControl,
                Keys.RightControl => KeyCode.RightControl,
                Keys.LeftAlt => KeyCode.LeftAlt,
                Keys.RightAlt => KeyCode.RightAlt,
                Keys.F1 => KeyCode.F1,
                Keys.F2 => KeyCode.F2,
                Keys.F3 => KeyCode.F3,
                Keys.F4 => KeyCode.F4,
                Keys.F5 => KeyCode.F5,
                Keys.F6 => KeyCode.F6,
                Keys.F7 => KeyCode.F7,
                Keys.F8 => KeyCode.F8,
                Keys.F9 => KeyCode.F9,
                Keys.F10 => KeyCode.F10,
                Keys.F11 => KeyCode.F11,
                Keys.F12 => KeyCode.F12,
                _ => KeyCode.None
            };
        }
    }

    /// <summary>
    /// Mouse state
    /// </summary>
    public struct MouseState
    {
        public int X;
        public int Y;
        public ButtonState LeftButton;
        public ButtonState RightButton;
        public ButtonState MiddleButton;
        public int ScrollWheelValue;
    }

    /// <summary>
    /// Mouse input - legacy Unity input
    /// </summary>
    public static class Mouse
    {
        public static MouseState GetState()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            var pos = UnityEngine.Input.mousePosition;
            return new MouseState
            {
                X = (int)pos.x,
                Y = (int)pos.y,
                LeftButton = UnityEngine.Input.GetMouseButton(0) ? ButtonState.Pressed : ButtonState.Released,
                RightButton = UnityEngine.Input.GetMouseButton(1) ? ButtonState.Pressed : ButtonState.Released,
                MiddleButton = UnityEngine.Input.GetMouseButton(2) ? ButtonState.Pressed : ButtonState.Released,
                ScrollWheelValue = (int)(UnityEngine.Input.mouseScrollDelta.y * 120)
            };
#else
            return new MouseState();
#endif
        }

        public static void SetPosition(int x, int y)
        {
            // Not supported in legacy Input without platform-specific handling
        }
    }

    /// <summary>
    /// GamePad input - stubbed for legacy builds
    /// </summary>
    public static class GamePad
    {
        public static GamePadState GetState(PlayerIndex playerIndex)
        {
            return new GamePadState { IsConnected = false };
        }

        public static bool SetVibration(PlayerIndex playerIndex, float leftMotor, float rightMotor)
        {
            return false;
        }
    }
}
