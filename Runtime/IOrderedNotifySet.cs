//.................................................................................................................//
//Interface for Set collection with ordered elements...............................................................//
//.................................................................................................................//
//.................................................................................................................//
//.................................................................................................................//
//.................................................................................................................//
using System;
using System.Collections.Generic;


namespace CrazyPanda.UnityCore.Collections
{
	public interface IOrderedNotifySet< T > : ICollection< T >
	{
		//.........................................................................................................//
		//Interface block
		//.........................................................................................................//

		#region Public Events
		/// <summary>
		/// Rise after collection changed
		/// </summary>
		event EventHandler< NotifySetChangedEventArgs< T > > OnCollectionChanged;
		#endregion

		#region Public Members
		/// <summary>
		/// Add unique item before first item of collection
		/// </summary>
		/// <param name="item">new item</param>
		void AddFirst( T item );

		/// <summary>
		/// Add unique item after last item of collection
		/// </summary>
		/// <param name="item">new item</param>
		void AddLast( T item );

		/// <summary>
		/// Add new item element after exist item
		/// </summary>
		/// <param name="item">exist item</param>
		/// <param name="newItem">new item to add</param>
		void AddAfter( T item, T newItem );

		/// <summary>
		/// Add unique item before exist item
		/// </summary>
		/// <param name="newItem">exist item</param>
		/// <param name="oldItem">new item to add</param>
		void AddBefore( T newItem, T oldItem );
		#endregion

		#region Public Properties
		/// <summary>
		/// First item of collection
		/// </summary>
		T First { get; }

		/// <summary>
		/// Last item of collection
		/// </summary>
		T Last { get; }
		#endregion
	}
	//.............................................................................................................//
	//.............................................................................................................//
	//.............................................................................................................//
	//.............................................................................................................//
	//.............................................................................................................//
	//.............................................................................................................//
	//.............................................................................................................//
	//.............................................................................................................//
}
