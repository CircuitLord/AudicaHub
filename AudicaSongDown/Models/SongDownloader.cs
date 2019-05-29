using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AudicaSongDown.Models {

	class SongDownloader {

		WebClient webClient = new WebClient();

		public async Task<bool> DownloadSong(Song song) {
			Uri uri = new Uri(song.downloadURL);

			try {
				await Task.Run(() => webClient.DownloadFileTaskAsync(uri, Properties.Settings.Default.CustomSongFolder + $"\\{song.fileName}"));

			} catch (Exception e) {
				MessageBox.Show(e.ToString());
				return false;
			}


			return true;
		}



	}
}
