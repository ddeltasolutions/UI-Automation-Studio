using System;
using System.Windows;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using UIDeskAutomationLib;

namespace UIAutomationStudio
{
	// Load from and Save to xml extension methods
	public static class XmlExtension
	{
		// Load Task from xml *******************
		public static bool LoadFromXmlFile(this Task task, string xmlFile)
		{
			XmlDocument doc = new XmlDocument();
			
			try
			{
				doc.Load(xmlFile);
			}
			catch (Exception ex)
			{
				Helper.MessageBoxShow("Cannot open the XML file: " + ex.Message);
				return false;
			}
			
			XmlNodeList taskNodeList = doc.GetElementsByTagName("Task");
			if (taskNodeList.Count == 0)
			{
				Helper.MessageBoxShow("The task is empty");
				return false;
			}
			
			return task.LoadFromXml(taskNodeList[0]);
		}
		
		public static bool LoadFromXml(this Task task, XmlNode taskNode)
		{
			XmlAttribute xmlAttribute = taskNode.Attributes["Speed"];
			if (xmlAttribute != null)
			{
				task.Speed = (Speed)Enum.Parse(typeof(Speed), xmlAttribute.InnerText);
			}
			if (task.Speed == Speed.Custom)
			{
				xmlAttribute = taskNode.Attributes["SpeedValue"];
				if (xmlAttribute != null)
				{
					try
					{
						task.SpeedValue = int.Parse(xmlAttribute.InnerText);
					}
					catch { }
				}
			}
			
			// Load actions
			XmlNodeList actionsNodeList = taskNode.SelectNodes("Actions");
			if (actionsNodeList.Count > 0)
			{
				XmlNode actionsNode = actionsNodeList[0];
				XmlNodeList actionNodeList = actionsNode.SelectNodes("Action");
				
				var allActions = new Dictionary<ActionBase, Tuple<int[], string>>();
				foreach (XmlNode actionNode in actionNodeList)
				{
					string type = actionNode.Attributes["Type"].InnerText;
					
					string weakBond = null;
					xmlAttribute = actionNode.Attributes["WeakBound"];
					if (xmlAttribute != null)
					{
						weakBond = xmlAttribute.InnerText;
					}
				
					if (type == "Normal")
					{
						Action action = new Action();
						action.LoadFromXml(actionNode);
						
						int nextId = -1;
						xmlAttribute = actionNode.Attributes["NextId"];
						if (xmlAttribute != null)
						{
							string nextIdString = xmlAttribute.InnerText;
							nextId = int.Parse(nextIdString);
						}
						
						// Store next action id in the NextOnFalse field
						var tuple = new Tuple<int[], string>(new int[] { nextId }, weakBond);
						allActions.Add(action, tuple);
					}
					else if (type == "Conditional")
					{
						ConditionalAction conditionalAction = new ConditionalAction();
						conditionalAction.LoadFromXml(actionNode);
						
						string nextOnFalseIdString = actionNode.Attributes["NextOnFalseId"].InnerText;
						int nextOnFalseId = int.Parse(nextOnFalseIdString);
						
						string nextOnTrueIdString = actionNode.Attributes["NextOnTrueId"].InnerText;
						int nextOnTrueId = int.Parse(nextOnTrueIdString);
						
						var tuple = new Tuple<int[], string>(new int[] { nextOnFalseId, nextOnTrueId }, weakBond);
						
						allActions.Add(conditionalAction, tuple);
						
						task.ConditionalCount++;
					}
					else if (type == "End")
					{
						EndAction endAction = new EndAction();
						endAction.LoadFromXml(actionNode);
						allActions.Add(endAction, null);
					}
				}
				
				foreach (var keyValuePair in allActions)
				{
					ActionBase action = keyValuePair.Key;
					
					Tuple<int[], string> tuple = keyValuePair.Value;
					int[] ids = (tuple != null ? tuple.Item1 : null);
					string weakBond = (tuple != null? tuple.Item2 : null);
					
					if (task.StartAction == null)
					{
						task.StartAction = action;
					}
					
					if (action is Action)
					{
						Action actionNormal = (Action)action;
						actionNormal.Next = allActions.Keys.FirstOrDefault(x => x.Id == ids[0]);
						
						if (actionNormal.Next != null && weakBond == null)
						{
							actionNormal.Next.Previous = actionNormal;
						}
					}
					else if (action is ConditionalAction)
					{
						ConditionalAction actionConditional = (ConditionalAction)action;
						
						actionConditional.NextOnFalse = allActions.Keys.First(x => x.Id == ids[0]);
						if (weakBond == null || !weakBond.Contains("OnFalse"))
						{
							actionConditional.NextOnFalse.Previous = actionConditional;
						}
						
						actionConditional.NextOnTrue = allActions.Keys.First(x => x.Id == ids[1]);
						if (weakBond == null || !weakBond.Contains("OnTrue"))
						{
							actionConditional.NextOnTrue.Previous = actionConditional;
						}
					}
				}
			}
			
			XmlNodeList loopActionsNodeList = taskNode.SelectNodes("LoopActions");
			if (loopActionsNodeList.Count > 0)
			{
				XmlNode loopActionsNode = loopActionsNodeList[0];
				XmlNodeList loopActionNodeList = loopActionsNode.SelectNodes("LoopAction");
				
				foreach (XmlNode loopActionNode in loopActionNodeList)
				{
					xmlAttribute = loopActionNode.Attributes["Type"];
					if (xmlAttribute == null)
					{
						continue;
					}
					string loopType = xmlAttribute.InnerText;
					
					int startActionId = 0, endActionId = 0;
					xmlAttribute = loopActionNode.Attributes["StartActionId"];
					if (xmlAttribute == null)
					{
						continue;
					}
					
					try
					{
						startActionId = int.Parse(xmlAttribute.InnerText);
					}
					catch { continue; }
					
					xmlAttribute = loopActionNode.Attributes["EndActionId"];
					if (xmlAttribute == null)
					{
						continue;
					}
					
					try
					{
						endActionId = int.Parse(xmlAttribute.InnerText);
					}
					catch { continue; }

					Action startAction = task.GetActionById(startActionId) as Action;
					if (startAction == null)
					{
						continue;
					}
					Action endAction = task.GetActionById(endActionId) as Action;
					if (endAction == null)
					{
						continue;
					}
					
					LoopAction newLoop = null;
					if (loopType == "Count")
					{
						int initialCount = 0;
						xmlAttribute = loopActionNode.Attributes["InitialCount"];
						if (xmlAttribute == null)
						{
							continue;
						}
						
						try
						{
							initialCount = int.Parse(xmlAttribute.InnerText);
						}
						catch { continue; }
					
						newLoop = new LoopCount(initialCount) { StartAction = startAction, EndAction = endAction };
					}
					else if (loopType == "Conditional")
					{
						XmlNodeList condActionNodeList = loopActionNode.SelectNodes("Action");
						if (condActionNodeList.Count == 0)
						{
							continue;
						}
						
						XmlNode condActionNode = condActionNodeList[0];
						
						ConditionalAction conditionalAction = new ConditionalAction();
						conditionalAction.LoadFromXml(condActionNode);
							
						newLoop = new LoopConditional() { ConditionalAction = conditionalAction, 
							StartAction = startAction, EndAction = endAction };
					}
					
					endAction.LoopAction = newLoop;
					task.LoopActions.Add(newLoop);
				}
			}
			
			return true;
		}
		// **************************************
		
