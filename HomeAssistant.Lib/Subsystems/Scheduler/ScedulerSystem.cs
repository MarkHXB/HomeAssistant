using SubSystemComponent;

namespace Scheduler
{
    /// <summary>
    /// Following parameters should pass:
    /// None
    /// </summary>
    public class SchedulerSystem : Subsystem
    {
        private Dictionary<string, string> _params;
        private List<Action> _actions;
        private int _maxRuns;

        public SchedulerSystem(Dictionary<string, string> @params, params Subsystem[] dependencies) :
            base(ConfigObject.LogFilePath, @params, dependencies)
        {
            _params = @params;
            _actions = new List<Action>();
            _maxRuns = 1; // default value
        }

        public override void Initialize()
        {
            ConfigHandler configHandler = new ConfigHandler(ConfigObject.ConfigFilePath);
            var config = configHandler.LoadConfig<ConfigObject>();
            
            _maxRuns = config?.MaxRunsPerMinute ?? 1;
        }

        public override async Task TaskObject(CancellationToken cancellationToken)
        {
            int interval = (60 / _maxRuns) * 1000; // calculate the interval in milliseconds
            await Task.Factory.StartNew(() => _actions[0]);
            //Timer timer = new Timer((state) =>
            //{
            //    for (int i = 0; i < _maxRuns; i++)
            //    {
            //        Action action = null;
            //        lock (_actions)
            //        {
            //            if (_actions.Count > 0)
            //            {
            //                action = _actions[0];
            //                _actions.RemoveAt(0);
            //            }
            //        }

            //        if (action != null)
            //        {
            //          // run the action in its own thread
            //        }
            //    }
            //}, null, interval, -1);

            //await Task.Delay(-1, cancellationToken);
        }

        public void AssignAction(Action runAction) => _actions.Add(runAction);
    }
}