using OverroidModel.Card;
using OverroidModel.Card.Effects;
using OverroidModel.Exceptions;

namespace OverroidModel.GameAction
{
    /// <summary>
    /// Action to open faced down battle cards.
    /// </summary>
    public class CardOpen : IGameAction
    {
        readonly PlayerAccount player;
        readonly CardName placedCardName;

        internal CardOpen(PlayerAccount player, CardName placedCardName)
        {
            this.player = player;
            this.placedCardName = placedCardName;
        }

        public PlayerAccount? Controller => player;

        public CardName? TargetCardName => placedCardName;

        public CardName? SecondTargetCardName => null;

        void IGameAction.Resolve(IMutableGame g)
        {
            var battle = g.CurrentBattle;
            Assertion(battle);

            battle.CardOf(player).Open();

            if (player == battle.AttackingPlayer)
            {
                var dp = battle.DefendingPlayer;
                g.PushToActionStack(new CardOpen(dp, battle.CardOf(dp).Name));
                return;
            }

            g.PushToActionStack(new BattleEnd());
            // set arrays in reverse of resolving order because stack resolves effects in "first in, last out" order
            var timings = new EffectTiming[3] { EffectTiming.PRE_BATTLE, EffectTiming.SECOND, EffectTiming.FIRST };
            var cards = new InGameCard[2] { battle.CardOf(battle.DefendingPlayer), battle.CardOf(battle.AttackingPlayer) };
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

        private void Assertion(Battle battle)
        {
            if (!battle.HasBothPlayersCards())
            {
                throw new GameLogicException("Battle cards cannot open. Not all cards have been set");
            }
            if (battle.CardOf(player).Name != placedCardName)
            {
                throw new GameLogicException("Card name to open must be same as actual card.");
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
        }
    }
}
