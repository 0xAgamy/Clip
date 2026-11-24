using Clips.Domain.Enums;
using Clips.Domain.Models;

namespace Clips.UnitTests.Domain;

public class VideoTests
{
    [Fact]
    public void NewVideo_HasProcessingStatusAndPublicVisibility()
    {
        var video = new Video();

        Assert.Equal(VideoStatus.Processing, video.Status);
        Assert.Equal(VideoVisibility.Public, video.Visibility);
    }

    [Fact]
    public void NewVideo_HasPublicId()
    {
        var video = new Video();

        Assert.NotEqual(Guid.Empty, video.PublicId);
    }
}
