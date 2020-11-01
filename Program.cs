using System;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Collections.Generic;
using System.Web;

namespace ExercicioAplicacaoISI
{
    class Program
    {
        public static Dictionary<int, string> dicLocais = new Dictionary<int, string>();

        //private static async Task<string> GetData(string url)
        //{
        //    var httpClient = new HttpClient();
        //    var resposta = await httpClient.GetAsync(url);
        //    var result = await resposta.Content.ReadAsStringAsync();

        //    return result;
        //}

        static Dictionary<int, string> LerLocais(string ficheiro)
        {

            Dictionary<int, string> dicLocais = new Dictionary<int, string>();

            // Expressão Regular para instanciar objeto Regex
            String erString = @"^[0-9]{7},[123],([1-9]?\d,){2}[A-Z]{3},([^,\n]*)$";


            // -> Processar o conteúdo do ficheiro
            // -----------------------------------------------
            // Alternativa 01: ler tudo da Stream para uma string 
            //                 e depois passa a processar o texto da string
            string csvString = null;
            using (StreamReader reader = new StreamReader(ficheiro))
            {
                csvString = reader.ReadToEnd();
            }
            MatchCollection matches = Regex.Matches(csvString, erString,
                                                     RegexOptions.Multiline);
            // RegexOptions.Multiline permite interpretar a String com tendo várias 
            // linhas (caracter '\n'), permitindo usar os operadores de início e fim de linha  ^ $

            foreach (Match m in matches)
            {
                // m corresponderá ao conteúdo de cada linha do ficheiro (considerando que respeita a ER)  
                Console.WriteLine(m.Value);
            }
            // FIM da alternativa 01


            // Alternativa 02: depois de ler o conteúdo do ficheiro para uma stream, 
            //                    vai acedendo "linha-a-linha"
            //    Regex g = new Regex(erString);
            //    using (StreamReader r = new StreamReader(ficheiro))
            //    {
            //        string line;

            //        while ((line = r.ReadLine()) != null)
            //        {
            //            // Tenta correspondência (macthing) da ER com cada linha do ficheiro
            //            Match m = g.Match(line);
            //            if (m.Success)
            //            {
            //                //  estrutura de cada linha com correspondência:
            //                //      globalIdLocal,idRegiao,idDistrito,idConcelho,idAreaAviso,local
            //                //  separar pelas vírgulas...
            //                string[] campos = m.Value.Split(',');
            //                int codLocal = int.Parse(campos[0]);
            //                string cidade = campos[5];
            //                // Guardar na estrutura de dados dicionário
            //                // dicLocais.Add( CHAVE ,  VALOR )
            //                dicLocais.Add(codLocal, cidade);
            //            }
            //            else
            //            {
            //                Console.WriteLine($"Linha inválida: {line}");
            //            }
            //        }
            //    }
            return dicLocais;
           }

            static void Main(string[] args)
            {
                Dictionary<int, string> dicLocais = LerLocais("../../locais.csv");
                foreach (KeyValuePair<int, string> kv in dicLocais)
                {
                    Console.WriteLine($"globalIdLocal = {kv.Key} cidade = {kv.Value}");

                }

                ExercicioAplicacaoISI.JSON.GravaJson();


            }

        }
    }
