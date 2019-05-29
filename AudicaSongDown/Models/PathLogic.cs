using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudicaSongDown.Dependencies;
using Microsoft.Win32;

namespace AudicaSongDown.Models {


	class PathLogic {

		public void GetSongFolder(bool overwrite = false, GlobalVar.SongMode mode = GlobalVar.SongMode.Audica) {

			//If we already have the folder, and the user doesn't want to overwrite that value, just return.
			if (overwrite == false && Properties.Settings.Default.CustomSongFolder != null) return;

			if (mode == GlobalVar.SongMode.Audica) {

				//Attempt to automatically find song folder.
				string installFolderSteam = GetSteamLocation(GlobalVar.AudicaSteamAppId);
				if (installFolderSteam != null) {
					SetSongFolder(installFolderSteam);
					return;
				}

				string installFolderOculus = GetValidOculusLocation(GlobalVar.AudicaOculusSubFolderPath, GlobalVar.AudicaExe);
				if (installFolderOculus != null) {
					SetSongFolder(installFolderOculus);
					return;
				}

				//Let the user manually select it.
				string installFolderUser = FolderSelectionDialog();
				if (installFolderUser != null) {
					SetSongFolder(installFolderUser);
					return;
				}

				void SetSongFolder(string installFolder) {
					Properties.Settings.Default.CustomSongFolder = installFolder + GlobalVar.AudicaAddonPath;
					Properties.Settings.Default.Save();
				}

			}

			if (mode == GlobalVar.SongMode.SynthRiders) {

				//Attempt to automatically find song folder.
				string installFolderSteam = GetSteamLocation(GlobalVar.SRSteamAppId);
				if (installFolderSteam != null) {
					SetSongFolder(installFolderSteam);
					return;
				}

				string installFolderOculus = GetValidOculusLocation(GlobalVar.SROculusSubFolderPath, GlobalVar.SRExe);
				if (installFolderOculus != null) {
					SetSongFolder(installFolderOculus);
					return;
				}

				//Let the user manually select it.
				string installFolderUser = FolderSelectionDialog();
				if (installFolderUser != null) {
					SetSongFolder(installFolderUser);
					return;
				}

				void SetSongFolder(string installFolder) {
					Properties.Settings.Default.CustomSongFolder = installFolder + GlobalVar.SRAddonPath;
					Properties.Settings.Default.Save();
				}
			}


			return;
		}

		// Bring up a dialog to chose a folder path in which to open or save a file.
		public string FolderSelectionDialog() {
			var fbd = new FolderBrowserDialog();
			fbd.Description = @"The tool was unable to detect your install folder. Please select it.";

			// Show the FolderBrowserDialog.
			DialogResult result = fbd.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK) {
				return fbd.SelectedPath;

			}

			if (result == System.Windows.Forms.DialogResult.Cancel) {
				//System.Windows.Application.Current.Shutdown();
			}

			return null;
		}


		public string GetSteamLocation(int appId) {
			try {
				var steamFinder = new SteamFinder();
				if (!steamFinder.FindSteam())
					return null;

				return steamFinder.FindGameFolder(appId);
			} catch (Exception ex) {
				Console.WriteLine(ex);
				return null;
			}

		}


		public string GetValidOculusLocation(string subFolderPath, string exeName) {

			string path = Registry.LocalMachine.OpenSubKey("SOFTWARE")?.OpenSubKey("WOW6432Node")?.OpenSubKey("Oculus VR, LLC")?.OpenSubKey("Oculus")?.OpenSubKey("Config")?.GetValue("InitialAppLibrary") as string;
			Console.WriteLine(path);
			if (path == null) {
				// No Oculus Home detected
				//return null;
			}



			// With the old Home
			//string folderPath = Path.Combine(path, subFolderPath);
			string folderPath = path + subFolderPath;
			//string fullAppPath = Path.Combine(folderPath, AppFileName);
			string fullAppPath = folderPath + exeName;

			if (File.Exists(fullAppPath)) {
				return folderPath;
			} else {
				// With the new Home / Dash
				using (RegistryKey librariesKey = Registry.CurrentUser.OpenSubKey("Software")?.OpenSubKey("Oculus VR, LLC")?.OpenSubKey("Oculus")?.OpenSubKey("Libraries")) {
					// Oculus libraries uses GUID volume paths like this "\\?\Volume{0fea75bf-8ad6-457c-9c24-cbe2396f1096}\Games\Oculus Apps", we need to transform these to "D:\Game"\Oculus Apps"
					WqlObjectQuery wqlQuery = new WqlObjectQuery("SELECT * FROM Win32_Volume");
					ManagementObjectSearcher searcher = new ManagementObjectSearcher(wqlQuery);
					Dictionary<string, string> guidLetterVolumes = new Dictionary<string, string>();

					foreach (ManagementBaseObject disk in searcher.Get()) {
						var diskId = ((string)disk.GetPropertyValue("DeviceID")).Substring(11, 36);
						var diskLetter = ((string)disk.GetPropertyValue("DriveLetter")) + @"\";

						if (!string.IsNullOrWhiteSpace(diskLetter)) {
							guidLetterVolumes.Add(diskId, diskLetter);
						}
					}

					// Search among the library folders
					foreach (string libraryKeyName in librariesKey.GetSubKeyNames()) {
						Console.WriteLine(libraryKeyName);
						using (RegistryKey libraryKey = librariesKey.OpenSubKey(libraryKeyName)) {
							string libraryPath = (string)libraryKey.GetValue("Path");
							folderPath = Path.Combine(guidLetterVolumes.First(x => libraryPath.Contains(x.Key)).Value, libraryPath.Substring(49), subFolderPath);
							fullAppPath = Path.Combine(folderPath, exeName);

							if (File.Exists(fullAppPath)) {
								return folderPath;
							}
						}
					}
				}
			}

			return null;
		}
	}
}
