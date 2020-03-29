namespace Five_Philosopher
{
	public class Fork
	{
		public Fork(bool isReadyToUse)
		{
			this.IsReadyToUse = true;
		}

		public bool IsReadyToUse { get; set; }
	}
}
