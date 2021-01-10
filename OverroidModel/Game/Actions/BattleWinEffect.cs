using OverroidModel.Card;

namespace OverroidModel.Game.Actions
{
    public abstract class BattleWinEffect : IGameAction
    {
        readonly PlayerAccount controller;
        readonly CardName sourceCardName;

        protected BattleWinEffect(PlayerAccount controller, CardName sourceCardName)
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
        }
    }
}
