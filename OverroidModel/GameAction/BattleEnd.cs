using OverroidModel.Card;
using OverroidModel.Card.Effects;

namespace OverroidModel.GameAction
{
    /// <summary>
    /// Action to compare the card values and determine the battle winner.
    /// </summary>
    public class BattleEnd : IGameAction
    {

        public PlayerAccount? Controller => null;

        public CardName? TargetCardName => null;

        public CardName? SecondTargetCardName => null;

        void IGameAction.Resolve(IMutableGame g)
        {
            var battle = g.CurrentBattle;
            battle.Finish();

            if (!g.HasFinished())
            {
                g.PushToActionStack(new RoundStart());
            }
            var timings = new EffectTiming[2] { EffectTiming.POST_BATTLE, EffectTiming.SINGULARITY };
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
    }
}
