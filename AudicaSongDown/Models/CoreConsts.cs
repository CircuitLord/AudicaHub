using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudicaSongDown.Models {


	public class GlobalVar {

		public const string AudicaAddonPath = @"\Audica_Data\StreamingAssets\HmxAudioAssets\songs";
		public const string SRAddonPath = @"\CustomSongs";

		public const string AudicaExe = "Audica.exe";
		public const string SRExe = "SynthRiders.exe";

		public const int AudicaSteamAppId = 1020340;
		public const int SRSteamAppId = 885000;

		public const string AudicaOculusSubFolderPath = @"Software\harmonix-audica-k\";
		public const string SROculusSubFolderPath = @"Software\kluge-interactive-synthriders\";

		public static readonly Uri AudicaSongAPI = new Uri("https://circuitcubed.com/asd/latest-songlist.json");
		public static readonly Uri SRSongAPI = new Uri("https://circuitcubed.com/srsd/latest-songlist.json");

		[Serializable]
		public enum SongMode {
			Audica,
			SynthRiders
		}

	}



}
