using System;
using System.Collections.Generic;
using System.Text;

namespace BingoLib
{
   public interface IObjectWithState
   {
      State State { get; set; }
   }

   public enum State
   {
      Added,
      Unchanged,
      Modified,
      Deleted
   }
}
