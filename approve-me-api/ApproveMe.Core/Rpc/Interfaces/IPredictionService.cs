using Textservice;

namespace ApproveMe.Core.Rpc.Interfaces;

public interface IPredictionService
{
    public Task<string> SuggestUserName(TextRequest request);
}