using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
#endif

namespace Monocle
{
    /// <summary>
    /// Unity-adapted MInput class.
    /// Uses Unity's new Input System instead of XNA input classes.
    /// The XNA types (Keys, Buttons, KeyboardState, etc.) are mapped to Unity equivalents.
    /// </summary>
    public static class MInput
    {
        internal static List<VirtualInput> VirtualInputs;
        public static bool Active = true;
        public static bool Disabled = false;
        public static bool ControllerHasFocus;
        public static bool IsControllerFocused;

        public static KeyboardData Keyboard { get; private set; }

        public static MouseData Mouse { get; private set; }

        public static GamePadData[] GamePads { get; private set; }

        internal static void Initialize()
        {
            Keyboard = new KeyboardData();
            Mouse = new MouseData();
            GamePads = new GamePadData[4];
            for (int playerIndex = 0; playerIndex < 4; ++playerIndex)
                GamePads[playerIndex] = new GamePadData(playerIndex);
            VirtualInputs = new List<VirtualInput>();
        }

        internal static void Shutdown()
        {
            foreach (GamePadData gamePad in GamePads)
                gamePad.StopRumble();
        }

        internal static void Update()
        {
            if (Active)
            {
                if (Engine.Commands != null && Engine.Commands.Open)
                {
                    Keyboard.UpdateNull();
                    Mouse.UpdateNull();
                }
                else
                {
                    Keyboard.Update();
                    Mouse.Update();
                }
                bool flag1 = false;
                bool flag2 = false;
                for (int index = 0; index < 4; ++index)
                {
                    GamePads[index].Update();
                    if (GamePads[index].HasAnyInput())
                    {
                        ControllerHasFocus = true;
                        flag1 = true;
                    }
                    if (GamePads[index].Attached)
                        flag2 = true;
                }
                if (!flag2 || !flag1 && Keyboard.HasAnyInput())
                    ControllerHasFocus = false;
            }
            else
            {
                Keyboard.UpdateNull();
                Mouse.UpdateNull();
                for (int index = 0; index < 4; ++index)
                    GamePads[index].UpdateNull();
            }
            UpdateVirtualInputs();
        }

        public static void UpdateNull()
        {
            Keyboard.UpdateNull();
            Mouse.UpdateNull();
            for (int index = 0; index < 4; ++index)
                GamePads[index].UpdateNull();
            UpdateVirtualInputs();
        }

        private static void UpdateVirtualInputs()
        {
            foreach (VirtualInput virtualInput in VirtualInputs)
                virtualInput.Update();
        }

        public static void RumbleFirst(float strength, float time) => GamePads[0].Rumble(strength, time);

        public static int Axis(bool negative, bool positive, int bothValue) => negative ? (positive ? bothValue : -1) : (positive ? 1 : 0);

        public static int Axis(float axisValue, float deadzone) => Math.Abs(axisValue) >= (double)deadzone ? Math.Sign(axisValue) : 0;

        public static int Axis(
            bool negative,
            bool positive,
            int bothValue,
            float axisValue,
            float deadzone)
        {
            int num = Axis(axisValue, deadzone);
            if (num == 0)
                num = Axis(negative, positive, bothValue);
            return num;
        }

        /// <summary>
        /// Unity-adapted KeyboardData using the new Input System.
        /// </summary>
        public class KeyboardData
        {
            private HashSet<Keys> _previousKeys = new HashSet<Keys>();
            private HashSet<Keys> _currentKeys = new HashSet<Keys>();
            #if ENABLE_INPUT_SYSTEM
            private UnityEngine.InputSystem.Keyboard _keyboard;
            #endif

// XNA compatibility: some game/UI code expects KeyboardState access.
public KeyboardState CurrentState => Microsoft.Xna.Framework.Input.Keyboard.GetState();

            internal KeyboardData()
            {
                #if ENABLE_INPUT_SYSTEM
                _keyboard = UnityEngine.InputSystem.Keyboard.current;
                #endif
            }

            internal void Update()
            {
                _previousKeys = new HashSet<Keys>(_currentKeys);
                _currentKeys.Clear();

                #if ENABLE_INPUT_SYSTEM
                _keyboard = UnityEngine.InputSystem.Keyboard.current;
                if (_keyboard == null) return;

                // Check all keys and add pressed ones to current state
                foreach (Keys key in Enum.GetValues(typeof(Keys)))
                {
                    if (key == Keys.None) continue;
                    
                    var unityKey = KeysToUnityKey(key);
                    if (unityKey != Key.None)
                    {
                        var keyControl = _keyboard[unityKey];
                        if (keyControl != null && keyControl.isPressed)
                        {
                            _currentKeys.Add(key);
                        }
                    }
                }
                #else
                var legacyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
                foreach (Keys key in Enum.GetValues(typeof(Keys)))
                {
                    if (key == Keys.None) continue;
                    if (legacyState.IsKeyDown(key))
                        _currentKeys.Add(key);
                }
                #endif
            }

