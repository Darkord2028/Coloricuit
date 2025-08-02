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
    public static Vector2 ToVector(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            _ => Vector2.zero
        };
    }
}
