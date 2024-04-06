# IoT Insights

## Decisões de Design e Implementação
Este arquivo README destina-se a fornecer uma visão geral das principais decisões de design e implementação adotadas no projeto, além de oferecer sugestões de melhorias e avanços futuros.

### Estrutura do Projeto
O projeto está organizado seguindo uma estrutura modular, onde cada componente é responsável por uma funcionalidade específica. A estrutura do projeto é a seguinte:

``` bash
./
│
├── src/IoT.Insights.Api           # Código-fonte do projeto
│   ├── Features/                  # Funcionalidades disponíveis no projeto
│   ├──── Authentication/          # Funcionalidades relacionadas à autenticação dos usuários
│   ├────── ...              
│   ├──── Metrics/                 # Funcionalidades relacionadas às métricas dos dispositivos
│   ├────── ...
│   ├── HttpClients/               # Configurações e implementações de clientes HTTP
│   ├────── ...
│   ├── TcpClients/                # Configurações e implementações de clientes TCP
│   ├────── ...
│   ├── Infrastructure/            # Configurações e implementações da infraestrutura do projeto
│   └────── ...    
│
├── tests/IoT.Insights.Api.Tests   # Testes do projeto
│   ├── Features/            
│   ├──── Authentication/          # Testes das funcionalidades de autenticação
│   ├────── ...              
│   ├──── Metrics/                 # Testes das funcionalidades de métricas
│   └────── ...              
│
└── README.md                       # Este arquivo
```

### Decisões de Design e Implementação

#### Diagramas

##### Diagrama de casos de uso
![Diagrama de caso de uso](https://github.com/Victorvhn/iot-insights/assets/38699623/81975356-e0bd-4fc5-8b41-4454316bd344)

--- 

##### Diagrama de sequência
![Diagrama de sequência](https://github.com/Victorvhn/iot-insights/assets/38699623/d27762c6-7f70-4faa-8335-c6cabc2affe5)

--- 

##### Estrutura

1. **Padrão de Arquitetura:** O projeto está estruturado na arquitetura [Vertical Slice](https://www.jimmybogard.com/vertical-slice-architecture/). Por ser um projeto pequeno, com poucas funcionalidades, optei por seguir essa arquitetura devido à sua simplicidade, sem a complexidade de várias camadas. A arquitetura [Vertical Slice](https://www.jimmybogard.com/vertical-slice-architecture/) é mais fácil de entender e implementar, especialmente para projetos menores ou com requisitos menos complexos, como é o caso deste. Não exige a divisão do código em camadas distintas, como as arquiteturas [limpas](https://medium.com/luizalabs/descomplicando-a-clean-architecture-cf4dfc4a1ac6) ou [hexagonais](https://medium.com/tableless/desvendando-a-arquitetura-hexagonal-52c56f8824c), o que facilita o desenvolvimento e a manutenção com base em funcionalidades.

2. **Utilização de Bibliotecas Externas:** Optei por utilizar bibliotecas externas quando apropriado, como o [FluentResults](https://github.com/altmann/FluentResults) para a implementação do [Result Pattern](https://medium.com/@wgyxxbf/result-pattern-a01729f42f8c), o [FluentValidation](https://github.com/FluentValidation/FluentValidation) para a validação dos dados das requisições, o [Flurl](https://github.com/tmenier/Flurl) para chamadas HTTP e também por fornecer uma estrutura completa para simulação de testes em comparação ao HttpClient padrão, e o [Mediator](https://github.com/martinothamar/Mediator) para a implementação do [Mediator Pattern](https://refactoring.guru/design-patterns/mediator) para comunicação entre Endpoints e Serviços.

3. **Testes Automatizados:** Optei por realizar apenas testes de integração. Como a arquitetura utilizada não é desacoplada como encontraríamos em uma arquitetura hexagonal, por exemplo, não consigo testar cada ponto da aplicação de forma isolada. Portanto, com os testes de integração, passamos por todos os componentes do sistema em um único teste, garantindo a integridade do código em um único modelo de testes.

## Sugestões de Melhorias e Avanços Futuros

1. **Background jobs:** Como ponto de melhoria, podemos considerar a implementação de uma rotina em background para otimizar o acesso aos dados de cadastros de dispositivos. Dado que os dados de cadastros de um dispositivo não mudam com frequência, podemos criar uma rotina que execute, por exemplo, a cada hora, buscando os dispositivos cadastrados com seus dados, realizando a filtragem necessária e armazenando-os em um banco de dados próprio da aplicação.
Com essa abordagem, quando o usuário solicitar métricas, a aplicação apenas precisaria conectar-se ao dispositivo para buscar as métricas, uma vez que os dados dos dispositivos cadastrados já estariam disponíveis. Além disso, podemos ir além e adicionar outra rotina, por exemplo, que execute a cada 10 minutos e que busque as métricas de cada dispositivo armazenado na base de dados (obtidos na rotina anterior) e as armazene igualmente. Dessa forma, quando o usuário solicitar essas métricas, nenhuma chamada externa além do banco de dados seria necessária, aumentando significativamente o desempenho da aplicação.

2. **Cache:** Se considerarmos a implementação anterior das rotinas em background, sabendo que as métricas dos dispositivos atualizam a cada 10 minutos, por exemplo, podemos adicionar um serviço de cache para tornar ainda mais rápido a obtenção desses dados. Com isso, apenas a primeira chamada à aplicação acessaria o banco de dados e armazenaria os dados retornados em memória, e todas as chamadas subsequentes buscariam nesses dados cacheados. Quando a rotina para atualizar métricas fosse executada, os dados de cache seriam invalidados/substituídos, reiniciando assim o ciclo anterior.

3. **Redução de chamadas HTTP:** Se for possível fazer uma alteração na API de consulta de dispositivos, uma opção viável seria modificar a rota de retorno dos dados dos dispositivos. Em vez de precisar buscar uma lista de IDs primeiro e depois realizar uma consulta para cada ID retornado para obter os dados, seria interessante ter uma rota que retorne todos os dispositivos junto com seus respectivos dados, permitindo filtrar pelos dados necessários (por exemplo, fabricante e comandos disponíveis). Isso reduziria significativamente o número de consultas externas.
