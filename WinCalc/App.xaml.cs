using System.Windows;

namespace WinCalc
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void OnStartup(object sender, StartupEventArgs e)
		{
			Views.CalculatorView view = new Views.CalculatorView();
			view.DataContext = new ViewModels.CalculatorViewModel();
			view.Show();
		}
	}
}
