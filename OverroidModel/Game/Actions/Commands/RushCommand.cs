using OverroidModel.Card;

namespace OverroidModel.Game.Actions.Commands
{
    public class RushCommand : IGameCommand
    {
        readonly PlayerAccount player;
        readonly CardName targetCardName;

        public RushCommand(PlayerAccount player, CardName targetCardName)
        {
            this.player = player;
            this.targetCardName = targetCardName;
        }

        public PlayerAccount Controller => CommandingPlayer;

        public PlayerAccount CommandingPlayer => player;

        public CardName? TargetCardName => targetCardName;

        public bool HasVisualEffect() => true;

        public bool IsCardEffect() => true;

        void IGameAction.Resolve(in IGame g)
        {
            var thisCard = g.CurrentBattle.CardOf(player);
            var targetCard = g.HandOf(player).RemoveCard(targetCardName);
            thisCard.SetGuessed();
            g.CurrentBattle.ReplaceCard(player, targetCard);
            g.HandOf(player).AddCard(targetCard);
        }
    }
}
