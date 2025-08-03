using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public static class Utilities
{
    public static bool IsPointerOverUI()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            int touchId = Touchscreen.current.primaryTouch.touchId.ReadValue();
            return EventSystem.current.IsPointerOverGameObject(touchId);
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            return EventSystem.current.IsPointerOverGameObject(Mouse.current.deviceId);
        }

        return false;
    }

    public static bool IsAnyInputOverUI()
    {
        if (EventSystem.current == null) return false;

        // Mouse or Touch
        if (IsPointerOverUI())
            return true;

        // Gamepad
        if (EventSystem.current.currentSelectedGameObject != null)
            return true;

        return false;
    }

}

public static class DirectionUtilities
{
    public static Vector2Int ToVector2Int(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Vector2Int.up,
            Direction.Down => Vector2Int.down,
            Direction.Left => Vector2Int.left,
            Direction.Right => Vector2Int.right,
            _ => Vector2Int.zero
        };
    }

    public static Direction GetOpposite(Direction dir)
    {
        return dir switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => dir
        };
    }

}

public static class ColorUtility
{
    private static readonly Dictionary<ColorType, Color> colorMap = new()
    {
        { ColorType.Red,     Color.red },
        { ColorType.Green,   Color.green },
        { ColorType.Blue,    Color.blue },
        { ColorType.Yellow,  Color.yellow },
        { ColorType.Orange,  new Color(1f, 0.5f, 0f) },     // RGB for Orange
        { ColorType.Purple,  new Color(0.5f, 0f, 0.5f) },   // RGB for Purple
        { ColorType.Cyan,    Color.cyan },
    };

    private static readonly Dictionary<(ColorType, ColorType), ColorType> mixTable = new()
    {
        // Red combinations
        {(ColorType.Red, ColorType.Green), ColorType.Yellow},
        {(ColorType.Red, ColorType.Blue), ColorType.Purple},
        {(ColorType.Red, ColorType.Yellow), ColorType.Orange},
        {(ColorType.Red, ColorType.Orange), ColorType.Red},
        {(ColorType.Red, ColorType.Purple), ColorType.Purple},
        {(ColorType.Red, ColorType.Cyan), ColorType.Cyan},

        // Blue combinations
        {(ColorType.Blue, ColorType.Green), ColorType.Cyan},
        {(ColorType.Blue, ColorType.Yellow), ColorType.Green},
        {(ColorType.Blue, ColorType.Orange), ColorType.Purple},
        {(ColorType.Blue, ColorType.Purple), ColorType.Purple},
        {(ColorType.Blue, ColorType.Cyan), ColorType.Cyan},

        // Add all other combinations...
    };

    public static ColorType? Mix(ColorType a, ColorType b)
    {
        if (a == b)
            return a;

        if (mixTable.TryGetValue((a, b), out var result))
            return result;

        if (mixTable.TryGetValue((b, a), out result))
            return result;

        return null; // No mix found
    }

    public static Color GetUnityColor(ColorType type)
    {
        if (colorMap.TryGetValue(type, out var color))
            return color;

        return Color.magenta; // Fallback for undefined colors
    }
}


