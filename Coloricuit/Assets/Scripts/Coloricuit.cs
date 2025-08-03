using UnityEngine;

[System.Serializable]
public class Coloricuit
{
    [Header("Coloricuit Config")]
    public ColorType color;
    public Shapes shape;
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
    Purple,
    Cyan
}

public enum Shapes
{
    Circle,
    Square,
    Triangle,
    Hexagon
}


