using Intercom.Data;

namespace Intercom.Core
{
	public class Models
	{
		public virtual string type { get; set; }
		public virtual Pages pages { get; set; }
		public virtual int total_count { get; set; }
	}
}