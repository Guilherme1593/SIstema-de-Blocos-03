using System;
using System.Collections.Generic;
using System.IO;

public class Bloco
{
    public int Codigo { get; set; }
    public string Nome { get; set; }
    public double Comprimento { get; set; }
    public double Largura { get; set; }
    public double Espessura { get; set; }
    public string Material { get; set; }
    public string Pedreira { get; set; }
    public double ValorCompra { get; set; }
    public double ValorVenda { get; set; }

    // Construtor para criar um bloco com código, nome, comprimento, largura, espessura, material e pedreira
    public Bloco(int codigo, string nome, double comprimento, double largura, double espessura, string material, string pedreira)
    {
        Codigo = codigo;
        Nome = nome;
        Comprimento = comprimento;
        Largura = largura;
        Espessura = espessura;
        Material = material;
        Pedreira = pedreira;
    }

    public Bloco()
    {
    }

    // Método para calcular o volume do bloco
    public double CalcularVolume()
    {
        return Comprimento * Largura * Espessura;
    }

    // Método para calcular o lucro de venda do bloco
    public double CalcularLucroVenda()
    {
        return ValorVenda - ValorCompra;
    }

    // Método para calcular o lucro de compra do bloco
    public double CalcularLucroCompra()
    {
        return ValorCompra - (Comprimento * Largura * Espessura * 0.1); // Adicionando uma margem de 10%
    }

    public override string ToString()
    {
        return $"Código: {Codigo}, Nome: {Nome}, Pedreira: {Pedreira}, Material: {Material}, Dimensões: {Comprimento}x{Largura}x{Espessura} mm";
    }
}

public class GestorBlocos
{
    private List<Bloco> Blocos;

    public GestorBlocos()
    {
        Blocos = new List<Bloco>();
    }

    public void AdicionarBloco(Bloco bloco)
    {
        Blocos.Add(bloco);
    }

    public List<Bloco> ListarBlocos()
    {
        return Blocos;
    }

    public Bloco BuscarBlocoPorCodigo(int codigo)
    {
        return Blocos.Find(b => b.Codigo == codigo);
    }

    public List<Bloco> ListarBlocosPorPedreira(string pedreira)
    {
        return Blocos.FindAll(b => b.Pedreira.Equals(pedreira, StringComparison.OrdinalIgnoreCase));
    }
}

public class BlocoFileManager
{
    private readonly string filePath;

    public BlocoFileManager(string filePath)
    {
        this.filePath = filePath;
    }

    public void SalvarBlocos(List<Bloco> blocos)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (var bloco in blocos)
                {
                    sw.WriteLine($"Bloco {bloco.Codigo}:");
                    sw.WriteLine($"Nome: {bloco.Nome}");
                    sw.WriteLine($"Comprimento: {bloco.Comprimento} mm");
                    sw.WriteLine($"Largura: {bloco.Largura} mm");
                    sw.WriteLine($"Espessura: {bloco.Espessura} mm");
                    sw.WriteLine($"Material: {bloco.Material}");
                    sw.WriteLine($"Pedreira: {bloco.Pedreira}");
                    sw.WriteLine($"Valor de Compra: {bloco.ValorCompra:C}");
                    sw.WriteLine($"Valor de Venda: {bloco.ValorVenda:C}");
                    sw.WriteLine(); // Adiciona uma linha em branco entre os blocos
                }
            }

            Console.WriteLine("Blocos cadastrados foram salvos no arquivo.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro ao escrever no arquivo: {ex.Message}");
        }
    }

    public List<Bloco> CarregarBlocos()
    {
        List<Bloco> blocos = new List<Bloco>();

        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                Bloco bloco = null;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("Bloco"))
                    {
                        bloco = new Bloco();
                        blocos.Add(bloco);
                        continue;
                    }

                    string[] parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        string key = parts[0].Trim();
                        string value = parts[1].Trim();

                        switch (key)
                        {
                            case "Nome":
                                bloco.Nome = value;
                                break;

                            case "Comprimento":
                                bloco.Comprimento = double.Parse(value.Split(' ')[0]);
                                break;

                            case "Largura":
                                bloco.Largura = double.Parse(value.Split(' ')[0]);
                                break;

                            case "Espessura":
                                bloco.Espessura = double.Parse(value.Split(' ')[0]);
                                break;

                            case "Material":
                                bloco.Material = value;
                                break;

                            case "Pedreira":
                                bloco.Pedreira = value;
                                break;

                            case "Valor de Compra":
                                bloco.ValorCompra = double.Parse(value.Split(' ')[0].Replace("R$", ""));
                                break;

                            case "Valor de Venda":
                                bloco.ValorVenda = double.Parse(value.Split(' ')[0].Replace("R$", ""));
                                break;
                        }
                    }
                }
            }

            Console.WriteLine("Blocos foram carregados do arquivo.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro ao ler o arquivo: {ex.Message}");
        }

        return blocos;
    }
}

public class Program
{
    static GestorBlocos gestor = new GestorBlocos();
    static BlocoFileManager blocoFileManager = new BlocoFileManager("C:\\Users\\guisa\\Desktop\\Blocos\\Blocoscadastrado.txt");

    public Program()
    {
    }

    static void Main()
    {
        int opcao;
        do
        {
            MostrarMenu();
            Console.Write("Escolha uma opção: ");
            if (int.TryParse(Console.ReadLine(), out opcao))
            {
                ExecutarOpcao(opcao);
            }
            else
            {
                Console.WriteLine("Opção inválida. Tente novamente.");
            }

        } while (opcao != 7);
    }

