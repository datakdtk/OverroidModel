using OverroidModel.Card;
using OverroidModel.Card.Effects;
using OverroidModel.Exceptions;

namespace OverroidModel.Game.Actions
{
    public class CardOpen : IGameAction
    {

        public PlayerAccount? Controller => null;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => false;

        void IGameAction.Resolve(in IGame g)
        {
            var battle = g.CurrentBattle;
            if (!battle.HasBothPlayersCards())
            {
                throw new UnavailableActionException("Battle cards cannot open. Not all cards have been set");
            }
            battle.OpenCards();

            g.PushToActionStack(new BattleEnd());
            // set arrays in reverse order because stack resolves effects in "first in, last out" order
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
    }
}
