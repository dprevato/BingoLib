using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace BingoLib
{
   public static class BingoIniManager
   {
      private static string PathToFileIni { get; set; } // percorso che porta alla posizione del file .INI
      private static string ApplicationName { get; set; }  // Nome dell'assembly principale

      //public static string IniFileName { get; set; } // Nome del file .INI (Default: NomeApp.INI)

      public static int Capacity = 512;

      [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
      private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

      [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
      private static extern int GetPrivateProfileString(string section, string key, string Default, StringBuilder retVal, int size, string filePath);

      [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
      private static extern int GetPrivateProfileString(string section, string key, string defaultValue, [In, Out] char[] retval, int size, string filePath);

      [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
      private static extern int GetPrivateProfileSection(string section, IntPtr keyValue, int size, string filePath);

      /// <summary>
      ///  Senza specificare un nome file, il nuovo file .INI avrà lo stesso nome dell'eseguibile; la posizione del file
      /// .INI sarà nella directory ApplicationData dell'utente, in una sottodirectory con lo stesso nome dell'eseguibile.
      /// </summary>
      public static void Set(string appName) {
         ApplicationName = appName.ToLowerInvariant();
         var IniFileName = $"{ApplicationName}.INI";
         var iniDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationName);
         if (!Directory.Exists(iniDir)) Directory.CreateDirectory(iniDir);
         PathToFileIni = new FileInfo(Path.Combine(iniDir, IniFileName)).FullName.ToLowerInvariant();
      }

      /// <summary>
      /// Restituisce il valore associato alla chiave specificata, oppure NULL se la chiave non viene trovata.
      /// </summary>
      /// <param name="key">Nome della chiave</param>
      /// <param name="section">Nome della sezione (opzionale)</param>
      /// <returns></returns>
      public static string Get(string key, string section = null)
      {
         var retval = new StringBuilder(255);
         GetPrivateProfileString(section ?? ApplicationName, key, null, retval, retval.MaxCapacity, PathToFileIni);
         return retval.ToString().Length > 0 ? retval.ToString() : null;
      }

      /// <summary>
      /// Scrive nel file INI la coppia chiave/valore specificata. Se la chiave è già presente, il valore viene aggiornato.
      /// </summary>
      /// <param name="key">Nome della chiave</param>
      /// <param name="value">Valore corrispondente</param>
      /// <param name="section">Nome della sezione (opzionale)</param>
      public static void Put(string key, string value, string section = null)
      {
         WritePrivateProfileString(section ?? ApplicationName, key, value, PathToFileIni);
      }

      /// <summary>
      /// Elimina dal file INI la coppia chiave/valore corrispondente alla chiave specificata. Se la chiave non esiste, la chiamata alla funzione non ha alcun effetto.
      /// </summary>
      /// <param name="key"></param>
      /// <param name="section"></param>
      public static void RemoveKey(string key, string section = null)
      {
         if (Exists(key, section)) Put(key, null, section ?? ApplicationName);
      }

      /// <summary>
      /// Elimina dal file INI la sezione specificata.
      /// </summary>
      /// <param name="section">Nome della sezione</param>
      public static void DeleteSection(string section = null)
      {
         if (section == null) return;
         Put(null, null, section);
      }

      /// <summary>
      /// Restituisce <b>true</b> se la chiave specificata esiste nel file INI
      /// </summary>
      /// <param name="key">Nome della chiave</param>
      /// <param name="section">Nome della sezione (opzionale)</param>
      /// <returns></returns>
      public static bool Exists(string key, string section = null)
      {
         return Get(key, section).Length > 0;
      }
   }
}
