using OverroidModel.Card;

namespace OverroidModel.Game.Actions
{
    /// <summary>
    /// Effect resolving action of Creator (3).
    /// </summary>
    public class LifemakerEffect : BattleWinEffect
    {
        /// <param name="controller">Card controller of the source card of the effect.</param>
        /// <param name="sourceCardName">Name of the card from which the effect was triggered.</param>
        internal LifemakerEffect(PlayerAccount controller, CardName sourceCardName) : base(controller, sourceCardName)
        {
        }
    }
}