		// write Task to xml ********************
		public static XmlDocument WriteToXml(this Task task)
		{
			XmlDocument doc = new XmlDocument();
			XmlElement root = doc.CreateElement("Task");
			root.SetAttribute("Speed", task.Speed.ToString());
			if (task.Speed == Speed.Custom)
			{
				root.SetAttribute("SpeedValue", task.SpeedValue.ToString());
			}
			doc.AppendChild(root);
			
			XmlElement actionsNode = doc.CreateElement("Actions");
			root.AppendChild(actionsNode);
			
			task.WriteToXml(doc, actionsNode, task.StartAction);
			
			if (task.LoopActions.Count > 0)
			{
				XmlElement loopActionsNode = doc.CreateElement("LoopActions");
				root.AppendChild(loopActionsNode);
				task.WriteToXml(doc, loopActionsNode, task.LoopActions);
			}
			
			return doc;
		}
		
		public static void WriteToXml(this Task task, XmlDocument doc, XmlElement loopActionsNode, List<LoopAction> loopActions)
		{
			foreach (LoopAction loopAction in loopActions)
			{
				XmlElement loopActionNode = doc.CreateElement("LoopAction");
				loopActionsNode.AppendChild(loopActionNode);
			
				if (loopAction is LoopConditional)
				{
					loopActionNode.SetAttribute("Type", "Conditional");
				
					LoopConditional loopConditional = (LoopConditional)loopAction;
				
					XmlElement condActionNode = loopConditional.ConditionalAction.WriteToXml(doc);
					loopActionNode.AppendChild(condActionNode);
				}
				else if (loopAction is LoopCount)
				{
					loopActionNode.SetAttribute("Type", "Count");
					
					LoopCount loopCount = (LoopCount)loopAction;
					loopActionNode.SetAttribute("InitialCount", loopCount.InitialCount.ToString());
				}
				
				loopActionNode.SetAttribute("StartActionId", loopAction.StartAction.Id.ToString());
				loopActionNode.SetAttribute("EndActionId", loopAction.EndAction.Id.ToString());
			}
		}
		
