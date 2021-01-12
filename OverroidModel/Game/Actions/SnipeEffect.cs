using OverroidModel.Card;

namespace OverroidModel.Game.Actions
{
    /// <summary>
    /// Effect resolving action of Death (13).
    /// </summary>
    public class SnipeEffect : IGameAction
    {
        readonly PlayerAccount controller;
        readonly CardName sourceCardName;

        /// <param name="controller">Card controller of the source card of the effect.</param>
        /// <param name="sourceCardName">Name of the card from which the effect was triggered.</param>
        internal SnipeEffect(PlayerAccount controller, CardName sourceCardName)
        {
            this.controller = controller;
            this.sourceCardName = sourceCardName;
        }

        public PlayerAccount Controller => controller;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => true;

        void IGameAction.Resolve(in IGame g)
        {
            g.CurrentBattle.CardOf(sourceCardName).OverrideValue(0);
        }
    }
}
