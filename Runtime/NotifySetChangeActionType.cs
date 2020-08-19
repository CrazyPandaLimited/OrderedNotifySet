namespace CrazyPanda.UnityCore.Collections
{
	public enum NotifySetChangeActionType
	{
		/// <summary>
		/// Add first item to empty collection
		/// </summary>
		AddFirst,

		/// <summary>
		/// Add item before exist item
		/// </summary>
		AddBefore,

		/// <summary>
		/// Add item after exist item
		/// </summary>
		AddAfter,

		/// <summary>
		/// Collection was cleared
		/// </summary>
		Clear,

		/// <summary>
		/// Remove item from collection
		/// </summary>
		Remove
	}
}