            internal void UpdateNull()
            {
                _previousKeys = new HashSet<Keys>(_currentKeys);
                _currentKeys.Clear();
            }

            public bool HasAnyInput() => _currentKeys.Count > 0;

            public bool Check(Keys key) => !Disabled && key != Keys.None && _currentKeys.Contains(key);

            public bool Pressed(Keys key) => !Disabled && key != Keys.None && _currentKeys.Contains(key) && !_previousKeys.Contains(key);

            public bool Released(Keys key) => !Disabled && key != Keys.None && !_currentKeys.Contains(key) && _previousKeys.Contains(key);

            public bool Check(Keys keyA, Keys keyB) => Check(keyA) || Check(keyB);

            public bool Pressed(Keys keyA, Keys keyB) => Pressed(keyA) || Pressed(keyB);

            public bool Released(Keys keyA, Keys keyB) => Released(keyA) || Released(keyB);

            public bool Check(Keys keyA, Keys keyB, Keys keyC) => Check(keyA) || Check(keyB) || Check(keyC);

            public bool Pressed(Keys keyA, Keys keyB, Keys keyC) => Pressed(keyA) || Pressed(keyB) || Pressed(keyC);

            public bool Released(Keys keyA, Keys keyB, Keys keyC) => Released(keyA) || Released(keyB) || Released(keyC);

            public int AxisCheck(Keys negative, Keys positive) => Check(negative) ? (Check(positive) ? 0 : -1) : (Check(positive) ? 1 : 0);

            public int AxisCheck(Keys negative, Keys positive, int both) => Check(negative) ? (Check(positive) ? both : -1) : (Check(positive) ? 1 : 0);

            /// <summary>
            /// Maps XNA Keys to Unity Input System Key.
            /// </summary>
            #if ENABLE_INPUT_SYSTEM
            private static Key KeysToUnityKey(Keys key)
            {
                return key switch
                {
                    Keys.A => Key.A,
                    Keys.B => Key.B,
                    Keys.C => Key.C,
                    Keys.D => Key.D,
                    Keys.E => Key.E,
                    Keys.F => Key.F,
                    Keys.G => Key.G,
                    Keys.H => Key.H,
                    Keys.I => Key.I,
                    Keys.J => Key.J,
                    Keys.K => Key.K,
                    Keys.L => Key.L,
                    Keys.M => Key.M,
                    Keys.N => Key.N,
                    Keys.O => Key.O,
                    Keys.P => Key.P,
                    Keys.Q => Key.Q,
                    Keys.R => Key.R,
                    Keys.S => Key.S,
                    Keys.T => Key.T,
                    Keys.U => Key.U,
                    Keys.V => Key.V,
                    Keys.W => Key.W,
                    Keys.X => Key.X,
                    Keys.Y => Key.Y,
                    Keys.Z => Key.Z,
                    Keys.D0 => Key.Digit0,
                    Keys.D1 => Key.Digit1,
                    Keys.D2 => Key.Digit2,
                    Keys.D3 => Key.Digit3,
                    Keys.D4 => Key.Digit4,
                    Keys.D5 => Key.Digit5,
                    Keys.D6 => Key.Digit6,
                    Keys.D7 => Key.Digit7,
                    Keys.D8 => Key.Digit8,
                    Keys.D9 => Key.Digit9,
                    Keys.Escape => Key.Escape,
                    Keys.Space => Key.Space,
                    Keys.Enter => Key.Enter,
                    Keys.Tab => Key.Tab,
                    Keys.Back => Key.Backspace,
                    Keys.Left => Key.LeftArrow,
                    Keys.Right => Key.RightArrow,
                    Keys.Up => Key.UpArrow,
                    Keys.Down => Key.DownArrow,
                    Keys.LeftShift => Key.LeftShift,
                    Keys.RightShift => Key.RightShift,
                    Keys.LeftControl => Key.LeftCtrl,
                    Keys.RightControl => Key.RightCtrl,
                    Keys.LeftAlt => Key.LeftAlt,
                    Keys.RightAlt => Key.RightAlt,
                    Keys.F1 => Key.F1,
                    Keys.F2 => Key.F2,
                    Keys.F3 => Key.F3,
                    Keys.F4 => Key.F4,
                    Keys.F5 => Key.F5,
                    Keys.F6 => Key.F6,
                    Keys.F7 => Key.F7,
                    Keys.F8 => Key.F8,
                    Keys.F9 => Key.F9,
                    Keys.F10 => Key.F10,
                    Keys.F11 => Key.F11,
                    Keys.F12 => Key.F12,
                    _ => Key.None
                };
            }
            #endif

        }


