namespace Skuttoo.Application.Dtos;

/// <summary>Coins and stars earned.</summary>
public sealed record Reward(int Coins, int Stars)
{
    public static readonly Reward None = new(0, 0);
}
