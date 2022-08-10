using System;

namespace UIAutomationStudio
{
	public abstract class LoopAction: ActionBase
	{
		public Action StartAction { get; set; }
		public Action EndAction { get; set; }
		
		public abstract Action GetNextAction();
		
		public LoopAction()
		{
			StartAction = null;
			EndAction = null;
		}
		
		public bool IsDrawnOnLeft
		{
			get
			{
				if (this.EndAction.Column == this.StartAction.Column && 
					this.EndAction.ColumnSpan < this.StartAction.ColumnSpan)
				{
					return true;
				}
				
				if (this.EndAction.Column < this.StartAction.Column)
				{
					return true;
				}
				
				ActionBase current = this.EndAction;
				ActionBase previous = current.Previous;
				while (previous != null)
				{
					if (previous is ConditionalAction)
					{
						ConditionalAction condAction = (ConditionalAction)previous;
						if (condAction.NextOnFalse == current)
						{
							return true;
						}
						else
						{
							return false;
						}
					}
					
					current = previous;
					previous = previous.Previous;
				}
				
				Action currentAction = this.StartAction;
				while (currentAction != this.EndAction)
				{
					if (currentAction.LoopAction != null)
					{
						return true;
					}
					
					if (!(currentAction.Next is Action))
					{
						break;
					}
					
					currentAction = (Action)(currentAction.Next);
				}
				
				return false;
			}
		}
	}
	
	public class LoopConditional: LoopAction
	{
		public ConditionalAction ConditionalAction { get; set; }
		
		public LoopConditional()
		{
			ConditionalAction = null;
		}
		
		public override Action GetNextAction()
		{
			if (this.ConditionalAction == null || this.ConditionalAction.Evaluate(true, false) != true)
			{
				return null;
			}
			
			return this.StartAction;
		}
	}
	
	public class LoopCount: LoopAction
	{
		public int InitialCount { get; set; }
		private int currentCount = 0;
		
		public LoopCount(int initialCount)
		{
			this.InitialCount = initialCount;
			this.currentCount = initialCount;
		}
		
		public void Reset()
		{
			this.currentCount = this.InitialCount;
		}
		
		public override Action GetNextAction()
		{
			if (this.currentCount > 0)
			{
				this.currentCount--;
			}
		
			if (this.currentCount <= 0)
			{
				return null;
			}
			
			return this.StartAction;
		}
	}
}