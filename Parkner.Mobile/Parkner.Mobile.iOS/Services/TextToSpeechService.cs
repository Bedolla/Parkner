using AVFoundation;
using Parkner.Mobile.Services;
using System;
using System.Threading.Tasks;

namespace Parkner.Mobile.iOS.Services
{
    public class TextToSpeechService : ITextToSpeechService, IDisposable
    {
        private AVSpeechSynthesizer synthesizer;
        private TaskCompletionSource<bool> tcsUtterance;
        private AVSpeechUtterance utterance;

        public TextToSpeechService() => this.synthesizer = new AVSpeechSynthesizer();

        public void Dispose()
        {
            this.synthesizer.DidFinishSpeechUtterance -= this.OnFinishedSpeechUtterance;
            this.utterance.Dispose();
            this.synthesizer.Dispose();
            this.utterance = null;
            this.synthesizer = null;
        }

        public async Task SpeakAsync(string text)
        {
            this.tcsUtterance = new TaskCompletionSource<bool>();

            this.synthesizer.DidFinishSpeechUtterance += this.OnFinishedSpeechUtterance;
            this.utterance = new AVSpeechUtterance(text)
            {
                Rate = AVSpeechUtterance.MaximumSpeechRate / 3,
                Voice = AVSpeechSynthesisVoice.FromLanguage("en-US"),
                Volume = 1.0f,
                PitchMultiplier = 1.0f
            };
            this.synthesizer.SpeakUtterance(this.utterance);
            await this.tcsUtterance.Task;
        }

        private void OnFinishedSpeechUtterance(object sender, AVSpeechSynthesizerUteranceEventArgs e)
        {
            this.tcsUtterance?.TrySetResult(true);
        }
    }
}
