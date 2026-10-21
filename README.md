# TiaIdentity: Autenticação, Autorização e Simplicidade

Biblioteca para simplificar a implementação de autenticação e autorização em aplicações ASP.NET Core.

## Manual de Utilização

### 1. Instalação

Instale o pacote NuGet do TiaIdentity:

[**TiaIdentity — NuGet**](https://www.nuget.org/packages/TiaIdentity/)

### 2. Configuração no Startup

Adicione os métodos `services.AddTiaIdentity()` e `app.UseTiaIdentity()` no arquivo `Startup.cs` do projeto.

Consulte o exemplo de configuração:

[Ver Startup.cs](https://github.com/hsbtiago/TiaIdentity/blob/master/exemplo/Startup.cs)

### 3. Configuração de Cookies

Configure os caminhos das páginas de login, acesso negado e demais opções de autenticação utilizando o método `AddCookie()`.

Consulte o [Startup.cs do projeto de exemplo](https://github.com/hsbtiago/TiaIdentity/blob/master/exemplo/Startup.cs).

### 4. Injeção de Dependência

Receba a dependência de `TiaIdentity.Autenticador` na controller responsável pela autenticação.

Exemplo:

[Ver AutenticacaoController.cs](https://github.com/hsbtiago/TiaIdentity/blob/master/exemplo/Controllers/AutenticacaoController.cs)

### 5. Login e Logout

Utilize os métodos assíncronos `LoginAsync` e `LogoutAsync` conforme a necessidade da aplicação.

Consulte a implementação no arquivo [AutenticacaoController.cs](https://github.com/hsbtiago/TiaIdentity/blob/master/exemplo/Controllers/AutenticacaoController.cs).

### 6. Sobrecargas do LoginAsync

O método `LoginAsync` possui duas sobrecargas:

- Uma que recebe os parâmetros individualmente.
- Outra que utiliza uma implementação da interface `TiaIdentity.IUsuario`.

Essa flexibilidade permite adaptar a autenticação ao modelo de usuário utilizado pela aplicação.

## Recursos Extras

O projeto de exemplo também contém funcionalidades complementares para ampliar a implementação de autenticação.

### Criptografia de Senhas

A classe de usuário do projeto de exemplo contém um método para criptografia de senhas.

**Recomendação:** em aplicações reais, utilize algoritmos específicos para armazenamento seguro de senhas, como Argon2id, scrypt, bcrypt ou PBKDF2, com salt e parâmetros adequados. Evite armazenar senhas em texto simples ou utilizar criptografia reversível para essa finalidade.

### Envio de E-mails

O projeto de exemplo inclui uma classe para envio de e-mails, utilizada nos fluxos de criação de usuários e recuperação de senhas.

Para habilitar esse recurso:

1. Configure as credenciais de e-mail no arquivo `appsettings.json`.
2. Descomente a chamada de envio de e-mail no método `UsuariosController.Criar`.
3. Descomente a chamada de envio de e-mail no método `AutenticacaoController.EsqueciMinhaSenha`.

**Atenção:** não armazene credenciais reais diretamente em arquivos versionados. Prefira variáveis de ambiente ou um gerenciador de segredos.

## Projeto de Exemplo

O repositório contém exemplos de configuração e utilização da biblioteca:

- [Startup.cs](https://github.com/hsbtiago/TiaIdentity/blob/master/exemplo/Startup.cs)
- [AutenticacaoController.cs](https://github.com/hsbtiago/TiaIdentity/blob/master/exemplo/Controllers/AutenticacaoController.cs)
- [Repositório TiaIdentity](https://github.com/hsbtiago/TiaIdentity)

## Licença

Consulte o arquivo `LICENSE` do repositório para obter informações sobre os termos de utilização e distribuição do projeto.
