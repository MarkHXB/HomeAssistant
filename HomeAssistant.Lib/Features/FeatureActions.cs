using HomeAssistant.Lib.Features.Features;
using System.Reflection;

namespace HomeAssistant.Lib.Features
{
    public class FeatureActions : IFeatureActions
    {
        private FeatureBase _featureBase;

        public FeatureActions()
        {
            _featureBase = new FeatureBase();
        }

        public Feature? Get(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return _featureBase.Features.FirstOrDefault(x => x.Name == name);
        }

        public IEnumerable<Feature> GetAll()
        {
            return _featureBase.Features;
        }

        public async Task Execute(string? featureName, ExecuteAction actionName, Dictionary<string, string> parameters)
        {
            if(string.IsNullOrWhiteSpace(featureName))
            {
                throw new ArgumentNullException(nameof(featureName));
            }

            Feature? feature = Get(featureName);

            if (feature == null)
            {
                throw new ArgumentNullException(nameof(feature));
            }

            switch (actionName)
            {
                case ExecuteAction.RUN:
                    await _featureBase.Run(feature);
                    break;
                default: throw new ArgumentException("Not supported action");
            }
        }
    }
}
