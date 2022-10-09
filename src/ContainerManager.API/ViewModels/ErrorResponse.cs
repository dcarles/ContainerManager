using System.Collections.Generic;
using System.Text.Json;

namespace ContainerManager.API.ViewModels
{
	public class ErrorResponse
	{
		public int StatusCode { get; set; }

		public string Message { get; set; }

		public List<ErrorResponseItem> Errors { get; set; }

		public override string ToString() => JsonSerializer.Serialize(this);
	}
}
