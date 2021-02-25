using OverroidModel.Card;

namespace OverroidModel.GameAction.Effects
{
    /// <summary>
    /// Effect resolving action of Doctor (4).
    /// </summary>
    public class JammingEffect : ICardEffectAction
    {
        readonly PlayerAccount controller;
        readonly CardName sourceCardName;

        /// <param name="controller">Card controller of the source card of the effect.</param>
        /// <param name="sourceCardName">Name of the card from which the effect was triggered.</param>
        internal JammingEffect(PlayerAccount controller, CardName sourceCardName)
        {
            this.controller = controller;
            this.sourceCardName = sourceCardName;
        }

        public PlayerAccount Controller => controller;

        public CardName? TargetCardName => null;

        public CardName? SecondTargetCardName => null;

        public CardName SourceCardName => sourceCardName;

        void IGameAction.Resolve(IMutableGame g)
        {
            g.DisableRoundEffects(g.CurrentBattle.Round, g.OpponentOf(controller));
        }
    }
}
