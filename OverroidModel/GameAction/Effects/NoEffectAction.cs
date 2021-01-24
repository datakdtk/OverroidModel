using OverroidModel.Card;

namespace OverroidModel.GameAction.Effects
{
    class NoEffectAction : ICardEffectAction
    {
        readonly PlayerAccount controller;
        readonly CardName sourceCardName;

        internal NoEffectAction(PlayerAccount controller, CardName sourceCardName)
        {
            this.controller = controller;
            this.sourceCardName = sourceCardName;
        }
        public PlayerAccount? Controller => controller;

        public CardName? TargetCardName => null;

        CardName ICardEffectAction.SourceCardName => sourceCardName;

        void IGameAction.Resolve(IMutableGame g)
        {
            return; // Do nothing.
        }
    }
}
