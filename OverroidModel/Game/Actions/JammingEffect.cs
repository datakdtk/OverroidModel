using OverroidModel.Card;

namespace OverroidModel.Game.Actions
{
    public class JammingEffect : IGameAction
    {
        readonly PlayerAccount controller;
        readonly CardName sourceCardName;

        internal JammingEffect(PlayerAccount controller, CardName sourceCardName)
        {
            this.controller = controller;
            this.sourceCardName = sourceCardName;
        }

        public PlayerAccount Controller => controller;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => true;

        void IGameAction.Resolve(in IGame g)
        {
            g.DisableRoundEffects(g.OpponentOf(controller), g.CurrentBattle.Round);
        }
    }
}
