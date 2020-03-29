using System.Threading;

namespace Five_Philosopher.Synchronizators
{
	public struct CustomLocker
	{
		private int resourceInUse;

		public void Lock()
		{
			while (true)
			{
				if (Interlocked.Exchange(ref resourceInUse, 1) == 0) return;
			}
		}

		public void Release()
		{
			Volatile.Write(ref resourceInUse, 0);
		}
	}
}
