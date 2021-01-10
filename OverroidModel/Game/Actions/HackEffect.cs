using OverroidModel.Card;

namespace OverroidModel.Game.Actions
{
    class HackEffect : IGameAction
    {
        readonly PlayerAccount controller;
        readonly CardName sourceCardName;

        internal HackEffect(PlayerAccount controller, CardName sourceCardName)
        {
            this.controller = controller;
            this.sourceCardName = sourceCardName;
        }

        public PlayerAccount Controller => controller;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => true;

        void IGameAction.Resolve(in IGame g)
        {
            var player = g.CurrentBattle.PlayerOf(sourceCardName);
            g.HandOf(player).RevealRandomCard();
        }
    }
}
