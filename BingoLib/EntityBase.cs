using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BingoLib
{
   public abstract class EntityBase : INotifyPropertyChanged, IObjectWithState
   {
      #region INotifyPropertyChanged Implementation

      public event PropertyChangedEventHandler PropertyChanged;

      [NotifyPropertyChangedInvocator]
      [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
      {
         //TODO: when we remove the old OnPropertyChanged method we need to uncomment the below line
         //OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
#pragma warning disable CS0618 // Type or member is obsolete
         OnPropertyChanged(propertyName);
#pragma warning restore CS0618 // Type or member is obsolete
      }

      /// <summary>
      /// Verifica se il valore di una proprietà è diverso da quello già contenuto in essa; se così è, assegna il nuovo valore e notifica il cambiamento ai listeners.
      /// </summary>
      /// <typeparam name="T">Tipo della proprietà</typeparam>
      /// <param name="storage">Riferimento ad una Property dotata di getter e di setter</param>
      /// <param name="value">Nuovo valore della proprietà</param>
      /// <param name="propertyName">Nome della proprietà, usato per la notifica ai listeners</param>
      /// <returns></returns>
      [UsedImplicitly]
      protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
      {
         if (EqualityComparer<T>.Default.Equals(storage, value))
         {
            return false;
         }

         storage = value;
         RaisePropertyChanged(propertyName);
         return true;
      }

      /// <summary>
      /// Verifica se il valore di una proprietà è diverso da quello già contenuto in essa; se così è, assegna il nuovo valore e notifica il cambiamento ai listeners.
      /// </summary>
      /// <typeparam name="T">Tipo della proprietà</typeparam>
      /// <param name="storage">Riferimento ad una Property dotata di getter e di setter</param>
      /// <param name="value">Nuovo valore della proprietà</param>
      /// <param name="OnChanged">Azione che viene chiamata soltanto se il valore della proprietà è cambiato</param>
      /// <param name="propertyName">Nome opzionale della proprietà; se non viene indicato, viene fornito automaticamente da [CallerMemberName]</param>
      /// <returns>True se il valore è cambiato, False, se il nuovo valore è uguale a quello esistente</returns>
      [UsedImplicitly]
      protected virtual bool SetProperty<T>(ref T storage, T value, Action OnChanged, [CallerMemberName] string propertyName = null)
      {
         if (EqualityComparer<T>.Default.Equals(storage, value))
         {
            return false;
         }

         storage = value;
         RaisePropertyChanged(propertyName);
         OnChanged?.Invoke();
         return true;
      }

      #endregion INotifyPropertyChanged Implementation

      public State State { get; set; }
   }
}