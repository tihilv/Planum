namespace Language.Common.Primitives;

public enum LineType: byte
{
    Plain = 1, // ---
    Dash = 2, // ...
    Dot = 3, // ~~
    Bold = 4, // ==
    Hidden = 5
}