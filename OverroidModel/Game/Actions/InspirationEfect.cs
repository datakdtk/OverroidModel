using OverroidModel.Card;

namespace OverroidModel.Game.Actions
{
    public class InspirationEfect : BattleWinEffect
    {
        internal InspirationEfect(PlayerAccount controller, CardName sourceCardName) : base(controller, sourceCardName)
        {
        }
    }
}
