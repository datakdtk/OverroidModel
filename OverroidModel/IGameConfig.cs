namespace OverroidModel
{
    /// <summary>
    /// Interface to get customized game rule.
    /// </summary>
    public interface IGameConfig
    {

        /// <summary>
        /// Whether defending player detect a card in battles.
        /// </summary>
        bool DetectionAvailable { get; }

        /// <summary>
        /// Whether adds Watcher card to first deck and uses 14 cards for the game.
        /// </summary>
        bool UsesWatcher { get; }
    }
}