        /// <summary>
        /// Unity-adapted MouseData using the new Input System.
        /// </summary>
        public class MouseData
        {
            #if ENABLE_INPUT_SYSTEM
            private UnityEngine.InputSystem.Mouse _mouse;
            #endif
            private bool _prevLeftButton;
            private bool _prevRightButton;
            private bool _prevMiddleButton;
            private bool _currLeftButton;
            private bool _currRightButton;
            private bool _currMiddleButton;
            private UnityEngine.Vector2 _prevPosition;
            private UnityEngine.Vector2 _currPosition;
            private float _prevScroll;
            private float _currScroll;

// XNA compatibility: some game/UI code expects MouseState access.
public MouseState CurrentState => Microsoft.Xna.Framework.Input.Mouse.GetState();

            internal MouseData()
            {
                #if ENABLE_INPUT_SYSTEM
                _mouse = UnityEngine.InputSystem.Mouse.current;
                #endif
            }

            internal void Update()
            {
                _prevLeftButton = _currLeftButton;
                _prevRightButton = _currRightButton;
                _prevMiddleButton = _currMiddleButton;
                _prevPosition = _currPosition;
                _prevScroll = _currScroll;

                #if ENABLE_INPUT_SYSTEM
                _mouse = UnityEngine.InputSystem.Mouse.current;
                if (_mouse != null)
                {
                    _currLeftButton = _mouse.leftButton.isPressed;
                    _currRightButton = _mouse.rightButton.isPressed;
                    _currMiddleButton = _mouse.middleButton.isPressed;
                    _currPosition = _mouse.position.ReadValue();
                    _currScroll = _mouse.scroll.ReadValue().y;
                }
                #else
                var legacy = Microsoft.Xna.Framework.Input.Mouse.GetState();
                _currLeftButton = legacy.LeftButton == ButtonState.Pressed;
                _currRightButton = legacy.RightButton == ButtonState.Pressed;
                _currMiddleButton = legacy.MiddleButton == ButtonState.Pressed;
                _currPosition = new UnityEngine.Vector2(legacy.X, legacy.Y);
                _currScroll = legacy.ScrollWheelValue;
                #endif
            }

            internal void UpdateNull()
            {
                _prevLeftButton = _currLeftButton;
                _prevRightButton = _currRightButton;
                _prevMiddleButton = _currMiddleButton;
                _prevPosition = _currPosition;
                _prevScroll = _currScroll;
                _currLeftButton = false;
                _currRightButton = false;
                _currMiddleButton = false;
            }

            public bool CheckLeftButton => _currLeftButton;

            public bool CheckRightButton => _currRightButton;

            public bool CheckMiddleButton => _currMiddleButton;

            public bool PressedLeftButton => _currLeftButton && !_prevLeftButton;

            public bool PressedRightButton => _currRightButton && !_prevRightButton;

            public bool PressedMiddleButton => _currMiddleButton && !_prevMiddleButton;

            public bool ReleasedLeftButton => !_currLeftButton && _prevLeftButton;

            public bool ReleasedRightButton => !_currRightButton && _prevRightButton;

            public bool ReleasedMiddleButton => !_currMiddleButton && _prevMiddleButton;

            public int Wheel => (int)_currScroll;

            public int WheelDelta => (int)(_currScroll - _prevScroll);

            public bool WasMoved => _currPosition.x != _prevPosition.x || _currPosition.y != _prevPosition.y;

            public float X
            {
                get => Position.X;
                set => Position = new Microsoft.Xna.Framework.Vector2(value, Position.Y);
            }

            public float Y
            {
                get => Position.Y;
                set => Position = new Microsoft.Xna.Framework.Vector2(Position.X, value);
            }

            public Microsoft.Xna.Framework.Vector2 Position
            {
                get
                {
                    var pos = new Microsoft.Xna.Framework.Vector2(_currPosition.x, _currPosition.y);
                    return Microsoft.Xna.Framework.Vector2.Transform(pos, Microsoft.Xna.Framework.Matrix.Invert(Engine.ScreenMatrix));
                }
                set
                {
                    var transformed = Microsoft.Xna.Framework.Vector2.Transform(value, Engine.ScreenMatrix);
                    #if ENABLE_INPUT_SYSTEM
                    if (_mouse != null)
                    {
                        _mouse.WarpCursorPosition(new UnityEngine.Vector2(transformed.X, transformed.Y));
                    }
                    #endif
                }
            }
        }

        /// <summary>
        /// Unity-adapted GamePadData using the new Input System.
        /// </summary>
        public class GamePadData
        {
            public readonly PlayerIndex PlayerIndex;
            public bool Attached;
            public bool HadInputThisFrame;
            private float rumbleStrength;
            private float rumbleTime;

            #if ENABLE_INPUT_SYSTEM
            private UnityEngine.InputSystem.Gamepad _gamepad;
            #endif
            
            // Previous frame state
            private bool[] _prevButtons = new bool[16];
            private UnityEngine.Vector2 _prevLeftStick;
            private UnityEngine.Vector2 _prevRightStick;
            private float _prevLeftTrigger;
            private float _prevRightTrigger;
            
