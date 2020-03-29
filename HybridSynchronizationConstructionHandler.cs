using Five_Philosopher.Helpers;
using System;
using System.Threading;

namespace Five_Philosopher
{
	public class HybridSynchronizationConstructionHandler : SynchronizationBaseHandler
	{
		private static object locker = new object();

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
			PutForks(leftFork, rightFork, philosopher);
			Think(philosopher, index);
		}

		private static void TakeForks(Fork left, Fork right, Philosopher philosopher)
		{
			bool isLocked = false;
			Monitor.Enter(locker, ref isLocked);
			try
			{
				bool canBeganToEat = left.IsReadyToUse && right.IsReadyToUse && philosopher.Status == PhilosopherStatus.Hungry;
				
				if (!canBeganToEat)
				{
					Monitor.Wait(locker);
				}

				left.IsReadyToUse = false;
				right.IsReadyToUse = false;
				philosopher.Status = PhilosopherStatus.Dining;
				philosopher.LastMeal = DateTime.Now;

				Console.Write($"\n Philosopher {philosopher.Id} is eating. Status is {philosopher.Status}");
			}
			catch
			{
				if (isLocked)
				{
					Monitor.Exit(locker);
				}
			}
		}

		private static void PutForks(Fork left, Fork right, Philosopher philosopher)
		{
			bool isLocked = false;
			Monitor.Enter(locker, ref isLocked);
			try
			{
				left.IsReadyToUse = true;
				right.IsReadyToUse = true;
				philosopher.Status = PhilosopherStatus.Thinking;

				Monitor.PulseAll(locker);

				Console.Write($" Philosopher {philosopher.Id} finished to eat. Status is {philosopher.Status}");
			}
			catch
			{
				if (isLocked)
				{
					Monitor.Exit(locker);
				}
			}
		}

		private static void Think(Philosopher philosopher, int index)
		{
			bool isLocked = false;
			Monitor.Enter(locker, ref isLocked);
			try
			{
				Thread.Sleep(50);
				philosopher.Status = PhilosopherStatus.Hungry;
				Console.Write($" Philosopher {index} has {philosopher.Status} status");
			}
			catch
			{
				if (isLocked)
				{
					Monitor.Exit(isLocked);
				}
			}
		}
	}
}
