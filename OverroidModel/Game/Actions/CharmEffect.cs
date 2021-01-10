using OverroidModel.Card;

namespace OverroidModel.Game.Actions
{
    public class CharmEffect : BattleWinEffect
    {
        internal CharmEffect(PlayerAccount controller, CardName sourceCardName) : base(controller, sourceCardName)
        {
        }
    }
}
