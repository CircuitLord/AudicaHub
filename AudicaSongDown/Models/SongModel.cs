using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AudicaSongDown.Models {



	public class Song {

		private string _songName { get; set; }
		public string songName {
			get {
				if (_songName != null) {
					return _songName;
				} else {
					//7 for audica
					//6 for SR
					return fileName.Substring(0, fileName.Length - 7);
				}
			}
			set {
				_songName = value;
			}
		}

		public List<string> difficulties { get; set; } = null;

		public string GetDifficulties() {

			if (difficulties == null) return "Not specified.";

			string diffs = "";
			int i = 0;
			foreach (string diff in difficulties) {
				if (i == 0) {
					diffs += diff;
				} else {
					diffs += ", " + diff;
				}
				i++;
			}
			return diffs;
		}

		public string authors { get; set; }

		public string fileName { get; set; }

		public string mapper { get; set; } = null;

		public double fileSize { get; set; } = 0;


		public string downloadURL { get; set; } = null;

		public string description { get; set; } = null;

		public string demoVideoLink { get; set; } = null;

		public string length { get; set; } = null;

		public BitmapImage icon { get; set; } = null;


		private string GenSongName(string fileName, GlobalVar.SongMode mode) {
			return "";
		}



	}




	public class SongsList {
		public ObservableCollection<Song> songs = new ObservableCollection<Song>();

	}
}
