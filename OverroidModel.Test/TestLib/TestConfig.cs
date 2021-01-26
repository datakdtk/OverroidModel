using OverroidModel.Config;

namespace OverroidModel.Test.TestLib
{
    class TestConfig : IGameConfig
    {
        public bool DetectionAvailable { get; set; }

        public bool UsesWatcher { get; set; }
    }
}
