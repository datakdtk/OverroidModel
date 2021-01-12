using OverroidModel.Card;
using OverroidModel.Exceptions;

namespace OverroidModel.Game.Actions.Commands
{
    /// <summary>
    /// Effect resolving action of Solder (11).
    /// </summary>
    public class RushCommand : IGameCommand
    {
        readonly PlayerAccount player;
        readonly CardName targetCardName;

        /// <param name="player">Card controller of the source card of the effect.</param>
        /// <param name="targetCardName"></param>
        public RushCommand(PlayerAccount player, CardName targetCardName)
        {
            this.player = player;
            this.targetCardName = targetCardName;
        }

        public PlayerAccount Controller => CommandingPlayer;

        public PlayerAccount CommandingPlayer => player;

        public CardName? TargetCardName => targetCardName;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => true;

        void IGameAction.Resolve(in IGame g)
        {
            var thisCard = g.CurrentBattle.CardOf(player);
            var targetCard = g.HandOf(player).RemoveCard(targetCardName);
            thisCard.SetGuessed();
            g.CurrentBattle.ReplaceCard(player, targetCard);
            g.HandOf(player).AddCard(targetCard);
        }

        void IGameCommand.Validate(IGame g)
        {
            if (!g.HandOf(player).HasCard(targetCardName))
            {
                throw new UnavailableActionException("Player does not have Rush target card");
            }
        }
    }
}
