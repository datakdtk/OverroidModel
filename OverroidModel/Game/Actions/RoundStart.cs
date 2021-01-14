using OverroidModel.Game.Actions.Commands;

namespace OverroidModel.Game.Actions
{
    /// <summary>
    /// Action to start new battle round.
    /// </summary>
    public class RoundStart : IGameAction
    {
        public PlayerAccount? Controller => null;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => false;

        void IGameAction.Resolve(IMutableGame g)
        {
            g.AddNewRound();
            g.AddCommandAuthorizer(new CommandAuthorizer<CardPlacement>(g.CurrentBattle.AttackingPlayer));
        }
    }
}
