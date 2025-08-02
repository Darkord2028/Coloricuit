using UnityEngine;

public struct Coloricuit
{
    [Header("Coloricuit Config")]
    public ColorType color;
    public Shapes shape;
    public Direction outputDirection;

    public override string ToString()
    {
        return $"Coloricuit: Color={color}, Shape={shape}, OutputDirection={outputDirection}";
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public enum ColorType
{
    Red,
    Green,
    Blue,
    Yellow,
    Orange,
    Purple
}

public enum Shapes
{
    Circle,
    Square,
    Triangle,
    Hexagon
}


