using HomeAssistant.Lib.Features.Features;

namespace HomeAssistant.Lib.Features
{
    internal interface IFeatureActions
    {
        Feature? Get(string name);
        IEnumerable<Feature> GetAll();
        Task Execute(string featureName, ExecuteAction actionName, Dictionary<string, string> parameters);
    }
}
