using System;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Collections.Generic;
using System.Xml;
using System.Web.Script.Serialization;
using System.Text.Json.Serialization;

namespace ExercicioAplicacaoISI
{
    class Program
    {
        static Dictionary<int, string> listaLocais = new Dictionary<int, string>();

        static PrevisaoIPMA LerPrevisoes(int globalIdLocal)
        {
            // Instanciar JavaScriptSerializer para converter o objeto para JSON
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

            string json = null;
            //using (StreamReader reader = new StreamReader("data_forecast/" + globalIdLocal + ".json")) ;
            string jsonPath = Path.GetFullPath(Path.Combine("data_forecast/" + globalIdLocal + ".json"));
            json = File.ReadAllText(jsonPath);

            PrevisaoIPMA obj = javaScriptSerializer.Deserialize<PrevisaoIPMA>(json);
            obj.local = listaLocais[obj.globalIdLocal];
            return obj;
        }

        static Dictionary<int, string> LerLocais(string ficheiro)
        {

            Dictionary<int, string> dicLocais = new Dictionary<int, string>();

            // Expressão Regular para instanciar objeto Regex
            String erString = @"^[0-9]+,[0-9],[0-9]+,[0-9]+,[A-Z]{3},[A-Z][a-z]+$";


            // Alternativa 02: depois de ler o conteúdo do ficheiro para uma stream, 
            //                    vai acedendo "linha-a-linha"
            Regex g = new Regex(erString);
            using (StreamReader r = new StreamReader(ficheiro))
            {
                string line;

                while ((line = r.ReadLine()) != null)
                {
                    // Tenta correspondência (macthing) da ER com cada linha do ficheiro
                    Match m = g.Match(line);
                    if (m.Success)
                    {
                        //  estrutura de cada linha com correspondência:
                        //      globalIdLocal,idRegiao,idDistrito,idConcelho,idAreaAviso,local
                        //  separar pelas vírgulas...
                        string[] campos = m.Value.Split(',');
                        int codLocal = int.Parse(campos[0]);
                        string cidade = campos[5];
                        // Guardar na estrutura de dados dicionário
                        // dicLocais.Add( CHAVE ,  VALOR )
                        dicLocais.Add(codLocal, cidade);
                    }
                    else
                    {
                        Console.WriteLine($"Linha inválida: {line}");
                    }
                }
            }
            return dicLocais;
        }

        static void Main(string[] args)
        {
            Dictionary<int, string> dicLocais = LerLocais(@"../../locais.csv");

            // Instanciar JavaScriptSerializer para converter o objeto para JSON
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

            foreach (KeyValuePair<int, string> kv in dicLocais)
            {
                Console.WriteLine($"globalIdLocal = {kv.Key} cidade = {kv.Value}");

                //Ler previsao para cada regiao
                PrevisaoIPMA previsao = LerPrevisoes(kv.Key);

                //Atribuir o nome do local à previsão
                previsao.local = kv.Value;

                var json = javaScriptSerializer.Serialize(previsao);

                if (!File.Exists(kv.Key + "-detalhes.json"))
                {
                    File.WriteAllText(kv.Key + "-detalhes.json", json);
                }

            }
            Console.ReadKey();
        }

        }
    }
