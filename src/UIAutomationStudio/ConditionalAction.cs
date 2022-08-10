using System;
using System.Windows;
using System.Collections.Generic;
using System.Xml;

namespace UIAutomationStudio
{
	public class ConditionalAction: ActionBase
	{
		public List<ConditionWrapper> ConditionWrappers { get; set; }
		
		public string Name { get; set; }
		public ActionBase NextOnTrue { get; set; }
		public ActionBase NextOnFalse { get; set; }
		
		public ConditionalAction()
		{
			this.ConditionWrappers = new List<ConditionWrapper>();
			this.Name = null;
			
			this.NextOnTrue = new EndAction();
			this.NextOnTrue.Previous = this;
			this.NextOnFalse = new EndAction();
			this.NextOnFalse.Previous = this;
		}
		
		public void DeepCopy(ConditionalAction destination)
		{
			destination.Name = this.Name;
			
			destination.ConditionWrappers = new List<ConditionWrapper>();
			foreach (ConditionWrapper conditionWrapper in this.ConditionWrappers)
			{
				ConditionWrapper conditionWrapperDest = new ConditionWrapper();
				conditionWrapper.DeepCopy(conditionWrapperDest);
				
				destination.ConditionWrappers.Add(conditionWrapperDest);
			}
		}
		
		public string GetDescription(bool full = false)
		{
			string description = "";
			foreach (ConditionWrapper condWrapper in ConditionWrappers)
			{
				if (description != "")
				{
					description += " " + condWrapper.LogicalOpDescription + " " + condWrapper.GetDescription(full);
				}
				else
				{
					description = condWrapper.GetDescription(full);
				}
			}
			return description;
		}
		
		public bool? Evaluate(bool message, bool messageCondition)
		{
			bool result = false;
			if (this.ConditionWrappers.Count == 1)
			{
				message = false;
			}
			
			for (int i = 0; i < this.ConditionWrappers.Count; i++)
			{
				bool? currentResult = null;
				ConditionWrapper currentConditionWrapper = this.ConditionWrappers[i];
			
				if (i == 0)
				{
					currentResult = currentConditionWrapper.Condition.Evaluate(messageCondition);
					if (currentResult == null)
					{
						if (message == true)
						{
							MessageBox.Show(MainWindow.Instance, "First Condition could not be evaluated");
						}
						return null;
					}
					result = currentResult.Value;
				}
				else
				{
					if (result == false && currentConditionWrapper.LogicalOp == LogicalOp.AND)
					{
						return false;
					}
					if (result == true && currentConditionWrapper.LogicalOp == LogicalOp.OR)
					{
						return true;
					}
				
					currentResult = currentConditionWrapper.Condition.Evaluate(messageCondition);
					if (currentResult == null)
					{
						if (message == true)
						{
							MessageBox.Show(MainWindow.Instance, Helper.GetOrdinalAsString(i) + " Condition could not be evaluated");
						}
						return null;
					}
					
					if (currentConditionWrapper.LogicalOp == LogicalOp.AND)
					{
						result = result && currentResult.Value;
					}
					else // LogicalOp.OR
					{
						result = result || currentResult.Value;
					}
				}
			}
		
			return result;
		}
		
		public bool LoadFromXml(XmlNode actionNode)
		{
			XmlAttribute xmlAttribute = actionNode.Attributes["Id"];
			if (xmlAttribute != null)
			{
				this.Id = int.Parse(xmlAttribute.InnerText);
			}
			
			xmlAttribute = actionNode.Attributes["Name"];
			if (xmlAttribute != null)
			{
				this.Name = xmlAttribute.InnerText;
			}
			
			XmlNodeList condWrappersNodeList = actionNode.SelectNodes("ConditionWrappers");
			if (condWrappersNodeList.Count == 0)
			{
				return false;
			}
			
			XmlNode condWrappersNode = condWrappersNodeList[0];
			XmlNodeList condWrapperNodeList = condWrappersNode.SelectNodes("ConditionWrapper");
			
			foreach (XmlNode condWrapperNode in condWrapperNodeList)
			{
				ConditionWrapper conditionWrapper = new ConditionWrapper();
				conditionWrapper.LoadFromXml(condWrapperNode);
				
				this.ConditionWrappers.Add(conditionWrapper);
			}
			
			return true;
		}
		
