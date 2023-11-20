using System.Text;
using Cybernetic.Infrastructure.Abstraction.Services;

namespace Cybernetic.Infrastructure.Implementation.Services;

/// <summary>
/// Implementation of <see cref="INameGenerator"/>.
/// </summary>
public class NameGenerator : INameGenerator
{
    private static string[] consonants =
    {
        "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x"
    };

    private static string[] vowels =
    {
        "a", "e", "i", "o", "u", "ae", "y"
    };

    /// <inhertidoc />
    public string Generate(int length)
    {
        Random random = new Random();
        var name = new StringBuilder();

        name.Append(consonants[random.Next(consonants.Length)].ToUpper());
        name.Append(random.Next(vowels.Length));

        var nameLength = 2;
        while (nameLength < length)
        {

            name.Append(consonants[random.Next(consonants.Length)]);
            name.Append(vowels[random.Next(vowels.Length)]);
            nameLength += 2;
        }

        return name.ToString();
    }
}