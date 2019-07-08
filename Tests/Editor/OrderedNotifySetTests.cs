using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;


namespace CrazyPanda.UnityCore.Collections.Tests
{
	public sealed class OrderedNotifySetTests
	{
		private OrderedNotifySet< object > _set;

		private NotifySetChangedEventArgs< object > _lastEventArgs;

		[ SetUp ]
		public void Init()
		{
			//create collection
			_set = new OrderedNotifySet< object >();

			//init event handle
			_lastEventArgs = null;
			_set.OnCollectionChanged += ( sender, args ) => _lastEventArgs = args;
		}

		[ Test ]
		public void AddFirstEmptyTest()
		{
			//arrange
			var newElement = new object();

			//act
			_set.AddFirst( newElement );

			//assert
			SetAssert( newElement );
			LastEventAssert( NotifySetChangeActionType.AddFirst, newElement );
		}

		[ Test ]
		public void AddFirstTest()
		{
			//arrange
			var existItem = new object();
			_set.AddFirst( existItem );

			//act
			var newElement = new object();
			_set.AddFirst( newElement );

			//assert
			SetAssert( newElement, existItem );
			LastEventAssert( NotifySetChangeActionType.AddBefore, newElement, existItem );
		}

		[ Test ]
		public void AddFirstNullTest()
		{
			//act-assert
			Assert.Throws< ArgumentNullException >( () => _set.AddFirst( null ) );
			Assert.Null( _lastEventArgs );
		}

		[ Test ]
		public void AddFirstDuplicationTest()
		{
			//arrange
			var existItem = new object();
			_set.Add( existItem );

			_lastEventArgs = null;

			//act-assert
			Assert.Throws< ArgumentException >( () => _set.AddFirst( existItem ) );
			Assert.Null( _lastEventArgs );
		}

		[ Test ]
		public void AddLastEmptyTest()
		{
			//arrange
			var newElement = new object();

			//act
			_set.AddLast( newElement );

			//assert
			SetAssert( newElement );
			LastEventAssert( NotifySetChangeActionType.AddFirst, newElement );
		}

		[ Test ]
		public void AddLastTest()
		{
			//arrange
			var existItem = new object();
			_set.AddFirst( existItem );

			//act
			var newElement = new object();
			_set.AddLast( newElement );

			//assert
			SetAssert( existItem, newElement );
			LastEventAssert( NotifySetChangeActionType.AddAfter, newElement, existItem );
		}

		[ Test ]
		public void AddLastNullTest()
		{
			//act-assert
			Assert.Throws< ArgumentNullException >( () => _set.AddLast( null ) );
			Assert.Null( _lastEventArgs );
		}

		[ Test ]
		public void AddLastDuplicationTest()
		{
			//arrange
			var existItem = new object();
			_set.Add( existItem );

			//act-assert
			Assert.Throws< ArgumentException >( () => _set.AddLast( existItem ) );
		}

		[ Test ]
		public void ContainsNullTest()
		{
			//act-assert
			Assert.Throws< ArgumentNullException >( () => _set.Contains( null ) );
		}

		[ TestCase( 1, 0 ) ]
		[ TestCase( 10, 0 ) ]
		[ TestCase( 10, 5 ) ]
		[ TestCase( 10, 9 ) ]
		public void RemoveTest( int elementsCount, int indexToRemove )
		{
			//arrange
			List< object > objectsCollection = ConstructTestSet( elementsCount );

			//act
			object elementToRemove = objectsCollection[ indexToRemove ];
			bool result = _set.Remove( elementToRemove );

			//assert
			Assert.True( result );
			SetAssert( objectsCollection.Where( x => !x.Equals( elementToRemove ) ) );
			LastEventAssert( NotifySetChangeActionType.Remove, null, elementToRemove );
		}

		[ Test ]
		public void RemoveNotExistTest()
		{
			//arrange
			var existItem = new object();
			_set.AddFirst( existItem );

			_lastEventArgs = null;

			//act
			bool result = _set.Remove( new object() );

			//assert
			Assert.False( result );
			SetAssert( existItem );
			Assert.Null( _lastEventArgs );
		}

		[ Test ]
		public void RemovewNullTest()
		{
			//act-assert
			Assert.Throws< ArgumentNullException >( () => _set.Remove( null ) );
			Assert.Null( _lastEventArgs );
		}

		[ TestCase( 0 ) ]
		[ TestCase( 1 ) ]
		[ TestCase( 2 ) ]
		public void ClearTest(int elementsCount)
		{
			//arrange
			ConstructTestSet( elementsCount );

			//act
			_set.Clear();

			//assert
			SetAssert( );
			LastEventAssert( NotifySetChangeActionType.Clear );
		}

