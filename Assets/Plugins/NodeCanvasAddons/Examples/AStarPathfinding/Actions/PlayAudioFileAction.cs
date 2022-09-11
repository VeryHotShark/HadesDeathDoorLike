using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding.Examples
{
    [Category("Audio")]
    [Name("Play Audio File")]
    [ParadoxNotion.Design.Icon("CInputMusic")]
    public class PlayAudioFileAction : ActionTask
    {
        [RequiredField]
        public BBParameter<string> ClipName;
        [SliderField(0, 1)]
        public float Volume = 1;
        public bool waitUntilFinish;

        private AudioClip _clip;

        protected override string info
        {
            get { return string.Format("PlayAudio <b>{0}</b>", ClipName.value); }
        }

        protected override void OnExecute()
        {
            _clip = (AudioClip)Resources.Load(ClipName.value, typeof(AudioClip));
            AudioSource.PlayClipAtPoint(_clip, agent.transform.position, Volume);
            if (!waitUntilFinish)
            { EndAction(); }
        }

        protected override void OnUpdate()
        {
            if (elapsedTime >= _clip.length)
                EndAction();
        }
    }
}