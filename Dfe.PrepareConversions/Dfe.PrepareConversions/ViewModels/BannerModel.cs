namespace Dfe.PrepareConversions.ViewModels;

#nullable enable

public record BannerModel(
    string Type,
    string Title,
    string[] Message,
    string? Heading = null
);