using OverroidModel.Card;
using OverroidModel.Exceptions;
using System.Diagnostics;

namespace OverroidModel.Game.Actions.Commands
{
    /// <summary>
    /// Effect resolving action of Spy (7).
    /// </summary>
    public class EspionageCommand : IGameCommand
    {
        readonly PlayerAccount player;
        readonly CardName targetMyCardName;
        readonly CardName? targetOpponentCardName;

        /// <param name="player">Card controller of the source card of the effect.</param>
        /// <param name="targetMyCardName">Name of card which will be chosen from the card controller's hand.</param>
        /// <param name="targetOpponentCardName">Name of card which will be chosen from the opponent's hand. If it is null, chosen randomly.</param>
        public EspionageCommand(PlayerAccount player, CardName targetMyCardName, CardName? targetOpponentCardName)
        {
            this.player = player;
            this.targetMyCardName = targetMyCardName;
            this.targetOpponentCardName = targetOpponentCardName;
        }

        public PlayerAccount Controller => CommandingPlayer;

        public PlayerAccount CommandingPlayer => player;

        public CardName? TargetCardName => null;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => true;

        void IGameAction.Resolve(in IGame g)
        {
            var myHand = g.HandOf(player);
            var opponentHand = g.HandOf(g.OpponentOf(player));
            Debug.Assert(myHand.Cards.Count == opponentHand.Cards.Count);
            var myCard = myHand.RemoveCard(targetMyCardName);
            var opponentCard = targetOpponentCardName == null
                ? opponentHand.RemoveRandomUnrevealedCard()
                : opponentHand.RemoveCard(targetOpponentCardName.Value);
            myCard.SetGuessed();
            opponentCard.SetGuessed();
            myHand.AddCard(opponentCard);
            opponentHand.AddCard(myCard);
            Debug.Assert(myHand.Cards.Count == opponentHand.Cards.Count);
        }

        void IGameCommand.Validate(IGame g)
        {
            if (!g.HandOf(player).HasCard(targetMyCardName))
            {
                throw new UnavailableActionException("Invalid Command: player does not have the card to exchange");
            }
            var opponentHand = g.HandOf(g.OpponentOf(player));
            if (targetOpponentCardName == null)
            {
                if (opponentHand.UnrevealedCardCount == 0)
                {
                    throw new UnavailableActionException("Invalid Command: cannot choose opponent at random. There is no unrevealed card");
                }
            } 
            else
            {
                if (!opponentHand.HasCard((CardName)targetOpponentCardName))
                {
                    throw new UnavailableActionException("Invalid Command: opponent does not have the card to exchange");
                }
            }
        }
    }
}
