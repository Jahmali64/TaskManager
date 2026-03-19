using System.Text.RegularExpressions;

namespace TaskManager.Domain.ValueObjects;

public sealed class Color
{
    public const string HexColorPattern = @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$";

    public string Value { get; }

    private static readonly Regex s_hexPattern = new(HexColorPattern, RegexOptions.Compiled);
    
    public static readonly Color Default = From("#6366F1");
    public static readonly Color Red = From("#EF4444");
    public static readonly Color Green = From("#22C55E");
    public static readonly Color Blue = From("#3B82F6");
    public static readonly Color Yellow = From("#EAB308");
    public static readonly Color Orange = From("#F97316");
    
    private Color(string value) => Value = value;

    public static Color From(string hex)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(hex);
        if (!s_hexPattern.IsMatch(hex))
        {
            throw new ArgumentException("Invalid hex color format. Expected format: #RRGGBB or #RGB.");
        }
        
        return new Color(hex.ToUpper());
    }
    
    public static implicit operator string(Color color) => color.Value;
    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is Color color && Value == color.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public static bool operator ==(Color? left, Color? right) => Equals(left, right);
    public static bool operator !=(Color? left, Color? right) => !Equals(left, right);
}