            // Current frame state
            private bool[] _currButtons = new bool[16];
            private UnityEngine.Vector2 _currLeftStick;
            private UnityEngine.Vector2 _currRightStick;
            private float _currLeftTrigger;
            private float _currRightTrigger;

            // Button indices for the bool arrays
            private const int BTN_A = 0;
            private const int BTN_B = 1;
            private const int BTN_X = 2;
            private const int BTN_Y = 3;
            private const int BTN_LSHOULDER = 4;
            private const int BTN_RSHOULDER = 5;
            private const int BTN_LSTICK = 6;
            private const int BTN_RSTICK = 7;
            private const int BTN_START = 8;
            private const int BTN_BACK = 9;
            private const int BTN_DPAD_UP = 10;
            private const int BTN_DPAD_DOWN = 11;
            private const int BTN_DPAD_LEFT = 12;
            private const int BTN_DPAD_RIGHT = 13;

            internal GamePadData(int playerIndex)
            {
                PlayerIndex = (PlayerIndex)Calc.Clamp(playerIndex, 0, 3);
            }

            #if ENABLE_INPUT_SYSTEM
            private UnityEngine.InputSystem.Gamepad GetGamepad()
            {
                var gamepads = UnityEngine.InputSystem.Gamepad.all;
                int index = (int)PlayerIndex;
                if (index < gamepads.Count)
                    return gamepads[index];
                return null;
            }
            #endif

            public bool HasAnyInput()
            {
                #if ENABLE_INPUT_SYSTEM
                bool wasConnected = Attached;
                bool isConnected = _gamepad != null;
                
                if (!wasConnected && isConnected)
                    return true;
                    
                // Check buttons
                for (int i = 0; i < _currButtons.Length; i++)
                {
                    if (_currButtons[i] != _prevButtons[i])
                        return true;
                }
                
                // Check triggers
                if (_currLeftTrigger > 0.01f || _currRightTrigger > 0.01f)
                    return true;
                    
                // Check sticks
                if (_currLeftStick.magnitude > 0.01f || _currRightStick.magnitude > 0.01f)
                    return true;
                    
                return false;
                #else
                return false;
                #endif
            }

            public void Update()
            {
                #if ENABLE_INPUT_SYSTEM
                // Save previous state
                Array.Copy(_currButtons, _prevButtons, _currButtons.Length);
                _prevLeftStick = _currLeftStick;
                _prevRightStick = _currRightStick;
                _prevLeftTrigger = _currLeftTrigger;
                _prevRightTrigger = _currRightTrigger;

                _gamepad = GetGamepad();
                bool wasAttached = Attached;
                Attached = _gamepad != null;

                if (!wasAttached && Attached)
                    IsControllerFocused = true;

                if (_gamepad != null)
                {
                    // Read buttons
                    _currButtons[BTN_A] = _gamepad.aButton.isPressed;
                    _currButtons[BTN_B] = _gamepad.bButton.isPressed;
                    _currButtons[BTN_X] = _gamepad.xButton.isPressed;
                    _currButtons[BTN_Y] = _gamepad.yButton.isPressed;
                    _currButtons[BTN_LSHOULDER] = _gamepad.leftShoulder.isPressed;
                    _currButtons[BTN_RSHOULDER] = _gamepad.rightShoulder.isPressed;
                    _currButtons[BTN_LSTICK] = _gamepad.leftStickButton.isPressed;
                    _currButtons[BTN_RSTICK] = _gamepad.rightStickButton.isPressed;
                    _currButtons[BTN_START] = _gamepad.startButton.isPressed;
                    _currButtons[BTN_BACK] = _gamepad.selectButton.isPressed;
                    _currButtons[BTN_DPAD_UP] = _gamepad.dpad.up.isPressed;
                    _currButtons[BTN_DPAD_DOWN] = _gamepad.dpad.down.isPressed;
                    _currButtons[BTN_DPAD_LEFT] = _gamepad.dpad.left.isPressed;
                    _currButtons[BTN_DPAD_RIGHT] = _gamepad.dpad.right.isPressed;

                    // Read sticks and triggers
                    _currLeftStick = _gamepad.leftStick.ReadValue();
                    _currRightStick = _gamepad.rightStick.ReadValue();
                    _currLeftTrigger = _gamepad.leftTrigger.ReadValue();
                    _currRightTrigger = _gamepad.rightTrigger.ReadValue();
                }
                else
                {
                    Array.Clear(_currButtons, 0, _currButtons.Length);
                    _currLeftStick = UnityEngine.Vector2.zero;
                    _currRightStick = UnityEngine.Vector2.zero;
                    _currLeftTrigger = 0f;
                    _currRightTrigger = 0f;
                }

                // Handle rumble
                if (rumbleTime > 0.0)
                {
                    rumbleTime -= Engine.DeltaTime;
                    if (rumbleTime <= 0.0)
                    {
                        _gamepad?.SetMotorSpeeds(0f, 0f);
                    }
                }
                #else
                Array.Copy(_currButtons, _prevButtons, _currButtons.Length);
                _prevLeftStick = _currLeftStick;
                _prevRightStick = _currRightStick;
                _prevLeftTrigger = _currLeftTrigger;
                _prevRightTrigger = _currRightTrigger;

                Array.Clear(_currButtons, 0, _currButtons.Length);
                _currLeftStick = UnityEngine.Vector2.zero;
                _currRightStick = UnityEngine.Vector2.zero;
                _currLeftTrigger = 0f;
                _currRightTrigger = 0f;

                var state = Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex);
                Attached = state.IsConnected;
                HadInputThisFrame = false;
                #endif
            }

