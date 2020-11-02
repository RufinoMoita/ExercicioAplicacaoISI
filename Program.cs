using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text.Json;
using System.Runtime.CompilerServices;

namespace ExercicioAplicacaoISI
{
    class Program
    {
        //static Dictionary<int, string> listaLocais = new Dictionary<int, string>();

        static PrevisaoIPMA LerPrevisaoIPMA(int globalIdLocal)
        {
            String jsonString = null;
            using (StreamReader reader = new StreamReader(@"../../data_forecast/" + globalIdLocal + ".json"))
            {
                jsonString = reader.ReadToEnd();
            }
            PrevisaoIPMA obj = JsonSerializer.Deserialize<PrevisaoIPMA>(jsonString);
            return obj;
        }

        static Dictionary<int, string> LerLocais(string ficheiro)
        {

            Dictionary<int, string> dicLocais = new Dictionary<int, string>();

            // Expressão Regular para instanciar objeto Regex
            String erString = @"^[0-9]{7},[123],([1-9]?\d,){2}[A-Z]{3},([^,\n]*)$";


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
                        //Console.WriteLine($"Linha inválida: {line}");
                    }
                }
            }
            return dicLocais;
        }

        static PrevisaoDia LerFicheiroPrevisaoDia(int globalIdLocal)
        {

            string jsonString;

            using (StreamReader reader = new StreamReader(@"../netcoreapp3.1/" + globalIdLocal + "-detalhes.json"))
            {
                jsonString = reader.ReadToEnd();
            }

            PrevisaoDia previsao = JsonSerializer.Deserialize<PrevisaoDia>(jsonString);

            return previsao;

        }

        static void Main(string[] args)
        {
            Dictionary<int, string> dicLocais = LerLocais(@"../../locais.csv");
            string json;

            foreach (KeyValuePair<int, string> kv in dicLocais)
            {
                //Console.WriteLine($"globalIdLocal = {kv.Key} cidade = {kv.Value}");

                //Ler previsao para cada regiao
                PrevisaoIPMA previsao = LerPrevisaoIPMA(kv.Key);
                PrevisaoDia previsaoDia = LerFicheiroPrevisaoDia(kv.Key);

                //Atribuir o nome do local à previsão
                previsao.local = kv.Value;

                json = JsonSerializer.Serialize(previsao);

                if (!File.Exists(kv.Key + "-detalhes.json"))
                {
                    File.WriteAllText(kv.Key + "-detalhes.json", json);
                }

                Console.WriteLine("Temperatura Maxima {0}", previsaoDia.precipitaProb);
            }

            Console.ReadKey();
        }

        }
    }
