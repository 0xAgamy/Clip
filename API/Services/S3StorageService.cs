using System;
using Amazon.S3;
using Amazon.S3.Model;

namespace Clips_api.Services;

public class S3StorageService : IFileStorageService
{

    private readonly IAmazonS3 _s3;
    private readonly string _bucketName;
    private readonly int _presignExpiryMinutes;
    private readonly ILogger<S3StorageService> _logger;
    public S3StorageService(IAmazonS3 s3, IConfiguration config,
        ILogger<S3StorageService> logger)
    {
        _s3 = s3;
        _bucketName = config["S3:BucketName"];
        _presignExpiryMinutes = int.TryParse(config["S3:PresignExpiryMinutes"], out var expiry) ? expiry : 120;
        _logger = logger;
    }
    public Task DeleteAsync(string ObjectKey, CancellationToken ct = default)=>
        _s3.DeleteObjectAsync(
                new DeleteObjectRequest
                {
                    BucketName=_bucketName,
                    Key=ObjectKey
                }, ct
        );
   

    public Task<string> GetPresignedUrlAsync(string ObjectKey, CancellationToken ct = default)
    {
        var url= _s3.GetPreSignedURL(new GetPreSignedUrlRequest
        {
            BucketName=_bucketName,
            Key=ObjectKey,
            Verb=HttpVerb.GET,
            Expires=DateTime.UtcNow.AddMinutes(_presignExpiryMinutes),
            Protocol=Protocol.HTTP
            
        });
        return Task.FromResult(url);
    }

    public async Task<string> UploadAsync(Stream content, string fileName, string contnetType, string folder, CancellationToken ct = default)
    {
        var ext= Path.GetExtension(fileName);
        var objectKey=$"{folder}/{Guid.NewGuid():N}{ext.ToLowerInvariant()}";

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = objectKey,
            InputStream = content,
            ContentType = string.IsNullOrWhiteSpace(contnetType) ? "application/octet-stream" : contnetType,
            AutoCloseStream = false
        };
        await _s3.PutObjectAsync(request,ct);
        _logger.LogInformation("Stored object {Key} in bucket {Bucket}", objectKey, _bucketName);
        return objectKey;
    }
}