            public void UpdateNull()
            {
                Array.Copy(_currButtons, _prevButtons, _currButtons.Length);
                _prevLeftStick = _currLeftStick;
                _prevRightStick = _currRightStick;
                _prevLeftTrigger = _currLeftTrigger;
                _prevRightTrigger = _currRightTrigger;

                Array.Clear(_currButtons, 0, _currButtons.Length);
                _currLeftStick = UnityEngine.Vector2.zero;
                _currRightStick = UnityEngine.Vector2.zero;
                _currLeftTrigger = 0f;
                _currRightTrigger = 0f;

                #if ENABLE_INPUT_SYSTEM
                _gamepad = GetGamepad();
                Attached = _gamepad != null;

                if (rumbleTime > 0.0)
                    rumbleTime -= Engine.DeltaTime;
                _gamepad?.SetMotorSpeeds(0f, 0f);
                #else
                Attached = false;
                if (rumbleTime > 0.0)
                    rumbleTime -= Engine.DeltaTime;
                #endif
            }

            public void Rumble(float strength, float time)
            {
                #if ENABLE_INPUT_SYSTEM
                if (rumbleTime > 0.0 && strength <= rumbleStrength && (strength != rumbleStrength || time <= rumbleTime))
                    return;
                _gamepad?.SetMotorSpeeds(strength, strength);
                rumbleStrength = strength;
                rumbleTime = time;
                #else
                rumbleStrength = strength;
                rumbleTime = time;
                #endif
            }

            public void StopRumble()
            {
                #if ENABLE_INPUT_SYSTEM
                _gamepad?.SetMotorSpeeds(0f, 0f);
                rumbleTime = 0.0f;
                #else
                rumbleTime = 0.0f;
                #endif
            }

            private int GetButtonIndex(Buttons button)
            {
                return button switch
                {
                    Buttons.A => BTN_A,
                    Buttons.B => BTN_B,
                    Buttons.X => BTN_X,
                    Buttons.Y => BTN_Y,
                    Buttons.LeftShoulder => BTN_LSHOULDER,
                    Buttons.RightShoulder => BTN_RSHOULDER,
                    Buttons.LeftStick => BTN_LSTICK,
                    Buttons.RightStick => BTN_RSTICK,
                    Buttons.Start => BTN_START,
                    Buttons.Back => BTN_BACK,
                    Buttons.DPadUp => BTN_DPAD_UP,
                    Buttons.DPadDown => BTN_DPAD_DOWN,
                    Buttons.DPadLeft => BTN_DPAD_LEFT,
                    Buttons.DPadRight => BTN_DPAD_RIGHT,
                    _ => -1
                };
            }

            public bool Check(Buttons button)
            {
                if (Disabled) return false;
                int idx = GetButtonIndex(button);
                if (idx >= 0) return _currButtons[idx];
                return false;
            }

            public bool Pressed(Buttons button)
            {
                if (Disabled) return false;
                int idx = GetButtonIndex(button);
                if (idx >= 0) return _currButtons[idx] && !_prevButtons[idx];
                return false;
            }

            public bool Released(Buttons button)
            {
                if (Disabled) return false;
                int idx = GetButtonIndex(button);
                if (idx >= 0) return !_currButtons[idx] && _prevButtons[idx];
                return false;
            }

            public bool Check(Buttons buttonA, Buttons buttonB) => Check(buttonA) || Check(buttonB);

            public bool Pressed(Buttons buttonA, Buttons buttonB) => Pressed(buttonA) || Pressed(buttonB);

            public bool Released(Buttons buttonA, Buttons buttonB) => Released(buttonA) || Released(buttonB);

            public bool Check(Buttons buttonA, Buttons buttonB, Buttons buttonC) => Check(buttonA) || Check(buttonB) || Check(buttonC);

            public bool Pressed(Buttons buttonA, Buttons buttonB, Buttons buttonC) => Pressed(buttonA) || Pressed(buttonB) || Pressed(buttonC);

