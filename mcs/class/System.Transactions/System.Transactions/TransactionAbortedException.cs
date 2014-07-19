//
// TransactionAbortedException.cs
//
// Author:
//	Atsushi Enomoto  <atsushi@ximian.com>
//
// (C)2005 Novell Inc,
//

#if NET_2_0
using System.Runtime.Serialization;

namespace System.Transactions
{
#if !WINDOWS_STORE_APP
	[Serializable]
#endif
	public class TransactionAbortedException : TransactionException
	{
		public TransactionAbortedException ()
		{
		}

		public TransactionAbortedException (string message)
			: base (message)
		{
		}

		public TransactionAbortedException (string message, Exception innerException)
			: base (message, innerException)
		{
		}

#if !WINDOWS_STORE_APP
		protected TransactionAbortedException (SerializationInfo info,
			StreamingContext context)
			: base (info, context)
		{
		}
#endif
	}
}

#endif
