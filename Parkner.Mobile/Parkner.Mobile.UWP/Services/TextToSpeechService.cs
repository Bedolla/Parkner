using Parkner.Mobile.Services;
using System;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Media.SpeechSynthesis;

namespace Parkner.Mobile.UWP.Services
{
    public class TextToSpeechService : ITextToSpeechService, IDisposable
    {
        private MediaPlayer mediaPlayer;
        private SpeechSynthesisStream stream;
        private SpeechSynthesizer synthesizer;
        private TaskCompletionSource<bool> tcsUtterance;

        public TextToSpeechService()
        {
            this.mediaPlayer = new MediaPlayer();
            this.synthesizer = new SpeechSynthesizer();
        }

        public void Dispose()
        {
            this.stream.Dispose();
            this.stream = null;
            this.synthesizer.Dispose();
            this.synthesizer = null;
            this.mediaPlayer.MediaEnded -= this.OnMediaPlayerEnded;
            this.mediaPlayer.Dispose();
            this.mediaPlayer = null;
        }

        public async Task SpeakAsync(string text)
        {
            this.tcsUtterance = new TaskCompletionSource<bool>();

            try
            {
                this.stream = await this.synthesizer.SynthesizeTextToStreamAsync(text);

                this.mediaPlayer.MediaEnded += this.OnMediaPlayerEnded;
                this.mediaPlayer.Source = MediaSource.CreateFromStream(this.stream, this.stream.ContentType);
                this.mediaPlayer.Play();

                await this.tcsUtterance.Task;
            }
            catch (Exception ex)
            {
                this.tcsUtterance.TrySetException(ex);
            }
        }

        private void OnMediaPlayerEnded(MediaPlayer sender, object args) => this.tcsUtterance.TrySetResult(true);
    }
}
