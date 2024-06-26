namespace FileDownloaderAPI.Consumer;

using MassTransit;
using Microsoft.Extensions.Logging;

using FileDownloaderAPI.Contracts;

public class FileUriConsumer : IConsumer<UriMessage>
{
    private readonly ILogger<UriMessage> _logger;

    public FileUriConsumer(ILogger<UriMessage> logger)
    {
        _logger = logger;
    }

    async Task WriteMessageToDirectory(UriMessage message)
    {
        var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
        Directory.CreateDirectory(uploadPath);
        string fileName = message.Path.Split('/').Last();

        if (fileName == "" || !fileName.Contains('.'))
        {
            _logger.LogInformation("Invalid or empty file name: {fileName}", fileName);
            return;
        }
        
        var filePath = $"{uploadPath}/{fileName}";
        _logger.LogInformation("Writing {message} to {filePath}", message, filePath);

        // скачиваем файл
        using HttpClient client = new();
        var response = client.GetAsync(message.Value);
        var stream = await response.Result.Content.ReadAsStreamAsync();

        // записываем файл
        await File.WriteAllBytesAsync(filePath, await response.Result.Content.ReadAsByteArrayAsync());

        _logger.LogInformation("Wrote {message} to {filePath}", message, filePath);
    }

    public async Task Consume(ConsumeContext<UriMessage> context)
    {
        _logger.LogInformation("Received {message}", context.Message);
        await WriteMessageToDirectory(context.Message);
    }
}