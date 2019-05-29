using AudicaSongDown.Interfaces;
using AudicaSongDown.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AudicaSongDown {

	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	/// 

	public partial class PageSwitcher : Window {

		BrushConverter converter = new BrushConverter();
		Brush blueBack;

		Page homePage, customSongPage, customEnvironmentsPage, uploadPage, userProfilePage, modPage, settingsPage;


		public PageSwitcher() {

			CachePages();

			blueBack = (Brush)converter.ConvertFromString("#FF2196F3");
			InitializeComponent();

			Main.Content = new HomePage();
			HandleSidebarActive(0);

		}

		private void CachePages() {
			homePage = new HomePage();
			customSongPage = new CustomSongPage();
			modPage = new ModPage();
			customEnvironmentsPage = new CustomEnvironmentsPage();
			uploadPage = new UploadPage();
			userProfilePage = new UserProfilePage();
			settingsPage = new SettingsPage();


		}




		private void UISideButton_Click(object sender, RoutedEventArgs e) {
			Main.Content = homePage;
			HandleSidebarActive(0);
		}

		private void UISideSongs_Click(object sender, RoutedEventArgs e) {
			Main.Content = customSongPage;
			HandleSidebarActive(1);
		}


		private void UISideMods_Click(object sender, RoutedEventArgs e) {
			Main.Content = modPage;
			HandleSidebarActive(2);
		}

		private void UISideEnvironments_Click(object sender, RoutedEventArgs e) {
			Main.Content = customEnvironmentsPage;
			HandleSidebarActive(3);
		}

		private void UISideUploadSong_Click(object sender, RoutedEventArgs e) {
			Main.Content = uploadPage;
			HandleSidebarActive(4);
		}

		private void UISideProfile_Click(object sender, RoutedEventArgs e) {
			Main.Content = userProfilePage;
			HandleSidebarActive(5);
		}

		private void UISideSettings_Click(object sender, RoutedEventArgs e) {
			Main.Content = settingsPage;
			HandleSidebarActive(6);
		}

		private void HandleSidebarActive(int selected) {

			int i = 0;
			foreach (Button button in UISidebar.Children) {
				var curr = (button as Button);

				if (i == selected) {
					curr.Background = blueBack;
					curr.BorderBrush = blueBack;
				} else {
					curr.Background = Brushes.Gray;
					curr.BorderBrush = Brushes.Gray;
				}


				i++;
			}

		}



	}
}