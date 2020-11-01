﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace ExercicioAplicacaoISI
{
    class JSON
    {
        #region Estado
 
        // Caminho para os ficheiros .json
        static string localPath = Path.GetFullPath(Path.Combine(@"../../1110600-detalhe.json"));
        #endregion


        public static void GravaJson()
        {
            Dictionary<int, string> bla = ExercicioAplicacaoISI.Program.dicLocais;
            ///Serializa o objeto para JSON e guarda-o numa string 
            var localJSON = new JavaScriptSerializer().Serialize(bla);
            // Escreve o texto nas strings nos respetivos ficheiros *.json
            File.WriteAllText(localPath, localJSON);
        }
    }
}
