using OverroidModel.Card;

namespace OverroidModel.Game.Actions
{
    public class TrampleEffect : IGameAction
    {
        readonly PlayerAccount controller;
        readonly CardName sourceCardName;

        internal TrampleEffect(PlayerAccount controller, CardName sourceCardName)
        {
            this.controller = controller;
            this.sourceCardName = sourceCardName;
        }

        public PlayerAccount Controller => controller;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => true;

        void IGameAction.Resolve(in IGame g)
        {
            g.DisableRoundEffects(g.OpponentOf(controller), (ushort)(g.CurrentBattle.Round + 1));
        }
    }
}
