using OpenTelemetry;
using OpenTelemetry.Logs;

namespace SampleApi.Processors
{
    public class LogEnrichProcessor : BaseProcessor<LogRecord>
    {
        private readonly string name;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogEnrichProcessor(IHttpContextAccessor? httpContextAccessor, string name = "LogEnrichProcessor")
        {
            this.name = name;
            _httpContextAccessor = httpContextAccessor;
        }

        public override void OnEnd(LogRecord record)
        {
            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            record.Attributes = record!.Attributes!.Append(new KeyValuePair<string, object>("Username", string.IsNullOrEmpty(username) ? "Anonymous" : username)).ToList();
        }
    }
}
