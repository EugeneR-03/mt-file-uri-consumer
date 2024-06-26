namespace FileDownloaderAPI.Contracts;

public record UriMessage
{
    Uri? uri;
    public string Value { get; init; }
    public string Scheme { get; init; }
    public string Source { get; init; }
    public string Path { get; init; }
    public string Query { get; init; }
    public string PathAndQuery { get; init; }
    public string Fragment { get; init; }

    public UriMessage(string value)
    {
        uri = new Uri(value);
        Value = value;
        Scheme = uri.Scheme;
        Source = uri.Host;
        Path = uri.AbsolutePath;
        Query = uri.Query;
        if (uri.Query.Length > 0)
            PathAndQuery = uri.AbsolutePath + uri.Query;
        else
            PathAndQuery = uri.AbsolutePath;
        
        Fragment = uri.Fragment;
    }

    public override string ToString() => Value;
}