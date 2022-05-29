﻿using System.Text;
using Language.Api.Syntax;
using Language.Common.Primitives;

namespace Bundle.Uml.Elements;

public class UmlSyntaxElement : SyntaxElement, IEquatable<UmlSyntaxElement>
{
    public readonly UmlFigure FirstFigure;
    public readonly Arrow? Arrow;
    public readonly UmlFigure? SecondFigure;
    public readonly string? Comment;

    public UmlSyntaxElement(UmlFigure firstFigure, Arrow? arrow = null, UmlFigure? secondFigure = null, string? comment = null)
    {
        FirstFigure = firstFigure;
        Arrow = arrow;
        SecondFigure = secondFigure;
        Comment = comment;
    }

    public bool Equals(UmlSyntaxElement? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return FirstFigure.Equals(other.FirstFigure) && Nullable.Equals(Arrow, other.Arrow) && Nullable.Equals(SecondFigure, other.SecondFigure) && Comment == other.Comment;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((UmlSyntaxElement)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = FirstFigure.GetHashCode();
            hashCode = (hashCode * 397) ^ Arrow.GetHashCode();
            hashCode = (hashCode * 397) ^ SecondFigure.GetHashCode();
            hashCode = (hashCode * 397) ^ (Comment != null ? Comment.GetHashCode() : 0);
            return hashCode;
        }
    }

    public static bool operator ==(UmlSyntaxElement? left, UmlSyntaxElement? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(UmlSyntaxElement? left, UmlSyntaxElement? right)
    {
        return !Equals(left, right);
    }

    public override String ToString()
    {
        var sb = new StringBuilder();

        sb.Append($"[{FirstFigure.ToString()}]");
        
        if (Arrow != null)
            sb.Append($" {Arrow.ToString()}");

        if (SecondFigure != null)
            sb.Append($" [{SecondFigure.ToString()}]");
        
        return sb.ToString();
    }

    public UmlSyntaxElement With(UmlFigure? firstFigure = null, UmlFigure? secondFigure = null)
    {
        return new UmlSyntaxElement(firstFigure ?? FirstFigure, Arrow, secondFigure ?? SecondFigure, Comment);
    }
}