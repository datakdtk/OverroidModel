using OverroidModel.Card;

namespace OverroidModel.Game.Actions
{
    /// <summary>
    /// Common implement for auto-win card effects.
    /// </summary>
    public abstract class BattleWinEffect : IGameAction
    {
        readonly PlayerAccount controller;
        readonly CardName sourceCardName;

        /// <param name="controller">Card controller of the source card of the effect.</param>
        /// <param name="sourceCardName">Name of the card from which the effect was triggered.</param>
        protected BattleWinEffect(PlayerAccount controller, CardName sourceCardName)
        {
            this.controller = controller;
            this.sourceCardName = sourceCardName;
        }

        public PlayerAccount Controller => controller;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => true;

        void IGameAction.Resolve(IMutableGame g)
        {
            var battle = g.CurrentBattle;
            battle.SetSpecialWinner(sourceCardName);
        }
    }
}