		public static void WriteToXml(this Task task, XmlDocument doc, XmlElement actionsNode, ActionBase startAction)
		{
			ActionBase currentAction = startAction;
			while (currentAction != null)
			{
				if (currentAction is Action)
				{
					Action currentActionNormal = (Action)currentAction;
					XmlElement actionNode = currentActionNormal.WriteToXml(doc);
					actionsNode.AppendChild(actionNode);
					
					if (currentActionNormal.HasWeakBond == true)
					{
						break;
					}
					
					currentAction = currentActionNormal.Next;
				}
				else if (currentAction is ConditionalAction)
				{
					ConditionalAction currentActionConditional = (ConditionalAction)currentAction;
					XmlElement actionNode = currentActionConditional.WriteToXml(doc);
					actionsNode.AppendChild(actionNode);
					
					if (currentActionConditional.HasOnFalseWeakBond == false)
					{
						task.WriteToXml(doc, actionsNode, currentActionConditional.NextOnFalse);
					}
					if (currentActionConditional.HasOnTrueWeakBond == false)
					{
						task.WriteToXml(doc, actionsNode, currentActionConditional.NextOnTrue);
					}
					
					break;
				}
				else if (currentAction is EndAction)
				{
					EndAction currentActionEnd = (EndAction)currentAction;
					XmlElement actionNode = currentActionEnd.WriteToXml(doc);
					actionsNode.AppendChild(actionNode);
					
					break;
				}
			}
		}
		// **************************************
		
		// load/write Condition from/to xml *****
		public static bool LoadFromXml(this Condition condition, XmlNode conditionNode)
		{
			XmlNodeList variableNodeList = conditionNode.SelectNodes("Variable");
			if (variableNodeList.Count > 0)
			{
				XmlNode variableNode = variableNodeList[0];
				condition.Variable.LoadFromXml(variableNode);
			}
			
			XmlAttribute xmlAttribute = conditionNode.Attributes["Operator"];
			if (xmlAttribute != null)
			{
				condition.Operator = (Operator)Enum.Parse(typeof(Operator), xmlAttribute.InnerText);
			}
			
			xmlAttribute = conditionNode.Attributes["Deny"];
			if (xmlAttribute != null)
			{
				try
				{
					condition.Deny = bool.Parse(xmlAttribute.InnerText);
				}
				catch { }
			}
			
			XmlNodeList valuesNodeList = conditionNode.SelectNodes("Values");
			if (valuesNodeList.Count == 0)
			{
				return false;
			}
			
			XmlNode valuesNode = valuesNodeList[0];
			XmlNodeList valueNodeList = valuesNode.SelectNodes("Value");
			
			if (valueNodeList.Count > 0 && condition.Values == null)
			{
				condition.Values = new List<object>();
			}
			
			foreach (XmlNode valueNode in valueNodeList)
			{
				xmlAttribute = valueNode.Attributes["Type"];
				if (xmlAttribute == null)
				{
					continue;
				}
				string type = xmlAttribute.InnerText;
				string valueStr = valueNode.InnerText;
				if (type == "Boolean")
				{
					bool val = bool.Parse(valueStr);
					condition.Values.Add(val);
				}
				else if (type == "Int32")
				{
					int val = int.Parse(valueStr);
					condition.Values.Add(val);
				}
				else if (type == "Double")
				{
					double val = double.Parse(valueStr);
					condition.Values.Add(val);
				}
				else if (type == "DateTime")
				{
					DateTime val = DateTime.Parse(valueStr, System.Globalization.CultureInfo.CurrentCulture);
					condition.Values.Add(val);
				}
				else // string
				{
					condition.Values.Add(valueStr);
				}
			}
			
			return true;
		}
		
