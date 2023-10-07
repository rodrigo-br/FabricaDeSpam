# Projetos

Os projetos construídos nessa solução são:

* [Producer](#producer)
* [Consumers](#consumers)
* [Domain](#domain)
* [Infrastructure](#infrastructure)
* [WebApi](#webapi)
* [WebApp](#webapp)
* [DockerCompose](#dockercompose)

## Producer

O Producer constrói uma mensagem para ser enviado a determinado tópico do Kafka.

O serviço utilizado para enviar a mensagem tenta conectar ao Kafka em loop no seu construtor, 
evitando que ele tente enviar uma mensagem caso o kafka esteja offline e
a mensagem se perca.

O producer não possui referência a nenhum outro projeto, sendo utilizado apenas
como serviço pela WebApi.

## Consumers

Todos os consumers possuem o objetivo de direcionar as mensagens para o
destinatário correto, por exemplo, uma mensagem que almeja enviar foto de um
gato por email, não deve enviar outro tipo de foto e nem enviar foto de gato
para o mobile.

Por isso, cada consumer é responsável pelo seu próprio tópico. Dessa forma,
caso o programa precise escalar (receber fotos de outros animais, por exemplo),
o consumer pode ser criado após os novos tópicos, pois o kafka irá armazenar
as mensagens mesmo que não haja nenhum consumer disponível. Além disso,
caso um consumer fique offline, não afetará todos envios de fotos, apenas
daquele consumer específico.

Os consumers possuem uma segurança extra para se manterem em loop sempre
recebendo mensagens e garantir que caso uma mensagem não seja enviada, ela
poderá ser consumida novamente para tentar uma nova entrega.

## Domain

Nesta solução, o Domain possui apenas as entidades e Dtos. Sem referência
a nenhum outro projeto.

## Infrastructure

O Infrastructure possui como referência o Domain, e serve como uma interface
para acesso ao banco de dados principal, incluindo serviços de segurança como
manipulação do JwT para autorizar acesso aos serviços mais sensíveis,
Identity para criação e utilização das contas de usuários, mapeamentos entre
Dto e entidades e o próprio repositório para manipulação do banco de dados.


## WebApi

A WebApi possui referência direta ao Infrastructure e ao Producer, utilizando
os serviços disponíveis conforme necessário através de injeção de dependência.

É responsável pela comunicação entre a aplicação Web, a Infrastructure e o
Producer, através de endpoints protegidos para serem acessados apenas
por requisição http pela aplicação Web.

## WebApp

A AplicaçãoWeb não possui referência com nenhum outro projeto e apenas faz requisições
diretamente para a WebApi, consumindo tokens através de sessões para conseguir
autorização para liberar suas funcionalidades, que inclui criação de conta, login,
enviar fotos e se cadastrar para receber fotos pelos tópicos disponíveis.

É a única parte de toda a solução que fica exposta ao usuário e lida com todas
a interações deste com os demais serviços.

## DockerCompose

Todos os projetos são orquestrados por um único docker-compose e um Dockerfile
para cada (exceto pelo Domain e Infrastructure).

Além dos projetos do ecossistema .NET, o docker-compose também possui
uma imagem para o Kafka e outra para o PostgreSQL.

É utilizado um arquivo .env para os valores sensíveis utilizados nesta solução,
como dados para acesso ao banco de dados, API Keys e outros.