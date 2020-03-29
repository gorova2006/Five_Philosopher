using Five_Philosopher.Helpers;
using Five_Philosopher.Synchronizators;
using System;
using System.Threading;

namespace Five_Philosopher
{
	public class UserSynchronizationModeHandler : SynchronizationBaseHandler
	{
		private static CustomLocker spinLock = new CustomLocker();

		public static void Process(Philosopher[] philosophers, Fork[] forks, int index)
		{
			if (!IsPhilosopherTheMostHungryBetweenNeighbors(index, philosophers))
			{
				return;
			}

			var leftFork = forks[index];
			var rightFork = index == philosophers.Length - 1 ? forks[0] : forks[index + 1];
			var philosopher = philosophers[index];
			spinLock.Lock();

			TakeForks(leftFork, rightFork, philosopher);
			PutForks(leftFork, rightFork, philosopher);

			Thread.Sleep(50);
			philosopher.Status = PhilosopherStatus.Hungry;
			Console.Write($" Philosopher {index} has {philosopher.Status} status");

			spinLock.Release();
		}

		private static void TakeForks(Fork left, Fork right, Philosopher philosopher)
		{
			left.IsReadyToUse = false;
			right.IsReadyToUse = false;
			philosopher.Status = PhilosopherStatus.Dining;
			philosopher.LastMeal = DateTime.Now;

			Console.Write($"\n Philosopher {philosopher.Id} is eating. Status is {philosopher.Status}");
		}

		private static void PutForks(Fork left, Fork right, Philosopher philosopher)
		{
			left.IsReadyToUse = true;
			right.IsReadyToUse = true;
			philosopher.Status = PhilosopherStatus.Thinking;

			Console.Write($" Philosopher {philosopher.Id} finished to eat. Status is {philosopher.Status}");
		}
	}
}