            public bool Released(Buttons buttonA, Buttons buttonB, Buttons buttonC) => Released(buttonA) || Released(buttonB) || Released(buttonC);

            public Microsoft.Xna.Framework.Vector2 GetLeftStick()
            {
                return new Microsoft.Xna.Framework.Vector2(_currLeftStick.x, -_currLeftStick.y);
            }

            public Microsoft.Xna.Framework.Vector2 GetLeftStick(float deadzone)
            {
                if (_currLeftStick.sqrMagnitude < deadzone * deadzone)
                    return Microsoft.Xna.Framework.Vector2.Zero;
                return new Microsoft.Xna.Framework.Vector2(_currLeftStick.x, -_currLeftStick.y);
            }

            public Microsoft.Xna.Framework.Vector2 GetRightStick()
            {
                return new Microsoft.Xna.Framework.Vector2(_currRightStick.x, -_currRightStick.y);
            }

            public Microsoft.Xna.Framework.Vector2 GetRightStick(float deadzone)
            {
                if (_currRightStick.sqrMagnitude < deadzone * deadzone)
                    return Microsoft.Xna.Framework.Vector2.Zero;
                return new Microsoft.Xna.Framework.Vector2(_currRightStick.x, -_currRightStick.y);
            }

            public bool LeftStickLeftCheck(float deadzone) => _currLeftStick.x <= -deadzone;

            public bool LeftStickLeftPressed(float deadzone) => _currLeftStick.x <= -deadzone && _prevLeftStick.x > -deadzone;

            public bool LeftStickLeftReleased(float deadzone) => _currLeftStick.x > -deadzone && _prevLeftStick.x <= -deadzone;

            public bool LeftStickRightCheck(float deadzone) => _currLeftStick.x >= deadzone;

            public bool LeftStickRightPressed(float deadzone) => _currLeftStick.x >= deadzone && _prevLeftStick.x < deadzone;

            public bool LeftStickRightReleased(float deadzone) => _currLeftStick.x < deadzone && _prevLeftStick.x >= deadzone;

            public bool LeftStickDownCheck(float deadzone) => _currLeftStick.y <= -deadzone;

            public bool LeftStickDownPressed(float deadzone) => _currLeftStick.y <= -deadzone && _prevLeftStick.y > -deadzone;

            public bool LeftStickDownReleased(float deadzone) => _currLeftStick.y > -deadzone && _prevLeftStick.y <= -deadzone;

            public bool LeftStickUpCheck(float deadzone) => _currLeftStick.y >= deadzone;

            public bool LeftStickUpPressed(float deadzone) => _currLeftStick.y >= deadzone && _prevLeftStick.y < deadzone;

            public bool LeftStickUpReleased(float deadzone) => _currLeftStick.y < deadzone && _prevLeftStick.y >= deadzone;

            public float LeftStickHorizontal(float deadzone)
            {
                float x = _currLeftStick.x;
                return Math.Abs(x) < deadzone ? 0.0f : x;
            }

            public float LeftStickVertical(float deadzone)
            {
                float y = _currLeftStick.y;
                return Math.Abs(y) < deadzone ? 0.0f : -y;
            }

            public bool RightStickLeftCheck(float deadzone) => _currRightStick.x <= -deadzone;

            public bool RightStickLeftPressed(float deadzone) => _currRightStick.x <= -deadzone && _prevRightStick.x > -deadzone;

            public bool RightStickLeftReleased(float deadzone) => _currRightStick.x > -deadzone && _prevRightStick.x <= -deadzone;

            public bool RightStickRightCheck(float deadzone) => _currRightStick.x >= deadzone;

            public bool RightStickRightPressed(float deadzone) => _currRightStick.x >= deadzone && _prevRightStick.x < deadzone;

            public bool RightStickRightReleased(float deadzone) => _currRightStick.x < deadzone && _prevRightStick.x >= deadzone;

            public bool RightStickDownCheck(float deadzone) => _currRightStick.y <= -deadzone;

            public bool RightStickDownPressed(float deadzone) => _currRightStick.y <= -deadzone && _prevRightStick.y > -deadzone;

            public bool RightStickDownReleased(float deadzone) => _currRightStick.y > -deadzone && _prevRightStick.y <= -deadzone;

            public bool RightStickUpCheck(float deadzone) => _currRightStick.y >= deadzone;

            public bool RightStickUpPressed(float deadzone) => _currRightStick.y >= deadzone && _prevRightStick.y < deadzone;

            public bool RightStickUpReleased(float deadzone) => _currRightStick.y < deadzone && _prevRightStick.y >= deadzone;

            public float RightStickHorizontal(float deadzone)
            {
                float x = _currRightStick.x;
                return Math.Abs(x) < deadzone ? 0.0f : x;
            }

            public float RightStickVertical(float deadzone)
            {
                float y = _currRightStick.y;
                return Math.Abs(y) < deadzone ? 0.0f : -y;
            }