		public static XmlElement WriteToXml(this Condition condition, XmlDocument doc)
		{
			XmlElement conditionNode = doc.CreateElement("Condition");
			
			XmlElement variableNode = condition.Variable.WriteToXml(doc);
			conditionNode.AppendChild(variableNode);
			
			conditionNode.SetAttribute("Operator", condition.Operator.ToString());
			
			if (condition.Deny == true)
			{
				conditionNode.SetAttribute("Deny", condition.Deny.ToString());
			}
			
			XmlElement valuesNode = doc.CreateElement("Values");
			foreach (object val in condition.Values)
			{
				XmlElement valueNode = doc.CreateElement("Value");
				valueNode.SetAttribute("Type", val.GetType().ToString().Replace("System.", ""));
				valueNode.InnerText = val.ToString();
				
				valuesNode.AppendChild(valueNode);
			}
			conditionNode.AppendChild(valuesNode);
			
			return conditionNode;
		}
		// **************************************
		
		// load/write Variable from/to xml ******
		public static bool LoadFromXml(this Variable variable, XmlNode variableNode)
		{
			XmlNodeList hierarchyNodeList = variableNode.SelectNodes("Hierarchy");
			if (hierarchyNodeList.Count > 0)
			{
				XmlNode hierarchyNode = hierarchyNodeList[0];
				variable.Element = new Element();
				variable.Element.LoadHierarchyFromXml(hierarchyNode);
			}
			
			XmlAttribute xmlAttribute = variableNode.Attributes["PropertyId"];
			if (xmlAttribute != null)
			{
				variable.PropertyId = (PropertyId)Enum.Parse(typeof(PropertyId), xmlAttribute.InnerText);
			}
			
			XmlNodeList parametersNodeList = variableNode.SelectNodes("Parameters");
			if (parametersNodeList.Count > 0)
			{
				XmlNode parametersNode = parametersNodeList[0];
				XmlNodeList parameterNodeList = parametersNode.SelectNodes("Parameter");
				
				if (parameterNodeList.Count > 0)
				{
					variable.Parameters = new List<object>();
				}
				
				foreach (XmlNode parameterNode in parameterNodeList)
				{
					xmlAttribute = parameterNode.Attributes["Type"];
					if (xmlAttribute == null)
					{
						continue;
					}
					
					string typeStr = xmlAttribute.InnerText;
					string valueStr = parameterNode.InnerText;
					
					if (typeStr == "Int32")
					{
						int val = int.Parse(valueStr);
						variable.Parameters.Add(val);
					}
					else if (typeStr == "Double")
					{
						double val = double.Parse(valueStr);
						variable.Parameters.Add(val);
					}
					else // string
					{
						variable.Parameters.Add(valueStr);
					}
				}
			}
		
			return true;
		}
		
