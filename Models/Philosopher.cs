using System;

namespace Five_Philosopher
{
	public class Philosopher
	{
		public Philosopher(int id)
		{
			this.Id = id;
		}

		public int Id { get; set; }

		public PhilosopherStatus Status { get; set; }

		public DateTime? LastMeal { get; set; }
	}
}
