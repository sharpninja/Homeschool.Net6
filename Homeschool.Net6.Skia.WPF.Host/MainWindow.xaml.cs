using System;
using System.Windows;

namespace Homeschool.Net6.WPF.Host
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
	
			root.Content = new global::Uno.UI.Skia.Platform.WpfHost(Dispatcher, () => new Homeschool.Net6.App());
		}
	}
}
