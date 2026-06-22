namespace Skuttoo.Domain.Enums;

/// <summary>
/// A supported language. Used as a subject's <c>ContentLanguage</c> (the language being
/// taught), which is distinct from the child's UI language. Serialized camelCase ("sv"/"en").
/// </summary>
public enum Language
{
    Sv,
    En,
}
