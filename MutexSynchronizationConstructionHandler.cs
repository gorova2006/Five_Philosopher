using Five_Philosopher.Helpers;
using System;
using System.Threading;

namespace Five_Philosopher
{
	public class MutexSynchronizationConstructionHandler : SynchronizationBaseHandler
	{
		private static Mutex mutexLock = new Mutex();

		public static void Process(Philosopher[] philosophers, Fork[] forks, int index)
		{
			if (!IsPhilosopherTheMostHungryBetweenNeighbors(index, philosophers))
			{
				return;
			}

			var leftFork = forks[index];
			var rightFork = index == philosophers.Length - 1 ? forks[0] : forks[index + 1];
			var philosopher = philosophers[index];

			TakeForks(leftFork, rightFork, philosopher);
		}

		private static void TakeForks(Fork leftFork, Fork rightFork, Philosopher philosopher)
		{
			try
			{
				bool canBeganToEat = leftFork.IsReadyToUse && rightFork.IsReadyToUse && philosopher.Status == PhilosopherStatus.Hungry;
				
				if (canBeganToEat && mutexLock.WaitOne())
				{
					leftFork.IsReadyToUse = false;
					rightFork.IsReadyToUse = false;
					philosopher.Status = PhilosopherStatus.Dining;
					philosopher.LastMeal = DateTime.Now;

					Console.Write($"\n Philosopher {philosopher.Id} is eating. Status is {philosopher.Status}");
					PutForks(leftFork, rightFork, philosopher);
				}

			}
			finally
			{
				if (mutexLock.WaitOne() && mutexLock != null)
				{
					mutexLock.ReleaseMutex();
				}
			}
		}

		private static void PutForks(Fork left, Fork right, Philosopher philosopher)
		{
			mutexLock.WaitOne();
			try
			{
				left.IsReadyToUse = true;
				right.IsReadyToUse = true;
				philosopher.Status = PhilosopherStatus.Thinking;

				Console.Write($" Philosopher {philosopher.Id} finished to eat. Status is {philosopher.Status}");
				Think(philosopher, philosopher.Id);
			}
			finally
			{
				if (mutexLock.WaitOne() && mutexLock != null)
				{
					mutexLock.ReleaseMutex();
				}
			}
		}

		private static void Think(Philosopher philosopher, int index)
		{
			mutexLock.WaitOne();
			try
			{
				Thread.Sleep(50);
				philosopher.Status = PhilosopherStatus.Hungry;
				Console.Write($" Philosopher {index} has {philosopher.Status} status");
			}
			finally
			{
				if (mutexLock.WaitOne() && mutexLock != null)
				{
					mutexLock.ReleaseMutex();
				}
			}
		}
	}
}
