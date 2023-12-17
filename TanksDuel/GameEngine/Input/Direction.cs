using System;

namespace GameEngine.Input
{
    /// <summary>
    /// Перечисление направлений
    /// </summary>
    [Flags]
    public enum Direction
    {
        Left = 1,
        Right = 2,
        Up = 4,
        Down = 8,
        All = Up | Down | Left | Right
    }
}
