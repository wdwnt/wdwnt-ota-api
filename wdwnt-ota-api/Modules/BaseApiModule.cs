using Nancy;

namespace wdwnt_ota_api.Modules
{
    public abstract class BaseApiModule : NancyModule
    {
        private readonly string _route;

        protected BaseApiModule(string route) : base($"/api/{route}")
        {
            _route = route;
        }
    }
}