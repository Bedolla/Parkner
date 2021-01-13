using System.Threading.Tasks;

namespace Parkner.Mobile.Services
{
    public interface ITextToSpeechService
    {
        Task SpeakAsync(string text);
    }
}
