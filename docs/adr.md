# Architect Decision Record

## LOJA VIRTUAL DE CALÇADOS - 6/4/2025

### Autor

Lucas Ramos

### Status

Proposta

### Contexto

Criação de um sistema e-commerce para venda de calçados.

### Decisão

Estimamos atingir uma média diária de 10 mil visitantes / dia no primeiro semestre.

A fim de criar uma estruta escalável que possa comportar um rápido crescimento, teremos o sistema arquitetado com microserviços, sendo eles:

1. **Identity** para controle e cadastro de usuários, tanto clientes quando administradores (backoffice)
1. **ProductCatalog** para gerenciar o catálogo de produtos disponíveis dado momento
1. **Orders** para controle das compras, bem como seus status

Suporte:

1. **Mensageria** recebe as ordens de compra
1. **Worker** para processar os pagamentos, lendo a fila da mensageria
1. **Cache** do catálogo de produtos para melhor performance de leitura e busca de dados

### Tecnologia

- Microserviços usando .NET Core 9.0
- Worker para processamento dos pagamentos em .NET 9.0
- RabbitMQ
- Redis
- MongoDB
- Kubernetes
- Next.js