//.................................................................................................................//
//Set collection with ordered elements.............................................................................//
//.................................................................................................................//
//.................................................................................................................//
//.................................................................................................................//
//.................................................................................................................//
using System;
using System.Collections;
using System.Collections.Generic;


namespace CrazyPanda.UnityCore.Collections
{
	public sealed class OrderedNotifySet< T > : IOrderedNotifySet< T >
	{
		//.........................................................................................................//
		//Parameters block
		//.........................................................................................................//

		private readonly LinkedList< T > _elementsList = new LinkedList< T >();
		private readonly Dictionary< T, LinkedListNode< T > > _elementsMap = new Dictionary< T, LinkedListNode< T > >();

		//.........................................................................................................//
		//Interface block
		//.........................................................................................................//

		public event Action< NotifySetChangedEventArgs< T > > OnCollectionChanged;

		public T First
		{
			get
			{
				//check count
				if( Count == 0 )
				{
					throw new InvalidOperationException( @"Cannot get first element from empty collection" );
				}

				return _elementsList.First.Value;
			}
		}

		public T Last
		{
			get
			{
				//check count
				if( Count == 0 )
				{
					throw new InvalidOperationException( @"Cannot get Last element from empty collection" );
				}

				return _elementsList.Last.Value;
			}
		}
		public int Count => _elementsList.Count;
		public bool IsReadOnly => false;

		public IEnumerator< T > GetEnumerator()
		{
			return _elementsList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add( T item )
		{
			AddFirst( item );
		}

		public void Clear()
		{
			//clear collections
			_elementsMap.Clear();
			_elementsList.Clear();

			//rise clear event
			OnCollectionChanged?.Invoke( NotifySetChangedEventArgs< T >.ConstructClear() );
		}

		public bool Contains( T item )
		{
			//check element not null!
			CheckNullElement( item );
			return _elementsMap.ContainsKey( item );
		}

		public void CopyTo( T[ ] array, int arrayIndex )
		{
			throw new NotImplementedException();
		}

		public bool Remove( T item )
		{
			//check element
			CheckNullElement( item );

			//check exist
			LinkedListNode< T > entry;
			if( !_elementsMap.TryGetValue( item, out entry ) )
			{
				return false;
			}

			//remove
			_elementsMap.Remove( item );
			_elementsList.Remove( entry.Value );

			//rise change event
			var args = NotifySetChangedEventArgs<T>.ConstructRemoveAction( item );
            OnCollectionChanged?.Invoke( args );

			return true;
		}

		public void AddFirst( T item )
		{
			if( !TryAddFirstInternal( item ) )
			{
				AddBefore( First, item );
			}
		}

		public void AddLast( T item )
		{
			if( !TryAddFirstInternal( item ) )
			{
				AddAfter( Last, item );
			}
		}

		public void AddAfter( T item, T newItem )
		{
			AddElementInternal( item, newItem, true );
		}

		public void AddBefore( T item, T newItem )
		{
			AddElementInternal( item, newItem, false );
		}

		//.........................................................................................................//
		//Private Logic
		//.........................................................................................................//

		/// <summary>
		/// Get exist entry from collection
		/// </summary>
		/// <param name="element">element to check</param>
		private  LinkedListNode< T > GetExistEntry( T element )
		{
			//check element not null
			CheckNullElement( element );

			//check exist in collection + null check in ContainsKey
			LinkedListNode< T > entry;
			if( !_elementsMap.TryGetValue( element, out entry ) )
			{
				throw new ArgumentException( string.Format( @"element: {0} not exist in collection", element ) );
			}

			return entry;
		}

		/// <summary>
		/// Check if item is copy of exist item
		/// </summary>
		/// <param name="element">element to check</param>
		private void CheckDuplicationItem( T element )
		{
			//check element not null
			CheckNullElement( element );

			//check exist
			if( _elementsMap.ContainsKey( element ) )
			{
				throw new ArgumentException( string.Format( @"element: {0} already exist in collection", element ) );
			}
		}

		/// <summary>
		/// Check element is null
		/// </summary>
		private void CheckNullElement( T item )
		{
			if( item == null )
			{
				throw new ArgumentNullException( @"item" );
			}
		}

		/// <summary>
		/// Internal base method to add all elements in collection
		/// </summary>
		private void AddElementInternal( T item, T newItem, bool after )
		{
			//check elements
			CheckDuplicationItem( newItem );
			LinkedListNode< T > oldNode = GetExistEntry( item );

			//add element after
			LinkedListNode< T > newNode = new LinkedListNode< T >( newItem );

			//select to add first/last + add
			if( after )
			{
				_elementsList.AddAfter( oldNode, newNode );
			}
			else
			{
				_elementsList.AddBefore( oldNode, newNode );
			}
			_elementsMap.Add( newItem, newNode );

			//rise event
			var args = after ? NotifySetChangedEventArgs< T >.ConstructAddAfterAction( item, newItem ) : NotifySetChangedEventArgs< T >.ConstructAddBeforeAction( item, newItem );
            OnCollectionChanged?.Invoke( args );
		}

		/// <summary>
		/// Try add first element to collection
		/// </summary>
		private bool TryAddFirstInternal( T item )
		{
			//check count
			if( Count != 0 )
			{
				return false;
			}

			//check null item
			CheckNullElement( item );

			//add to collections
			LinkedListNode< T > newNode = _elementsList.AddFirst( item );
			_elementsMap.Add( item, newNode );

            //change event
            OnCollectionChanged?.Invoke( NotifySetChangedEventArgs< T >.ConstructAddFirst( item ) );
			return true;
		}
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
