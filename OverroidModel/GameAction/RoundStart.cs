using OverroidModel.Card;
using OverroidModel.GameAction.Commands;

namespace OverroidModel.GameAction
{
    /// <summary>
    /// Action to start new battle round.
    /// </summary>
    public class RoundStart : IGameAction
    {
        public PlayerAccount? Controller => null;

        public CardName? TargetCardName => null;

        void IGameAction.Resolve(IMutableGame g)
        {
            g.AddNewRound();
            g.AddCommandAuthorizer(new CommandAuthorizer<CardPlacement>(g.CurrentBattle.AttackingPlayer));
        }
    }
}
