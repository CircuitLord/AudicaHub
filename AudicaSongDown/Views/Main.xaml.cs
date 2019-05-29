using AudicaSongDown.Models;
using AutoUpdaterDotNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
	/// Interaction logic for Main.xaml
	/// </summary>
	public partial class Main : Window {

		//Core Const
		SongMode songMode = SongMode.Audica;

		SongsList songsList = new SongsList();
		SongsList searchSongsList = new SongsList();

		PathLogic pathLogic = new PathLogic();
		SongFetcher songFetcher = new SongFetcher();
		SongDownloader songDownloader = new SongDownloader();

		bool isSearching = false;

		public Main() {
			this.DataContext = this;

			pathLogic.GetSongFolder(false, songMode);

			InitializeComponent();


			UISongsSearch.IsEnabled = false;
			UIDownloadBtn.IsEnabled = false;
			UIUninstallBtn.IsEnabled = false;
			UIPreviewSong.IsEnabled = false;

			AutoUpdater.Start("https://circuitcubed.com/asd/ASDVersions.xml");

			FetchSongsListAsync();







		}


		private void UpdateSongInstalledUI(bool isInstalled) {
			if (isInstalled) {
				UIDownloadBtn.IsEnabled = false;
				UIUninstallBtn.IsEnabled = true;
			} else {
				UIDownloadBtn.IsEnabled = true;
				UIUninstallBtn.IsEnabled = false;
			}
		}

		private async void FetchSongsListAsync() {
			songsList = await songFetcher.FetchAll(songMode);
			if (songsList == null) return;

			UISongsList.ItemsSource = songsList.songs;
		}

		private Song GetSelectedSong() {
			Song song = new Song();
			if (UISongsList.SelectedIndex == -1) return song;

			return songsList.songs[UISongsList.SelectedIndex];

			if (isSearching == false) {
				return songsList.songs[UISongsList.SelectedIndex];
			} else {
				return searchSongsList.songs[UISongsList.SelectedIndex];
			}
		}

		private void UpdateSongDescUI(string name, string authors, string mapper, string difficulties, string length) {
			UISongName.Text = name;
			UISongAuthors.Text = authors;
			UISongMapper.Text = mapper;
			UISongDifficulties.Text = difficulties;
			UISongLength.Text = length;
		}

		private void UISongsSearch_TextChanged(object sender, TextChangedEventArgs e) {
			//UISongsList.SelectedIndex = -1;
			string search = UISongsSearch.Text;
			searchSongsList.songs.Clear();

			if (UISongsSearch.Text == "") {
				UISongsList.ItemsSource = songsList.songs;
				isSearching = false;
				return;
			}

			foreach (Song song in songsList.songs) {
				if (string.IsNullOrWhiteSpace(song.songName)) song.songName = "error";

				if (song.songName.ToLower().Contains(search)) {
					searchSongsList.songs.Add(song);
				}
			}

			UISongsList.ItemsSource = searchSongsList.songs;
			isSearching = true;


		}

		private void UISongsList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			UIPreviewSong.IsEnabled = true;
			Song song = GetSelectedSong();
			if (song == null) return;

			//UISongInfo.Text = $"Discord Uploader: {song.uploader}\nFile Name: {song.fileName}\nFile Size: {Math.Round((song.fileSize / 1024) / 1024, 1)}MB";

			if (File.Exists(Properties.Settings.Default.CustomSongFolder + "\\" + song.fileName)) {
				UpdateSongInstalledUI(true);
			} else {
				UpdateSongInstalledUI(false);
			}


			UpdateSongDescUI(song.songName, song.authors, song.mapper, song.GetDifficulties(), song.length);
		}


		//BUTTONS

		private void UIPreviewSong_Click(object sender, RoutedEventArgs e) {
			Song song = GetSelectedSong();

			Console.WriteLine(song.songName);
			if (song.demoVideoLink == "") song.demoVideoLink = ("https://www.youtube.com/results?search_query=Audica " + song.songName);

			System.Diagnostics.Process.Start(song.demoVideoLink);

		}

		private async void UIDownloadBtn_Click(object sender, RoutedEventArgs e) {

			UISongsList.IsEnabled = false;

			await songDownloader.DownloadSong(GetSelectedSong());

			UISongsList.IsEnabled = true;

			UpdateSongInstalledUI(true);
		}

		private void UISettingsBtn_Click(object sender, RoutedEventArgs e) {
			Settings settingsWindow = new Settings();
			settingsWindow.Show();
		}

		private void UIUninstallBtn_Click(object sender, RoutedEventArgs e) {
			MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this song?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.Yes) {

				var song = GetSelectedSong();
				File.Delete(Properties.Settings.Default.CustomSongFolder + $"\\{song.fileName}");
				UpdateSongInstalledUI(false);


			}
		}
	}
}
