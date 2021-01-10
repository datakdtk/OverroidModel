using OverroidModel.Card;

namespace OverroidModel.Game.Actions
{
    public class ReversalEffect : IGameAction
    {
        readonly PlayerAccount controller;
        readonly CardName sourceCardName;

        internal ReversalEffect(PlayerAccount controller, CardName sourceCardName)
        {
            this.controller = controller;
            this.sourceCardName = sourceCardName;
        }

        public PlayerAccount Controller => controller;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => true;

        void IGameAction.Resolve(in IGame g)
        {
            g.CurrentBattle.SetToReverseCardValues();
        }
    }
}