		public XmlElement WriteToXml(XmlDocument doc)
		{
			XmlElement actionNode = doc.CreateElement("Action");
			actionNode.SetAttribute("Type", "Conditional");
			actionNode.SetAttribute("Id", this.Id.ToString());
			actionNode.SetAttribute("Name", this.Name);
			
			if (this.NextOnFalse != null)
			{
				actionNode.SetAttribute("NextOnFalseId", this.NextOnFalse.Id.ToString());
			}
			if (this.NextOnTrue != null)
			{
				actionNode.SetAttribute("NextOnTrueId", this.NextOnTrue.Id.ToString());
			}
			
			string weakBond = "";
			if (this.HasOnFalseWeakBond == true)
			{
				weakBond += "OnFalse";
			}
			if (this.HasOnTrueWeakBond == true)
			{
				weakBond += "OnTrue";
			}
			if (weakBond != "")
			{
				actionNode.SetAttribute("WeakBound", weakBond);
			}
			
			XmlElement condWrappersNode = doc.CreateElement("ConditionWrappers");
			actionNode.AppendChild(condWrappersNode);
			
			foreach (ConditionWrapper conditionWrapper in ConditionWrappers)
			{
				XmlElement condWrapperNode = conditionWrapper.WriteToXml(doc);
				condWrappersNode.AppendChild(condWrapperNode);
			}
		
			return actionNode;
		}
		
		public bool HasOnFalseWeakBond
		{
			get
			{
				return this.NextOnFalse.Previous != this;
			}
		}
		
		public bool HasOnTrueWeakBond
		{
			get
			{
				return this.NextOnTrue.Previous != this;
			}
		}
	}
	
	public class ConditionWrapper
	{
		public Condition Condition { get; set; }
		public LogicalOp LogicalOp { get; set; }
		
		public ConditionWrapper()
		{
			this.Condition = null;
			this.LogicalOp = LogicalOp.None;
		}
		
		public bool LoadFromXml(XmlNode condWrapperNode)
		{
			XmlAttribute xmlAttribute = condWrapperNode.Attributes["LogicalOp"];
			if (xmlAttribute != null)
			{
				this.LogicalOp = (LogicalOp)Enum.Parse(typeof(LogicalOp), xmlAttribute.InnerText);
			}
			
			XmlNodeList conditionNodeList = condWrapperNode.SelectNodes("Condition");
			if (conditionNodeList.Count == 0)
			{
				return false;
			}
			
			XmlNode conditionNode = conditionNodeList[0];
			this.Condition = new Condition();
			this.Condition.LoadFromXml(conditionNode);
			
			return true;
		}
		
		public XmlElement WriteToXml(XmlDocument doc)
		{
			XmlElement condWrapperNode = doc.CreateElement("ConditionWrapper");
			if (this.LogicalOp != LogicalOp.None)
			{
				condWrapperNode.SetAttribute("LogicalOp", this.LogicalOp.ToString());
			}
			
			XmlElement conditionNode = this.Condition.WriteToXml(doc);
			condWrapperNode.AppendChild(conditionNode);
			
			return condWrapperNode;
		}
		
		public void DeepCopy(ConditionWrapper destination)
		{
			if (this.Condition != null)
			{
				destination.Condition = new Condition();
				Condition.DeepCopy(this.Condition, destination.Condition);
			}
			
			destination.LogicalOp = this.LogicalOp;
		}
		
		public string LogicalOpDescription
		{
			get
			{
				return (this.LogicalOp == LogicalOp.None ? "" : this.LogicalOp.ToString());
			}
		}
		
		public string GetDescription(bool full = false)
		{
			if (full == false)
			{
				return Condition.Variable.PropertyDescription + " " + Condition.Description;
			}
			else
			{
				return Condition.Variable.DisplayElement + " " + Condition.Variable.PropertyDescription + 
					" " + Condition.Description;
			}
		}
	}
	
	public class EndAction: ActionBase
	{
		public bool LoadFromXml(XmlNode actionNode)
		{
			XmlAttribute xmlAttribute = actionNode.Attributes["Id"];
			if (xmlAttribute != null)
			{
				this.Id = int.Parse(xmlAttribute.InnerText);
			}
			
			return true;
		}
	
		public XmlElement WriteToXml(XmlDocument doc)
		{
			XmlElement actionNode = doc.CreateElement("Action");
			actionNode.SetAttribute("Type", "End");
			actionNode.SetAttribute("Id", this.Id.ToString());
			
			return actionNode;
		}
	}
}