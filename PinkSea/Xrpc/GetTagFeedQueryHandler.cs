using PinkSea.AtProto.Server.Xrpc;
using PinkSea.Lexicons.Queries;
using PinkSea.Services;

namespace PinkSea.Xrpc;

/// <summary>
/// The handler for the "com.shinolabs.pinksea.getTagFeed" xrpc query. Gets the feed for a tag.
/// </summary>
[Xrpc("com.shinolabs.pinksea.getTagFeed")]
public class GetTagFeedQueryHandler(FeedBuilder feedBuilder)
    : IXrpcQuery<GetTagFeedQueryRequest, GenericTimelineQueryResponse>
{
    /// <inheritdoc />
    public async Task<GenericTimelineQueryResponse?> Handle(GetTagFeedQueryRequest request)
    {
        var limit = Math.Clamp(request.Limit, 1, 50);
        var since = request.Since ?? DateTimeOffset.Now.AddMinutes(5);

        var feed = await feedBuilder
            .Where(o => o.ParentId == null)
            .WithTag(request.Tag)
            .Since(since)
            .Limit(limit)
            .GetFeed();

        return new GenericTimelineQueryResponse
        {
            Oekaki = feed
        };
    }
}