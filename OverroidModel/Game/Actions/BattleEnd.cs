using OverroidModel.Card;
using OverroidModel.Card.Effects;

namespace OverroidModel.Game.Actions
{
    /// <summary>
    /// Action to compare the card values and determine the battle winner.
    /// </summary>
    class BattleEnd : IGameAction
    {

        public PlayerAccount? Controller => null;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => false;

        void IGameAction.Resolve(IMutableGame g)
        {
            var battle = g.CurrentBattle;
            battle.JudgeWinnerByValues();

            if (!g.HasFinished())
            {
                g.PushToActionStack(new RoundStart());
            }
            var cards = new InGameCard[2] { battle.CardOf(battle.DefendingPlayer), battle.CardOf(battle.AttackingPlayer) };
            foreach (var c in cards)
            {
                if (c.Effect.Timing == EffectTiming.POST_BATTLE && c.Effect.ConditionIsSatisfied(c.Name, g))
                    {
                        g.PushToActionStack(c.Effect.GetAction(c.Name, g));
                    }
            }
        }
    }
}
