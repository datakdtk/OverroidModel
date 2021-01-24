using OverroidModel.Card;

namespace OverroidModel.GameAction
{
    /// <summary>
    /// Effect resolving action of Diva (8).
    /// </summary>
    public class InspirationEfect : BattleWinEffect
    {
        /// <param name="controller">Card controller of the source card of the effect.</param>
        /// <param name="sourceCardName">Name of the card from which the effect was triggered.</param>
        internal InspirationEfect(PlayerAccount controller, CardName sourceCardName) : base(controller, sourceCardName)
        {
        }
    }
}
