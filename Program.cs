using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Five_Philosopher
{
	class Program
	{
		static void Main(string[] args)
		{
			IList<Philosopher> philosophers = new List<Philosopher>();
			IList<Fork> forks = new List<Fork>();

			//repressents hybrid synchronization
			//Action<Philosopher[], Fork[], int> process = (ph, f, ind) => HybridSynchronizationConstructionHandler.Process(ph, f, ind);
			//repressent Synchronization in the user mode
			//Action<Philosopher[], Fork[], int> process = (ph, f, ind) => UserSynchronizationModeHandler.Process(ph, f, ind);

			//repressent Synchronization in the kernel mode
			Action<Philosopher[], Fork[], int> process = (ph, f, index) => MutexSynchronizationConstructionHandler.Process(ph, f, index);

			Initialize(philosophers, forks, 5);

			while (true)
			{
				for (int i = 0; i < philosophers.Count; i++)
				{
					int index = i;
					Task.Run(() => process(philosophers.ToArray(), forks.ToArray(), index));
				}
			}
		}

		private static void Initialize(IList<Philosopher> philosophers, IList<Fork> forks, int philosophersAmount)
		{
			for (int i = 0; i < philosophersAmount; i++)
			{
				philosophers.Add(new Philosopher(i));
				forks.Add(new Fork(true));
			}
		}
	}
}
