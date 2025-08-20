
namespace MyImageApp.Services
{
    public interface IImageGenerationService
    {
        Task<string> GenerateImageAsync(string prompt);
        Task<bool> IsServiceAvailableAsync();
    }
}