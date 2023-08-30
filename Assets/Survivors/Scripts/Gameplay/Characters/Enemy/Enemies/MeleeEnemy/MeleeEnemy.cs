namespace Survivors.Gameplay
{
    public class MeleeEnemy : Enemy
    {
        protected override SettingsBase GetStateMachineSettings()
        {
            return new MeleeEnemyStateMachine.Settings()
            {
                DamageTaker = damageTaker,
                Health = health,
                OnDead = EnemyDead,
                AttackTarget = GameplayManager.Instance.Player
            };
        }
    }
}
