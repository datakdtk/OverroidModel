using OverroidModel.Card;

namespace OverroidModel.GameAction.Effects
{
    /// <summary>
    /// Effect resolving action of Watcher (0). Effect name is not official.
    /// </summary>
    public class PredictEffect : ICardEffectAction
    {
        readonly PlayerAccount controller;
        readonly CardName sourceCardName;

        /// <param name="controller">Card controller of the source card of the effect.</param>
        /// <param name="sourceCardName">Name of the card from which the effect was triggered.</param>
        internal PredictEffect(PlayerAccount controller, CardName sourceCardName)
        {
            this.controller = controller;
            this.sourceCardName = sourceCardName;
        }

        public CardName SourceCardName => sourceCardName;

        public PlayerAccount? Controller => controller;

        public CardName? TargetCardName => null;

        public CardName? SecondTargetCardName => null;

        void IGameAction.Resolve(IMutableGame g)
        {
            g.HiddenCard.RevealTo(controller);
        }
    }
}
