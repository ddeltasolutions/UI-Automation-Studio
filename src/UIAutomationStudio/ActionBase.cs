
namespace UIAutomationStudio
{
	public abstract class ActionBase
	{
		public int Id { get; set; }
		public System.Windows.Controls.Grid GridNode { get; set; }
		public ActionBase Previous { get; set; }
		
		public int Row { get; set; }
		public int Column { get; set; }
		public int ColumnSpan { get; set; }
		
		public ActionBase()
		{
			Previous = null;
			GridNode = null;
			
			Row = 0;
			Column = 0;
			ColumnSpan = 1;
		}
		
		public void DeepCopy(ActionBase destination)
		{
			if (this is Action && destination is Action)
			{
				Action thisAction = (Action)this;
				thisAction.DeepCopy((Action)destination);
			}
			else if (this is ConditionalAction && destination is ConditionalAction)
			{
				ConditionalAction thisAction = (ConditionalAction)this;
				thisAction.DeepCopy((ConditionalAction)destination);
			}
		}
		
		public bool IsInHierarchyOf(ActionBase action)
		{
			ActionBase current = action;
			while (current != null)
			{
				if (current == this)
				{
					return true;
				}
				current = current.Previous;
			}
			
			return false;
		}
		
		public bool IsDescendentOf(ActionBase action)
		{
			ActionBase current = action;
			while (current != null)
			{
				if (current == this)
				{
					return true;
				}
				
				if (current is Action)
				{
					Action actionNormal = (Action)current;
					
					if (actionNormal.HasWeakBond)
					{
						return false;
					}
					current = actionNormal.Next;
				}
				else if (current is ConditionalAction)
				{
					ConditionalAction actionConditional = (ConditionalAction)current;
					if (actionConditional.NextOnFalse.Previous == current && 
						this.IsDescendentOf(actionConditional.NextOnFalse) == true)
					{
						return true;
					}
					
					if (actionConditional.NextOnTrue.Previous == current &&
						this.IsDescendentOf(actionConditional.NextOnTrue) == true)
					{
						return true;
					}
					break;
				}
				else if (current is EndAction)
				{
					break;
				}
			}
			
			return false;
		}
		
		public event System.Action DeletedEvent;
		
		public void Deleted(bool deleteHierarchy = true)
		{
			if (this is ConditionalAction && deleteHierarchy == true)
			{
				this.HierarchyDeleted();
			}
			else
			{
				if (DeletedEvent != null)
				{
					DeletedEvent();
				}
			}
		}
		
		private void HierarchyDeleted()
		{
			ActionBase current = this;
			while (current != null)
			{
				current.Deleted(false);
				if (current is Action)
				{
					current = ((Action)current).Next;
				}
				else if (current is ConditionalAction)
				{
					ConditionalAction currentConditional = (ConditionalAction)current;
					if (currentConditional.HasOnFalseWeakBond == false)
					{
						currentConditional.NextOnFalse.HierarchyDeleted();
					}
					if (currentConditional.HasOnTrueWeakBond == false)
					{
						currentConditional.NextOnTrue.HierarchyDeleted();
					}
					break;
				}
				else if (current is EndAction)
				{
					break;
				}
			}
		}
		
		public virtual void DrawUnselected() { }
	}
}