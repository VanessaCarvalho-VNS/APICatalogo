
namespace APICatalogo.Logging
{

    public class CustomerLogger : ILogger
    {
        readonly string loggerName;

        readonly CustomLoggerProviderConfiguration loggerConfig;

        public CustomerLogger(string name, CustomLoggerProviderConfiguration Config)
        {
            loggerName = name;
            loggerConfig = Config;
           
        }

        public CustomLoggerProviderConfiguration Config { get; }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
           return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, 
            Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string mensagem = $"{logLevel.ToString()}: {eventId} - {formatter(state, exception)}";

            EscreverTextoNoAquivo(mensagem);
        }

        private void EscreverTextoNoAquivo(string mensagem)
        {
            string caminhoArquivoLog = @"c:\dados\log\vanessa_Log.txt";

            using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
            {
                try
                {
                    streamWriter.WriteLine(mensagem);
                    streamWriter.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

    }

}
