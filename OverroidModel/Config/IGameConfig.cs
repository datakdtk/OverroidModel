using System;
using System.Collections.Generic;
using System.Text;

namespace OverroidModel.Config
{
    /// <summary>
    /// Interface to get customized game rule.
    /// </summary>
    interface IGameConfig
    {

        /// <summary>
        /// Whether defending player detect a card in battles.
        /// </summary>
        bool DetectionAvailable { get; }
    }
}
