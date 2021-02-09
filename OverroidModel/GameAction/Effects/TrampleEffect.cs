using OverroidModel.Card;

namespace OverroidModel.GameAction.Effects
{
    /// <summary>
    /// Effect resolving action of Legion (10).
    /// </summary>
    public class TrampleEffect : ICardEffectAction
    {
        readonly PlayerAccount controller;
        readonly CardName sourceCardName;

        /// <param name="controller">Card controller of the source card of the effect.</param>
        /// <param name="sourceCardName">Name of the card from which the effect was triggered.</param>
        internal TrampleEffect(PlayerAccount controller, CardName sourceCardName)
        {
            this.controller = controller;
            this.sourceCardName = sourceCardName;
        }

        public PlayerAccount Controller => controller;

        public CardName? TargetCardName => null;

        public CardName? SecondTargetCardName => null;

        CardName ICardEffectAction.SourceCardName => sourceCardName;

        void IGameAction.Resolve(IMutableGame g)
        {
            g.DisableRoundEffects((ushort)(g.CurrentBattle.Round + 1), g.OpponentOf(controller));
        }
    }
}
