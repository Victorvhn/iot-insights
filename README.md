# IoT Insights

## Decisões de Design e Implementação
Este arquivo README destina-se a fornecer uma visão geral das principais decisões de design e implementação adotadas no projeto, além de oferecer sugestões de melhorias e avanços futuros.

### Estrutura do Projeto
O projeto está organizado seguindo uma estrutura modular, onde cada componente é responsável por uma funcionalidade específica. A estrutura do projeto é a seguinte:

``` bash
./
│
├── src/                     # Código-fonte do projeto
│   ├── features/            # Funcionalidade do projeto
│   ├──── Authentication/    # Autenticação do usuário
│   ├────── Endpoint.cs      
│   ├──── Metrics/           # Metricas dos dispositivos
│   └────── Endpoint.cs      
│
├── tests/                # Testes do projeto
│   ├── features/            # Funcionalidade do projeto
│   ├──── Authentication/    # Testes de autenticação
│   └──── Metrics/           # Testes de metricas
│
└── README.md             # Este arquivo

```

### Decisões de Design e Implementação

1. **Padrão de Arquitetura Vertical Slices:** O projeto está estruturado na arquitetura de Vertical Slices. Por ser um projeto pequeno, com poucas funcionalidades, optei por seguir essa arquitetura devido à sua simplicidade, sem a complexidade de várias camadas. A arquitetura de Vertical Slices é mais fácil de entender e implementar, especialmente para projetos menores ou com requisitos menos complexos, como é o caso deste. Não exige a divisão do código em camadas distintas, como as arquiteturas hexagonais ou o domain driven design, o que facilita o desenvolvimento e a manutenção com base em funcionalidades.

2. **Utilização de Bibliotecas Externas:** Optei por utilizar bibliotecas externas quando apropriado, como o FluentResults para a implementação do Result Pattern, o FluentValidation para a validação das requisições do usuário, o Flurl para chamadas HTTP e também por fornecer uma estrutura completa para simulação de testes em comparação ao HttpClient padrão, e o Mediator para a implementação do Mediator Pattern para comunicação entre Endpoints e Serviços.

3. **Testes Automatizados:** Optei por realizar apenas testes de integração. Como a arquitetura utilizada não está tão desacoplada como encontraríamos em uma arquitetura de domain driven design, por exemplo, não consigo testar cada ponto da aplicação de forma isolada. Portanto, com os testes de integração, passamos por todos os componentes do sistema em um único teste, garantindo a integridade do código em um único modelo de testes.

## Sugestões de Melhorias e Avanços Futuros