    static void MostrarMenu()
    {
        Console.WriteLine("Menu:");
        Console.WriteLine("1 - Cadastrar Bloco");
        Console.WriteLine("2 - Listar Blocos");
        Console.WriteLine("3 - Buscar Bloco por Código");
        Console.WriteLine("4 - Listar Blocos por Pedreira");
        Console.WriteLine("5 - Calcular Lucro de Compra e Venda");
        Console.WriteLine("6 - Calcular Volume do Bloco");
        Console.WriteLine("7 - Salvar blocos cadastrados no Bloco de Notas");
        Console.WriteLine("8 - SAIR");
    }

    static void ExecutarOpcao(int opcao)
    {
        switch (opcao)
        {
            case 1:
                CadastrarBloco();
                break;

            case 2:
                ListarBlocos();
                break;

            case 3:
                BuscarBlocoPorCodigo();
                break;

            case 4:
                ListarBlocosPorPedreira();
                break;

            case 5:
                CalcularLucro();
                break;

            case 6:
                CalcularVolume();
                break;

            case 7:
                SalvarBlocosNoBlocoDeNotas();
                break;

            case 8:
                Console.WriteLine("Saindo do programa.");
                break;

            default:
                Console.WriteLine("Opção inválida. Tente novamente.");
                break;
        }
    }

    static void CadastrarBloco()
    {
        Console.Write("Informe o código do bloco: ");
        int codigo = int.Parse(Console.ReadLine());

        Console.Write("Informe o nome do bloco: ");
        string nome = Console.ReadLine();

        Console.Write("Informe o comprimento do bloco (mm): ");
        double comprimento = double.Parse(Console.ReadLine());

        Console.Write("Informe a largura do bloco (mm): ");
        double largura = double.Parse(Console.ReadLine());

        Console.Write("Informe a espessura do bloco (mm): ");
        double espessura = double.Parse(Console.ReadLine());

        Console.Write("Informe o material do bloco: ");
        string material = Console.ReadLine();

        Console.Write("Informe a pedreira do bloco: ");
        string pedreira = Console.ReadLine();

        Console.Write("Informe o valor de compra do bloco: ");
        double valorCompra = double.Parse(Console.ReadLine());

        Console.Write("Informe o valor de venda do bloco: ");
        double valorVenda = double.Parse(Console.ReadLine());

        Bloco bloco = new Bloco(codigo, nome, comprimento, largura, espessura, material, pedreira)
        {
            ValorCompra = valorCompra,
            ValorVenda = valorVenda
        };

        gestor.AdicionarBloco(bloco);

        Console.WriteLine("Bloco cadastrado com sucesso!");

        // Adicionando bloco ao arquivo
        List<Bloco> blocos = gestor.ListarBlocos();
        blocoFileManager.SalvarBlocos(blocos);
    }

    static void ListarBlocos()
    {
        Console.WriteLine("Lista de Blocos:");
        foreach (var bloco in gestor.ListarBlocos())
        {
            Console.WriteLine(bloco);
        }
    }

    static void BuscarBlocoPorCodigo()
    {
        Console.Write("Informe o código do bloco a ser buscado: ");
        int codigo = int.Parse(Console.ReadLine());

        var bloco = gestor.BuscarBlocoPorCodigo(codigo);

        if (bloco != null)
        {
            Console.WriteLine("Bloco encontrado:");
            Console.WriteLine(bloco);
        }
        else
        {
            Console.WriteLine("Bloco não encontrado.");
        }
    }

    static void ListarBlocosPorPedreira()
    {
        Console.Write("Informe a pedreira a ser filtrada: ");
        string pedreira = Console.ReadLine();

        var blocosFiltrados = gestor.ListarBlocosPorPedreira(pedreira);

        if (blocosFiltrados.Count > 0)
        {
            Console.WriteLine($"Blocos da pedreira '{pedreira}':");
            foreach (var bloco in blocosFiltrados)
            {
                Console.WriteLine(bloco);
            }
        }
        else
        {
            Console.WriteLine($"Nenhum bloco encontrado na pedreira '{pedreira}'.");
        }
    }

    static void CalcularLucro()
    {
        Console.Write("Informe o código do bloco para calcular o lucro: ");
        int codigo = int.Parse(Console.ReadLine());

        var bloco = gestor.BuscarBlocoPorCodigo(codigo);

        if (bloco != null)
        {
            double lucroCompra = bloco.CalcularLucroCompra();
            double lucroVenda = bloco.CalcularLucroVenda();

            Console.WriteLine($"Lucro de Compra: {lucroCompra:C}");
            Console.WriteLine($"Lucro de Venda: {lucroVenda:C}");
        }
        else
        {
            Console.WriteLine("Bloco não encontrado.");
        }
    }

    static void CalcularVolume()
    {
        Console.Write("Informe o código do bloco para calcular o volume: ");
        int codigo = int.Parse(Console.ReadLine());

        var bloco = gestor.BuscarBlocoPorCodigo(codigo);

        if (bloco != null)
        {
            double volume = bloco.CalcularVolume();
            Console.WriteLine($"Volume do Bloco: {volume} mm³");
        }
        else
        {
            Console.WriteLine("Bloco não encontrado.");
        }
    }

    static void SalvarBlocosNoBlocoDeNotas()
    {
        List<Bloco> blocos = gestor.ListarBlocos();
        blocoFileManager.SalvarBlocos(blocos);
    }
}
