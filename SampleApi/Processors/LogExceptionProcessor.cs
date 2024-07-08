using OpenTelemetry;
using OpenTelemetry.Logs;

namespace SampleApi.Processors
{
    internal class LogStackTraceExceptionProcessor : BaseProcessor<LogRecord>
    {
        private readonly string name;

        public LogStackTraceExceptionProcessor(string name = "LogExceptionProcessor")
        {
            this.name = name;
        }

        public override void OnEnd(LogRecord record)
        {
            if (record.Exception != null)
            {
                record.Attributes = record!.Attributes!.Append(new KeyValuePair<string, object?>("exception.stacktrace", record!.Exception!.StackTrace)).ToList();
                record.Attributes = record!.Attributes!.Append(new KeyValuePair<string, object?>("exception.innerexception", record!.Exception!.InnerException!)).ToList();

            }
        }
    }

}
