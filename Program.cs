using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Windows;
using System.Media;


namespace FloriculturaConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)  
        {
            var client = new MongoClient("mongodb+srv://otaviohs:<password>@otavio.uga7jnx.mongodb.net/?retryWrites=true&w=majority&appName=otavio");
            var database = client.GetDatabase("floricultura");
            var collection = database.GetCollection<Planta>("floricultura");

            bool sair = false;
            while (!sair)
            {
                Console.WriteLine("========== Floricultura ==========");
                Console.WriteLine("1. Adicionar Planta");
                Console.WriteLine("2. Atualizar Planta");
                Console.WriteLine("3. Consultar Planta");
                Console.WriteLine("4. Deletar Planta");
                Console.WriteLine("5. Sair");
                Console.WriteLine("=============================");
                Console.Write("Escolha uma opção: ");
                var opcao = Console.ReadLine();
                Console.WriteLine();

                switch (opcao)
                {
                    case "1":
                        await AdicionarPlanta(collection);
                        break;
                    case "2":                        
                        await AtualizarPlanta(collection);
                        break;
                    case "3":
                        await ConsultarPlanta(collection);
                        break;
                    case "4":
                        await DeletarPlanta(collection);
                        break;
                    case "5":
                        sair = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static async Task AdicionarPlanta(IMongoCollection<Planta> collection)
        {
        Console.Write("Nome científico");
        var nomecientifico = Console.ReadLine();
        Console.Write("Nome comum");
        var nomecomum = Console.ReadLine();
        Console.Write("Família");
        var familia = Console.ReadLine();
        Console.Write("Origem");
        var origem = Console.ReadLine();
        Console.Write("Descrição");
        var descricao = Console.ReadLine();
        Console.Write("Imagem");
        var imagem = Console.ReadLine();

         var planta = new Planta
         {
            NomeCientifico = nomecientifico,
            NomeComum = nomecomum,
            Familia = familia,
            Origem = origem,
            Descricao = descricao,
            Imagem = imagem
         };

         await collection.InsertOneAsync(planta);
         Console.WriteLine("Planta adicionada com sucesso!");     
        }

        static async Task ConsultarPlanta(IMongoCollection<Planta> collection)
        {
            Console.Write("Digite o nome comum:");
            var nomecomum = Console.ReadLine();
            var filter = Builders<Planta>.Filter.Eq(p=>p.NomeComum,nomecomum);
            var planta = await
            collection.Find(filter).FirstOrDefaultAsync();
            if(planta != null){
                Console.WriteLine($"Planta encontrada:");
                Console.WriteLine($"Nome Cientifico: {planta.NomeCientifico}");
                Console.WriteLine($"Nome Comum: {planta.NomeComum}");
                Console.WriteLine($"Familia {planta.Familia}");
                Console.WriteLine($"Origem: {planta.Origem}");
                Console.WriteLine($"Descricao: {planta.Descricao}");
                Console.WriteLine($"Imagem: {planta.Imagem}");

            }else{
                Console.WriteLine("Planta não encontrada.");
            }
        }

        static async Task AtualizarPlanta(IMongoCollection<Planta> collection)
        {
            Console.Write("Digite o nome comum da planta que deseja atualizar: ");
            var nomecomum = Console.ReadLine();
            var filter = Builders<Planta>.Filter.Eq(p => p.NomeComum, nomecomum);
            var planta = await collection.Find(filter).FirstOrDefaultAsync();
            if (planta != null)
            {
                Console.Write("Digite o novo nome cientifico: ");
                var novoNomeCientifico = Console.ReadLine();
                Console.Write("Digite o novo nome comum: ");
                var novoNomeComum = Console.ReadLine();
                Console.Write("Digite a nova familia: ");
                var novoFamilia = Console.ReadLine();
                Console.Write("Digite a nova origem: ");
                var novoOrigem = Console.ReadLine();
                Console.Write("Digite a nova descricao: ");
                var novoDescricao = Console.ReadLine();
                Console.Write("Digite a nova imagem: ");
                var novoImagem = Console.ReadLine();

                var update = Builders<Planta>.Update.Set(p => p.NomeCientifico, novoNomeCientifico)
                                                     .Set(p => p.NomeComum, novoNomeComum)
                                                     .Set(p => p.Familia, novoFamilia)
                                                     .Set(p => p.Origem, novoOrigem)
                                                     .Set(p => p.Descricao, novoDescricao)
                                                     .Set(p => p.Imagem, novoImagem);

                await collection.UpdateOneAsync(filter, update);
                Console.WriteLine("Planta atualizada com sucesso!");
            }
            else
            {
                Console.WriteLine("Planta não encontrada.");
            }
        }

        static async Task DeletarPlanta(IMongoCollection<Planta> collection)
        {
            Console.Write("Digite o nome comum da Planta que deseja deletar: ");
            var nomecomum = Console.ReadLine();
            var filter = Builders<Planta>.Filter.Eq(p => p.NomeComum, nomecomum);
            var result = await collection.DeleteOneAsync(filter);

            if (result.DeletedCount > 0)
            {
                Console.WriteLine("Planta deletada com sucesso!");
            }
            else
            {
                Console.WriteLine("Planta não encontrada.");
            }
        }

    }
}
public class Planta
    {
        public ObjectId Id { get; set; }
        public string NomeCientifico { get; set; }
        public string NomeComum { get; set; }
        public string Familia { get; set; }
        public string Origem { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
    }


