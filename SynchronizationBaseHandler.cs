namespace Five_Philosopher.Helpers
{
	public abstract class SynchronizationBaseHandler
	{
		public static bool IsPhilosopherTheMostHungryBetweenNeighbors(int index, Philosopher[] philosophers)
		{
			int leftNeighbor = index == 0 ? philosophers.Length - 1 : index - 1;
			int rightNeighbor = index == philosophers.Length - 1 ? 0 : index + 1;

			if (philosophers[index].LastMeal == null)
			{
				return true;
			}

			if (philosophers[leftNeighbor].LastMeal == null || philosophers[rightNeighbor].LastMeal == null)
			{
				return false;
			}

			var mostHungryNeighbor = philosophers[leftNeighbor].LastMeal < philosophers[rightNeighbor].LastMeal
				? leftNeighbor
				: rightNeighbor;

			return philosophers[index].LastMeal < philosophers[mostHungryNeighbor].LastMeal;
		}
	}
}
