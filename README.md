# Mentoria .NET | Backend

## Projeto estudo - e-Commerce >> sapatos

Microservices

- marca
- modelo
- tamanho

Pode vender outras categorias como camiseta, bermuda

### Catalogo

Peciso catalogo, dados de cada produto

- foto,
- detalhes
- preço

colocar no carrinho compras
- retirar
- finalizar compra

Cache e validacao

### Pagamento

- pagamento assincrono, ao pagar manda uma mensgem e deixa na fila
tem que mandr mensagem deopis que deu bom ou ruim e alternativas

10 req por segundo do gateway pgto

Pedido precisa de estatus

Estoque de itens mas tem prevendas se tiver no carrinho (concorrencia)

### Relatorios admin

- Faturamento
por cat. talvez quartil, etc. Controle de estoque

Webhooks  e mensagerias comunicacao entre servicos

### User Auth


## Homework


Abrir o mapa mental que tenho que escrever


desenhar documento

começar a falar de tecnologias que podemos usar pra cada problema

## Redis

cache distribuido todo mundo acessa
regra microservio: um nao pode acesar o banco de outro direto, por exemplo

um servico alimenta o cache e os outros leem dele

## Documentos

Documentar tecnologias - ADR >> mudança ou criaç`áo de arquitetura

objetivo ar um proposta de arch pro cliente explicando por que dos porques
justificativas, beneficios, alternativas condieradas etc.

Colocar as tec. usadas

estilo de doc que posso usar no dia a dia!


