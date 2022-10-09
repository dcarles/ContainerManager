using System.Collections.Generic;

namespace ContainerManager.API.ViewModels
{
	public class ErrorResponseItem
	{
		public string Field { get; set; }

		public IEnumerable<string> Errors { get; set; }

		public override string ToString() => $"{Field}: {string.Join(",", Errors)}";
	}
}
