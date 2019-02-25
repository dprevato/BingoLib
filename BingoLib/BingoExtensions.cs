using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace BingoLib
{
   public static class BingoExtensions
   {
      /// <summary>
      /// Check if the string contains a numeric value
      /// </summary>
      /// <param name="value">The string to be checked</param>
      /// <returns>true if the string value can be evaluated as a number</returns>
      public static bool IsNumeric(this string value)
      {
         var isNum = new Regex(@"^(" +
                               /*Hex*/ @"0x[0-9a-fA-F]+" + "|" +
                               /*Bin*/ @"0b[01]+" + "|" +
                               /*Oct*/ @"0[0-7]*" + "|" +
                               /*Dec*/ @"((?!0)|[-+]|(?=0+\.))(\d*\.)?\d+(e\d+)?" +
                               ")$"
                              );
         return isNum.IsMatch(value.Replace('_', '0'));
      }

      /// <summary>
      /// Check if a string is empty, null, or contains only white space chars
      /// </summary>
      /// <param name="value"></param>
      /// <returns>true if the string is null, empty or contains only white space chars</returns>
      public static bool IsBlank(this string value)
      {
         return string.IsNullOrWhiteSpace(value);
      }

      /// <summary>
      /// Estensione al tipo DateTime: restituisce la data corrispondente  al primo giorno del mese della data indicata.
      /// </summary>
      /// <param name="value">un valore DateTime</param>
      /// <returns></returns>
      public static DateTime MonthBegin(this DateTime value)
      {
         return value.Date.AddDays(1 - value.Day);
      }

      /// <summary>
      /// Estensione al tipo DateTime: restituisce la data corrispondente all'ultimo giorno del mese della data indicata
      /// </summary>
      /// <param name="value">un valore DateTime</param>
      /// <returns></returns>
      public static DateTime MonthEnd(this DateTime value)
      {
         return value.MonthBegin().AddMonths(1).AddDays(-1);
      }

      //  I seguenti due metodi permettono di trasformare una generica lista di oggetti in un oggetto DataTable. Permette di evitare
      //  l'exception "DataSet does not support system.nullable che si verifica ad esempio con quelle merde sciolte dei Crystal Report
      public static DataTable ToDataTable<T>(this IEnumerable<T> collection, string tableName)
      {
         var tbl = ToDataTable(collection);
         tbl.TableName = tableName;
         return tbl;
      }

      public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
      {
         var dataTable = new DataTable();
         var t = typeof(T);
         var pia = t.GetProperties();

         for (var i = 0; i < pia.Length; i++)
         {
            dataTable.Columns.Add(pia[i].Name, Nullable.GetUnderlyingType(pia[i].PropertyType) ?? pia[i].PropertyType);
            dataTable.Columns[i].AllowDBNull = true;
         }

         //  Popolamento della tabella con i dati contenuti nella collection
         foreach (var item in collection)
         {
            var dr = dataTable.NewRow();
            dr.BeginEdit();

            foreach (var t1 in pia)
            {
               var temp = t1.GetValue(item, null);
               if (temp == null || (temp.GetType().Name == "Char" && ((char)temp).Equals('\0')))
               {
                  dr[t1.Name] = DBNull.Value;
               }
               else
               {
                  dr[t1.Name] = temp;
               }
            }
            dr.EndEdit();
            dataTable.Rows.Add(dr);
         }
         return dataTable;
      }

      public static void Flush(this string s)
      {
         s = string.Empty;
      }

   }
}
