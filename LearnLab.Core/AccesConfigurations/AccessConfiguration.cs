using System.Diagnostics.CodeAnalysis;

namespace LearnLab.Core.AccesConfigurations;

public class AccessConfiguration
{
    [AllowNull]
    public string? Issuer { get; set; }

    [AllowNull]
    public string? Audience { get; set; }
}
