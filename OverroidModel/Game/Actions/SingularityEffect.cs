using OverroidModel.Card;

namespace OverroidModel.Game.Actions
{
    public class SingularityEffect : IGameAction
    {
        readonly PlayerAccount controller;
        readonly CardName sourceCardName;

        internal SingularityEffect(PlayerAccount controller,  CardName sourceCardName)
        {
            this.controller = controller;
            this.sourceCardName = sourceCardName;
        }

        public PlayerAccount Controller => controller;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => true;

        void IGameAction.Resolve(in IGame g)
        {
            g.SetSpecialWinner(g.CurrentBattle.PlayerOf(sourceCardName));
        }
    }
}
