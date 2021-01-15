using OverroidModel.Card;
using OverroidModel.Exceptions;

namespace OverroidModel.Game.Actions.Commands
{
    /// <summary>
    /// Action to place battle card from hand.
    /// </summary>
    public class CardPlacement : IGameCommand
    {
        readonly PlayerAccount player;
        readonly CardName cardNameToPlace;
        readonly CardName? detectedCardName;

        /// <param name="player">Player who places a card.</param>
        /// <param name="cardNameToPlace">Card to be placed.</param>
        /// <param name="detectedCardName">Called card name for detection if detected by defending player.</param>
        public CardPlacement(PlayerAccount player, CardName cardNameToPlace, CardName? detectedCardName)
        {
            this.player = player;
            this.cardNameToPlace = cardNameToPlace;
            this.detectedCardName = detectedCardName;
        }

        public PlayerAccount Controller => CommandingPlayer;

        public PlayerAccount CommandingPlayer => player;

        public CardName? TargetCardName => cardNameToPlace;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => false;

        void IGameAction.Resolve(IMutableGame g)
        {
            var card = g.HandOf(player).RemoveCard(cardNameToPlace);
            var battle = g.CurrentBattle;
            if (player == battle.AttackingPlayer)
            {
                battle.SetCard(player, card, null);
                g.AddCommandAuthorizer(new CommandAuthorizer<CardPlacement>(g.OpponentOf(player)));
            } 
            else
            {
                var detectedCardName = g.DetectionAvailable ? this.detectedCardName : null;

                battle.SetCard(player, card, detectedCardName);
                g.PushToActionStack(new CardOpen());
            }
        }

        void IGameCommand.Validate(IGameInformation g)
        {
            if (g.CurrentBattle.HasCardOf(player))
            {
                throw new UnavailableActionException("Invalid Command: player has already set card");
            }
            if (g.HandOf(player).HasCard(cardNameToPlace))
            {
                throw new UnavailableActionException("Invalid Command: player does not have the card to set");
            }
        }
    }
}
