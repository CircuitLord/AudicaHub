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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static AudicaSongDown.Models.GlobalVar;

namespace AudicaSongDown.Pages {
	/// <summary>
	/// Interaction logic for CustomSongPage.xaml
	/// </summary>
	public partial class CustomSongPage : Page {

		//Core Const
		SongMode songMode = SongMode.Audica;

		SongsList songsList = new SongsList();
		SongsList searchSongsList = new SongsList();

		PathLogic pathLogic = new PathLogic();
		SongFetcher songFetcher = new SongFetcher();
		SongDownloader songDownloader = new SongDownloader();






		public CustomSongPage() {

			pathLogic.GetSongFolder(false, songMode);

			InitializeComponent();

			FetchSongsListAsync();


		}


		private async void FetchSongsListAsync() {
			songsList = await songFetcher.FetchAll(songMode);
			if (songsList == null) return;

			UISongsList.ItemsSource = songsList.songs;
		}




		private void UISongsList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			/*
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
			*/
		}

	}
}
