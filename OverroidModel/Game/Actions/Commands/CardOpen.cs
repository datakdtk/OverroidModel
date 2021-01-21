using OverroidModel.Card;
using OverroidModel.Card.Effects;
using OverroidModel.Exceptions;

namespace OverroidModel.Game.Actions.Commands
{
    /// <summary>
    /// Action to open faced down battle cards.
    /// </summary>
    public class CardOpen : IGameCommand
    {
        readonly PlayerAccount player;
        readonly CardName placedCardName;

        /// <param name="player">Player who places a card.</param>
        /// <param name="placedCardName">Name of card set to the battle.</param>
        public CardOpen(PlayerAccount player, CardName placedCardName)
        {
            this.player = player;
            this.placedCardName = placedCardName;
        }

        public PlayerAccount? Controller => player;

        public PlayerAccount CommandingPlayer => player;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => false;

        void IGameAction.Resolve(IMutableGame g)
        {
            var battle = g.CurrentBattle;
            var card = battle.CardOf(player);
            card.Open();

            if (player == battle.AttackingPlayer)
            {
                g.AddCommandAuthorizer(new CommandAuthorizer<CardOpen>(battle.DefendingPlayer));
                return;
            }

            g.PushToActionStack(new BattleEnd());
            // set arrays in reverse of resolving order because stack resolves effects in "first in, last out" order
            var cards = new InGameCard[2] { battle.CardOf(battle.DefendingPlayer), battle.CardOf(battle.AttackingPlayer) };
            var timings = new EffectTiming[3] { EffectTiming.PRE_BATTLE, EffectTiming.SECOND, EffectTiming.FIRST };
            foreach (var t in timings)
            {
                foreach (var c in cards)
                {
                    if (c.Effect.Timing == t && c.Effect.ConditionIsSatisfied(c.Name, g))
                    {
                        g.PushToActionStack(c.Effect.GetAction(c.Name, g));
                    }
                }
            }
        }

        void IGameCommand.Validate(IGameInformation g)
        {
            var battle = g.CurrentBattle;
            // Assertion of game state. Expected to be never thrown because command .
            if (!battle.HasBothPlayersCards())
            {
                throw new GameLogicException("Battle cards cannot open. Not all cards have been set");
            }
            var attakingPlayerCard = battle.CardOf(battle.AttackingPlayer);
            if (player == battle.AttackingPlayer)
            {
                if (attakingPlayerCard.Visibility == CardVisibility.Opened)
                {
                    throw new GameLogicException("Attacking player has already opened a card.");
                }
            }
            else
            {
                if (attakingPlayerCard.Visibility != CardVisibility.Opened)
                {
                    throw new GameLogicException("Attacking player has not opened a card yet.");
                }
            }
            // Assertion end.

            var card = battle.CardOf(player);
            if (battle.CardOf(player).Name != placedCardName)
            {
                throw new UnavailableActionException("Card name to open must be same as actual card.");
            }
        }
    }
}
