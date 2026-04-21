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
        bool asksAboutSky = normalizedPrompt.Contains("sky");
        bool asksAboutColor = normalizedPrompt.Contains("color")
            || normalizedPrompt.Contains("colour")
            || normalizedPrompt.Contains("blue")
            || normalizedPrompt.Contains("gray")
            || normalizedPrompt.Contains("grey");

        if (!asksAboutSky || !asksAboutColor)
        {
            return false;
        }

        response = Response;
        return true;
    }
}
