namespace Bundle.Uml.Elements;

public enum UmlFigureType: byte
{
    Participant = 1,
    Actor = 2, // :a:
    Boundary = 3,
    Control = 4,
    Entity = 5,
    Database = 6,
    Collections = 7,
    Queue = 8,
    UseCase = 9, // (u)
    Agent = 10,
    Artifact = 11,
    Node = 12,
    Card = 13,
    Circle = 14,
    Cloud = 15,
    File = 16,
    Folder = 17,
    Component = 18, // [c]
    Frame = 19,
    Hexagon = 20,
    Interface = 21, // ()i
    Label = 22,
    Package = 23,
    Person = 24,
    Storage = 25,
    Rectangle = 26,
    Stack = 27,
}