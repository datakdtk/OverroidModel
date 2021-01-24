using OverroidModel.Card;
using OverroidModel.Exceptions;

namespace OverroidModel.GameAction.Commands
{
    /// <summary>
    /// Effect resolving action of Hack (2).
    /// </summary>
    public class HackCommand : IGameCommand
    {
        readonly PlayerAccount controller;
        readonly CardName targetCardName;

        /// <param name="controller">Card controller of the source card of the effect.</param>
        /// <param name="targetCardName">Card name to reveal</param>
        public HackCommand(PlayerAccount controller, CardName targetCardName)
        {
            this.controller = controller;
            this.targetCardName = targetCardName;
        }

        public PlayerAccount Controller => controller;

        public PlayerAccount CommandingPlayer => controller;

        public CardName? TargetCardName => targetCardName;

        void IGameAction.Resolve(IMutableGame g)
        {
            var opponentHand = g.HandOf(g.OpponentOf(CommandingPlayer));
            var card = opponentHand.CardOf(targetCardName);
            if (card == null)
            {
                throw new GameLogicException("Opponent does not have card to be revealed."); ;
            }
            card.RevealByHack();
        }

        void IGameCommand.Validate(IGameInformation g)
        {
            var opponentHand = g.HandOf(g.OpponentOf(CommandingPlayer));
            if (!opponentHand.HasCard(targetCardName))
            {
                throw new UnavailableActionException("Opponent does not have card to be revealed."); ;
            }
        }
    }
}
