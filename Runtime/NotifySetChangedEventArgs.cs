using System;

namespace CrazyPanda.UnityCore.Collections
{
	public sealed class NotifySetChangedEventArgs<T> : EventArgs
	{
		#region Public Fields
		public readonly T oldItem;
		public readonly T newItem;
		public readonly NotifySetChangeActionType changeActionTypeType;
		#endregion

		#region Private Constructors
		private NotifySetChangedEventArgs( NotifySetChangeActionType changeActionTypeType, T oldItem, T newItem )
		{
			this.oldItem = oldItem;
			this.newItem = newItem;
			this.changeActionTypeType = changeActionTypeType;
		}
		#endregion

		#region Public Static Members

		/// <summary>
		/// Construct NotifySetChangedEventArgs for remove item from collection
		/// </summary>
		public static NotifySetChangedEventArgs< T > ConstructRemoveAction( T item )
		{
			return new NotifySetChangedEventArgs< T >( NotifySetChangeActionType.Remove, item, default );
		}

		/// <summary>
		/// Construct NotifySetChangedEventArgs for AddAfter item in collection
		/// </summary>
		public static NotifySetChangedEventArgs< T > ConstructAddAfterAction( T item, T newItem )
		{
			return new NotifySetChangedEventArgs< T >( NotifySetChangeActionType.AddAfter, item, newItem );
		}

		/// <summary>
		/// Construct NotifySetChangedEventArgs for AddBefore item in collection
		/// </summary>
		public static NotifySetChangedEventArgs< T > ConstructAddBeforeAction( T item,T newItem  )
		{
			return new NotifySetChangedEventArgs< T >( NotifySetChangeActionType.AddBefore, item, newItem );
		}

		/// <summary>
		/// Construct NotifySetChangedEventArgs for clear collection
		/// </summary>
		public static NotifySetChangedEventArgs< T > ConstructClear()
		{
			return new NotifySetChangedEventArgs< T >( NotifySetChangeActionType.Clear, default, default );
		}

		/// <summary>
		/// Construct NotifySetChangedEventArgs for add first element to collection
		/// </summary>
		public static NotifySetChangedEventArgs< T > ConstructAddFirst( T item )
		{
			return new NotifySetChangedEventArgs< T >( NotifySetChangeActionType.AddFirst, default, item );
		}
		#endregion
	}
}
