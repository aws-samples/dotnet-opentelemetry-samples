using OpenTelemetry;
using OpenTelemetry.Logs;

namespace SampleApi.Processors
{
    internal class LogExceptionStackTraceProcessor : BaseProcessor<LogRecord>
    {
        private readonly string name;

        public LogExceptionStackTraceProcessor(string name = "LogExceptionProcessor")
        {
            this.name = name;
        }

        public override void OnEnd(LogRecord record)
        {
            if (record.Exception != null)
            {
                record.Attributes = [.. record.Attributes!, new KeyValuePair<string, object?>("ProcessorName", name)];
                record.Attributes = [.. record.Attributes!, new KeyValuePair<string, object?>("exception.stacktrace", record!.Exception!.StackTrace)];
                record.Attributes = [.. record.Attributes!, new KeyValuePair<string, object?>("exception.innerexception", record!.Exception!.InnerException)];
            }
        }
    }

}
