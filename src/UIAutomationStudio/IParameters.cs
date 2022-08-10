using System.Collections.Generic;

namespace UIAutomationStudio
{
	public interface IParameters
	{
		void Init(List<object> parameters);
	}
	
	public interface IValidateCondition
	{
		bool ValidateParams(Condition condition);
		void Init(Condition condition);
	}
}