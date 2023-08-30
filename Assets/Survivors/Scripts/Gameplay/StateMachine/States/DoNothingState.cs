using Survivors.AI;

namespace Survivors.Gameplay
{
    public class DoNothingState : State
    {
        public override void SetSettings(StateSettingsBase _stateSettings)
        {
        }

        public override bool CanEnter()
        {
            return true;
        }

        public override bool IsStateWorking()
        {
            return true;
        }

        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
        }

        public override void Continue()
        {
        }

        public override void Pause()
        {
        }
    }
}