		public static XmlElement WriteToXml(this Variable variable, XmlDocument doc)
		{
			XmlElement variableNode = doc.CreateElement("Variable");
			
			XmlElement hierarchyNode = variable.Element.WriteHierarchyToXml(doc);
			variableNode.AppendChild(hierarchyNode);
			
			variableNode.SetAttribute("PropertyId", variable.PropertyId.ToString());
			
			if (variable.Parameters != null)
			{
				XmlElement parametersNode = doc.CreateElement("Parameters");
				foreach (object parameter in variable.Parameters)
				{
					XmlElement parameterNode = doc.CreateElement("Parameter");
					parameterNode.SetAttribute("Type", parameter.GetType().ToString().Replace("System.", ""));
					parameterNode.InnerText = parameter.ToString();
					parametersNode.AppendChild(parameterNode);
				}
				variableNode.AppendChild(parametersNode);
			}
			
			return variableNode;
		}
		// **************************************
		
		// load/write Element from/to xml *******
		public static bool LoadHierarchyFromXml(this Element element, XmlNode hierarchyNode)
		{
			XmlNodeList elementNodeList = hierarchyNode.SelectNodes("Element");
			
			if (elementNodeList.Count > 0)
			{
				element.LoadFromXml(elementNodeList[0]);
				
				for (int i = 1; i < elementNodeList.Count; i++)
				{
					Element parent = new Element();
					parent.LoadFromXml(elementNodeList[i]);
					element.Parent = parent;
					
					element = parent;
				}
			}
			
			return true;
		}
		
		public static bool LoadFromXml(this Element element, XmlNode elementNode)
		{
			XmlAttribute xmlAttribute = elementNode.Attributes["ControlType"];
			if (xmlAttribute == null)
			{
				return false;
			}
			
			element.ControlType = (ControlType)Enum.Parse(typeof(ControlType), xmlAttribute.InnerText);
			
			xmlAttribute = elementNode.Attributes["WNT"];
			if (xmlAttribute != null)
			{
				bool wnt = false;
				if (bool.TryParse(xmlAttribute.InnerText, out wnt) == true)
				{
					element.WasNameTruncated = wnt;
				}
			}
			
			xmlAttribute = elementNode.Attributes["Index"];
			if (xmlAttribute != null)
			{
				int idx = 0;
				if (int.TryParse(xmlAttribute.InnerText, out idx) == true)
				{
					element.Index = idx;
				}
			}
			
			xmlAttribute = elementNode.Attributes["ClassName"];
			if (xmlAttribute != null)
			{
				element.ClassName = xmlAttribute.InnerText;
			}
			
			xmlAttribute = elementNode.Attributes["FrameworkId"];
			if (xmlAttribute != null)
			{
				element.FrameworkId = xmlAttribute.InnerText;
			}
			
			XmlNodeList nameNodeList = elementNode.SelectNodes("Name");
			if (nameNodeList.Count > 0)
			{
				element.Name = nameNodeList[0].InnerText;
			}
			
			return true;
		}
		
		public static XmlElement WriteHierarchyToXml(this Element element, XmlDocument doc)
		{
			XmlElement hierarchyNode = doc.CreateElement("Hierarchy");
			
			XmlElement elementNode = element.WriteToXml(doc);
			hierarchyNode.AppendChild(elementNode);
			
			Element parent = element.Parent;
			while (parent != null)
			{
				XmlElement parentNode = parent.WriteToXml(doc);
				hierarchyNode.AppendChild(parentNode);
				
				parent = parent.Parent;
			}
			
			return hierarchyNode;
		}
		
		public static XmlElement WriteToXml(this Element element, XmlDocument doc)
		{
			XmlElement elementNode = doc.CreateElement("Element");
			elementNode.SetAttribute("ControlType", element.ControlType.ToString());
			if (element.Index > 0)
			{
				elementNode.SetAttribute("Index", element.Index.ToString());
			}
			
			if (element.ClassName != null)
			{
				elementNode.SetAttribute("ClassName", element.ClassName);
				elementNode.SetAttribute("FrameworkId", element.FrameworkId);
			}
			
			if (element.WasNameTruncated == true)
			{
				elementNode.SetAttribute("WNT", element.WasNameTruncated.ToString());
			}
			
			if (element.Name != null)
			{
				XmlElement nameNode = doc.CreateElement("Name");
				nameNode.InnerText = element.Name;
				elementNode.AppendChild(nameNode);
			}
			
			return elementNode;
		}
		// **************************************
		
