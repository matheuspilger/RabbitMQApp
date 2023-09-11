# RabbitMQ Application

Essa aplicação tem como finalidade receber nomes de varios arquivos de acordo com a máscara ABCD_SMSSender_20230909083515.txt e realizar uma contagem de arquivos recebidos.

Para o projeto foi utilizado as seguintes itens:

- Docker
- .NET 7.0
- RabbitMQ
- OpenTelemetry
- Jaeger
- Unit Tests

Com base nisso, a aplicação ira realizar o tracing com ajuda do OpenTelemetry e exibirá as métricas no Jaeger.

Para enviar mensagem a fila do RabbitMQ foi criado um endpoint.

- Endpoint do Jaeger: http://localhost:16686
- Endpoint da Aplicação: https://localhost:32769/swagger/index.html
## Documentação da API

#### Enviar mensagens a fila do RabbitMQ

```https
  POST /CreateMessage
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `filenames` | `CreateMessageRequest` | **Obrigatório**. JSON: {"fileNames": ["string" ]} |
