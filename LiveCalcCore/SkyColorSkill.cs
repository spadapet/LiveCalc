namespace CalcCore;

public static class SkyColorSkill
{
    private static readonly char[] WordDelimiters = [' ', '\t', '\r', '\n', '.', ',', '?', '!', ';', ':', '"', '\'', '(', ')', '[', ']', '{', '}', '-', '_', '/'];

    public const string Response = "The sky is gray.";

    public static bool TryGetResponse(string? prompt, out string response)
    {
        response = string.Empty;

        if (string.IsNullOrWhiteSpace(prompt))
        {
            return false;
        }

        string[] words = prompt
            .ToLowerInvariant()
            .Split(WordDelimiters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        bool asksAboutSky = words.Contains("sky");
        bool asksAboutSkyColor = words.Contains("color") || words.Contains("colour");
        bool asksIfSkyHasColor = ContainsSequence(words, "is", "the", "sky")
            || ContainsSequence(words, "sky", "is");

        if (!asksAboutSky || (!asksAboutSkyColor && !asksIfSkyHasColor))
        {
            return false;
        }

        response = Response;
        return true;
    }

    private static bool ContainsSequence(string[] words, params string[] sequence)
    {
        if (sequence.Length == 0 || words.Length < sequence.Length)
        {
            return false;
        }

        for (int i = 0; i <= words.Length - sequence.Length; i++)
        {
            bool isMatch = true;
            for (int j = 0; j < sequence.Length; j++)
            {
                if (words[i + j] != sequence[j])
                {
                    isMatch = false;
                    break;
                }
            }

            if (isMatch)
            {
                return true;
            }
        }

        return false;
    }
}
