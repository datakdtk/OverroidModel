using OverroidModel.Card;

namespace OverroidModel.Game.Actions
{
    public class MiracleEffect : IGameAction
    {
        readonly PlayerAccount controller;
        readonly CardName sourceCardName;

        internal MiracleEffect(PlayerAccount controller, CardName sourceCardName)
        {
            this.controller = controller;
            this.sourceCardName = sourceCardName;
        }

        public PlayerAccount Controller => controller;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => true;

        void IGameAction.Resolve(in IGame g)
        {
            var battle = g.CurrentBattle;
            battle.SetSpecialWinner(sourceCardName);
            g.SetSpecialWinner(battle.PlayerOf(sourceCardName));
        }
    }
}