            public int DPadHorizontal
            {
                get
                {
                    if (_currButtons[BTN_DPAD_RIGHT]) return 1;
                    return _currButtons[BTN_DPAD_LEFT] ? -1 : 0;
                }
            }

            public int DPadVertical
            {
                get
                {
                    if (_currButtons[BTN_DPAD_DOWN]) return 1;
                    return _currButtons[BTN_DPAD_UP] ? -1 : 0;
                }
            }

            public Microsoft.Xna.Framework.Vector2 DPad => new(DPadHorizontal, DPadVertical);

            public bool DPadLeftCheck => _currButtons[BTN_DPAD_LEFT];

            public bool DPadLeftPressed => _currButtons[BTN_DPAD_LEFT] && !_prevButtons[BTN_DPAD_LEFT];

            public bool DPadLeftReleased => !_currButtons[BTN_DPAD_LEFT] && _prevButtons[BTN_DPAD_LEFT];

            public bool DPadRightCheck => _currButtons[BTN_DPAD_RIGHT];

            public bool DPadRightPressed => _currButtons[BTN_DPAD_RIGHT] && !_prevButtons[BTN_DPAD_RIGHT];

            public bool DPadRightReleased => !_currButtons[BTN_DPAD_RIGHT] && _prevButtons[BTN_DPAD_RIGHT];

            public bool DPadUpCheck => _currButtons[BTN_DPAD_UP];

            public bool DPadUpPressed => _currButtons[BTN_DPAD_UP] && !_prevButtons[BTN_DPAD_UP];

            public bool DPadUpReleased => !_currButtons[BTN_DPAD_UP] && _prevButtons[BTN_DPAD_UP];

            public bool DPadDownCheck => _currButtons[BTN_DPAD_DOWN];

            public bool DPadDownPressed => _currButtons[BTN_DPAD_DOWN] && !_prevButtons[BTN_DPAD_DOWN];

            public bool DPadDownReleased => !_currButtons[BTN_DPAD_DOWN] && _prevButtons[BTN_DPAD_DOWN];

            public bool LeftTriggerCheck(float threshold) => !Disabled && _currLeftTrigger >= threshold;

            public bool LeftTriggerPressed(float threshold) => !Disabled && _currLeftTrigger >= threshold && _prevLeftTrigger < threshold;

            public bool LeftTriggerReleased(float threshold) => !Disabled && _currLeftTrigger < threshold && _prevLeftTrigger >= threshold;

            public bool RightTriggerCheck(float threshold) => !Disabled && _currRightTrigger >= threshold;

            public bool RightTriggerPressed(float threshold) => !Disabled && _currRightTrigger >= threshold && _prevRightTrigger < threshold;

            public bool RightTriggerReleased(float threshold) => !Disabled && _currRightTrigger < threshold && _prevRightTrigger >= threshold;

            public float Axis(Buttons button, float threshold)
            {
                if (Disabled) return 0.0f;
                
                switch (button)
                {
                    case Buttons.DPadUp:
                    case Buttons.DPadDown:
                    case Buttons.DPadLeft:
                    case Buttons.DPadRight:
                    case Buttons.Start:
                    case Buttons.Back:
                    case Buttons.LeftStick:
                    case Buttons.RightStick:
                    case Buttons.LeftShoulder:
                    case Buttons.RightShoulder:
                    case Buttons.A:
                    case Buttons.B:
                    case Buttons.X:
                    case Buttons.Y:
                        if (Check(button)) return 1f;
                        break;
                    case Buttons.LeftThumbstickLeft:
                        if (_currLeftStick.x <= -threshold) return -_currLeftStick.x;
                        break;
                    case Buttons.RightTrigger:
                        if (_currRightTrigger >= threshold) return _currRightTrigger;
                        break;
                    case Buttons.LeftTrigger:
                        if (_currLeftTrigger >= threshold) return _currLeftTrigger;
                        break;
                    case Buttons.RightThumbstickUp:
                        if (_currRightStick.y >= threshold) return _currRightStick.y;
                        break;
                    case Buttons.RightThumbstickDown:
                        if (_currRightStick.y <= -threshold) return -_currRightStick.y;
                        break;
                    case Buttons.RightThumbstickRight:
                        if (_currRightStick.x >= threshold) return _currRightStick.x;
                        break;
                    case Buttons.RightThumbstickLeft:
                        if (_currRightStick.x <= -threshold) return -_currRightStick.x;
                        break;
                    case Buttons.LeftThumbstickUp:
                        if (_currLeftStick.y >= threshold) return _currLeftStick.y;
                        break;
                    case Buttons.LeftThumbstickDown:
                        if (_currLeftStick.y <= -threshold) return -_currLeftStick.y;
                        break;
                    case Buttons.LeftThumbstickRight:
                        if (_currLeftStick.x >= threshold) return _currLeftStick.x;
                        break;
                }
                return 0.0f;
            }

