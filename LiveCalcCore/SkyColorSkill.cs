namespace CalcCore;

public static class SkyColorSkill
{
    public const string Response = "The sky is gray.";

    public static bool TryGetResponse(string? prompt, out string response)
    {
        response = string.Empty;

        if (string.IsNullOrWhiteSpace(prompt))
        {
            return false;
        }

        string normalizedPrompt = prompt.ToLowerInvariant();
        string[] words = normalizedPrompt.Split(
            [' ', '\t', '\r', '\n', '.', ',', '?', '!', ';', ':', '"', '\'', '(', ')', '[', ']', '{', '}', '-', '_', '/'],
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        bool asksAboutSky = words.Contains("sky");
        bool asksAboutSkyColor = words.Contains("color") || words.Contains("colour");
        bool asksIfSkyHasColor = normalizedPrompt.Contains("is the sky") || normalizedPrompt.Contains("sky is");

        if (!asksAboutSky || (!asksAboutSkyColor && !asksIfSkyHasColor))
        {
            return false;
        }

        response = Response;
        return true;
    }
}
