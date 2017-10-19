using System;
using System.Linq;
using System.Windows.Input;
using WinCalc.Common;

namespace WinCalc.ViewModels
{
	public class CalculatorViewModel : ViewModelBase
	{
		private Command<string> _buttonPressedCommand;
		private string _currentInput;
		private string _textExpression;
		private bool binaryOperationPressed;
		private string calcmemory;

		public string CurrentInput
		{
			get { return _currentInput; }
			set
			{
				_currentInput = value.Replace(',', '.'); //replace to avoid errors
				OnPropertyChanged("CurrentInput");
			}
		}

		public string TextExpression
		{
			get { return _textExpression; }
			set
			{
				_textExpression = value;
				OnPropertyChanged("TextExpression");
			}
		}

		private void buttonPressed(string button)
		{
			try 
			{
				var binaryOperations = new string[] { "+", "-", "*", "/" };
				if (binaryOperations.Contains(button)) 
				{
					TextExpression = ProcessBinaryOperation(button);
				} 
				else 
				{
					ProcessOtherButtons(button);
				}
			} 
			catch 
			{
				CurrentInput = "Error";
			}
		}

		private void ClearDisplay()
		{
			TextExpression = string.Empty;
			CurrentInput = "0";
		}

		private string ProcessBinaryOperation(string button)
		{
			if (!binaryOperationPressed) {
				binaryOperationPressed = true;
				return string.Concat(TextExpression, " ", CurrentInput, " ", button);
			} 
			else 
			{
				var textExpressionTokens = TextExpression.Split(' ');
				textExpressionTokens[textExpressionTokens.Length - 1] = button;
				return string.Join(" ", textExpressionTokens);
			}
		}

		private void ProcessOtherButtons(string button)
		{
			switch (button) {
				case "C":
					ClearDisplay();
					break;

				case "CE":
					CurrentInput = "0";
					break;

				case "±":
					if (!CurrentInput.Contains('-'))
						CurrentInput = "-" + CurrentInput;
					else {
						CurrentInput = CurrentInput.Substring(1);
					}
					break;

				case "<-":
					if (CurrentInput.Length == 0 || CurrentInput == "-" || CurrentInput == "Error" || CurrentInput == "NaN")
						CurrentInput = "0";
					else
						CurrentInput = CurrentInput.Remove(CurrentInput.Length - 1);
					break;

				case "x^2":
					NCalc.Expression powExp = new NCalc.Expression("Round(Pow(" + CurrentInput + ", 2), 3)");
					CurrentInput = powExp.Evaluate().ToString();
					break;

				case "√":
					NCalc.Expression sqrtExp = new NCalc.Expression("Round(Sqrt(" + CurrentInput + "), 3)");
					CurrentInput = sqrtExp.Evaluate().ToString();
					break;

				case "1/x":
					CurrentInput = new NCalc.Expression($"Round(1/{CurrentInput}, 3)").Evaluate().ToString();
					break;

				case "MS":
					calcmemory = CurrentInput;
					break;

				case "M+":
					calcmemory = new NCalc.Expression(calcmemory + "+" + CurrentInput).Evaluate().ToString();
					break;

				case "M-":
					calcmemory = new NCalc.Expression(calcmemory + "-" + CurrentInput).Evaluate().ToString();
					break;

				case "MR":
					if (!String.IsNullOrEmpty(calcmemory))
						CurrentInput = calcmemory;
					break;

				case "MC":
					calcmemory = "0";
					break;

				case ".":
					if (!CurrentInput.Contains("."))
						CurrentInput += ".";
					break;

				case "=":
					NCalc.Expression exp = new NCalc.Expression(TextExpression + CurrentInput);
					TextExpression = string.Empty;
					CurrentInput = exp.Evaluate().ToString();
					break;

				default:
					if (binaryOperationPressed) {
						// if previous operation is bin operation to clear old input values
						CurrentInput = button;
						binaryOperationPressed = false;
					} else {
						CurrentInput = CurrentInput.TrimStart('0') + button; // add new value to display and remove start 0
					}
					break;
			}
		}

		public ICommand ButtonPressedCommand
		{
			get
			{
				if (_buttonPressedCommand == null) {
					_buttonPressedCommand = new Command<string>(buttonPressed);
				}
				return _buttonPressedCommand;
			}
		}
		
		public CalculatorViewModel()
		{
			ClearDisplay();
		}
	}
}
