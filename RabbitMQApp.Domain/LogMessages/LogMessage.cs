namespace RabbitMQApp.Domain.LogMessages
{
    public static class LogMessage
    {
        public static string SuccessPublished => "Mensagem publicada com sucesso na fila do RabbitMQ.";
        public static string ErrorPublishing => "Erro ao publicar mensagem na fila do RabbitMQ.";
        public static string WorkerActive => $"Worker is running: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
    }
}
