using OverroidModel.Card;
using System.Diagnostics;

namespace OverroidModel.Game.Actions.Commands
{
    public class EspionageCommand : IGameCommand
    {
        readonly PlayerAccount player;
        readonly CardName targetMyCardName;
        readonly CardName? targetOpponentCardName;

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
    }
}
