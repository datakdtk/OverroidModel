using OverroidModel.Card;

namespace OverroidModel.GameAction
{
    public class GameStart : IGameAction
    {
        public PlayerAccount? Controller => null;

        public CardName? TargetCardName => null;

        public CardName? SecondTargetCardName => null;

        void IGameAction.Resolve(IMutableGame g)
        {
            // This is dummy action. Do nothing.
        }
    }
}
