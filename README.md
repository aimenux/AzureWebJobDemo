[![.NET](https://github.com/aimenux/AzureWebJobDemo/actions/workflows/ci.yml/badge.svg)](https://github.com/aimenux/AzureWebJobDemo/actions/workflows/ci.yml)

# AzureWebJobDemo
```
Using azure web jobs
```

> In this demo, i m using [webjob sdk v3](https://docs.microsoft.com/en-us/azure/app-service/webjobs-sdk-how-to#version-3x) in order to build producer/consumer webjobs.
>
> - `ProducerWebJob` add message to an azure queue based on a `timer trigger`
>
> - `ConsumerWebJob` get message from an azure queue based on a `queue trigger`

**`Tools`** : vs22, net 6, azure storage queue, serilog, application-insights