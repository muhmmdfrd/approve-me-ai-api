using ApproveMe.Core.Rpc.Interfaces;
using Grpc.Net.Client;
using Textservice;

namespace ApproveMe.Core.Rpc.Services;

public class PredictionService : IPredictionService
{
    public async Task<string> SuggestUserName(TextRequest request)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:50051");
        var client = new TextService.TextServiceClient(channel);
        var result = await client.ProcessTextAsync(request);
        return result.ProcessedText;
    }
}