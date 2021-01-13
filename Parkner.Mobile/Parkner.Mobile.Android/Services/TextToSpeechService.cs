using Android.Speech.Tts;
using Parkner.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Object = Java.Lang.Object;

namespace Parkner.Mobile.Droid.Services
{
    public class TextToSpeechService : Object, ITextToSpeechService, TextToSpeech.IOnInitListener,
                                       #pragma warning disable CS0618
                                       TextToSpeech.IOnUtteranceCompletedListener
        #pragma warning restore CS0618
    {
        private TaskCompletionSource<bool> tcsInitialize;
        private TaskCompletionSource<bool> tcsUtterance;
        private TextToSpeech Speaka { get; set; }

        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
                this.tcsInitialize.TrySetResult(true);
            else
                this.tcsInitialize.TrySetException(new ArgumentException("Failed to initialize TTS engine."));
        }

        public void OnUtteranceCompleted(string utteranceId) => this.tcsUtterance.TrySetResult(true);

        public async Task SpeakAsync(string text)
        {
            await this.Initialize();

            if (this.tcsUtterance?.Task != null) await this.tcsUtterance.Task;

            this.tcsUtterance = new TaskCompletionSource<bool>();

            // Set the utterance id so the completed listener fires

            // Use an obsolete overload so it works on older API levels
            #pragma warning disable CS0618
            this.Speaka.Speak(text, QueueMode.Flush, new Dictionary<string, string> {{TextToSpeech.Engine.KeyParamUtteranceId, new Guid().ToString()}});
            #pragma warning restore CS0618
            await this.tcsUtterance.Task;
        }

        private Task<bool> Initialize()
        {
            if (this.tcsInitialize != null) return this.tcsInitialize.Task;

            this.tcsInitialize = new TaskCompletionSource<bool>();
            try
            {
                this.Speaka = new TextToSpeech(MainActivity.Instancia, this);
                #pragma warning disable CS0618
                this.Speaka.SetOnUtteranceCompletedListener(this);
                #pragma warning restore CS0618
            }
            catch (Exception ex)
            {
                this.tcsInitialize.TrySetException(ex);
            }
            return this.tcsInitialize.Task;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Speaka?.Stop();
                this.Speaka?.Shutdown();
                this.Speaka = null;
            }

            base.Dispose(disposing);
        }
    }
}
