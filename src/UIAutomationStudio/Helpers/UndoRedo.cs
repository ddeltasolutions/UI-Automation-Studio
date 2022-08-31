using System;
using System.Windows;
using System.Collections.Generic;

namespace UIAutomationStudio
{
	public static class UndoRedo
	{
		private static List<Task> tasks = new List<Task>();
		private static int position = -1;
		
		public static void Reset(Task task)
		{
			tasks.Clear();
			
			if (task == null)
			{
				position = -1;
				return;
			}
			
			tasks.Add(task);
			position = 0;
		}
		
		public static void TaskSaved(Task task)
		{
			foreach (Task crtTask in tasks)
			{
				if (task != crtTask)
				{
					crtTask.IsModified = true;
					crtTask.ExeFile = task.ExeFile;
				}
			}
		}
		
		public static void AddSnapshot(Task task)
		{
			if (position < 0)
			{
				throw new Exception("UndoRedo has not been initialized. Use Reset() to initialize it.");
				return;
			}
		
			Task cloneTask = new Task();
			task.DeepCopy(cloneTask);
			
			if (position < tasks.Count - 1)
			{
				tasks.RemoveRange(position + 1, tasks.Count - position - 1);
			}
			
			tasks.Insert(position, cloneTask);
			position++;
		}
		
		public static bool CanUndo
		{
			get
			{
				return position > 0;
			}
		}
		
		public static bool CanRedo
		{
			get
			{
				return (position >= 0 && position < tasks.Count - 1);
			}
		}
		
		public static Task Undo()
		{
			if (position <= 0)
			{
				return null;
			}
			
			position--;
			return tasks[position];
		}
		
		public static Task Redo()
		{
			if (position < 0 || position >= tasks.Count - 1)
			{
				return null;
			}
			
			position++;
			return tasks[position];
		}
	}
}