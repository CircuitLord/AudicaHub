using AudicaSongDown.Models;
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
using static AudicaSongDown.Models.GlobalVar;

namespace AudicaSongDown.Views {
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public partial class Settings : Window {

		SongMode songMode = SongMode.Audica;
		PathLogic pathLogic = new PathLogic();


		public Settings() {
			InitializeComponent();

			UpdateSongFolderUI();
		}

		private void UISelectSongFolder_Click(object sender, RoutedEventArgs e) {
			string installFolderUser = pathLogic.FolderSelectionDialog();
			Properties.Settings.Default.CustomSongFolder = installFolderUser;
			Properties.Settings.Default.Save();
			UpdateSongFolderUI();
		}

		private void UIAutodetectSongFolder_Click(object sender, RoutedEventArgs e) {
			pathLogic.GetSongFolder(true, songMode);
			UpdateSongFolderUI();
		}

		private void UpdateSongFolderUI() {
			UISongFolder.Text = Properties.Settings.Default.CustomSongFolder;
		}
	}
}