		[ TestCase( 1, 0 ) ]
		[ TestCase( 4, 0 ) ]
		[ TestCase( 4, 3 ) ]
		[ TestCase( 4, 2 ) ]
		public void AddBeforeTest( int elementsCount, int position )
		{
			//arrange
			List< object > objectsCollection = ConstructTestSet( elementsCount );
			object existItem = objectsCollection[ position ];

			LinkedList< object > realList = new LinkedList< object >( objectsCollection );
			LinkedListNode< object > existItemNode = realList.Find( existItem );

			//act
			object itemToAdd = new object();
			_set.AddBefore( existItemNode.Value, itemToAdd );

			//assert
			LastEventAssert( NotifySetChangeActionType.AddBefore, itemToAdd, existItemNode.Value );

			realList.AddBefore( existItemNode, itemToAdd );
			SetAssert( realList );
		}

		[ Test ]
		public void AddBeforeNullTest()
		{
			//act-assert
			Assert.Throws< ArgumentNullException >( () => _set.AddBefore( null, new object() ) );
			Assert.Throws< ArgumentNullException >( () => _set.AddBefore( new object(), null ) );

			Assert.Null( _lastEventArgs );
		}

		[ Test ]
		public void AddBeforeNotExistTest()
		{
			//arrange
			ConstructTestSet( 1 );
			_lastEventArgs = null;

			//act-assert
			Assert.Throws< ArgumentException >( () => _set.AddBefore( new object(), new object() ) );
			Assert.Null( _lastEventArgs );
		}

		
		[ Test ]
		public void AddBeforeDuplicateTest()
		{
			//arrange
			List<object> objectsCollection =  ConstructTestSet( 1 );
			_lastEventArgs = null;

			//act-assert
			Assert.Throws< ArgumentException >( () => _set.AddBefore( objectsCollection[ 0 ], objectsCollection[ 0 ] ) );
			Assert.Null( _lastEventArgs );
		}

		[ TestCase( 1, 0 ) ]
		[ TestCase( 4, 0 ) ]
		[ TestCase( 4, 3 ) ]
		[ TestCase( 4, 2 ) ]
		public void AddAfterTest( int elementsCount, int position )
		{
			//arrange
			List< object > objectsCollection = ConstructTestSet( elementsCount );
			object existItem = objectsCollection[ position ];

			LinkedList< object > realList = new LinkedList< object >( objectsCollection );
			LinkedListNode< object > existItemNode = realList.Find( existItem );

			//act
			object itemToAdd = new object();
			_set.AddAfter( existItemNode.Value, itemToAdd );

			//assert
			LastEventAssert( NotifySetChangeActionType.AddAfter, itemToAdd, existItemNode.Value );

			realList.AddAfter( existItemNode, itemToAdd );
			SetAssert( realList );
		}

		[ Test ]
		public void AddAfterNullTest()
		{
			//act-assert
			Assert.Throws< ArgumentNullException >( () => _set.AddAfter( null, new object() ) );
			Assert.Throws< ArgumentNullException >( () => _set.AddAfter( new object(), null ) );

			Assert.Null( _lastEventArgs );
		}

		[ Test ]
		public void AddAfterNotExistTest()
		{
			//arrange
			ConstructTestSet( 1 );
			_lastEventArgs = null;

			//act-assert
			Assert.Throws< ArgumentException >( () => _set.AddAfter( new object(), new object() ) );
			Assert.Null( _lastEventArgs );
		}

		
		[ Test ]
		public void AddAfterDuplicateTest()
		{
			//arrange
			List<object> objectsCollection =  ConstructTestSet( 1 );
			_lastEventArgs = null;

			//act-assert
			Assert.Throws< ArgumentException >( () => _set.AddAfter( objectsCollection[ 0 ], objectsCollection[ 0 ] ) );
			Assert.Null( _lastEventArgs );
		}

		private void LastEventAssert(NotifySetChangeActionType type, object newItem = null, object oldItem = null)
		{
			Assert.AreEqual( _lastEventArgs.newItem, newItem );
			Assert.AreEqual( _lastEventArgs.oldItem, oldItem );
			Assert.AreEqual( _lastEventArgs.changeActionTypeType, type );
		}

		private void SetAssert( params object[ ] referenceCollection )
		{
			SetAssert( ( IEnumerable< object > ) referenceCollection );
		}

		private List< object > ConstructTestSet(int count)
		{
			List< object > result = new List< object >( count );
			for( int i = 0; i < count; i++ )
			{
				var newItem = new object();
				result.Add( newItem );
				_set.AddLast( newItem );
			}

			return result;
		}

		private void SetAssert( IEnumerable< object > referenceCollection )
		{
			//collection compare
			CollectionAssert.AreEqual( referenceCollection, _set );

			//compare interface
			int expectCount = referenceCollection.Count();

			Assert.AreEqual( _set.Count, expectCount );

			if( expectCount > 0 )
			{
				Assert.AreEqual( _set.First, referenceCollection.First() );
				Assert.AreEqual( _set.Last, referenceCollection.Last() );
			}
			else
			{
				//check exception on get element from empty collection
				Assert.Throws< InvalidOperationException >( () =>
				{
					var testVar = _set.First;
				} );

				Assert.Throws< InvalidOperationException >( () =>
				{
					var testVar = _set.Last;
				} );
			}

			//check exist
			foreach( object item in referenceCollection )
			{
				Assert.True( _set.Contains( item ) );
			}
		}
	}
}