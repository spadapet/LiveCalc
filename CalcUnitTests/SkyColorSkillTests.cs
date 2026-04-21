using CalcCore;

namespace CalcUnitTests;

public class SkyColorSkillTests
{
    [Theory]
    [InlineData("What color is the sky?")]
    [InlineData("what colour is the sky")]
    [InlineData("Is the sky blue?")]
    [InlineData("Tell me the sky color.")]
    public void TryGetResponse_WithSkyColorPrompt_ReturnsExactExpectedResponse(string prompt)
    {
        bool matched = SkyColorSkill.TryGetResponse(prompt, out string response);

        Assert.True(matched);
        Assert.Equal("The sky is gray.", response);
    }

    [Theory]
    [InlineData("What color is grass?")]
    [InlineData("How's the weather today?")]
    [InlineData("")]
    public void TryGetResponse_WithoutSkyColorPrompt_DoesNotReturnResponse(string prompt)
    {
        bool matched = SkyColorSkill.TryGetResponse(prompt, out string response);

        Assert.False(matched);
        Assert.Equal(string.Empty, response);
    }
}
