using OverroidModel.Card;

namespace OverroidModel.Game.Actions
{
    public class LifemakerEffect : BattleWinEffect
    {
        internal LifemakerEffect(PlayerAccount controller, CardName sourceCardName) : base(controller, sourceCardName)
        {
        }
    }
}
