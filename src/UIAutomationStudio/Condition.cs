using System;
using System.Windows;
using System.Collections.Generic;
using System.Xml;

namespace UIAutomationStudio
{
	public class Condition
	{
		public Variable Variable { get; set; }
		public Operator Operator { get; set; }
		public List<object> Values { get; set; }
		public bool Deny { get; set; }
		
		public Condition()
		{
			Variable = new Variable();
			Values = null;
			Deny = false;
		}
		
		public static void DeepCopy(Condition source, Condition destination)
		{
			Variable.DeepCopy(source.Variable, destination.Variable);
			destination.Operator = source.Operator;
			
			if (source.Values == null)
			{
				destination.Values = null;
			}
			else
			{
				destination.Values = new List<object>();
				foreach (object item in source.Values)
				{
					destination.Values.Add(item);
				}
			}
			
			destination.Deny = source.Deny;
		}
		
		public string Description
		{
			get
			{
				string operatorString = "";
				if (Operator == Operator.Equals)
				{
					if (Deny == true)
					{
						operatorString = "not equal to";
					}
					else
					{
						operatorString = "=";
					}
				}
				else if (Operator == Operator.GreaterThan)
				{
					if (Deny == true)
					{
						operatorString = "<=";
					}
					else
					{
						operatorString = ">";
					}
				}
				else if (Operator == Operator.GreaterThanOrEquals)
				{
					if (Deny == true)
					{
						operatorString = "<";
					}
					else
					{
						operatorString = ">=";
					}
				}
				else if (Operator == Operator.LessThan)
				{
					if (Deny == true)
					{
						operatorString = ">=";
					}
					else
					{
						operatorString = "<";
					}
				}
				else if (Operator == Operator.LessThanOrEquals)
				{
					if (Deny == true)
					{
						operatorString = ">";
					}
					else
					{
						operatorString = "<=";
					}
				}
				else if (Operator == Operator.Between || Operator == Operator.Outside || 
					Operator == Operator.Like)
				{
					if (Deny == true)
					{
						operatorString = "Not " + Operator.ToString();
					}
					else
					{
						operatorString = Operator.ToString();
					}
				}
				else if (Operator == Operator.Contains)
				{
					if (Deny == true)
					{
						operatorString = "Doesn't Contain";
					}
					else
					{
						operatorString = Operator.ToString();
					}
				}
				else if (Operator == Operator.StartsWith)
				{
					if (Deny == true)
					{
						operatorString = "Doesn't Start With";
					}
					else
					{
						operatorString = "Starts With";
					}
				}
				else if (Operator == Operator.EndsWith)
				{
					if (Deny == true)
					{
						operatorString = "Doesn't End With";
					}
					else
					{
						operatorString = "Ends With";
					}
				}
				
				if (this.Variable.PropertyType == PropertyType.Text)
				{
					if (this.Values.Count > 0)
					{
						Type firstParamType = this.Values[0].GetType();
						if (firstParamType == typeof(string))
						{
							string val = (string)this.Values[0];
							return operatorString + " \"" + val + "\"";
						}
						else if (firstParamType == typeof(double))
						{
							if (this.Values.Count == 1)
							{
								double val = (double)this.Values[0];
								return operatorString + " " + val;
							}
							else if (this.Values.Count >= 2)
							{
								double val1 = (double)this.Values[0];
								double val2 = (double)this.Values[1];
								return operatorString + " " + val1 + " and " + val2;
							}
						}
						else if (firstParamType == typeof(DateTime))
						{
							if (this.Values.Count == 1)
							{
								DateTime val = (DateTime)this.Values[0];
								return operatorString + " " + val.ToString("MM/dd/yyyy");
							}
							else if (this.Values.Count >= 2)
							{
								DateTime val1 = (DateTime)this.Values[0];
								DateTime val2 = (DateTime)this.Values[1];
								return operatorString + " " + val1.ToString("MM/dd/yyyy") + 
									" and " + val2.ToString("MM/dd/yyyy");
							}
						}
					}
				}
				else if (this.Variable.PropertyType == PropertyType.Number)
				{
					if (this.Values.Count == 1)
					{
						double val = (double)this.Values[0];
						return operatorString + " " + val;
					}
					else if (this.Values.Count >= 2)
					{
						double val1 = (double)this.Values[0];
						double val2 = (double)this.Values[1];
						return operatorString + " " + val1 + " and " + val2;
					}
				}
				else if (this.Variable.PropertyType == PropertyType.YesNo)
				{
					if (this.Values.Count > 0)
					{
						bool val = (bool)this.Values[0];
						string valString = (val == true ? "Yes" : "No");
						return operatorString + " " + valString;
					}
				}
				else if (this.Variable.PropertyType == PropertyType.Date)
				{
					if (this.Values.Count == 1)
					{
						DateTime val = (DateTime)this.Values[0];
						return operatorString + " " + val.ToString("MM/dd/yyyy");
					}
					else if (this.Values.Count >= 2)
					{
						DateTime val1 = (DateTime)this.Values[0];
						DateTime val2 = (DateTime)this.Values[1];
						return operatorString + " " + val1.ToString("MM/dd/yyyy") + 
							" and " + val2.ToString("MM/dd/yyyy");
					}
				}
				
				return null;
			}
		}
		