		// load/write Action from/to xml ********
		public static bool LoadFromXml(this Action action, XmlNode actionNode)
		{
			XmlAttribute xmlAttribute = actionNode.Attributes["Id"];
			if (xmlAttribute != null)
			{
				action.Id = int.Parse(xmlAttribute.InnerText);
			}
		
			XmlNodeList hierarchyNodeList = actionNode.SelectNodes("Hierarchy");
			if (hierarchyNodeList.Count > 0)
			{
				XmlNode hierarchyNode = hierarchyNodeList[0];
				action.Element = new Element();
				action.Element.LoadHierarchyFromXml(hierarchyNode);
			}
			
			XmlNodeList actionIdNodeList = actionNode.SelectNodes("ActionId");
			if (actionIdNodeList.Count == 0)
			{
				return false;
			}
			
			XmlNode actionIdNode = actionIdNodeList[0];
			xmlAttribute = actionIdNode.Attributes["Name"];
			if (xmlAttribute == null)
			{
				return false;
			}
			string actionIdName = xmlAttribute.InnerText;
			
			// translate from enum string to enum value
			action.ActionId = (ActionIds)Enum.Parse(typeof(ActionIds), actionIdName);
			
			XmlNodeList parameterNodeList = actionIdNode.SelectNodes("Parameter");
			
			if (parameterNodeList.Count > 0)
			{
				action.Parameters = new List<object>();
			}
			foreach (XmlNode parameterNode in parameterNodeList)
			{
				string parameterString = parameterNode.InnerText;
				xmlAttribute = parameterNode.Attributes["Type"];
				
				string typeString = null;
				if (xmlAttribute != null)
				{
					typeString = xmlAttribute.InnerText;
				}
				
				if (typeString == "Boolean")
				{
					bool val = bool.Parse(parameterString);
					action.Parameters.Add(val);
				}
				else if (typeString == "Int32")
				{
					int val = int.Parse(parameterString);
					action.Parameters.Add(val);
				}
				else if (typeString == "Double")
				{
					double val = double.Parse(parameterString);
					action.Parameters.Add(val);
				}
				else if (typeString == "DateTime")
				{
					DateTime val = DateTime.Parse(parameterString, System.Globalization.CultureInfo.CurrentCulture);
					action.Parameters.Add(val);
				}
				else if (typeString == "UIDeskAutomationLib.VirtualKeys")
				{
					VirtualKeys val = (VirtualKeys)Enum.Parse(typeof(VirtualKeys), parameterString);
					action.Parameters.Add(val);
				}
				else // string
				{
					action.Parameters.Add(parameterString);
				}
			}
			
			return true;
		}
		
		public static XmlElement WriteToXml(this Action action, XmlDocument doc)
		{
			XmlElement actionNode = doc.CreateElement("Action");
			actionNode.SetAttribute("Type", "Normal");
			actionNode.SetAttribute("Id", action.Id.ToString());
			if (action.Next != null)
			{
				actionNode.SetAttribute("NextId", action.Next.Id.ToString());
				if (action.HasWeakBond)
				{
					actionNode.SetAttribute("WeakBound", "True");
				}
			}
			
			if (action.Element != null)
			{
				XmlElement hierarchyNode = action.Element.WriteHierarchyToXml(doc);
				actionNode.AppendChild(hierarchyNode);
			}
			
			XmlElement actionIdNode = doc.CreateElement("ActionId");
			actionIdNode.SetAttribute("Name", action.ActionId.ToString());
			
			if (action.Parameters != null)
			{
				foreach (object parameter in action.Parameters)
				{
					XmlElement parameterNode = doc.CreateElement("Parameter");
					
					string paramType = parameter.GetType().ToString();
					parameterNode.SetAttribute("Type", paramType.Replace("System.", ""));
					
					parameterNode.InnerText = parameter.ToString();
					actionIdNode.AppendChild(parameterNode);
				}
			}

			actionNode.AppendChild(actionIdNode);
			
			return actionNode;
		}
		// **************************************
	}
}