using System;
using System.Windows.Input;

namespace WinCalc.Common
{
	public class Command<T> : ICommand
	{
		#region Constructors

		/// <summary>
		///     Constructor
		/// </summary>
		public Command(Action<T> executeMethod)
		{
			if (executeMethod == null) {
				throw new ArgumentNullException("executeMethod");
			}
			_executeMethod = executeMethod;
		}

		/// <summary>
		/// 

		#endregion

		#region Public Methods

		/// <summary>
		///     Method to determine if the command can be executed
		/// </summary>
		public bool CanExecute(T parameter)
		{
			if (_canExecuteMethod != null) {
				return _canExecuteMethod(parameter);
			}
			return true;
		}

		/// <summary>
		///    Execution of the command
		/// </summary>
		public void Execute(T parameter)
		{
			if (_executeMethod != null) {
				_executeMethod(parameter);
			}
		}

		#endregion

		#region ICommand Members

		/// <summary>
		///     ICommand.CanExecuteChanged implementation
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}


		bool ICommand.CanExecute(object parameter)
		{
			// if T is of value type and the parameter is not
			// set yet, then return false if CanExecute delegate
			// exists, else return true
			if (parameter == null &&
				typeof(T).IsValueType) {
				return (_canExecuteMethod == null);
			}
			return CanExecute((T)parameter);
		}

		void ICommand.Execute(object parameter)
		{
			Execute((T)parameter);
		}

		#endregion

		#region Data

		private readonly Action<T> _executeMethod = null;
		private readonly Func<T, bool> _canExecuteMethod = null;

		#endregion
	}
}