		public bool? Evaluate(bool message = true)
		{
			object val = this.Variable.Evaluate();
			if (val == null)
			{
				if (message == true)
				{
					MessageBox.Show("Condition could not be evaluated. The UI element may not be available.");
				}
				return null;
			}
			
			if (this.Values == null || this.Values.Count == 0)
			{
				return null;
			}
			
			bool? retValue = null;
			Type type = this.Values[0].GetType();
			if (type == typeof(bool))
			{
				retValue = (bool)val == (bool)this.Values[0];
			}
			else if (type == typeof(double))
			{
				double dVal = 0.0;
				if (this.Variable.PropertyType == PropertyType.Text)
				{
					if (double.TryParse(val.ToString(), out dVal) == false)
					{
						MessageBox.Show(MainWindow.Instance, "Condition could not be evaluated. The value is expected to be a number but it's not.");
						return null;
					}
				}
				else
				{
					try
					{
						dVal = Convert.ToDouble(val);
					}
					catch (Exception ex)
					{
						MessageBox.Show("Cast: " + ex.Message);
						return null;
					}
				}
			
				if (this.Operator == Operator.Equals)
				{
					retValue = dVal == (double)this.Values[0];
				}
				else if (this.Operator == Operator.GreaterThan)
				{
					retValue = dVal > (double)this.Values[0];
				}
				else if (this.Operator == Operator.GreaterThanOrEquals)
				{
					retValue = dVal >= (double)this.Values[0];
				}
				else if (this.Operator == Operator.LessThan)
				{
					retValue = dVal < (double)this.Values[0];
				}
				else if (this.Operator == Operator.LessThanOrEquals)
				{
					retValue = dVal <= (double)this.Values[0];
				}
				else if (this.Operator == Operator.Between && this.Values.Count >= 2)
				{
					retValue = dVal >= (double)this.Values[0] && dVal <= (double)this.Values[1];
				}
				else if (this.Operator == Operator.Outside && this.Values.Count >= 2)
				{
					retValue = dVal < (double)this.Values[0] || dVal > (double)this.Values[1];
				}
			}
			else if (type == typeof(DateTime))
			{
				DateTime dateVal;
				if (this.Variable.PropertyType == PropertyType.Text)
				{
					if (DateTime.TryParse(val.ToString(), out dateVal) == false)
					{
						MessageBox.Show(MainWindow.Instance, "Condition could not be evaluated. The value is expected to be a date but it's not.");
						return null;
					}
				}
				else
				{
					try
					{
						dateVal = Convert.ToDateTime(val);
					}
					catch (Exception ex)
					{
						MessageBox.Show("Cast: " + ex.Message);
						return null;
					}
				}
			
				if (this.Operator == Operator.Equals)
				{
					retValue = dateVal == (DateTime)this.Values[0];
				}
				else if (this.Operator == Operator.GreaterThan)
				{
					retValue = dateVal > (DateTime)this.Values[0];
				}
				else if (this.Operator == Operator.GreaterThanOrEquals)
				{
					retValue = dateVal >= (DateTime)this.Values[0];
				}
				else if (this.Operator == Operator.LessThan)
				{
					retValue = dateVal < (DateTime)this.Values[0];
				}
				else if (this.Operator == Operator.LessThanOrEquals)
				{
					retValue = dateVal <= (DateTime)this.Values[0];
				}
				else if (this.Operator == Operator.Between && this.Values.Count >= 2)
				{
					retValue = dateVal >= (DateTime)this.Values[0] && dateVal <= (DateTime)this.Values[1];
				}
				else if (this.Operator == Operator.Outside && this.Values.Count >= 2)
				{
					retValue = dateVal < (DateTime)this.Values[0] || dateVal > (DateTime)this.Values[1];
				}
			}
			else if (type == typeof(string))
			{
				string valString = val.ToString();
				string param = this.Values[0].ToString();
				bool caseSensitive = (bool)this.Values[this.Values.Count - 1]; // last value is the case sensitive bool
				
				if (caseSensitive == false)
				{
					valString = valString.ToLower();
					param = param.ToLower();
				}
				
				if (this.Operator == Operator.Equals)
				{
					retValue = valString == param;
				}
				else if (this.Operator == Operator.StartsWith)
				{
					retValue = valString.StartsWith(param);
				}
				else if (this.Operator == Operator.EndsWith)
				{
					retValue = valString.EndsWith(param);
				}
				else if (this.Operator == Operator.Contains)
				{
					retValue = valString.Contains(param);
				}
				else if (this.Operator == Operator.Like)
				{
					retValue = Helper.MatchStrings(valString, param);
				}
			}
		
			if (this.Deny == true && retValue.HasValue)
			{
				return !retValue.Value;
			}
			
			return retValue;
		}
	}
	
	public enum Operator
	{
		Equals,
		GreaterThan,
		GreaterThanOrEquals,
		LessThan,
		LessThanOrEquals,
		Between,
		Outside,
		Like,
		StartsWith,
		EndsWith,
		Contains
	}
	
	public enum LogicalOp
	{
		None,
		AND,
		OR
	}
}