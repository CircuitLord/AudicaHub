using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AudicaSongDown.Models {

	class SongFetcher {

		WebClient webClient = new WebClient();


		public async Task<SongsList> FetchAll(GlobalVar.SongMode mode) {

			SongsList list = new SongsList();

			if (mode == GlobalVar.SongMode.Audica) {
				return list = await DownloadSongList(GlobalVar.AudicaSongAPI);
			}

			if (mode == GlobalVar.SongMode.SynthRiders) {
				return list = await DownloadSongList(GlobalVar.SRSongAPI);
			}

			return list;


		}

		private async Task<SongsList> DownloadSongList(Uri uri) {
			SongsList list = new SongsList();
			try {
				string json = await Task.Run(() => webClient.DownloadStringTaskAsync(uri));
				JsonConvert.PopulateObject(json, list);

			} catch (WebException e) {
				MessageBox.Show(e.ToString());
				return null;
			}
			return list;
		}






	}
}
