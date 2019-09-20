
namespace MusicPlugin.MusicProviders
{
    public abstract class MusicProvider
    {
        public abstract string Name { get; }
        public abstract void TurnOn( string musicRequest );
        public abstract void TurnOff();
        public abstract void Play();
        public abstract void Pause();
        public abstract void Next();
        public abstract void Previous();
        public abstract string WhoIsIt();
        public abstract bool IsPlaying();
    }
}
