using OverroidModel.Card;

namespace OverroidModel.GameAction.Effects
{
    /// <summary>
    /// Effect resolving action of Overroid (12).
    /// </summary>
    public class SingularityEffect : ICardEffectAction
    {
        readonly PlayerAccount controller;
        readonly CardName sourceCardName;

        /// <param name="controller">Card controller of the source card of the effect.</param>
        /// <param name="sourceCardName">Name of the card from which the effect was triggered.</param>
        internal SingularityEffect(PlayerAccount controller, CardName sourceCardName)
        {
            this.controller = controller;
            this.sourceCardName = sourceCardName;
        }

        public PlayerAccount Controller => controller;

        public CardName? TargetCardName => null;

        CardName ICardEffectAction.SourceCardName => sourceCardName;

        void IGameAction.Resolve(IMutableGame g)
        {
            g.SetSpecialWinner(g.CurrentBattle.PlayerOf(sourceCardName));
        }
    }
}
