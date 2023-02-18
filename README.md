A biblioteca LocalData é uma biblioteca .NET Standard 2.0 que fornece uma classe DataFile para gerenciar arquivos de dados criptografados.

Uso da biblioteca
Para usar a biblioteca LocalData, você precisa adicionar a referência ao seu projeto e importar o namespace LocalData no seu código.


```
using LocalData;
```
DataFile
A classe DataFile é a principal classe da biblioteca e fornece métodos para armazenar e recuperar dados criptografados em um arquivo. A classe possui os seguintes construtores:

Construtor sem parâmetros
```
public DataFile()
```
O construtor padrão cria um arquivo criptografado chamado "date.bin" na pasta atual do aplicativo e usa a senha "A-32-CHARACTER-PASSWORD-!@#$%¨&*" e o IV "16CHARACTERPASSW" para criptografar os dados.

Construtor com parâmetros
```
public DataFile(string filePath, string key, string iv, string customPrefix = null)
```
Este construtor cria um arquivo criptografado com o caminho filePath e usa a chave key e o IV iv para criptografar os dados. O parâmetro customPrefix é opcional e define o prefixo usado para todas as variáveis armazenadas no arquivo.

Métodos
A classe DataFile fornece os seguintes métodos:

Clear(): Apaga o arquivo criptografado.
VerifyExistCFG(string variable): Verifica se uma variável com o nome especificado existe no arquivo criptografado.
LoadCfg<T>(string variable, T defaultValue): Carrega o valor de uma variável com o nome especificado do arquivo criptografado. Se a variável não existir, retorna o valor padrão especificado em defaultValue.
SaveCfg<T>(string variable, T value): Salva o valor especificado em value para uma variável com o nome especificado em variable.
GetAllVariableNames(): Retorna um objeto IEnumerable<string> que contém o nome de todas as variáveis armazenadas no arquivo criptografado.
CryptFileHandler
A classe CryptFileHandler é usada internamente pela classe DataFile para criptografar e descriptografar dados. Você não precisa usar essa classe diretamente em seu código.

Algumas restrições do uso da biblioteca e do código apresentado incluem:

1 - A classe CryptFileHandler usa criptografia AES com uma chave de 32 bytes e um vetor de inicialização de 16 bytes. Se esses valores forem alterados ou definidos incorretamente, a criptografia pode falhar ou ser menos segura. É necessario que tenha a primeira chave tenha 32 caracteres e a segunda 16.

2 - O código assume que o arquivo de dados será armazenado no disco em um local específico. Se o caminho ou nome do arquivo for alterado, o código pode não funcionar corretamente.

3 -O código não tem mecanismos de segurança para impedir o acesso ou a edição não autorizada dos dados armazenados.

4 - A classe DataFile não é thread-safe e não fornece mecanismos de bloqueio ou exclusão mútua para garantir que apenas uma thread acesse o arquivo de dados ao mesmo tempo.

5 - A classe DataFile permite que os usuários armazenem qualquer tipo de objeto como um valor, mas isso pode causar problemas de serialização ou desserialização se o tipo de objeto não for serializável ou não puder ser convertido de volta para o tipo original.

6 - O código não tem tratamento de exceções para lidar com erros ou exceções que podem ocorrer durante a execução, o que pode levar a um comportamento inesperado ou falhas no aplicativo que usa essa biblioteca.

7 - Não é recomendado trabalhar com muitos volumes de dados pois a biblioteca salva tudo diretamente no armazenamento do diposito.

8 - Não ultilize quebras de linhas nas string, o caractere \n é ultilizado para a separação de dados.