            public bool Check(Buttons button, float threshold)
            {
                if (Disabled) return false;
                
                switch (button)
                {
                    case Buttons.DPadUp:
                    case Buttons.DPadDown:
                    case Buttons.DPadLeft:
                    case Buttons.DPadRight:
                    case Buttons.Start:
                    case Buttons.Back:
                    case Buttons.LeftStick:
                    case Buttons.RightStick:
                    case Buttons.LeftShoulder:
                    case Buttons.RightShoulder:
                    case Buttons.A:
                    case Buttons.B:
                    case Buttons.X:
                    case Buttons.Y:
                        return Check(button);
                    case Buttons.LeftThumbstickLeft:
                        return LeftStickLeftCheck(threshold);
                    case Buttons.RightTrigger:
                        return RightTriggerCheck(threshold);
                    case Buttons.LeftTrigger:
                        return LeftTriggerCheck(threshold);
                    case Buttons.RightThumbstickUp:
                        return RightStickUpCheck(threshold);
                    case Buttons.RightThumbstickDown:
                        return RightStickDownCheck(threshold);
                    case Buttons.RightThumbstickRight:
                        return RightStickRightCheck(threshold);
                    case Buttons.RightThumbstickLeft:
                        return RightStickLeftCheck(threshold);
                    case Buttons.LeftThumbstickUp:
                        return LeftStickUpCheck(threshold);
                    case Buttons.LeftThumbstickDown:
                        return LeftStickDownCheck(threshold);
                    case Buttons.LeftThumbstickRight:
                        return LeftStickRightCheck(threshold);
                }
                return false;
            }

            public bool Pressed(Buttons button, float threshold)
            {
                if (Disabled) return false;
                
                switch (button)
                {
                    case Buttons.DPadUp:
                    case Buttons.DPadDown:
                    case Buttons.DPadLeft:
                    case Buttons.DPadRight:
                    case Buttons.Start:
                    case Buttons.Back:
                    case Buttons.LeftStick:
                    case Buttons.RightStick:
                    case Buttons.LeftShoulder:
                    case Buttons.RightShoulder:
                    case Buttons.A:
                    case Buttons.B:
                    case Buttons.X:
                    case Buttons.Y:
                        return Pressed(button);
                    case Buttons.LeftThumbstickLeft:
                        return LeftStickLeftPressed(threshold);
                    case Buttons.RightTrigger:
                        return RightTriggerPressed(threshold);
                    case Buttons.LeftTrigger:
                        return LeftTriggerPressed(threshold);
                    case Buttons.RightThumbstickUp:
                        return RightStickUpPressed(threshold);
                    case Buttons.RightThumbstickDown:
                        return RightStickDownPressed(threshold);
                    case Buttons.RightThumbstickRight:
                        return RightStickRightPressed(threshold);
                    case Buttons.RightThumbstickLeft:
                        return RightStickLeftPressed(threshold);
                    case Buttons.LeftThumbstickUp:
                        return LeftStickUpPressed(threshold);
                    case Buttons.LeftThumbstickDown:
                        return LeftStickDownPressed(threshold);
                    case Buttons.LeftThumbstickRight:
                        return LeftStickRightPressed(threshold);
                }
                return false;
            }

            public bool Released(Buttons button, float threshold)
            {
                if (Disabled) return false;
                
                switch (button)
                {
                    case Buttons.DPadUp:
                    case Buttons.DPadDown:
                    case Buttons.DPadLeft:
                    case Buttons.DPadRight:
                    case Buttons.Start:
                    case Buttons.Back:
                    case Buttons.LeftStick:
                    case Buttons.RightStick:
                    case Buttons.LeftShoulder:
                    case Buttons.RightShoulder:
                    case Buttons.A:
                    case Buttons.B:
                    case Buttons.X:
                    case Buttons.Y:
                        return Released(button);
                    case Buttons.LeftThumbstickLeft:
                        return LeftStickLeftReleased(threshold);
                    case Buttons.RightTrigger:
                        return RightTriggerReleased(threshold);
                    case Buttons.LeftTrigger:
                        return LeftTriggerReleased(threshold);
                    case Buttons.RightThumbstickUp:
                        return RightStickUpReleased(threshold);
                    case Buttons.RightThumbstickDown:
                        return RightStickDownReleased(threshold);
                    case Buttons.RightThumbstickRight:
                        return RightStickRightReleased(threshold);
                    case Buttons.RightThumbstickLeft:
                        return RightStickLeftReleased(threshold);
                    case Buttons.LeftThumbstickUp:
                        return LeftStickUpReleased(threshold);
                    case Buttons.LeftThumbstickDown:
                        return LeftStickDownReleased(threshold);
                    case Buttons.LeftThumbstickRight:
                        return LeftStickRightReleased(threshold);
                }
                return false;
            }
        }
    }
}
